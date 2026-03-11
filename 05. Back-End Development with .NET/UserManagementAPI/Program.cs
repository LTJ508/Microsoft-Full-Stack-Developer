using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// ── In-memory store ──────────────────────────────────────────────────────────
var users = new Dictionary<int, User>
{
    [1] = new User(1, "Alice Johnson", "alice@techhive.com", "HR"),
    [2] = new User(2, "Bob Smith",    "bob@techhive.com",   "IT"),
};
int nextId = 3;

// ── Validation helper ────────────────────────────────────────────────────────
// BUG FIX: centralised validation prevents invalid data from ever reaching
// the store and returns descriptive 400 messages instead of crashing.
static bool IsValidEmail(string? email) =>
    !string.IsNullOrWhiteSpace(email) &&
    Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

static List<string> Validate(UserRequest? req)
{
    var errors = new List<string>();
    if (req is null) { errors.Add("Request body is required."); return errors; }
    if (string.IsNullOrWhiteSpace(req.Name))      errors.Add("'name' is required and cannot be empty.");
    if (!IsValidEmail(req.Email))                  errors.Add("'email' must be a valid email address (e.g. user@example.com).");
    if (string.IsNullOrWhiteSpace(req.Department)) errors.Add("'department' is required and cannot be empty.");
    return errors;
}

// ── Endpoints ────────────────────────────────────────────────────────────────

// GET /users — retrieve all users
// BUG FIX: snapshot to List so serialisation is stable and not tied to the
// live dictionary reference; wrapped in try-catch for unexpected failures.
app.MapGet("/users", () =>
{
    try
    {
        return Results.Ok(users.Values.ToList());
    }
    catch (Exception ex)
    {
        return Results.Problem($"An unexpected error occurred while retrieving users: {ex.Message}");
    }
});

// GET /users/{id} — retrieve a specific user
// BUG FIX: returns a clear 404 with a message instead of throwing KeyNotFoundException.
app.MapGet("/users/{id:int}", (int id) =>
{
    try
    {
        return users.TryGetValue(id, out var user)
            ? Results.Ok(user)
            : Results.NotFound(new { message = $"User with ID {id} was not found." });
    }
    catch (Exception ex)
    {
        return Results.Problem($"An unexpected error occurred: {ex.Message}");
    }
});

// POST /users — create a new user
// BUG FIX: validates input, rejects duplicate emails, and handles exceptions.
app.MapPost("/users", (UserRequest? req) =>
{
    try
    {
        var errors = Validate(req);
        if (errors.Count > 0)
            return Results.BadRequest(new { errors });

        // Prevent duplicate email addresses (case-insensitive).
        if (users.Values.Any(u => string.Equals(u.Email, req!.Email, StringComparison.OrdinalIgnoreCase)))
            return Results.Conflict(new { message = $"A user with email '{req!.Email}' already exists." });

        var user = new User(
            nextId++,
            req!.Name!.Trim(),
            req.Email!.Trim().ToLowerInvariant(),
            req.Department!.Trim());

        users[user.Id] = user;
        return Results.Created($"/users/{user.Id}", user);
    }
    catch (Exception ex)
    {
        return Results.Problem($"An unexpected error occurred while creating the user: {ex.Message}");
    }
});

// PUT /users/{id} — update an existing user
// BUG FIX: validates input, checks 404, prevents email collision with other users.
app.MapPut("/users/{id:int}", (int id, UserRequest? req) =>
{
    try
    {
        var errors = Validate(req);
        if (errors.Count > 0)
            return Results.BadRequest(new { errors });

        if (!users.ContainsKey(id))
            return Results.NotFound(new { message = $"User with ID {id} was not found." });

        // Allow the same email for the same user but block collision with others.
        if (users.Values.Any(u => u.Id != id &&
                string.Equals(u.Email, req!.Email, StringComparison.OrdinalIgnoreCase)))
            return Results.Conflict(new { message = $"Another user with email '{req!.Email}' already exists." });

        users[id] = new User(
            id,
            req!.Name!.Trim(),
            req.Email!.Trim().ToLowerInvariant(),
            req.Department!.Trim());

        return Results.Ok(users[id]);
    }
    catch (Exception ex)
    {
        return Results.Problem($"An unexpected error occurred while updating the user: {ex.Message}");
    }
});

// DELETE /users/{id} — remove a user
app.MapDelete("/users/{id:int}", (int id) =>
{
    try
    {
        if (!users.Remove(id))
            return Results.NotFound(new { message = $"User with ID {id} was not found." });

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem($"An unexpected error occurred while deleting the user: {ex.Message}");
    }
});

app.Run();

// ── Models ───────────────────────────────────────────────────────────────────
record User(int Id, string Name, string Email, string Department);

// BUG FIX: fields are nullable so that missing JSON properties are caught by
// Validate() and return a 400, rather than causing an unhandled exception.
record UserRequest(string? Name, string? Email, string? Department);

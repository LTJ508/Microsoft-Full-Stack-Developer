using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// ── Middleware pipeline (order matters) ───────────────────────────────────────
// 1. Error-handling — outermost layer, catches any unhandled exception and
//    returns a consistent JSON error response instead of a raw 500 page.
app.UseMiddleware<ErrorHandlingMiddleware>();

// 2. Authentication — validates the Bearer token on every request before
//    any endpoint logic runs; rejects invalid/missing tokens with 401.
app.UseMiddleware<AuthenticationMiddleware>();

// 3. Logging — records the HTTP method, path, and final response status code
//    for auditing purposes (runs after auth so only authorised traffic is logged).
app.UseMiddleware<LoggingMiddleware>();

// ── In-memory store ──────────────────────────────────────────────────────────
var users = new Dictionary<int, User>
{
    [1] = new User(1, "Alice Johnson", "alice@techhive.com", "HR"),
    [2] = new User(2, "Bob Smith",    "bob@techhive.com",   "IT"),
};
int nextId = 3;

// ── Validation helper ────────────────────────────────────────────────────────
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
app.MapPost("/users", (UserRequest? req) =>
{
    try
    {
        var errors = Validate(req);
        if (errors.Count > 0)
            return Results.BadRequest(new { errors });

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
app.MapPut("/users/{id:int}", (int id, UserRequest? req) =>
{
    try
    {
        var errors = Validate(req);
        if (errors.Count > 0)
            return Results.BadRequest(new { errors });

        if (!users.ContainsKey(id))
            return Results.NotFound(new { message = $"User with ID {id} was not found." });

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
record UserRequest(string? Name, string? Email, string? Department);

// ── Middleware implementations ────────────────────────────────────────────────

/// <summary>
/// Middleware 1 — Error handling.
/// Registered first (outermost) so it wraps the entire pipeline.
/// Catches any unhandled exception and returns a uniform JSON error response
/// so callers never receive a raw HTML 500 page.
/// </summary>
class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                context.Request.Method, context.Request.Path);

            // Only write the error response if headers haven't been sent yet.
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode  = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = "Internal server error." });
            }
        }
    }
}

/// <summary>
/// Middleware 2 — Token-based authentication.
/// Reads the Authorization header and validates a static Bearer token
/// stored in appsettings.json under Auth:Token.
/// Returns 401 Unauthorized for missing or invalid tokens.
/// </summary>
class AuthenticationMiddleware(RequestDelegate next, IConfiguration config)
{
    private const string BearerPrefix = "Bearer ";

    public async Task InvokeAsync(HttpContext context)
    {
        var validToken = config["Auth:Token"];
        var authHeader  = context.Request.Headers.Authorization.ToString();

        bool hasValidToken =
            authHeader.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase) &&
            authHeader[BearerPrefix.Length..] == validToken;

        if (!hasValidToken)
        {
            context.Response.StatusCode  = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(
                new { error = "Unauthorized. Provide a valid Bearer token in the Authorization header." });
            return;
        }

        await next(context);
    }
}

/// <summary>
/// Middleware 3 — Request/response logging.
/// Registered last (innermost) so it logs only requests that have passed
/// authentication. Records the HTTP method, path, and response status code.
/// </summary>
class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation("→ REQUEST   {Method} {Path}",
            context.Request.Method, context.Request.Path);

        await next(context);

        logger.LogInformation("← RESPONSE  {Method} {Path} [{StatusCode}]",
            context.Request.Method, context.Request.Path, context.Response.StatusCode);
    }
}

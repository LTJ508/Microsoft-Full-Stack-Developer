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

// ── Endpoints ────────────────────────────────────────────────────────────────

// GET / — simple health check
app.MapGet("/", () => "Hello World!");

// GET /users — retrieve all users
app.MapGet("/users", () => Results.Ok(users.Values));

// GET /users/{id} — retrieve a specific user
app.MapGet("/users/{id:int}", (int id) =>
    users.TryGetValue(id, out var user)
        ? Results.Ok(user)
        : Results.NotFound(new { message = $"User {id} not found." }));

// POST /users — create a new user
app.MapPost("/users", (UserRequest req) =>
{
    var user = new User(nextId++, req.Name, req.Email, req.Department);
    users[user.Id] = user;
    return Results.Created($"/users/{user.Id}", user);
});

// PUT /users/{id} — update an existing user
app.MapPut("/users/{id:int}", (int id, UserRequest req) =>
{
    if (!users.ContainsKey(id))
        return Results.NotFound(new { message = $"User {id} not found." });

    users[id] = new User(id, req.Name, req.Email, req.Department);
    return Results.Ok(users[id]);
});

// DELETE /users/{id} — remove a user
app.MapDelete("/users/{id:int}", (int id) =>
{
    if (!users.Remove(id))
        return Results.NotFound(new { message = $"User {id} not found." });

    return Results.NoContent();
});

app.Run();

// ── Models ───────────────────────────────────────────────────────────────────
record User(int Id, string Name, string Email, string Department);
record UserRequest(string Name, string Email, string Department);

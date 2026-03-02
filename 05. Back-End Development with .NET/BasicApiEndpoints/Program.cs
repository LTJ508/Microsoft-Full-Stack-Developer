var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Root Path");
app.MapGet("/downloads", () => "Downloads Path");
app.MapPut("/", () => "Root Path with PUT method");
app.MapDelete("/", () => "Root Path with DELETE method");
app.MapPost("/", () => "Root Path with POST method");

app.Run();

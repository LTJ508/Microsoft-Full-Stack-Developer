var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/users/{userId}/posts/{slug}", (int userId, string slug) =>
{
    return $"User ID: {userId}, Post ID: {slug}";
});

app.MapGet("/products/{id:int:min(0)}", (int id) =>
{
    return $"Product ID: {id}";
});

app.MapGet("/report/{year?}", (int? year = 2026) =>
{
    return $"Report for year: {year}";
});

app.MapGet("/files/{*filepath}", (string filepath) =>
{
    return $"File path: {filepath}";
});

app.MapGet("/search", (string? q, int page = 1) =>
{
    return $"Search query: {q}, Page: {page}";
});

app.MapGet("/store/{category}/{productID:int?}/{*extraPath}", (string category, int? productId, string? extraPath, bool inStock = true) =>
{
    return $"Category: {category}, Product ID: {productId}, Extra Path: {extraPath}, In Stock: {inStock}";
});

app.Run();

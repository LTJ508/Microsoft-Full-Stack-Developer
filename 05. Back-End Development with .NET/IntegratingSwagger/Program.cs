using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Swashbuckle.AspNetCore;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var blogs = new List<Blog>
{
    new Blog { Title = "My First Blog", Body = "This is my first post." },
    new Blog { Title = "My Second Blog", Body = "This is my second post." },
};

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "I am root!");

app.MapGet("/blogs", () =>
{
    return blogs;
});

app.MapGet("/blogs/{id}", Results<Ok<Blog>, NotFound>([Description("The ID of the blog to retrieve.")] int id) =>
{
    if (id < 0 || id >= blogs.Count)
        return TypedResults.NotFound();
    else
        return TypedResults.Ok(blogs[id]);
})
.WithSummary("Get a blog by ID")
.WithDescription("Return a single blog");

app.MapPost("/blogs", (Blog blog) =>
{
    blogs.Add(blog);
    return Results.Created($"/blogs/{blogs.Count - 1}", blog);
});

app.MapDelete("/blogs/{id}", (int id) =>
{
    if (id < 0 || id >= blogs.Count)
    {
        return Results.NotFound();
    }
    else
    {
        // var blog = blogs[id];
        blogs.RemoveAt(id);
        return Results.NoContent();
    }
});

app.MapPut("/blogs/{id}", (int id, Blog updatedBlog) =>
{
    if (id < 0 || id >= blogs.Count)
    {
        return Results.NotFound();
    }
    else
    {
        blogs[id] = updatedBlog;
        return Results.Ok(updatedBlog);
    }
});

app.Run();

public class Blog
{
    public required string Title { get; set; }
    public required string Body { get; set; }
}
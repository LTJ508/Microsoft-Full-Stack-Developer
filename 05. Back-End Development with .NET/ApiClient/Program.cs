// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;

using BlogApi;


// await new SwaggerClientGenerator().GenerateClient();

var httpClient = new HttpClient();
var apiBaseUrl = "http://localhost:5093";

var client = new BlogApiClient(apiBaseUrl, httpClient);

var blogs = await client.BlogsAllAsync();

foreach (var blogg in blogs)
{
    Console.WriteLine($"Title: {blogg.Title}");
}

await client.BlogsDELETEAsync(0);

var blog = new Blog
{
    Title = "New Blog",
    Body = "This is a new blog post."
};

await client.BlogsPOSTAsync(blog);

// var httpResults = await httpClient.GetAsync($"{apiBaseUrl}/blogs");

// if (httpResults.StatusCode != System.Net.HttpStatusCode.OK)
// {
//     Console.WriteLine("Failed to fetch blogs.");
//     return;
// }

// var blogStream = await httpResults.Content.ReadAsStreamAsync();

// var options = new System.Text.Json.JsonSerializerOptions
// {
//     PropertyNameCaseInsensitive = true
// };

// var blogs = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Blog>>(blogStream, options);

// if (blogs != null)
// {
//     foreach (var blog in blogs)
//     {
//         Console.WriteLine($"Title: {blog.Title}");
//     }
// }

// class Blog
// {
//     public required string Title { get; set; }
//     public required string Body { get; set; }
// }
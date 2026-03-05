using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "I am root!");

app.MapPost("/auto", (Person personFromClinet) =>
{
    return Results.Ok(personFromClinet);
});

app.MapPost("/json", async (HttpContext context) =>
{
    var person = await context.Request.ReadFromJsonAsync<Person>();
    return TypedResults.Json(person);
});

app.MapPost("/custom-options", async (HttpContext context) =>
{
    var options = new JsonSerializerOptions
    {
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };
    
    var person = await context.Request.ReadFromJsonAsync<Person>(options);
    return TypedResults.Json(person);
});

app.MapPost("/xml", async(HttpContext context) =>
{
    var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();

    var xmlSerilizer = new XmlSerializer(typeof(Person));
    var stringReader = new StringReader(body);

    var person = xmlSerilizer.Deserialize(stringReader);
    return TypedResults.Ok(person);
});

app.Run();

public class Person
{
    required public string UserName { get; set; }
    public int? UserAge { get; set; }
}
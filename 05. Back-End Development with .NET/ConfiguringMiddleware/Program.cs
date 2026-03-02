var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging((o)=>{});
var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine("Logic Before 1");
    await next.Invoke();
    Console.WriteLine("Logic After 1");
});

app.Use(async (context, next) =>
{
    Console.WriteLine("Logic Before 2");
    await next.Invoke();
    Console.WriteLine("Logic After 2");
});

app.Use(async (context, next) =>
{
    Console.WriteLine("Logic Before 3");
    await next.Invoke();
    Console.WriteLine("Logic After 3");
});

// app.UseHttpLogging();

app.MapGet("/", () => "Hello World!");
app.MapGet("/hello", () => "This is the hello route");
















app.Run();

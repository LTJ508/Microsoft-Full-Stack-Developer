var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMyService, MyService>();
// builder.Services.AddScoped<IMyService, MyService>();
// builder.Services.AddTransient<IMyService, MyService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var myService = context.RequestServices.GetRequiredService<IMyService>();
    myService.LogCreation("First Middleware");
    await next.Invoke();
});

app.Use(async (context, next) =>
{
    var myService = context.RequestServices.GetRequiredService<IMyService>();
    myService.LogCreation("Second Middleware");
    await next.Invoke();
});

app.MapGet("/", (IMyService myService) => {
    myService.LogCreation("Root");
    return Results.Ok("Check the console for the service creation logs");
});

app.Run();

public interface IMyService
{
    void LogCreation(string message);
}

public class MyService: IMyService
{
    private readonly int _serviceId;

    public MyService()
    {
        _serviceId = new Random().Next(100000, 999999);
    }

    public void LogCreation(string message)
    {
        Console.WriteLine($"Service ID: {_serviceId} - {message}");
    }
}
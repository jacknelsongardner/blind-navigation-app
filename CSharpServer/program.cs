using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        // Serve static files (HTML, JS)
        app.UseStaticFiles();

        // Handle GET request
        app.MapGet("/get", async context =>
        {
            var response = new { message = "Hello from GET request!" };
            await context.Response.WriteAsJsonAsync(response);
        });

        // Handle POST request
        app.MapPost("/submit", async context =>
        {
            var request = await JsonSerializer.DeserializeAsync<JsonElement>(context.Request.Body);
            string name = request.GetProperty("name").GetString();

            var response = new { message = $"Received POST data: {name}" };
            await context.Response.WriteAsJsonAsync(response);
        });

        // Run the application
        app.Run();
    }
}

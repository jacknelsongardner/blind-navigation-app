using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

class Program
{
    private static Dictionary<string, string> setup = new()
    {
        { "speed", "1" },
        { "language", "english" }
    };

    private static Dictionary<string, List<String>> favorites = new()
    {
        { "favorites", ["Room 101",
                    "Room 102",
                    "Room 103",
                    "Room 203"] }
    };

    private static Dictionary<string,  List<String>> instructions = new()
    {
        { "steps", ["Walk down the hall roughly five meters",
                    "Turn left and walk down the hall roughly twelve meters",
                    "Turn right and walk down the hall roughly five meters",
                    "Enter the door on your left"] }
    };

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add CORS to allow requests from anyone
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        // Use CORS
        app.UseCors();

        // Serve static files (HTML, JS)
        app.UseStaticFiles();

        // Endpoint to get setup information
        app.MapPost("/setup", async context =>
        {
            await context.Response.WriteAsJsonAsync(new { items = setup });
        });

        // Endpoint to set setup information
        app.MapPost("/set-setup", async context =>
        {
            try
            {
                var newSetup = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(context.Request.Body);
                setup = newSetup;
                await context.Response.WriteAsJsonAsync(new { status = "Setup updated successfully." });
            }
            catch
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsJsonAsync(new { error = "Invalid setup data." });
            }
        });

        // Endpoint to get navigation instructions
        app.MapPost("/navigate", async context =>
        {
            try
            {
                var request = await JsonSerializer.DeserializeAsync<JsonElement>(context.Request.Body);
                string destination = request.GetProperty("destination").GetString();

                var navigationInstructions = instructions;

                await context.Response.WriteAsJsonAsync(navigationInstructions);
            }
            catch
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsJsonAsync(new { error = "Invalid or missing destination parameter." });
            }
        });

        // Endpoint to get favorite destinations
        app.MapPost("/favorite-destinations", async context =>
        {
            var favoriteDestinations = favorites;
            await context.Response.WriteAsJsonAsync(favoriteDestinations);
        });

        // Run the application
        app.Run();
    }
}

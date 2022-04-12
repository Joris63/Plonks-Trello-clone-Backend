using Ocelot.DependencyInjection;
using Ocelot.Middleware;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("ocelot.json")
                            .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

builder.Services.AddOcelot(configuration);

var app = builder.Build();


app.UseCors("cors");

app.UseOcelot();

app.Run();

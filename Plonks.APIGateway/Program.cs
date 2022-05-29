using Ocelot.Middleware;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("ocelot.json")
                            .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000", "https://plonks.nl")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});


var ocelotConfig = builder.Environment.IsDevelopment() ? "ocelot.Development.json" : "ocelot.json";
builder.Configuration.AddJsonFile(ocelotConfig);

var app = builder.Build();


app.UseCors("cors");

app.UseOcelot().Wait();

app.Run();

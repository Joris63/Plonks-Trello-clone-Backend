using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plonks.Cards.Helpers;
using Plonks.Cards.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DB")));

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

builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IChecklistService, ChecklistService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddControllers();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserConsumer>();
    x.AddConsumer<ListConsumer>();

    x.AddBus(provider => Bus.Factory.CreateUsingAzureServiceBus(config =>
    {
        config.Host(configuration.GetConnectionString("ServiceBus"));

        config.ReceiveEndpoint("card.user", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<UserConsumer>(provider);
        });

        config.ReceiveEndpoint("card.list", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<ListConsumer>(provider);
        });
    }));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("cors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

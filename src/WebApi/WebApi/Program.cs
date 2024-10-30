using DAL.Helpers;
using Dependencies.Registration;
using Common.Database;
using DAL.Database;
using Microsoft.EntityFrameworkCore;
using DAL.Database.Migrations;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

DependencyRegistration.RegisterAllDependencies(
    builder.Services,
    builder.Configuration,
    nameof(WebApi));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                builder.Configuration.GetAllowedOrigins(
                    )).AllowAnyHeader(
                ).AllowAnyMethod(
                ).AllowCredentials();
        });
});

builder.Services.AddControllers();

var app = builder.Build();

app.MigrateDatabase<AppDbContext>();

await app.AddInitialDataIfReq(
    app.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

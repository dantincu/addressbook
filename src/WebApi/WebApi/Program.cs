using DAL.Helpers;
using Dependencies.Registration;
using Common.Database;
using DAL.Database;
using Microsoft.EntityFrameworkCore;
using DAL.Database.Migrations;
using System.Text.Json.Serialization;

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

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
});

var app = builder.Build();

app.MigrateDatabase<AppDbContext>();

await app.AddInitialDataIfReq(
    app.Configuration);

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

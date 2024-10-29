using DAL.Helpers;
using Dependencies.Registration;
using Common.Database;
using DAL.Database;
using Microsoft.EntityFrameworkCore;
using DAL.Database.Migrations;

var builder = WebApplication.CreateBuilder(args);

DependencyRegistration.RegisterAllDependencies(
    builder.Services,
    builder.Configuration,
    nameof(WebApi));

// var conn = ConnectionStrings.CreateConnectionString();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// var workDir = Directory.GetCurrentDirectory();

app.MigrateDatabase<AppDbContext>();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

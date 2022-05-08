using System.Reflection;
using Npgsql;
using CorePGIdentityTest.Models;
using CorePGIdentityTest.Data;
using Microsoft.EntityFrameworkCore;
using CorePGIdentityTest.Services;
using CorePGIdentityTest.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<AppSettings>(true);
builder.Configuration.AddEnvironmentVariables();

// order of config is
// 1. appsettings
// 2. secrets
// 3. env variables

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("Default");

var conStrBuilder = new NpgsqlConnectionStringBuilder(connStr);

conStrBuilder.Password = builder.Configuration["DbPassword"];
var connection = conStrBuilder.ConnectionString;

builder.Services.AddDbContext<ApiDbContext>(options =>
    options
    .UseNpgsql(connection)
    .UseSnakeCaseNamingConvention());

//builder.Services.AddIdentity<ApplicationUser, IdentityRole<long>>()
//    .AddEntityFrameworkStores<ApiDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApiDbContext>();

builder.Configuration.AddEnvironmentVariables();
var pw = builder.Configuration["DbPassword"];
var secret1a = builder.Configuration["TestSecret1"];
var secret1 = builder.Configuration["AppSettings:TestSecret1"];
var secret2 = builder.Configuration["AppSettings:TestSecret2"];

var appSettings =
    builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

var app = builder.Build();
DatabaseManagementService.MigrationInitialisation(app);
DatabaseManagementService.SeedData(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


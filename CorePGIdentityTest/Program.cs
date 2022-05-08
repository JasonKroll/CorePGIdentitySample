using System.Reflection;
using Npgsql;
using CorePGIdentityTest.Models;
using CorePGIdentityTest.Data;
using Microsoft.EntityFrameworkCore;
using CorePGIdentityTest.Services;
using CorePGIdentityTest.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using FirebaseAdmin;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using System.Security.Claims;
using CorePGIdentityTest.Extensions;

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

//FirebaseApp.Create(new AppOptions
//{
//    Credential = GoogleCredential.GetApplicationDefault()
//});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // none of these work
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
    //options.ClaimsIdentity.UserIdClaimType = "sub";
    // add "|" to allowed characters as auth0 uses that as a separator
    // i.e    "{provider}|{randomcharacters}"
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+|";

})
.AddEntityFrameworkStores<ApiDbContext>();

var jwtOptions = new JwtOptions();
builder.Configuration.GetSection("Jwt:Firebase").Bind(jwtOptions);


// Add Firebase auth
builder.Services.AddFirebaseAuth(jwtOptions);


var connStr = builder.Configuration.GetConnectionString("Default");
var conStrBuilder = new NpgsqlConnectionStringBuilder(connStr);
conStrBuilder.Password = builder.Configuration["DbPassword"];
var connection = conStrBuilder.ConnectionString;

builder.Services.AddDbContext<ApiDbContext>(options =>
    options
    .UseNpgsql(connection)
    .UseSnakeCaseNamingConvention());


builder.Configuration.AddEnvironmentVariables();
var pw = builder.Configuration["DbPassword"];
var secret1a = builder.Configuration["TestSecret1"];
var secret1 = builder.Configuration["AppSettings:TestSecret1"];
var secret2 = builder.Configuration["AppSettings:TestSecret2"];

var appSettings =
    builder.Configuration.GetSection("AppSettings").Get<AppSettings>();

builder.Services.AddTransient<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    DatabaseManagementService.MigrationInitialisation(app);
    DatabaseManagementService.SeedData(app);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


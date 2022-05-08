using System;
using System.Collections.Generic;
using System.Linq;
using CorePGIdentityTest.Data;
using CorePGIdentityTest.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CorePGIdentityTest.Services
{
    public static class DatabaseManagementService
    {
        public static void MigrationInitialisation(IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            // Takes all of our migrations files and apply them against the database in case they are not implemented
            serviceScope?.ServiceProvider?.GetService<ApiDbContext>()?.Database.Migrate();
        }

        public static void SeedData(IApplicationBuilder app)
        {
            List<WeatherForecast> forecasts = new List<WeatherForecast>()
            {
                new WeatherForecast() { Id = 1, Date = DateTime.UtcNow, Summary = "Cool", TemperatureC = 20 },
                new WeatherForecast() { Id = 2, Date = DateTime.UtcNow, Summary = "Warm", TemperatureC = 25 },
                new WeatherForecast() { Id = 3, Date = DateTime.UtcNow, Summary = "Hot", TemperatureC = 30 }
            };

            using var serviceScope = app.ApplicationServices.CreateScope();
            // Takes all of our migrations files and apply them against the database in case they are not implemented
            ApiDbContext? context = serviceScope.ServiceProvider.GetService<ApiDbContext>();
            foreach (var forcast in forecasts)
            {
                if (!context.WeatherForecasts.Any(x => x.Id == forcast.Id))
                    context.WeatherForecasts.Add(forcast);
            }
            context?.SaveChanges();
        }
    }
}

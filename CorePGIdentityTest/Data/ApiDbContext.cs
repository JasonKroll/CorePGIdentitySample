using System;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CorePGIdentityTest.Entities;

namespace CorePGIdentityTest.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseIdentityColumns();
            //    List<WeatherForecast> forecasts = new List<WeatherForecast>()
            //    {
            //        new WeatherForecast() { Id = 1, Date = DateTime.Now, Summary = "Cool", TemperatureC = 20 },
            //        new WeatherForecast() { Id = 2, Date = DateTime.Now, Summary = "Warm", TemperatureC = 25 },
            //        new WeatherForecast() { Id = 3, Date = DateTime.Now, Summary = "Hot", TemperatureC = 30 }
            //    };

            //    modelBuilder.Entity<WeatherForecast>().HasData(forecasts.ToArray());
            modelBuilder.Entity<WeatherForecast>()
                   .HasIndex(b => b.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}


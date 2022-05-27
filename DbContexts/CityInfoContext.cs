using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions options) : 
            base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description = "The one with that big park."
                },
                new City("Antwerp")
                {
                    Id = 2,
                    Description = "The one with catherdral."
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "The one with Eifel Tower."
                });

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Central Park")
                {
                    Id=1,
                    CityId = 1,
                    Description = "Most vistited park in US."
                },
                new PointOfInterest("Empire State Building")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "Gothic Cathedral."
                },
                new PointOfInterest("Catherdral")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "Most vistited park in US."
                },
                new PointOfInterest("Eiffel Tower")
                {
                    Id = 4,
                    CityId = 3,
                    Description = "Wrought iron tower."
                },
                new PointOfInterest("Louvre")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "Largest museum."
                });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointOfInterest { get; set; } = null!;
    }
}

using cars.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace cars.Persistence
{
    public class CarsDbContext : DbContext
    {
        // dbsets
        // Cars DbSet
        public DbSet<Car> Cars { get; set; }
        // Make DBSet
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        // Feature DBSet
        public DbSet<Feature> Features { get; set; }

        // pluralizing the table name
        public DbSet<Photo> Photos { get; set; }
        // constructor that takes a connection and pass it to the base class
        // dbcontext will use System.Configuration.ConfigurationManager to look into web.config
        // Under connections strings element then it will look for a connection string(connectionString)
        // This has changed in EntityFramework core
        public CarsDbContext(DbContextOptions<CarsDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<CarFeature>().HasKey(cf =>
                new { cf.CarId, cf.FeatureId });
        }

    }
}
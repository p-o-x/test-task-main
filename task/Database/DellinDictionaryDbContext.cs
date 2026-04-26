using Microsoft.EntityFrameworkCore;
using System.Reflection;
using task.Models;

namespace task.Database
{
    public class DellinDictionaryDbContext : DbContext
    {
        public DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Office> Offices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Office>().OwnsOne(office => office.Coordinates, coordinates =>
            {
                coordinates.Property(c => c.Latitude).HasColumnName("Latitude");
                coordinates.Property(c => c.Longitude).HasColumnName("Longitude");
            });
            builder.Entity<Office>().HasOne(office => office.Phones).WithOne(phone => phone.Office);
            builder.Entity<Office>().HasIndex(office => office.Id).IsUnique();
            builder.Entity<Office>().HasIndex(office => office.AddressCity);
            builder.Entity<Office>().HasIndex(office => office.AddressRegion);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

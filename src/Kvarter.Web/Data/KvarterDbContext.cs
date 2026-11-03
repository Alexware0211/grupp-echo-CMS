using Kvarter.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kvarter.Web.Data;

public class KvarterDbContext : DbContext
{
    public KvarterDbContext(DbContextOptions<KvarterDbContext> options)
        : base(options)
    {
    }

    public DbSet<NewsletterSubscriber> NewsletterSubscribers => Set<NewsletterSubscriber>();

    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewsletterSubscriber>(e =>
        {
            e.ToTable("kvarterNewsletterSubscriber");
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).HasMaxLength(256).IsRequired();
            e.Property(x => x.Culture).HasMaxLength(10).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<ContactMessage>(e =>
        {
            e.ToTable("kvarterContactMessage");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasMaxLength(256).IsRequired();
            e.Property(x => x.Subject).HasMaxLength(200).IsRequired();
            e.Property(x => x.Message).HasMaxLength(4000).IsRequired();
            e.Property(x => x.Culture).HasMaxLength(10).IsRequired();
            e.HasIndex(x => x.CreatedAtUtc);
        });

        modelBuilder.Entity<WeatherForecast>(e =>
        {
            e.ToTable("kvarterWeatherForecast");
            e.HasKey(x => x.Id);
            e.Property(x => x.LocationKey).HasMaxLength(100).IsRequired();
            e.HasIndex(x => new { x.LocationKey, x.Date }).IsUnique();
        });
    }
}

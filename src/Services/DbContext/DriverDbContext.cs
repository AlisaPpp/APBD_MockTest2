using Microsoft.EntityFrameworkCore;
using Models;

namespace Services.DbContext;

public partial class DriverDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DriverDbContext() { }
    public DriverDbContext(DbContextOptions<DriverDbContext> options) : base(options) { }
    
    public virtual DbSet<CarManufacturer> CarManufacturers { get; set; }
    public virtual DbSet<Car> Cars { get; set; }
    public virtual DbSet<Driver> Drivers { get; set; }
    public virtual DbSet<DriverCompetition> DriverCompetitions { get; set; }
    public virtual DbSet<Competition> Competitions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarManufacturer>(entity =>
        {
            entity.ToTable("CarManufacturer");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired();
        });
        
        modelBuilder.Entity<Car>(entity =>
        {
            entity.ToTable("Car");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.CarManufacturerId)
                .IsRequired();
            
            entity.HasOne(d => d.CarManufacturer)
                .WithMany()
                .HasForeignKey(d => d.CarManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.ModelName)
                .HasMaxLength(200)
                .IsRequired();
            
            entity.Property(e => e.Number)
                .IsRequired();
        });
        
        modelBuilder.Entity<Driver>(entity =>
        {
            entity.ToTable("Driver");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .IsRequired();
            
            entity.Property(e => e.Birthday)
                .IsRequired();
            
            entity.Property(e => e.CarId)
                .IsRequired();
            
            entity.HasOne(d => d.Car)
                .WithMany()
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Competition>(entity =>
        {
            entity.ToTable("Competition");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired();
            
            
        });
        
        modelBuilder.Entity<DriverCompetition>(entity =>
        {
            entity.ToTable("DriverCompetition");
            
            entity.HasKey(e => new { e.DriverId, e.CompetitionId });

            entity.Property(e => e.Date)
                .IsRequired();

            entity.Property(e => e.DriverId)
                .IsRequired();
            
            entity.Property(e => e.CompetitionId)
                .IsRequired();
            
            entity.HasOne(d => d.Driver)
                .WithMany(p => p.DriverCompetitions)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            
            entity.HasOne(d => d.Competition)
                .WithMany(p => p.DriverCompetitions)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
    
}
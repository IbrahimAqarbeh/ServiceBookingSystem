using Microsoft.EntityFrameworkCore;

namespace Service_Booking_System.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Users> Users { get; set; }
    public DbSet<Services> Services { get; set; }
    public DbSet<Bookings> Bookings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Users>()
            .Property(u => u.Discriminator)
            .HasDefaultValue("User");
        
        modelBuilder.Entity<Users>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<Users>()
            .Property(u => u.UserId)
            .HasDefaultValueSql("uuid_generate_v4()");
        
        modelBuilder.Entity<Services>()
            .Property(s => s.ServiceId)
            .HasDefaultValueSql("uuid_generate_v4()");

        modelBuilder.Entity<Bookings>()
            .Property(b => b.BookingId)
            .HasDefaultValueSql("uuid_generate_v4()");
    }
}
 

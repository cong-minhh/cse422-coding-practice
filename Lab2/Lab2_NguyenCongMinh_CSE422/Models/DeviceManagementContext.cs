using Microsoft.EntityFrameworkCore;

namespace Lab2_NguyenCongMinh_CSE422.Models
{
    public class DeviceManagementContext : DbContext
    {
        public DeviceManagementContext(DbContextOptions<DeviceManagementContext> options) : base(options) { }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceCategory> DeviceCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure table names explicitly
            modelBuilder.Entity<Device>().ToTable("Device");
            modelBuilder.Entity<DeviceCategory>().ToTable("DeviceCategory");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            
            // User-Role many-to-many relationship
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // User-Device one-to-many relationship
            modelBuilder.Entity<Device>()
                .HasOne(d => d.User)
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.UserId)
                .IsRequired(false);  // Make UserId nullable

            // Device-Category relationship
            modelBuilder.Entity<Device>()
                .HasOne(d => d.Category)
                .WithMany(c => c.Devices)
                .HasForeignKey(d => d.DeviceCategoryId);

            // Configure indexes
            modelBuilder.Entity<Device>()
                .HasIndex(d => d.Code)
                .IsUnique();

            // Configure cascade delete behavior
            modelBuilder.Entity<Device>()
                .HasOne(d => d.Category)
                .WithMany(c => c.Devices)
                .HasForeignKey(d => d.DeviceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

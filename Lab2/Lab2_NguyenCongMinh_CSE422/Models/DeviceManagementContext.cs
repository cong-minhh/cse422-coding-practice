using Microsoft.EntityFrameworkCore;

namespace Lab2_NguyenCongMinh_CSE422.Models
{
    public class DeviceManagementContext : DbContext
    {
        public DeviceManagementContext(DbContextOptions<DeviceManagementContext> options) : base(options) { }
        public DbSet<Device> Device { get; set; }
        public DbSet<DeviceCategory> DeviceCategory { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
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
                .HasForeignKey(d => d.UserId);
        }
    }
}

using Lab2_NguyenCongMinh_CSE422.Models;
using Lab2_NguyenCongMinh_CSE422.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Lab2_NguyenCongMinh_CSE422.Data
{
    public static class DbSeeder
    {
        public static void Seed(DeviceManagementContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();
            
            // Apply any pending migrations
            context.Database.Migrate();

            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "Manager" },
                    new Role { Name = "User" }
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            if (!context.DeviceCategories.Any())
            {
                var categories = new List<DeviceCategory>
                {
                    new DeviceCategory { Name = "Máy Tính Xách Tay" },
                    new DeviceCategory { Name = "Thiết Bị Mạng"},
                    new DeviceCategory { Name = "Thiết Bị Văn Phòng"},
                    new DeviceCategory { Name = "Thiết Bị Di Động" },
                    new DeviceCategory { Name = "Thiết Bị Âm Thanh" }
                };
                context.DeviceCategories.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { FullName = "Nguyễn Thành Đạt", Email = "dat.nguyen@techcorp.vn", PhoneNumber = "0912345678" },
                    new User { FullName = "Trần Thị Minh Anh", Email = "minhanh.tran@viettech.com", PhoneNumber = "0987654321" },
                    new User { FullName = "Phạm Xuân Hoàng", Email = "hoang.pham@saigontech.vn", PhoneNumber = "0909123456" },
                    new User { FullName = "Lê Thị Thanh Hà", Email = "ha.le@hanoisoft.vn", PhoneNumber = "0898765432" },
                    new User { FullName = "Vũ Đức Minh", Email = "minh.vu@vntech.com", PhoneNumber = "0976543210" }
                };
                context.Users.AddRange(users);
                context.SaveChanges();

                // Assign roles to users
                var adminRole = context.Roles.First(r => r.Name == "Admin");
                var managerRole = context.Roles.First(r => r.Name == "Manager");
                var userRole = context.Roles.First(r => r.Name == "User");

                var userRoles = new List<UserRole>
                {
                    new UserRole { UserId = users[0].Id, RoleId = adminRole.Id },
                    new UserRole { UserId = users[0].Id, RoleId = managerRole.Id },
                    new UserRole { UserId = users[1].Id, RoleId = managerRole.Id },
                    new UserRole { UserId = users[2].Id, RoleId = userRole.Id },
                    new UserRole { UserId = users[3].Id, RoleId = userRole.Id },
                    new UserRole { UserId = users[4].Id, RoleId = userRole.Id }
                };
                context.UserRoles.AddRange(userRoles);
                context.SaveChanges();
            }

            if (!context.Devices.Any())
            {
                var laptopCategory = context.DeviceCategories.First(c => c.Name == "Máy Tính Xách Tay");
                var networkCategory = context.DeviceCategories.First(c => c.Name == "Thiết Bị Mạng");
                var officeCategory = context.DeviceCategories.First(c => c.Name == "Thiết Bị Văn Phòng");
                var users = context.Users.ToList();

                var devices = new List<Device>
                {
                    new Device 
                    { 
                        Name = "Laptop Dell XPS 9570 - Phòng Kỹ Thuật",
                        Code = "LP-DELL-9570-KT",
                        DeviceCategoryId = laptopCategory.Id,
                        Status = Models.DeviceStatus.InUse,
                        DateOfEntry = DateTime.Now,
                        UserId = users[0].Id
                    },
                    new Device 
                    { 
                        Name = "Router Cisco Meraki MX250 - Tầng 5",
                        Code = "NET-CISCO-MX250-T5",
                        DeviceCategoryId = networkCategory.Id,
                        Status = Models.DeviceStatus.InUse,
                        DateOfEntry = DateTime.Now,
                        UserId = users[1].Id
                    },
                    new Device 
                    { 
                        Name = "Máy In HP LaserJet Pro - Văn Thư",
                        Code = "PRN-HP-LJ-VT",
                        DeviceCategoryId = officeCategory.Id,
                        Status = Models.DeviceStatus.UnderMaintenance,
                        DateOfEntry = DateTime.Now,
                        UserId = users[2].Id
                    },
                    // Multiple devices for one user
                    new Device 
                    { 
                        Name = "MacBook Pro M2 - Phòng Thiết Kế",
                        Code = "LP-MAC-M2-TK",
                        DeviceCategoryId = laptopCategory.Id,
                        Status = Models.DeviceStatus.InUse,
                        DateOfEntry = DateTime.Now,
                        UserId = users[0].Id
                    },
                    new Device 
                    { 
                        Name = "Switch HP Aruba 2930F - Phòng Server",
                        Code = "NET-HP-2930F-SV",
                        DeviceCategoryId = networkCategory.Id,
                        Status = Models.DeviceStatus.Broken,
                        DateOfEntry = DateTime.Now,
                        UserId = users[0].Id
                    }
                };
                context.Devices.AddRange(devices);
                context.SaveChanges();
            }
        }
    }
}

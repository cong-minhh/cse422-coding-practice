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

            // Only seed if tables are empty
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "Admin"},
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
                    new DeviceCategory { Name = "Máy Tính Xách Tay", Description = "Laptop và máy tính xách tay cao cấp" },
                    new DeviceCategory { Name = "Thiết Bị Mạng", Description = "Router, Switch và các thiết bị mạng" },
                    new DeviceCategory { Name = "Thiết Bị Văn Phòng", Description = "Máy in, máy scan và các thiết bị văn phòng" },
                    new DeviceCategory { Name = "Thiết Bị Di Động", Description = "Điện thoại, máy tính bảng và phụ kiện" },
                    new DeviceCategory { Name = "Thiết Bị Âm Thanh", Description = "Loa, tai nghe và thiết bị âm thanh chuyên nghiệp" },
                    new DeviceCategory { Name = "Màn Hình", Description = "Màn hình máy tính, màn hình gaming" },
                    new DeviceCategory { Name = "Thiết Bị Lưu Trữ", Description = "Ổ cứng, USB, thiết bị lưu trữ" },
                    new DeviceCategory { Name = "Phụ Kiện", Description = "Chuột, bàn phím, webcam" }
                };
                context.DeviceCategories.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { FullName = "Nguyễn Văn An", Email = "nguyenvanan@example.com", PhoneNumber = "0901234567" },
                    new User { FullName = "Trần Thị Bình", Email = "tranthiminh@example.com", PhoneNumber = "0912345678" },
                    new User { FullName = "Lê Hoàng Cường", Email = "lehoangcuong@example.com", PhoneNumber = "0923456789" },
                    new User { FullName = "Phạm Thị Dung", Email = "phamthidung@example.com", PhoneNumber = "0934567890" },
                    new User { FullName = "Hoàng Văn Em", Email = "hoangvanem@example.com", PhoneNumber = "0945678901" },
                    new User { FullName = "Vũ Thị Phương", Email = "vuthiphuong@example.com", PhoneNumber = "0956789012" },
                    new User { FullName = "Đặng Văn Giang", Email = "dangvangiang@example.com", PhoneNumber = "0967890123" },
                    new User { FullName = "Bùi Thị Hoa", Email = "buithihoa@example.com", PhoneNumber = "0978901234" },
                    new User { FullName = "Ngô Văn Inh", Email = "ngovanin@example.com", PhoneNumber = "0989012345" },
                    new User { FullName = "Mai Thị Kim", Email = "maithikim@example.com", PhoneNumber = "0990123456" }
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
                    new UserRole { UserId = users[1].Id, RoleId = managerRole.Id },
                    new UserRole { UserId = users[2].Id, RoleId = managerRole.Id },
                    new UserRole { UserId = users[3].Id, RoleId = userRole.Id },
                    new UserRole { UserId = users[4].Id, RoleId = userRole.Id },
                    new UserRole { UserId = users[5].Id, RoleId = userRole.Id },
                    new UserRole { UserId = users[6].Id, RoleId = userRole.Id },
                    new UserRole { UserId = users[7].Id, RoleId = userRole.Id },
                    new UserRole { UserId = users[8].Id, RoleId = userRole.Id },
                    new UserRole { UserId = users[9].Id, RoleId = userRole.Id }
                };
                context.UserRoles.AddRange(userRoles);
                context.SaveChanges();
            }

            if (!context.Devices.Any())
            {
                var random = new Random();
                var users = context.Users.ToList();
                var categories = context.DeviceCategories.ToList();
                var devices = new List<Device>();

                // List of device names for each category
                var laptops = new[] { "Dell XPS 13", "MacBook Pro", "Lenovo ThinkPad", "HP Spectre", "Asus ZenBook" };
                var networkDevices = new[] { "Cisco Switch", "TP-Link Router", "Netgear Access Point", "D-Link Modem", "Ubiquiti UniFi" };
                var officeDevices = new[] { "HP LaserJet", "Canon Scanner", "Epson Printer", "Brother Fax", "Xerox Copier" };
                var mobileDevices = new[] { "iPhone 15", "Samsung Galaxy", "iPad Pro", "Xiaomi Mi Pad", "Surface Pro" };
                var audioDevices = new[] { "Sony WH-1000XM4", "Bose QuietComfort", "JBL Speaker", "AirPods Pro", "Jabra Elite" };
                var monitors = new[] { "Dell Ultrasharp", "LG UltraGear", "Samsung Odyssey", "ASUS ProArt", "BenQ Designer" };
                var storage = new[] { "Samsung SSD", "WD Black", "Seagate Barracuda", "Crucial MX500", "Kingston SSD" };
                var accessories = new[] { "Logitech MX Master", "Keychron K2", "Logitech C920", "Razer DeathAdder", "Microsoft Ergonomic" };

                // Helper function to generate devices for a category
                void AddDevicesForCategory(DeviceCategory category, string[] deviceNames, int count)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var deviceName = deviceNames[random.Next(deviceNames.Length)];
                        var status = (Models.DeviceStatus)random.Next(3); // Random status
                        var assignedUser = status == Models.DeviceStatus.InUse ? users[random.Next(users.Count)] : null;

                        devices.Add(new Device
                        {
                            Name = deviceName,
                            Code = $"{category.Name.Substring(0, 2).ToUpper()}{random.Next(1000, 9999)}",
                            DeviceCategoryId = category.Id,
                            Status = status,
                            UserId = assignedUser?.Id,
                            DateOfEntry = DateTime.Now.AddDays(-random.Next(1, 365))
                        });
                    }
                }

                // Add devices for each category
                foreach (var category in categories)
                {
                    string[] deviceNames = category.Name switch
                    {
                        "Máy Tính Xách Tay" => laptops,
                        "Thiết Bị Mạng" => networkDevices,
                        "Thiết Bị Văn Phòng" => officeDevices,
                        "Thiết Bị Di Động" => mobileDevices,
                        "Thiết Bị Âm Thanh" => audioDevices,
                        "Màn Hình" => monitors,
                        "Thiết Bị Lưu Trữ" => storage,
                        _ => accessories
                    };

                    AddDevicesForCategory(category, deviceNames, random.Next(3, 8));
                }

                context.Devices.AddRange(devices);
                context.SaveChanges();
            }
        }
    }
}

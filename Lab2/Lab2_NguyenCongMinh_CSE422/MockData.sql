IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DeviceManagementDb')
BEGIN
    CREATE DATABASE DeviceManagementDb;
END
GO

USE DeviceManagementDb;
GO

-- Create tables if they don't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Role')
CREATE TABLE [Role] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(255)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'User')
CREATE TABLE [User] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [FullName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL UNIQUE,
    [PhoneNumber] NVARCHAR(20) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserRole')
CREATE TABLE [UserRole] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT PK_UserRole PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT FK_UserRole_User FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
    CONSTRAINT FK_UserRole_Role FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id])
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DeviceCategory')
CREATE TABLE [DeviceCategory] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(255)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Device')
CREATE TABLE [Device] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Code] NVARCHAR(50) NOT NULL UNIQUE,
    [DeviceCategoryId] INT NOT NULL,
    [Status] INT NOT NULL,
    [DateOfEntry] DATETIME NOT NULL,
    [UserId] INT,
    CONSTRAINT FK_Device_DeviceCategory FOREIGN KEY ([DeviceCategoryId]) 
        REFERENCES [DeviceCategory]([Id]) ON DELETE NO ACTION,
    CONSTRAINT FK_Device_User FOREIGN KEY ([UserId]) 
        REFERENCES [User]([Id]) ON DELETE NO ACTION
);

-- Clear existing data
DELETE FROM [UserRole];
DELETE FROM [Device];
DELETE FROM [DeviceCategory];
DELETE FROM [User];
DELETE FROM [Role];

-- Reset identity columns
DBCC CHECKIDENT ('[Device]', RESEED, 0);
DBCC CHECKIDENT ('[DeviceCategory]', RESEED, 0);
DBCC CHECKIDENT ('[User]', RESEED, 0);
DBCC CHECKIDENT ('[Role]', RESEED, 0);

-- Insert Device Categories
INSERT INTO [DeviceCategory] ([Name], [Description]) VALUES
(N'Máy Tính Xách Tay', N'Laptop và máy tính xách tay cao cấp'),
(N'Thiết Bị Mạng', N'Router, Switch và các thiết bị mạng'),
(N'Thiết Bị Văn Phòng', N'Máy in, máy scan và các thiết bị văn phòng'),
(N'Thiết Bị Di Động', N'Điện thoại, máy tính bảng và phụ kiện'),
(N'Thiết Bị Âm Thanh', N'Loa, tai nghe và thiết bị âm thanh chuyên nghiệp');

-- Insert Users with Vietnamese names
INSERT INTO [User] ([FullName], [Email], [PhoneNumber]) VALUES
(N'Nguyễn Thành Đạt', 'dat.nguyen@techcorp.vn', '0912345678'),
(N'Trần Thị Minh Anh', 'minhanh.tran@viettech.com', '0987654321'),
(N'Phạm Xuân Hoàng', 'hoang.pham@saigontech.vn', '0909123456'),
(N'Lê Thị Thanh Hà', 'ha.le@hanoisoft.vn', '0898765432'),
(N'Vũ Đức Minh', 'minh.vu@vntech.com', '0976543210'),
(N'Đặng Thu Thảo', 'thao.dang@saigonit.vn', '0923456789'),
(N'Hoàng Minh Tuấn', 'tuan.hoang@vietsoft.com', '0934567890'),
(N'Bùi Thị Hồng Nhung', 'nhung.bui@techviet.vn', '0945678901'),
(N'Đỗ Quang Hải', 'hai.do@vnware.com', '0956789012'),
(N'Ngô Thị Mai Linh', 'linh.ngo@saigontech.vn', '0967890123');

-- Insert Devices with specific Vietnamese context
INSERT INTO [Device] ([Name], [Code], [DeviceCategoryId], [Status], [DateOfEntry], [UserId]) VALUES
-- Laptops
(N'Laptop Dell XPS 9570 - Phòng Kỹ Thuật', 'LP-DELL-9570-KT', 1, 1, '2024-01-10', 1),
(N'MacBook Pro M2 - Phòng Thiết Kế', 'LP-MAC-M2-TK', 1, 1, '2024-01-11', 2),
(N'ThinkPad X1 Carbon - Phòng Giám Đốc', 'LP-TPX1-GD', 1, 1, '2024-01-12', 3),

-- Network Devices
(N'Router Cisco Meraki MX250 - Tầng 5', 'NET-CISCO-MX250-T5', 2, 1, '2024-01-13', 4),
(N'Switch HP Aruba 2930F - Phòng Server', 'NET-HP-2930F-SV', 2, 1, '2024-01-14', 5),
(N'Access Point Ubiquiti UniFi - Khu A', 'NET-UBI-UAP-KA', 2, 1, '2024-01-15', 1),

-- Office Equipment
(N'Máy In HP LaserJet Pro - Văn Thư', 'PRN-HP-LJ-VT', 3, 1, '2024-01-16', 6),
(N'Máy Scan Epson DS-870 - Kế Toán', 'SCN-EPS-870-KT', 3, 1, '2024-01-17', 7),
(N'Máy Photocopy Canon - Hành Chính', 'CPY-CAN-HC', 3, 2, '2024-01-18', 8),

-- Mobile Devices
(N'iPad Pro 12.9 - Phòng Họp', 'MOB-IPAD-PH', 4, 1, '2024-01-19', 9),
(N'Samsung Galaxy S23 Ultra - Kinh Doanh', 'MOB-SS-S23U-KD', 4, 1, '2024-01-20', 10),
(N'iPhone 15 Pro Max - Giám Đốc', 'MOB-IPH-15PM-GD', 4, 1, '2024-01-21', 3),

-- Audio Equipment
(N'Loa Hội Nghị Poly - Phòng Họp A', 'AUD-POLY-PHA', 5, 1, '2024-01-22', 1),
(N'Micro Shure SM58 - Phòng Training', 'AUD-SHR-SM58-PT', 5, 1, '2024-01-23', 2),
(N'Tai Nghe Sony WH-1000XM5 - IT', 'AUD-SNY-WH5-IT', 5, 1, '2024-01-24', 4);

-- Insert Roles
INSERT INTO [Role] ([Name], [Description]) VALUES
('Admin', N'Quản trị hệ thống'),
('Manager', N'Quản lý thiết bị'),
('User', N'Người dùng thông thường');

-- Assign Roles to Users
INSERT INTO [UserRole] ([UserId], [RoleId]) VALUES
(1, 1), -- Nguyễn Thành Đạt - Admin
(1, 2), -- Nguyễn Thành Đạt - Manager
(2, 2), -- Trần Thị Minh Anh - Manager
(3, 2), -- Phạm Xuân Hoàng - Manager
(4, 3), -- Lê Thị Thanh Hà - User
(5, 3), -- Vũ Đức Minh - User
(6, 3), -- Đặng Thu Thảo - User
(7, 3), -- Hoàng Minh Tuấn - User
(8, 3), -- Bùi Thị Hồng Nhung - User
(9, 3), -- Đỗ Quang Hải - User
(10, 3); -- Ngô Thị Mai Linh - User

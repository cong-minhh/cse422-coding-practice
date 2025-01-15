-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DeviceManagementDb')
BEGIN
    CREATE DATABASE DeviceManagementDb;
END
GO

USE DeviceManagementDb;
GO

-- Create Tables
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Role')
CREATE TABLE Role (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'User')
CREATE TABLE [User] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserRole')
CREATE TABLE UserRole (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    CONSTRAINT PK_UserRole PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRole_User FOREIGN KEY (UserId) REFERENCES [User](Id),
    CONSTRAINT FK_UserRole_Role FOREIGN KEY (RoleId) REFERENCES Role(Id)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DeviceCategory')
CREATE TABLE DeviceCategory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(500) NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Device')
CREATE TABLE Device (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Code NVARCHAR(50) NOT NULL,
    DeviceCategoryId INT NOT NULL,
    Status INT NOT NULL,
    DateOfEntry DATETIME NOT NULL,
    UserId INT NULL,
    CONSTRAINT FK_Device_DeviceCategory FOREIGN KEY (DeviceCategoryId) REFERENCES DeviceCategory(Id),
    CONSTRAINT FK_Device_User FOREIGN KEY (UserId) REFERENCES [User](Id)
);
GO

-- Insert Mock Data
-- Clear existing data
DELETE FROM Device;
DELETE FROM UserRole;
DELETE FROM [User];
DELETE FROM Role;
DELETE FROM DeviceCategory;
GO

-- Insert Roles
INSERT INTO Role (Name) VALUES 
('Admin'),
('User'),
('Technician');
GO

-- Insert Users
INSERT INTO [User] (FullName, Email, PhoneNumber) VALUES 
('John Doe', 'john.doe@example.com', '123-456-7890'),
('Jane Smith', 'jane.smith@example.com', '234-567-8901'),
('Bob Johnson', 'bob.johnson@example.com', '345-678-9012'),
('Alice Brown', 'alice.brown@example.com', '456-789-0123');
GO

-- Insert UserRoles
INSERT INTO UserRole (UserId, RoleId) VALUES 
(1, 1), -- John Doe is Admin
(2, 2), -- Jane Smith is User
(3, 3), -- Bob Johnson is Technician
(4, 2); -- Alice Brown is User
GO

-- Insert Device Categories
INSERT INTO DeviceCategory (Name, Description) VALUES 
('Laptop', 'Portable computers for work and productivity'),
('Desktop', 'Stationary computers for office use'),
('Printer', 'Network and local printers'),
('Phone', 'Mobile phones and smartphones'),
('Network Equipment', 'Routers, switches, and networking devices');
GO

-- Insert Devices
INSERT INTO Device (Name, Code, DeviceCategoryId, Status, DateOfEntry, UserId) VALUES 
('Dell XPS 13', 'LAP-001', 1, 1, '2024-01-01', 1),  -- Laptop assigned to John
('HP EliteDesk', 'DSK-001', 2, 1, '2024-01-02', 2), -- Desktop assigned to Jane
('Canon MF445dw', 'PRN-001', 3, 2, '2024-01-03', NULL), -- Printer (broken)
('iPhone 13', 'PHN-001', 4, 1, '2024-01-04', 3),    -- Phone assigned to Bob
('Cisco Switch', 'NET-001', 5, 3, '2024-01-05', NULL), -- Network (maintenance)
('ThinkPad X1', 'LAP-002', 1, 1, '2024-01-06', 4),  -- Laptop assigned to Alice
('HP LaserJet', 'PRN-002', 3, 1, '2024-01-07', NULL), -- Printer (in use)
('Dell OptiPlex', 'DSK-002', 2, 2, '2024-01-08', NULL); -- Desktop (broken)
GO

-- Print success message
PRINT 'Database and mock data created successfully!';

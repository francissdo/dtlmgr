-- SQL Server Database Setup Script for DTL Manager
-- Script: 01_create_tables.sql
-- Description: Creates the main tables for the DTL Manager application

-- Create Users table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE Users (
        Id int IDENTITY(1,1) PRIMARY KEY,
        Username nvarchar(50) NOT NULL UNIQUE,
        Email nvarchar(255) NOT NULL UNIQUE,
        PasswordHash nvarchar(255) NOT NULL,
        CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE()
    );
END

-- Create Products table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
BEGIN
    CREATE TABLE Products (
        Id int IDENTITY(1,1) PRIMARY KEY,
        Name nvarchar(100) NOT NULL,
        Description nvarchar(500),
        Price decimal(18,2) NOT NULL CHECK (Price >= 0),
        Stock int NOT NULL CHECK (Stock >= 0),
        CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE()
    );
END

-- Create indexes for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Users_Username')
    CREATE INDEX IX_Users_Username ON Users(Username);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Users_Email')
    CREATE INDEX IX_Users_Email ON Users(Email);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Products_Name')
    CREATE INDEX IX_Products_Name ON Products(Name);

-- Create triggers to automatically update UpdatedAt
IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'tr_Users_UpdatedAt')
    DROP TRIGGER tr_Users_UpdatedAt;
GO

CREATE TRIGGER tr_Users_UpdatedAt
ON Users
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users 
    SET UpdatedAt = GETUTCDATE()
    FROM Users u
    INNER JOIN inserted i ON u.Id = i.Id;
END
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'tr_Products_UpdatedAt')
    DROP TRIGGER tr_Products_UpdatedAt;
GO

CREATE TRIGGER tr_Products_UpdatedAt
ON Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Products 
    SET UpdatedAt = GETUTCDATE()
    FROM Products p
    INNER JOIN inserted i ON p.Id = i.Id;
END
GO

-- Add table descriptions
EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Application users with authentication information', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'Users';

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Product catalog with inventory management', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'Products';

-- SQL Server Database Update Script for DTL Manager
-- Script: 03_update_scripts.sql
-- Description: Common update and maintenance scripts

-- Add new columns if they don't exist (example for future use)
-- Add IsActive column to Users table if it doesn't exist
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'IsActive')
BEGIN
    ALTER TABLE Users ADD IsActive bit DEFAULT 1;
END

-- Add Category column to Products table if it doesn't exist
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'Category')
BEGIN
    ALTER TABLE Products ADD Category nvarchar(50) DEFAULT 'General';
END

-- Update existing data with categories
UPDATE Products SET Category = 'Computers' WHERE Name LIKE '%Laptop%' OR Name LIKE '%MacBook%';
UPDATE Products SET Category = 'Peripherals' WHERE Name LIKE '%Mouse%' OR Name LIKE '%Keyboard%' OR Name LIKE '%Headphones%' OR Name LIKE '%Headset%' OR Name LIKE '%Microphone%';
UPDATE Products SET Category = 'Monitors' WHERE Name LIKE '%Monitor%';
UPDATE Products SET Category = 'Accessories' WHERE Name LIKE '%Hub%' OR Name LIKE '%Arm%' OR Name LIKE '%Power Bank%' OR Name LIKE '%Stream Deck%';

-- Create additional indexes for new columns
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Users_IsActive')
    CREATE INDEX IX_Users_IsActive ON Users(IsActive);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Products_Category')
    CREATE INDEX IX_Products_Category ON Products(Category);

-- Clean up any test or invalid data
DELETE FROM Users WHERE Email LIKE '%test%' OR Email LIKE '%example.org%';
DELETE FROM Products WHERE Price <= 0 OR Stock < 0;

-- Update statistics
UPDATE STATISTICS Users;
UPDATE STATISTICS Products;

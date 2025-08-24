-- PostgreSQL Database Update Script for DTL Manager
-- Script: 03_update_scripts.sql
-- Description: Common update and maintenance scripts

-- Add new columns if they don't exist (example for future use)
DO $$ 
BEGIN
    -- Example: Add IsActive column to Users table if it doesn't exist
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name='users' AND column_name='isactive') THEN
        ALTER TABLE Users ADD COLUMN IsActive BOOLEAN DEFAULT TRUE;
    END IF;
    
    -- Example: Add Category column to Products table if it doesn't exist
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name='products' AND column_name='category') THEN
        ALTER TABLE Products ADD COLUMN Category VARCHAR(50) DEFAULT 'General';
    END IF;
END $$;

-- Update existing data with categories
UPDATE Products SET Category = 'Computers' WHERE Name LIKE '%Laptop%' OR Name LIKE '%MacBook%';
UPDATE Products SET Category = 'Peripherals' WHERE Name LIKE '%Mouse%' OR Name LIKE '%Keyboard%' OR Name LIKE '%Headphones%' OR Name LIKE '%Headset%' OR Name LIKE '%Microphone%';
UPDATE Products SET Category = 'Monitors' WHERE Name LIKE '%Monitor%';
UPDATE Products SET Category = 'Accessories' WHERE Name LIKE '%Hub%' OR Name LIKE '%Arm%' OR Name LIKE '%Power Bank%' OR Name LIKE '%Stream Deck%';

-- Create additional indexes for new columns
CREATE INDEX IF NOT EXISTS idx_users_isactive ON Users(IsActive);
CREATE INDEX IF NOT EXISTS idx_products_category ON Products(Category);

-- Clean up any test or invalid data
DELETE FROM Users WHERE Email LIKE '%test%' OR Email LIKE '%example.org%';
DELETE FROM Products WHERE Price <= 0 OR Stock < 0;

-- Update statistics
ANALYZE Users;
ANALYZE Products;

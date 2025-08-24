-- PostgreSQL Database Setup Script for DTL Manager
-- Script: 01_create_tables.sql
-- Description: Creates the main tables for the DTL Manager application

-- Create Users table
CREATE TABLE IF NOT EXISTS Users (
    Id SERIAL PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Create Products table
CREATE TABLE IF NOT EXISTS Products (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(500),
    Price DECIMAL(18,2) NOT NULL CHECK (Price >= 0),
    Stock INTEGER NOT NULL CHECK (Stock >= 0),
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Create indexes for better performance
CREATE INDEX IF NOT EXISTS idx_users_username ON Users(Username);
CREATE INDEX IF NOT EXISTS idx_users_email ON Users(Email);
CREATE INDEX IF NOT EXISTS idx_products_name ON Products(Name);

-- Create function to automatically update UpdatedAt timestamp
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.UpdatedAt = NOW();
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Create triggers to automatically update UpdatedAt
DROP TRIGGER IF EXISTS update_users_updated_at ON Users;
CREATE TRIGGER update_users_updated_at 
    BEFORE UPDATE ON Users 
    FOR EACH ROW 
    EXECUTE FUNCTION update_updated_at_column();

DROP TRIGGER IF EXISTS update_products_updated_at ON Products;
CREATE TRIGGER update_products_updated_at 
    BEFORE UPDATE ON Products 
    FOR EACH ROW 
    EXECUTE FUNCTION update_updated_at_column();

COMMENT ON TABLE Users IS 'Application users with authentication information';
COMMENT ON TABLE Products IS 'Product catalog with inventory management';

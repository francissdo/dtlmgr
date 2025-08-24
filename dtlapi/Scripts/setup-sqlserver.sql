-- SQL Server Database Setup Script

-- Create Database (run this separately if needed)
-- CREATE DATABASE DTLManager;
-- GO
-- USE DTLManager;
-- GO

-- Create Products table
CREATE TABLE Products (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name nvarchar(100) NOT NULL,
    Description nvarchar(500),
    Price decimal(18,2) NOT NULL CHECK (Price >= 0),
    Stock int NOT NULL CHECK (Stock >= 0),
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE()
);

-- Insert sample data
INSERT INTO Products (Name, Description, Price, Stock, CreatedAt, UpdatedAt) VALUES
('Laptop', 'High-performance laptop for developers', 1299.99, 15, GETUTCDATE(), GETUTCDATE()),
('Wireless Mouse', 'Ergonomic wireless mouse with precision tracking', 49.99, 50, GETUTCDATE(), GETUTCDATE()),
('Mechanical Keyboard', 'RGB mechanical keyboard with blue switches', 129.99, 25, GETUTCDATE(), GETUTCDATE()),
('USB-C Hub', 'Multi-port USB-C hub with HDMI and Ethernet', 79.99, 30, GETUTCDATE(), GETUTCDATE()),
('Monitor Stand', 'Adjustable monitor stand with storage drawer', 89.99, 20, GETUTCDATE(), GETUTCDATE());

-- Create index for better performance
CREATE INDEX IX_Products_Name ON Products(Name);

-- Display inserted data
SELECT * FROM Products;

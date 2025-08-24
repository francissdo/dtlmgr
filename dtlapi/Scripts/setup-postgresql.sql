-- PostgreSQL Database Setup Script

-- Create Database (run this separately if needed)
-- CREATE DATABASE dtlmanager;
-- \c dtlmanager;

-- Create Products table
CREATE TABLE Products (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(500),
    Price DECIMAL(18,2) NOT NULL CHECK (Price >= 0),
    Stock INTEGER NOT NULL CHECK (Stock >= 0),
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Insert sample data
INSERT INTO Products (Name, Description, Price, Stock, CreatedAt, UpdatedAt) VALUES
('Laptop', 'High-performance laptop for developers', 1299.99, 15, NOW(), NOW()),
('Wireless Mouse', 'Ergonomic wireless mouse with precision tracking', 49.99, 50, NOW(), NOW()),
('Mechanical Keyboard', 'RGB mechanical keyboard with blue switches', 129.99, 25, NOW(), NOW()),
('USB-C Hub', 'Multi-port USB-C hub with HDMI and Ethernet', 79.99, 30, NOW(), NOW()),
('Monitor Stand', 'Adjustable monitor stand with storage drawer', 89.99, 20, NOW(), NOW());

-- Create index for better performance
CREATE INDEX idx_products_name ON Products(Name);

-- Display inserted data
SELECT * FROM Products;

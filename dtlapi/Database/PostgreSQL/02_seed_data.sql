-- PostgreSQL Database Setup Script for DTL Manager
-- Script: 02_seed_data.sql
-- Description: Inserts initial data for the DTL Manager application

-- Insert initial users (passwords are hashed for 'password123')
INSERT INTO Users (Username, Email, PasswordHash, CreatedAt, UpdatedAt) VALUES
('admin', 'admin@dtlmanager.com', '$2a$11$rKxH4b0.f7Z0C8qR6j8xfOmP0O2yO1qwV3QxZqYQ1Yk8a9ZU3Q6qi', NOW(), NOW()),
('john_doe', 'john.doe@example.com', '$2a$11$rKxH4b0.f7Z0C8qR6j8xfOmP0O2yO1qwV3QxZqYQ1Yk8a9ZU3Q6qi', NOW(), NOW()),
('jane_smith', 'jane.smith@example.com', '$2a$11$rKxH4b0.f7Z0C8qR6j8xfOmP0O2yO1qwV3QxZqYQ1Yk8a9ZU3Q6qi', NOW(), NOW()),
('mike_wilson', 'mike.wilson@example.com', '$2a$11$rKxH4b0.f7Z0C8qR6j8xfOmP0O2yO1qwV3QxZqYQ1Yk8a9ZU3Q6qi', NOW(), NOW()),
('sarah_davis', 'sarah.davis@example.com', '$2a$11$rKxH4b0.f7Z0C8qR6j8xfOmP0O2yO1qwV3QxZqYQ1Yk8a9ZU3Q6qi', NOW(), NOW())
ON CONFLICT (Username) DO NOTHING;

-- Insert products with realistic data
INSERT INTO Products (Name, Description, Price, Stock, CreatedAt, UpdatedAt) VALUES
('Dell XPS 13 Laptop', 'Ultrabook with Intel Core i7, 16GB RAM, 512GB SSD', 1299.99, 12, NOW(), NOW()),
('Logitech MX Master 3', 'Advanced wireless mouse with precision tracking and customizable buttons', 89.99, 45, NOW(), NOW()),
('Keychron K8 Mechanical Keyboard', 'Wireless mechanical keyboard with hot-swappable switches and RGB backlight', 129.99, 28, NOW(), NOW()),
('CalDigit TS3 Plus USB-C Hub', 'Professional USB-C dock with Thunderbolt 3, multiple ports', 249.99, 18, NOW(), NOW()),
('Herman Miller Monitor Arm', 'Ergonomic adjustable monitor arm with cable management', 195.00, 15, NOW(), NOW()),
('Apple MacBook Air M2', 'Lightweight laptop with M2 chip, 8GB RAM, 256GB SSD', 1199.00, 8, NOW(), NOW()),
('Samsung 27" 4K Monitor', 'UHD monitor with HDR support and USB-C connectivity', 399.99, 22, NOW(), NOW()),
('Sony WH-1000XM4 Headphones', 'Noise-canceling wireless headphones with premium audio', 349.99, 35, NOW(), NOW()),
('Blue Yeti USB Microphone', 'Professional USB microphone for streaming and recording', 99.99, 40, NOW(), NOW()),
('Anker PowerCore 26800 Power Bank', 'High-capacity portable charger with fast charging support', 65.99, 60, NOW(), NOW()),
('Corsair K95 RGB Platinum', 'Premium mechanical gaming keyboard with macro keys', 199.99, 20, NOW(), NOW()),
('Razer DeathAdder V3 Pro', 'Wireless gaming mouse with high-precision sensor', 149.99, 32, NOW(), NOW()),
('SteelSeries Arctis 7P Headset', 'Wireless gaming headset with lossless audio', 179.99, 25, NOW(), NOW()),
('ASUS ROG Swift Monitor', '27" 144Hz gaming monitor with G-Sync technology', 549.99, 14, NOW(), NOW()),
('Elgato Stream Deck', 'Customizable control surface for content creation', 149.99, 18, NOW(), NOW())
ON CONFLICT DO NOTHING;

-- Display confirmation of inserted data
SELECT 'Users' as Table_Name, COUNT(*) as Record_Count FROM Users
UNION ALL
SELECT 'Products', COUNT(*) FROM Products;

# Database Scripts Documentation

This folder contains database setup and migration scripts for the DTL Manager application.

## Folder Structure

```
Database/
├── PostgreSQL/
│   ├── 01_create_tables.sql     # Creates Users and Products tables with indexes and triggers
│   ├── 02_seed_data.sql         # Inserts initial test data for users and products
│   └── 03_update_scripts.sql    # Contains update scripts and schema modifications
└── SqlServer/
    ├── 01_create_tables.sql     # Creates Users and Products tables with indexes and triggers
    ├── 02_seed_data.sql         # Inserts initial test data for users and products
    └── 03_update_scripts.sql    # Contains update scripts and schema modifications
```

## Usage Instructions

### PostgreSQL Setup

1. **Create Database** (if not exists):
   ```sql
   CREATE DATABASE dtlmanager;
   ```

2. **Connect to Database**:
   ```bash
   psql -U postgres -d dtlmanager
   ```

3. **Run Scripts in Order**:
   ```bash
   \i Database/PostgreSQL/01_create_tables.sql
   \i Database/PostgreSQL/02_seed_data.sql
   \i Database/PostgreSQL/03_update_scripts.sql
   ```

### SQL Server Setup

1. **Create Database** (if not exists):
   ```sql
   CREATE DATABASE DTLManager;
   ```

2. **Use Database**:
   ```sql
   USE DTLManager;
   ```

3. **Run Scripts in Order**:
   - Execute `Database/SqlServer/01_create_tables.sql`
   - Execute `Database/SqlServer/02_seed_data.sql`
   - Execute `Database/SqlServer/03_update_scripts.sql`

## Configuration

Update your `appsettings.json` or environment-specific configuration files to use the database:

### PostgreSQL Configuration
```json
{
  "Database": {
    "Provider": "postgresql",
    "ConnectionString": "Host=localhost;Database=dtlmanager;Username=your_user;Password=your_password"
  }
}
```

### SQL Server Configuration
```json
{
  "Database": {
    "Provider": "sqlserver",
    "ConnectionString": "Server=localhost;Database=DTLManager;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

## Default User Accounts

The seed data creates the following test users (password: `password123`):

- **admin** (admin@dtlmanager.com) - Administrator account
- **john_doe** (john.doe@example.com) - Test user
- **jane_smith** (jane.smith@example.com) - Test user
- **mike_wilson** (mike.wilson@example.com) - Test user  
- **sarah_davis** (sarah.davis@example.com) - Test user

## Tables Created

### Users Table
- **Id**: Primary key (auto-increment)
- **Username**: Unique username (max 50 chars)
- **Email**: Unique email address (max 255 chars)
- **PasswordHash**: BCrypt hashed password
- **CreatedAt**: Timestamp when user was created
- **UpdatedAt**: Timestamp when user was last updated (auto-updated via trigger)

### Products Table
- **Id**: Primary key (auto-increment)
- **Name**: Product name (max 100 chars)
- **Description**: Product description (max 500 chars)
- **Price**: Product price (decimal, must be >= 0)
- **Stock**: Stock quantity (integer, must be >= 0)
- **CreatedAt**: Timestamp when product was created
- **UpdatedAt**: Timestamp when product was last updated (auto-updated via trigger)

## Features

- **Automatic Timestamps**: UpdatedAt fields are automatically updated via database triggers
- **Data Validation**: Check constraints ensure data integrity
- **Indexes**: Performance indexes on commonly queried fields
- **Unique Constraints**: Prevent duplicate usernames and emails
- **Realistic Test Data**: 15 realistic products and 5 test users

## Migration Notes

When updating from the old demo data system:
1. The application now uses real database connections instead of in-memory data
2. Password hashing has been upgraded to BCrypt for better security
3. All CRUD operations now persist to the database
4. User authentication is backed by database storage

## Security Notes

- Passwords are hashed using BCrypt with appropriate salt rounds
- Default test passwords should be changed in production
- Consider implementing password complexity requirements
- Database connections should use appropriate security configurations

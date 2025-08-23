# DtlMgr - Data Management System

This is a full-stack application consisting of a .NET Core Web API backend (DtlApi) and a React.js frontend (dtlmgr).

## Features

### Backend (DtlApi)
- .NET 9.0 Web API
- Entity Framework Core with multiple database provider support:
  - SQL Server
  - PostgreSQL
  - XML dataset (in-memory)
- JWT-based OAuth 2.0 authentication
- RESTful API for Products and Categories
- Clean architecture with Repository pattern
- Swagger/OpenAPI documentation

### Frontend (dtlmgr)
- React.js application
- Material-UI components
- Data grid with CRUD operations
- JWT token-based authentication
- Responsive design

## Prerequisites

- Node.js (v14 or later)
- .NET 9.0 SDK
- SQL Server or PostgreSQL (optional - can use XML mode)

## Setup Instructions

### Backend Setup (DtlApi)

1. Navigate to the DtlApi directory:
   ```bash
   cd DtlApi
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Configure the database provider in `appsettings.json`:
   - Set `DatabaseProvider` to "SqlServer", "PostgreSQL", or "Xml"
   - Update connection strings as needed

4. Run database migrations (for SQL Server/PostgreSQL):
   ```bash
   dotnet ef database update
   ```

5. Run the API:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:7148` (or check the console output for the exact port).

### Frontend Setup (dtlmgr)

1. Navigate to the dtlmgr directory:
   ```bash
   cd dtlmgr
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Update the `.env` file if your API is running on a different port:
   ```
   REACT_APP_API_URL=https://localhost:7148/api
   ```

4. Start the development server:
   ```bash
   npm start
   ```

The application will be available at `http://localhost:3000`.

## Database Configuration

### SQL Server (Default)
Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DtlApiDb;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

### PostgreSQL
1. Set `DatabaseProvider` to "PostgreSQL"
2. Update the connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=DtlApiDb;Username=postgres;Password=yourpassword"
}
```

### XML Dataset
1. Set `DatabaseProvider` to "Xml"
2. The system will use the `data.xml` file for sample data

## Usage

1. Register a new user account
2. Login with your credentials
3. Use the data grid to:
   - View products
   - Add new products
   - Edit existing products
   - Delete products

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and get JWT token

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### Categories
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category

## Development Notes

### Adding New Entities
1. Create model in `Models/` folder
2. Add DbSet to `ApplicationDbContext`
3. Create repository interface in `Data/Interfaces/`
4. Implement repository in `Data/Repositories/`
5. Create DTOs in `DTOs/` folder
6. Create controller in `Controllers/` folder
7. Register repository in `Program.cs`

### Database Switching
The application supports easy switching between database providers by changing the `DatabaseProvider` setting in `appsettings.json`. This makes it flexible for different deployment environments.

## Security

- JWT tokens expire after 60 minutes by default
- Passwords are hashed using ASP.NET Core Identity
- CORS is configured to allow the React frontend
- All API endpoints (except auth) require authentication

## Production Deployment

1. Update connection strings for production databases
2. Change JWT settings in production configuration
3. Build frontend for production: `npm run build`
4. Publish backend: `dotnet publish -c Release`
5. Configure reverse proxy (IIS, Nginx, etc.)
6. Set up HTTPS certificates

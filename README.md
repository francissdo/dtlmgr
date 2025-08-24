# DTL Manager - Full Stack Application

This project consists of a .NET Core REST API (dtlapi) and a React TypeScript frontend (dtlmgr) for managing products with authentication.

## Project Structure

```
dtlmgr/
├── dtlapi/                     # .NET Core Web API
│   ├── Controllers/            # API Controllers
│   ├── Models/                 # Data Models
│   ├── Data/                   # Data Access Layer
│   ├── Services/               # Business Logic
│   ├── Configuration/          # Configuration Classes
│   └── SampleData/             # Sample JSON data
├── dtlapi.Tests/              # Unit Tests
├── dtlmgr/                    # React TypeScript Frontend
│   ├── src/
│   │   ├── components/        # React Components
│   │   ├── pages/             # Page Components
│   │   ├── services/          # API Services
│   │   └── types/             # TypeScript Definitions
└── README.md
```

## Features

### Backend API (dtlapi)
- **Authentication**: JWT-based OAuth authentication
- **Swagger Documentation**: Auto-generated API documentation
- **Multi-Database Support**: Switch between SQL Server, PostgreSQL, and JSON file storage
- **Data Access**: Dapper ORM for efficient database operations
- **CRUD Operations**: Complete product management
- **Unit Tests**: Comprehensive test coverage
- **CORS Support**: Configured for React frontend

### Frontend (dtlmgr)
- **React with TypeScript**: Type-safe frontend development
- **Bootstrap UI**: Professional, responsive design
- **Authentication**: JWT token management
- **Protected Routes**: Route-based authentication
- **CRUD Interface**: Full product management interface
- **Error Handling**: Comprehensive error management

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Node.js (v16 or later)
- npm or yarn

### Backend Setup (dtlapi)

1. **Navigate to the API directory:**
   ```bash
   cd dtlapi
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Configure the database provider in `appsettings.json`:**
   ```json
   {
     "Database": {
       "Provider": "Json",           // Options: "Json", "SqlServer", "PostgreSql"
       "ConnectionString": "",       // Required for SQL Server/PostgreSQL
       "DataPath": "SampleData"      // Required for JSON provider
     }
   }
   ```

4. **For SQL Server/PostgreSQL, update the connection string:**
   ```json
   {
     "Database": {
       "Provider": "SqlServer",
       "ConnectionString": "Server=localhost;Database=DTLManager;Trusted_Connection=true;TrustServerCertificate=true;",
       "DataPath": ""
     }
   }
   ```

5. **Create database tables (for SQL Server/PostgreSQL):**
   ```sql
   CREATE TABLE Products (
       Id int IDENTITY(1,1) PRIMARY KEY,
       Name nvarchar(100) NOT NULL,
       Description nvarchar(500),
       Price decimal(18,2) NOT NULL,
       Stock int NOT NULL,
       CreatedAt datetime2 NOT NULL,
       UpdatedAt datetime2 NOT NULL
   );
   ```

   For PostgreSQL:
   ```sql
   CREATE TABLE Products (
       Id SERIAL PRIMARY KEY,
       Name VARCHAR(100) NOT NULL,
       Description VARCHAR(500),
       Price DECIMAL(18,2) NOT NULL,
       Stock INTEGER NOT NULL,
       CreatedAt TIMESTAMP NOT NULL,
       UpdatedAt TIMESTAMP NOT NULL
   );
   ```

6. **Run the API:**
   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:7224` with Swagger UI at the root URL.

### Frontend Setup (dtlmgr)

1. **Navigate to the React directory:**
   ```bash
   cd dtlmgr
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Update the API URL in `src/services/apiService.ts` if needed:**
   ```typescript
   private baseURL = 'https://localhost:7224/api';
   ```

4. **Start the development server:**
   ```bash
   npm start
   ```

   The React app will be available at `http://localhost:3000`.

## Authentication

### Demo Credentials
- **Admin User**: `admin` / `admin123`
- **Regular User**: `user` / `user123`

### JWT Configuration
The JWT settings can be configured in `appsettings.json`:
```json
{
  "Jwt": {
    "Key": "your-secret-key-must-be-at-least-32-characters-long-for-security",
    "Issuer": "dtlapi",
    "Audience": "dtlapi-client",
    "ExpirationHours": 24
  }
}
```

## API Endpoints

### Authentication
- `POST /api/auth/login` - Authenticate user and get JWT token

### Products
- `GET /api/products` - Get all products (requires authentication)
- `GET /api/products/{id}` - Get product by ID (requires authentication)
- `POST /api/products` - Create new product (requires authentication)
- `PUT /api/products/{id}` - Update product (requires authentication)
- `DELETE /api/products/{id}` - Delete product (requires authentication)

## Data Providers

### JSON Provider (Default)
- Stores data in JSON files in the `SampleData` directory
- No database setup required
- Good for development and testing

### SQL Server Provider
- Uses Microsoft.Data.SqlClient
- Requires SQL Server database
- Production-ready with full ACID compliance

### PostgreSQL Provider
- Uses Npgsql driver
- Requires PostgreSQL database
- Open-source alternative to SQL Server

## Testing

### Run API Tests
```bash
cd dtlapi.Tests
dotnet test
```

### Run React Tests
```bash
cd dtlmgr
npm test
```

## Building for Production

### Build API
```bash
cd dtlapi
dotnet build --configuration Release
dotnet publish --configuration Release
```

### Build React App
```bash
cd dtlmgr
npm run build
```

## Docker Support (Optional)

You can containerize the applications:

### API Dockerfile (dtlapi/Dockerfile)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["dtlapi.csproj", "."]
RUN dotnet restore "dtlapi.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "dtlapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dtlapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dtlapi.dll"]
```

## Technology Stack

### Backend
- **.NET 8.0**: Latest LTS version of .NET
- **ASP.NET Core**: Web API framework
- **Dapper**: Lightweight ORM
- **JWT Bearer**: Authentication
- **Swagger/OpenAPI**: API documentation
- **xUnit**: Unit testing framework

### Frontend
- **React 18**: UI library
- **TypeScript**: Type-safe JavaScript
- **React Router**: Client-side routing
- **Bootstrap**: UI framework
- **Axios**: HTTP client
- **React Bootstrap**: Bootstrap components for React

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new features
5. Ensure all tests pass
6. Submit a pull request

## License

This project is licensed under the MIT License.

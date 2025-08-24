# Swagger Troubleshooting Guide

## ✅ Swagger is Now Working!

The Swagger UI is now accessible at: **http://localhost:5272**

## Issues Fixed

### 1. **Port Conflict**
- **Problem**: Port 5271 was already in use
- **Solution**: Changed to port 5272 in `launchSettings.json`

### 2. **XML Documentation**
- **Problem**: XML documentation file was not being generated
- **Solution**: Added these properties to `dtlapi.csproj`:
  ```xml
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
  ```

### 3. **Process Lock**
- **Problem**: Previous API process was still running
- **Solution**: Killed the process with `taskkill /f /im dtlapi.exe`

## Swagger Configuration

The Swagger configuration in `Program.cs` is correctly set up with:
- JWT Bearer authentication support
- API documentation at root URL (`/`)
- Security definitions for protected endpoints

## How to Use Swagger

1. **Start the API**: `cd dtlapi && dotnet run`
2. **Open browser**: Navigate to http://localhost:5272
3. **Authenticate**: 
   - Click "Authorize" button
   - Use `/api/auth/login` endpoint with credentials:
     - Username: `admin`, Password: `admin123`
     - OR Username: `user`, Password: `user123`
   - Copy the returned token
   - Click "Authorize" and enter: `Bearer YOUR_TOKEN_HERE`
4. **Test endpoints**: All `/api/products` endpoints are now accessible

## Available Endpoints

- `POST /api/auth/login` - Get JWT token
- `GET /api/products` - List all products (requires auth)
- `GET /api/products/{id}` - Get product by ID (requires auth)
- `POST /api/products` - Create product (requires auth)
- `PUT /api/products/{id}` - Update product (requires auth)
- `DELETE /api/products/{id}` - Delete product (requires auth)

## Testing Authentication Flow

1. Use the login endpoint with demo credentials
2. Copy the token from the response
3. Click "Authorize" and enter `Bearer {token}`
4. All protected endpoints will now work

## Startup Scripts

Use these convenient scripts to start both applications:

**Windows Batch:**
```cmd
start-all.bat
```

**PowerShell:**
```powershell
.\start-all.ps1
```

## URLs
- **API/Swagger**: http://localhost:5272
- **React App**: http://localhost:3000 (when started)

## Common Issues and Solutions

### Issue: "Swagger page not found"
- **Solution**: Ensure you're using http://localhost:5272 (not https)
- **Check**: API is running on correct port

### Issue: "401 Unauthorized on protected endpoints"
- **Solution**: Use the login endpoint first to get a token
- **Then**: Add the token using the "Authorize" button

### Issue: "CORS errors"
- **Solution**: CORS is configured for http://localhost:3000
- **Check**: React app uses correct API URL in `apiService.ts`

### Issue: "Port already in use"
- **Solution**: Kill existing processes: `taskkill /f /im dtlapi.exe`
- **Or**: Change port in `launchSettings.json`

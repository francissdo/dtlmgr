using dtlapi.Configuration;
using dtlapi.Data;
using dtlapi.Data.Providers.Json;
using dtlapi.Data.Providers.SqlServer;
using dtlapi.Data.Providers.PostgreSql;
using dtlapi.Data.Providers.Xml;
using dtlapi.Data.Providers.RestApi;
using dtlapi.Data.Providers.Soap;
using dtlapi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
var databaseSettings = builder.Configuration.GetSection("Database").Get<DatabaseSettings>() ?? new DatabaseSettings();
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "DTL API", 
        Version = "v1",
        Description = "A REST API for product management with OAuth authentication"
    });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

builder.Services.AddAuthorization();

// Configure data provider based on settings
switch (databaseSettings.Provider.ToLowerInvariant())
{
    case "sqlserver":
        builder.Services.AddSingleton<IDataProvider>(provider => 
            new SqlServerDataProvider(databaseSettings.ConnectionString));
        builder.Services.AddScoped<IProductRepository, SqlServerProductRepository>();
        builder.Services.AddScoped<IUserRepository, SqlServerUserRepository>();
        break;
    case "postgresql":
        builder.Services.AddSingleton<IDataProvider>(provider => 
            new PostgreSqlDataProvider(databaseSettings.ConnectionString));
        builder.Services.AddScoped<IProductRepository, PostgreSqlProductRepository>();
        builder.Services.AddScoped<IUserRepository, PostgreSqlUserRepository>();
        break;
    case "xml":
        var xmlDataPath = Path.IsPathRooted(databaseSettings.DataPath) 
            ? databaseSettings.DataPath 
            : Path.Combine(builder.Environment.ContentRootPath, databaseSettings.DataPath);
        builder.Services.AddSingleton(provider => new XmlDataProvider(xmlDataPath));
        builder.Services.AddScoped<IProductRepository, XmlProductRepository>();
        builder.Services.AddScoped<IUserRepository, XmlUserRepository>();
        break;
    case "json":
    default:
        var dataPath = Path.IsPathRooted(databaseSettings.DataPath) 
            ? databaseSettings.DataPath 
            : Path.Combine(builder.Environment.ContentRootPath, databaseSettings.DataPath);
        builder.Services.AddSingleton(provider => new JsonDataProvider(dataPath));
        builder.Services.AddScoped<IProductRepository, JsonProductRepository>();
        builder.Services.AddScoped<IUserRepository, JsonUserRepository>();
        break;
}

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();

// Register external service providers
builder.Services.AddHttpClient<RestApiProductService>();
builder.Services.AddHttpClient<RestApiUserService>();
builder.Services.AddScoped<SoapProductService>();
builder.Services.AddScoped<SoapUserService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DTL API v1");
        c.RoutePrefix = "swagger"; // Serve the Swagger UI at /swagger
    });
}

// app.UseHttpsRedirection(); // Commented out for local development
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

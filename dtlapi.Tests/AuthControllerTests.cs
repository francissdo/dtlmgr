using dtlapi.Controllers;
using dtlapi.Models;
using dtlapi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace dtlapi.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginRequest = new LoginRequest { Username = "admin", Password = "admin123" };
            var authResponse = new AuthResponse 
            { 
                Token = "fake-jwt-token",
                Username = "admin",
                Expiration = DateTime.UtcNow.AddHours(24)
            };
            _mockAuthService.Setup(service => service.LoginAsync(loginRequest)).ReturnsAsync(authResponse);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResponse = Assert.IsType<AuthResponse>(okResult.Value);
            Assert.Equal("admin", returnedResponse.Username);
            Assert.Equal("fake-jwt-token", returnedResponse.Token);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequest { Username = "invalid", Password = "invalid" };
            _mockAuthService.Setup(service => service.LoginAsync(loginRequest)).ReturnsAsync((AuthResponse?)null);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result.Result);
        }
    }
}

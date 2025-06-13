using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using ExpenseTrackerAPI.Controllers;

namespace ExpenseTrackerAPI.Tests.Controllers
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
        public async Task Login_WithValidCredentials_ReturnsOk()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "user@example.com", Password = "Password123" };
            var expectedResponse = new AuthResponse { Token = "token", RefreshToken = "refreshToken" };

            _mockAuthService.Setup(s => s.AuthenticateAsync(loginRequest)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeOfType<ApiResponse<AuthResponse>>();
            var apiResponse = okResult.Value as ApiResponse<AuthResponse>;
            apiResponse!.Data.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "wrong@example.com", Password = "wrongpass" };
            _mockAuthService.Setup(s => s.AuthenticateAsync(loginRequest)).ReturnsAsync((AuthResponse?)null);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task Refresh_WithValidTokens_ReturnsOk()
        {
            // Arrange
            var refreshRequest = new RefreshTokenRequest { Token = "token", RefreshToken = "refreshToken" };
            var expectedResponse = new AuthResponse { Token = "newToken", RefreshToken = "newRefreshToken" };

            _mockAuthService.Setup(s => s.RefreshAccessTokenAsync(refreshRequest.Token, refreshRequest.RefreshToken))
                            .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Refresh(refreshRequest);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var apiResponse = okResult!.Value as ApiResponse<AuthResponse>;
            apiResponse!.Data.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Refresh_WithInvalidToken_ReturnsUnauthorized()
        {
            // Arrange
            var refreshRequest = new RefreshTokenRequest { Token = "badtoken", RefreshToken = "badrefresh" };
            _mockAuthService.Setup(s => s.RefreshAccessTokenAsync(refreshRequest.Token, refreshRequest.RefreshToken))
                            .ReturnsAsync((AuthResponse?)null);

            // Act
            var result = await _controller.Refresh(refreshRequest);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }
    }
}

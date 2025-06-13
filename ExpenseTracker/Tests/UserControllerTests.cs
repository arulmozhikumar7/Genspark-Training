using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExpenseTrackerAPI.Controllers;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Enums;
using FluentAssertions;

namespace ExpenseTrackerAPI.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnConflict_WhenEmailIsTaken()
        {
            var request = new RegisterUserRequest
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "Password123!"
            };

            _mockUserService.Setup(s => s.IsEmailTakenAsync(request.Email)).ReturnsAsync(true);

            var result = await _controller.Register(request);

            var conflictResult = result as ObjectResult;
            conflictResult.Should().NotBeNull();
            conflictResult!.StatusCode.Should().Be(409);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsRegistered()
        {
            var request = new RegisterUserRequest
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "Password123!"
            };

            _mockUserService.Setup(s => s.IsEmailTakenAsync(request.Email)).ReturnsAsync(false);
            _mockUserService.Setup(s => s.RegisterUserAsync(It.IsAny<User>(), request.Password)).ReturnsAsync(true);

            var result = await _controller.Register(request);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task RegisterAdmin_ShouldReturnOk_WhenAdminIsRegistered()
        {
            var request = new RegisterUserRequest
            {
                UserName = "adminuser",
                Email = "admin@example.com",
                Password = "Password123!"
            };

            _mockUserService.Setup(s => s.IsEmailTakenAsync(request.Email)).ReturnsAsync(false);
            _mockUserService.Setup(s => s.RegisterUserAsync(It.IsAny<User>(), request.Password)).ReturnsAsync(true);

            var result = await _controller.RegisterAdmin(request);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            Guid testId = Guid.NewGuid();
            _mockUserService.Setup(s => s.GetUserByIdAsync(testId)).ReturnsAsync((User?)null);

            var result = await _controller.GetById(testId);

            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetById_ShouldReturnUser_WhenUserExists()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "johndoe",
                Email = "john@example.com",
                Role = Roles.User
            };

            _mockUserService.Setup(s => s.GetUserByIdAsync(user.Id)).ReturnsAsync(user);

            var result = await _controller.GetById(user.Id);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetByEmail_ShouldReturnNotFound_WhenEmailDoesNotExist()
        {
            var email = "missing@example.com";
            _mockUserService.Setup(s => s.GetUserByEmailAsync(email)).ReturnsAsync((User?)null);

            var result = await _controller.GetByEmail(email);

            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetByEmail_ShouldReturnUser_WhenEmailExists()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "janedoe",
                Email = "jane@example.com",
                Role = Roles.Admin
            };

            _mockUserService.Setup(s => s.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

            var result = await _controller.GetByEmail(user.Email);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
        }
    }
}

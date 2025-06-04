using HospitalManagementAPI.Data;
using HospitalManagementAPI.DTOs;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Services;
using HospitalManagementAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Test
{
    [TestFixture]
    public class AuthServiceTests
    {
        private HospitalDbContext _context;
        private Mock<IEncryptionService> _encryptionMock;
        private Mock<ITokenService> _tokenMock;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HospitalDbContext>()
                .UseInMemoryDatabase(databaseName: "HospitalTestDb_" + System.Guid.NewGuid())
                .Options;

            _context = new HospitalDbContext(options);
            _encryptionMock = new Mock<IEncryptionService>();
            _tokenMock = new Mock<ITokenService>();
            var configMock = new Mock<IConfiguration>();

            _authService = new AuthService(_context, configMock.Object, _encryptionMock.Object, _tokenMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task Register_ShouldSucceed_WhenUsernameIsUnique()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "test@example.com",
                Password = "password123",
                Role = "Admin"
            };

            byte[] hash = new byte[] { 1, 2, 3 };
            byte[] salt = new byte[] { 4, 5, 6 };

            _encryptionMock.Setup(e => e.CreatePasswordHash(registerDto.Password, out hash, out salt));

            // Act
            var result = await _authService.Register(registerDto);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("User registered successfully.", result.Data);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "test@example.com");
            Assert.IsNotNull(user);
            Assert.AreEqual("Admin", user.Role);
        }

        [Test]
        public async Task Register_ShouldFail_WhenUsernameExists()
        {
            // Arrange
            var existingUser = new User
            {
                Username = "test@example.com",
                Role = "Admin"
            };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var registerDto = new RegisterDto
            {
                Username = "test@example.com",
                Password = "password123",
                Role = "Admin"
            };

            // Act
            var result = await _authService.Register(registerDto);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Username already exists.", result.Message);
        }

        [Test]
        public async Task Login_ShouldSucceed_WithCorrectCredentials()
        {
            // Arrange
            var password = "password123";
            byte[] hash = new byte[] { 1, 2, 3 };
            byte[] salt = new byte[] { 4, 5, 6 };

            var user = new User
            {
                Username = "login@example.com",
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = "User"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _encryptionMock.Setup(e => e.VerifyPasswordHash(password, hash, salt)).Returns(true);
            _tokenMock.Setup(t => t.CreateToken(It.IsAny<User>())).Returns("mock-token");

            var loginDto = new LoginDto
            {
                Username = "login@example.com",
                Password = password
            };

            // Act
            var result = await _authService.Login(loginDto);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("mock-token", result.Data);
        }

        [Test]
        public async Task Login_ShouldFail_WhenUserNotFound()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "missing@example.com",
                Password = "password123"
            };

            // Act
            var result = await _authService.Login(loginDto);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User not found.", result.Message);
        }

        [Test]
        public async Task Login_ShouldFail_WithIncorrectPassword()
        {
            // Arrange
            var password = "password123";
            byte[] hash = new byte[] { 1, 2, 3 };
            byte[] salt = new byte[] { 4, 5, 6 };

            var user = new User
            {
                Username = "wrongpass@example.com",
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = "User"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _encryptionMock.Setup(e => e.VerifyPasswordHash(password, hash, salt)).Returns(false);

            var loginDto = new LoginDto
            {
                Username = "wrongpass@example.com",
                Password = password
            };

            // Act
            var result = await _authService.Login(loginDto);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Incorrect password.", result.Message);
        }
    }
}

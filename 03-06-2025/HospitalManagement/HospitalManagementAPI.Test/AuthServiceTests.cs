using HospitalManagementAPI.Data;
using HospitalManagementAPI.DTOs;
using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Services;
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
        private IAuthService _authService;

        // Delegate matching CreatePasswordHash signature with out parameters
        private delegate void CreatePasswordHashDelegate(string password, out byte[] passwordHash, out byte[] passwordSalt);

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HospitalDbContext>()
                .UseInMemoryDatabase(databaseName: "HospitalTestDb")
                .Options;

            _context = new HospitalDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

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
        public async Task Register_ShouldAddNewUser()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "newuser",
                Password = "password123",
                Role = "Admin"
            };

            byte[] dummyHash = new byte[] { 1, 2, 3 };
            byte[] dummySalt = new byte[] { 4, 5, 6 };

            //mock for CreatePasswordHash with out parameters
            _encryptionMock.Setup(e => e.CreatePasswordHash(It.IsAny<string>(), out It.Ref<byte[]>.IsAny, out It.Ref<byte[]>.IsAny))
                .Callback(new CreatePasswordHashDelegate((string password, out byte[] hash, out byte[] salt) =>
                {
                    hash = dummyHash;
                    salt = dummySalt;
                }));

            // Act
            var result = await _authService.Register(registerDto);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("User registered successfully.", result.Data);

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == "newuser");
            Assert.IsNotNull(userInDb);
            Assert.AreEqual("Admin", userInDb.Role);
            Assert.AreEqual(dummyHash, userInDb.PasswordHash);
            Assert.AreEqual(dummySalt, userInDb.PasswordSalt);
        }

        [Test]
        public async Task Register_ShouldFail_WhenUsernameExists()
        {
            // Arrange
            var existingUser = new Models.User
            {
                Username = "existinguser",
                Role = "User"
            };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var registerDto = new RegisterDto
            {
                Username = "existinguser",
                Password = "password123",
                Role = "Admin"
            };

            // Act
            var result = await _authService.Register(registerDto);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Username already exists.", result.Message);
        }
    }
}

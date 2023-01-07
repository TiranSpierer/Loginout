using DataAccess.DataModels;
using DataAccess.Repository;
using DataService.Services;
using Microsoft.AspNetCore.Identity;

namespace TestDataService;

public class TestUserService
{
    private Mock<IRepository<User>> _mockRepository;
    private Mock<IPasswordHasher<User>> _mockPasswordHasher;
    private UserService _userService;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<IRepository<User>>();
        _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
        _userService = new UserService(_mockRepository.Object, _mockPasswordHasher.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _mockRepository.Reset();
        _mockPasswordHasher.Reset();
    }

    [Test]
    [TestCase("test", "password")]
    [TestCase("test", "")]
    [TestCase("test", null)]
    public async Task AuthenticateAsync_WithValidUsernameAndPassword_ReturnsTrue(string username, string password)
    {
        // Arrange
        User user = new User
        {
            Id = username,
            Password = _mockPasswordHasher.Object.HashPassword(new User(), password)
        };
        _mockRepository.Setup(r => r.GetById(username)).ReturnsAsync(user);
        _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), password)).Returns(PasswordVerificationResult.Success);

        // Act
        bool result = await _userService.AuthenticateAsync(username, password);

        // Assert
        Assert.IsTrue(result);
    }


    [Test]
    [TestCase("test", "password")]
    public async Task AuthenticateAsync_WithInvalidUsername_ReturnsFalse(string username, string password)
    {
        // Arrange
        _mockRepository.Setup(r => r.GetById(username)).ReturnsAsync((User)null);

        // Act
        bool result = await _userService.AuthenticateAsync(username, password);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    [TestCase("test", "password")]
    public async Task AuthenticateAsync_WithInvalidPassword_ReturnsFalse(string username, string password)
    {
        // Arrange
        User user = new User
        {
            Id = username,
            Password = _mockPasswordHasher.Object.HashPassword(new User(), "invalid password")
        };
        _mockRepository.Setup(r => r.GetById(username)).ReturnsAsync(user);
        _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(user, user.Password, password)).Returns(PasswordVerificationResult.Failed);

        // Act
        bool result = await _userService.AuthenticateAsync(username, password);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    [TestCase("test", "password", "Test User")]
    public async Task RegisterAsync_WithValidUsername_ReturnsTrue(string username, string password, string name)
    {
        // Arrange
        _mockRepository.Setup(r => r.Create(It.IsAny<User>())).ReturnsAsync(true);

        // Act
        bool result = await _userService.RegisterAsync(username, password, name);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    [TestCase("", "password", "Test User")]
    [TestCase(null, "password", "Test User")]
    public async Task RegisterAsync_WithEmptyUsername_ReturnsFalse(string username, string password, string name)
    {
        // Arrange

        // Act
        bool result = await _userService.RegisterAsync(username, password, name);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    [TestCase("test", "password", "Test User")]
    public async Task RegisterAsync_WithFailedCreate_ReturnsFalse(string username, string password, string name)
    {
        // Arrange
        _mockRepository.Setup(r => r.Create(It.IsAny<User>())).ReturnsAsync(false);

        // Act
        bool result = await _userService.RegisterAsync(username, password, name);

        // Assert
        Assert.IsFalse(result);
    }

}

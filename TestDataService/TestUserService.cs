using System.Collections;
using DataAccess.DataModels;
using DataAccess.Repository;
using DataService.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace TestDataService;

public class TestUserService
{
    private Mock<IRepository<User>>     _mockRepository     = null!;
    private Mock<IPasswordHasher<User>> _mockPasswordHasher = null!;
    private UserService                 _userService        = null!;

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
        _mockRepository.Setup(r => r.GetByIdAsync(username)).ReturnsAsync(user);
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
        _mockRepository.Setup(r => r.GetByIdAsync(username)).ReturnsAsync((User?)null);

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
        _mockRepository.Setup(r => r.GetByIdAsync(username)).ReturnsAsync(user);
        _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(user, user.Password, password)).Returns(PasswordVerificationResult.Failed);

        // Act
        bool result = await _userService.AuthenticateAsync(username, password);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    [TestCase("test", "password")]
    [TestCase("test", "", "Name")]
    [TestCase("test", null, null, new[] { Privilege.EditUsers })]
    [TestCase("test", "password", "Name", new[] { Privilege.EditUsers, Privilege.DeleteUsers })]
    public async Task RegisterAsync_WithValidInput_ReturnsTrue(string username, string password = "", string name = "", IEnumerable<Privilege>? privileges = null)
    {
        // Arrange
        var user = new User
                   {
                       Id       = username,
                       Password = _mockPasswordHasher.Object.HashPassword(new User(), password),
                       Name     = name
                   };

        user.UserPrivileges = privileges?.Select(privilege => new UserPrivilege
                                                              {
                                                                  UserId    = user.Id,
                                                                  Privilege = privilege
                                                              }).ToList();

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(true);

        // Act
        var result = await _userService.RegisterAsync(username, password, name, privileges);

        // Assert
        Assert.That(result, Is.True);
    }


    [Test]
    [TestCase("", "password", "Test User")]
    [TestCase(null, "password")]
    public async Task RegisterAsync_WithInvalidUsername_ReturnsFalse(string username, string password, string name = "")
    {
        // Arrange

        // Act
        var result = await _userService.RegisterAsync(username, password, name);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase("test", "password", "Test User")]
    public async Task RegisterAsync_WithFailedCreate_ReturnsFalse(string username, string password, string name)
    {
        // Arrange
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync(false);

        // Act
        var result = await _userService.RegisterAsync(username, password, name);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetAllUsersAsync_WithNoUsers_ReturnsEmptyList()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllIncludingPropertiesAsync(It.IsAny<Expression<Func<User, object>>>())).ReturnsAsync(Enumerable.Empty<User>());

        // Act
        IEnumerable<User> result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllUsersAsync_WithUsers_ReturnsListOfUsers()
    {
        // Arrange
        User user1 = new() { Id = "user1" };
        User user2 = new() { Id = "user2" };
        _mockRepository.Setup(r => r.GetAllIncludingPropertiesAsync(It.IsAny<Expression<Func<User, object>>>())).ReturnsAsync(new List<User> { user1, user2 });

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(),      Is.EqualTo(2));
            Assert.That((ICollection)result, Does.Contain(user1));
            Assert.That((ICollection)result, Does.Contain(user2));
        });
        
    }

    [Test]
    [TestCase("test", "test1")]
    [TestCase("", "test2")]
    [TestCase(null, "test3")]
    public async Task EditAsync_WithNonExistentUser_ReturnsFalse(string originalUsername, string updatedUsername)
    {
        // Arrange
        User updatedUser = new User { Id = updatedUsername };
        _mockRepository.Setup(r => r.GetByIdAsync(originalUsername)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.EditAsync(originalUsername, updatedUser);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task EditAsync_WithUpdatedUsernameThatExists_ReturnsFalse()
    {
        // Arrange
        var originalUsername = "test";
        var updatedUsername  = "test2";
        var originalUser     = new User { Id = originalUsername };
        var updatedUser      = new User { Id = updatedUsername };
        _mockRepository.Setup(r => r.GetByIdAsync(originalUsername)).ReturnsAsync(originalUser);
        _mockRepository.Setup(r => r.GetByIdAsync(updatedUsername)).ReturnsAsync(updatedUser);

        // Act
        var result = await _userService.EditAsync(originalUsername, updatedUser);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task EditAsync_WithSuccessfulUpdate_ReturnsTrue()
    {
        // Arrange
        var username = "test";
        var originalUser = new User
                            {
                                Id   = username,
                                Name = "Zenya"
                            };
        var updatedUser = new User
                          {
                              Id   = username,
                              Name = "Doron"
        };
        _mockRepository.Setup(r => r.GetByIdAsync(username)).ReturnsAsync(originalUser);
        _mockRepository.Setup(r => r.UpdateAsync(username, updatedUser)).ReturnsAsync(true);

        // Act
        var result = await _userService.EditAsync(username, updatedUser);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task EditAsync_WithSuccessfulCreate_ReturnsTrue()
    {
        // Arrange
        var originalUsername = "test";
        var updatedUsername  = "test2";
        var originalUser     = new User { Id = originalUsername };
        var updatedUser      = new User { Id = updatedUsername };
        _mockRepository.Setup(r => r.GetByIdAsync(originalUsername)).ReturnsAsync(originalUser);
        _mockRepository.Setup(r => r.GetByIdAsync(updatedUsername)).ReturnsAsync((User?)null);
        _mockRepository.Setup(r => r.DeleteAsync(originalUsername)).ReturnsAsync(true);
        _mockRepository.Setup(r => r.CreateAsync(updatedUser)).ReturnsAsync(true);

        // Act
        var result = await _userService.EditAsync(originalUsername, updatedUser);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task EditAsync_WithUnsuccessfulCreate_ReturnsFalse()
    {
        // Arrange
        var originalUsername = "test";
        var updatedUsername  = "test2";
        var originalUser     = new User { Id = originalUsername };
        var updatedUser      = new User { Id = updatedUsername };
        _mockRepository.Setup(r => r.GetByIdAsync(originalUsername)).ReturnsAsync(originalUser);
        _mockRepository.Setup(r => r.GetByIdAsync(updatedUsername)).ReturnsAsync((User?)null);
        _mockRepository.Setup(r => r.DeleteAsync(originalUsername)).ReturnsAsync(true);
        _mockRepository.Setup(r => r.CreateAsync(updatedUser)).ReturnsAsync(false);

        // Act
        var result = await _userService.EditAsync(originalUsername, updatedUser);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task DeleteAsync_WithExistingOrNonExistingUsername_ReturnsExpectedResult(bool expectedResult)
    {
        // Arrange
        var username = "test";
        _mockRepository.Setup(r => r.DeleteAsync(username)).ReturnsAsync(expectedResult);

        // Act
        var result = await _userService.DeleteAsync(username);

        // Assert
        Assert.That(expectedResult, Is.EqualTo(result));
    }
}

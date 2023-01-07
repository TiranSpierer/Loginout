namespace TestDataAccess;

public class TestRepository
{
    private Mock<EnvueDbContextFactory> _mockDbContextFactory;
    private Mock<EnvueDbContext> _mockDbContext;
    private Mock<DbSet<User>> _mockDbSet;

    private Repository<User> _repository;
    private string _connectionString;
    private string _username;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        // Build the configuration object
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        _connectionString = configuration.GetConnectionString("Default")!;
    }

    [SetUp]
    public void SetUp()
    {
        _mockDbSet = new Mock<DbSet<User>>();
        DbContextOptions options = new DbContextOptionsBuilder().UseInMemoryDatabase(_connectionString).Options;
        _mockDbContext = new Mock<EnvueDbContext>(options);
        _mockDbContextFactory = new Mock<EnvueDbContextFactory>(_connectionString);

        _repository = new Repository<User>(_mockDbContextFactory.Object);

        _username = Guid.NewGuid().ToString();
    }

    [TearDown]
    public async Task TearDown()
    {
        // Delete the record with the random ID from the database
        await _repository.Delete(_username);

        _mockDbContextFactory.Reset();
        _mockDbContext.Reset();
        _mockDbSet.Reset();
    }

    [Test]
    public async Task Create_WithValidEntity_ReturnsTrue()
    {
        // Arrange
        User entity = new User { Id = _username, Name = "test" };
        _mockDbSet.Setup(x => x.AddAsync(entity, It.IsAny<CancellationToken>())).Returns((User model, CancellationToken token) => new ValueTask<EntityEntry<User>>());
        _mockDbContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        bool result = await _repository.Create(entity);

        // Assert
        Assert.IsTrue(result);
        _mockDbSet.Verify(s => s.AddAsync(entity, default), Times.Once());
        _mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
    }

    [Test]
    public async Task Create_WithInvalidEntity_ReturnsFalse()
    {
        // Arrange
        User entity = new User { Id = "", Name = "test" };
        _mockDbSet.Setup(s => s.AddAsync(entity, default)).ThrowsAsync(new Exception());

        // Act
        bool result = await _repository.Create(entity);

        // Assert
        Assert.IsFalse(result);
        _mockDbSet.Verify(s => s.AddAsync(entity, default), Times.Once());
        _mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Never());
    }

    [Test]
    public async Task GetById_WithValidId_ReturnsEntity()
    {
        // Arrange
        User entity = new User { Id = "1", Name = "test" };
        object id = 1;
        _mockDbSet.Setup(s => s.FindAsync(id)).ReturnsAsync(entity);

        // Act
        User? result = await _repository.GetById(id);

        // Assert
        Assert.That(result, Is.EqualTo(entity));
        _mockDbSet.Verify(s => s.FindAsync(id), Times.Once());
    }

    [Test]
    public async Task GetById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        object id = "";

        // Act
        User? result = await _repository.GetById(id);

        // Assert
        Assert.IsNull(result);
        _mockDbSet.Verify(s => s.FindAsync(id), Times.Never());
    }

    [Test]
    public async Task GetAll_ReturnsListOfEntities()
    {
        // Arrange
        List<User> entities = new List<User> { new User() { Id = "123" }, new User() { Id = "456" } };

        _mockDbSet.As<IAsyncEnumerable<User>>().Setup(s => s.ToListAsync()).ReturnsAsync(entities);

        // Act
        IEnumerable<User> result = await _repository.GetAll();

        // Assert
        Assert.That(result, Is.EqualTo(entities));
        _mockDbSet.Verify(s => s.ToListAsync(), Times.Once());
    }

    [Test]
    public async Task Update_WithValidIdAndEntity_UpdatesEntity()
    {
        // Arrange
        User entity = new User();
        User updatedEntity = new User();
        object id = 1;
        _mockDbSet.Setup(s => s.FindAsync(id)).ReturnsAsync(entity);
        _mockDbContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        await _repository.Update(id, updatedEntity);

        // Assert
        _mockDb.Set.Verify(s => s.FindAsync(id), Times.Once());
        _mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        // Assert updated values have been copied to entity
        //Assert.AreEqual(updatedEntity, entity);
    }

    [Test]
    public async Task Update_WithInvalidId_CreatesEntity()
    {
        // Arrange
        User entity = new User();
        User updatedEntity = new User();
        object id = 1;
        _mockDbSet.Setup(s => s.FindAsync(id)).ReturnsAsync((T)null);
        _mockDbSet.Setup(s => s.AddAsync(updatedEntity, default)).ReturnsAsync(updatedEntity);
        _mockDbContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        await _repository.Update(id, updatedEntity);

        // Assert
        _mockDbSet.Verify(s => s.FindAsync(id), Times.Once());
        _mockDbSet.Verify(s => s.AddAsync(updatedEntity, default), Times.Once());
        _mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
    }

    [Test]
    public async Task Update_WithValidIdAndEntity_DeletesEntityAndCreatesNewEntity()
    {
        // Arrange
        User entity = new User();
        User updatedEntity = new User();
        object id = 1;
        _mockDbSet.Setup(s => s.FindAsync(id)).ReturnsAsync(entity);
        _mockDbSet.Setup(s => s.Remove(entity)).Returns(entity);
        _mockDbSet.Setup(s => s.AddAsync(updatedEntity, default)).ReturnsAsync(updatedEntity);
        _mockDbContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        await _repository.Update(id, updatedEntity);

        // Assert
        _mockDbSet.Verify(s => s.FindAsync(id), Times.Once());
        _mockDbSet.Verify(s => s.Remove(entity), Times.Once());
        _mockDbSet.Verify(s => s.AddAsync(updatedEntity, default), Times.Once());
        _mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Exactly(2));
    }

    [Test]
    public async Task Delete_WithValidId_DeletesEntity()
    {
        // Arrange
        User entity = new();
        object id = 1;
        _mockDbSet.Setup(s => s.FindAsync(id)).ReturnsAsync(entity);
        _mockDbContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        await _repository.Delete(id);

        // Assert
        _mockDbSet.Verify(s => s.FindAsync(id), Times.Once());
        _mockDbSet.Verify(s => s.Remove(entity), Times.Once());
        _mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
    }

    [Test]
    public async Task Delete_WithInvalidId_DoesNotDeleteEntity()
    {
        // Arrange
        object id = 1;
        _mockDbSet.Setup(s => s.FindAsync(id)).ReturnsAsync((User?)null);

        // Act
        await _repository.Delete(id);

        // Assert
        _mockDbSet.Verify(s => s.FindAsync(id), Times.Once());
        _mockDbSet.Verify(s => s.Remove(It.IsAny<User>()), Times.Never());
        _mockDbContext.Verify(c => c.SaveChangesAsync(default), Times.Never());
    }




}

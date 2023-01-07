﻿


namespace TestDataAccess;

public class TestRepository
{
    private Mock<EnvueDbContextFactory> _mockDbContextFactory;
    private Mock<DbContext> _mockDbContext;
    private Mock<DbSet<User>> _mockDbSet;
    private Repository<User> _repository;

    [SetUp]
    public void SetUp()
    {
        _mockDbContextFactory = new Mock<EnvueDbContextFactory>();
        _mockDbContext = new Mock<DbContext>();
        _mockDbSet = new Mock<DbSet<User>>();

        _mockDbContextFactory.Setup(f => f.CreateDbContext()).Returns(_mockDbContext.Object);
        _mockDbContext.Setup(c => c.Set<User>()).Returns(_mockDbSet.Object);

        _repository = new Repository<User>(_mockDbContextFactory.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _mockDbContextFactory.ReseUser();
        _mockDbContext.ReseUser();
        _mockDbSet.ReseUser();
    }

    [Test]
    public async Task Create_WithValidEntity_ReturnsTrue()
    {
        // Arrange
        T entity = new User();
        _mockDbSet.Setup(s => s.AddAsync(entity, default)).ReturnsAsync(entity);
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
        User entity = new User();
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
        User entity = new User();
        object id = 1;
        _mockDbSet.Setup(s => s.FindAsync(id)).ReturnsAsync(entity);

        // Act
        User result = await _repository.GetById(id);

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
        User result = await _repository.GetById(id);

        // Assert
        Assert.IsNull(result);
        _mockDbSet.Verify(s => s.FindAsync(id), Times.Never());
    }

    [Test]
    public async Task GetAll_ReturnsListOfEntities()
    {
        // Arrange
        List<User>entities = new List<User>{ new User(), new User() };
        _mockDbSet.As<IAsyncEnumerable<User>>().Setup(s => s.ToListAsync()).ReturnsAsync(entities);

        // Act
        IEnumerable<User>result = await _repository.GetAll();

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

namespace TestDataAccess;


public class RepositoryTests
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
    public async Task Create_ReturnsTrue_WhenEntityIsCreatedSuccessfully()
    {
        // Arrange
        User user = new User { Id = _username, Name = "test" };

        // Configure the mock DbSet object to return a Task that completes successfully when its AddAsync method is called
        _mockDbSet.Setup(x => x.AddAsync(user, It.IsAny<CancellationToken>())).Returns((User model, CancellationToken token) => new ValueTask<EntityEntry<User>>());

        // Configure the mock EnvueDbContext object to return a Task that completes successfully when its SaveChangesAsync method is called
        _mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        // Act
        bool result = await _repository.Create(user);

        // Assert
        Assert.IsTrue(result);

    }

}
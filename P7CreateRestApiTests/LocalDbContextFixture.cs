namespace P7CreateRestApiTests;

public class LocalDbContextFixture : IDisposable
{
    public LocalDbContext Context { get; private set; }

    public LocalDbContextFixture()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        Context = new LocalDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
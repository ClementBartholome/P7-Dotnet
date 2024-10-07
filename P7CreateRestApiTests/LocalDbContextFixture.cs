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
    
    public void ClearDatabase()
    {
        Context.BidLists.RemoveRange(Context.BidLists);
        Context.SaveChanges();
    }
    
    public void Seed(LocalDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.BidLists.AddRange(
            new BidList
            {
                BidListId = 1,
                Account = "Account1",
                BidType = "Type1",
                BidQuantity = 100
            },
            new BidList
            {
                BidListId = 2,
                Account = "Account2",
                BidType = "Type2",
                BidQuantity = 200
            }
        );
        
        context.SaveChanges();
    }
    
    
}
using P7CreateRestApi.Domain;

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
        if (Context.BidLists.Any())
        {
            Context.BidLists.RemoveRange(Context.BidLists);
        }
        
        if (Context.CurvePoints.Any())
        {
            Context.CurvePoints.RemoveRange(Context.CurvePoints);
        }
        
        if (Context.Ratings.Any())
        {
            Context.Ratings.RemoveRange(Context.Ratings);
        }
        
        if (Context.RuleNames.Any())
        {
            Context.RuleNames.RemoveRange(Context.RuleNames);
        }
        
        if (Context.Trades.Any())
        {
            Context.Trades.RemoveRange(Context.Trades);
        }

        if (Context.Users.Any())
        {
            Context.Users.RemoveRange(Context.Users);
        }
        
        if (Context.UserRoles.Any())
        {
            Context.UserRoles.RemoveRange(Context.UserRoles);
        }
        
       

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
        
        context.CurvePoints.AddRange(
            new CurvePoint
            {
                CurveId = 1,
                Term = 1,
                CurvePointValue = 1,
            },
            new CurvePoint
            {
                CurveId = 2,
                Term = 2,
                CurvePointValue = 2,
            }
        );

        context.Ratings.AddRange(
            new Rating
            {
                MoodysRating = "Moodys1",
                SandPRating = "SandP1",
                FitchRating = "Fitch1",
                OrderNumber = 1
            },
            new Rating
            {
                MoodysRating = "Moodys2",
                SandPRating = "SandP2",
                FitchRating = "Fitch2",
                OrderNumber = 2
            }
        );
        
        context.RuleNames.AddRange(
            new RuleName
            {
                Name = "RuleName1",
                Description = "Description1",
                Json = "Json1",
                Template =  "Template1",
                SqlStr = "SqlStr1",
                SqlPart = "SqlPart1"
            },
            new RuleName
            {
                Name = "RuleName2",
                Description = "Description2",
                Json = "Json2",
                Template =  "Template2",
                SqlStr = "SqlStr2",
                SqlPart = "SqlPart2"
                
            }
        );
        
        context.Trades.AddRange(
            new Trade
            {
                Account = "Account1",
                AccountType = "Type1",
                Benchmark = "Benchmark1",
                Book = "Book1",
                CreationName = "CreationName1",
                DealName = "DealName1",
                RevisionName = "RevisionName1",
                TradeSecurity = "TradeSecurity1",
                TradeStatus = "TradeStatus1",
                Trader = "Trader1",
                CreationDate = DateTime.Now,
                RevisionDate = DateTime.Now
            },
            new Trade
            {
                Account = "Account2",
                AccountType = "Type2",
                Benchmark = "Benchmark2",
                Book = "Book2",
                CreationName = "CreationName2",
                DealName = "DealName2",
                RevisionName = "RevisionName2",
                TradeSecurity = "TradeSecurity2",
                TradeStatus = "TradeStatus2",
                Trader = "Trader2",
                CreationDate = DateTime.Now,
                RevisionDate = DateTime.Now
            }
        );

        context.SaveChanges();
    }
    
    
}
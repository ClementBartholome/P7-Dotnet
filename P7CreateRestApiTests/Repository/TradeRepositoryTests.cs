using Xunit.Abstractions;

namespace P7CreateRestApiTests.Repository;

public class TradeRepositoryTests : IClassFixture<LocalDbContextFixture>
{
    private readonly TradeRepository _repository;
    private readonly LocalDbContext _context;
    private readonly ITestOutputHelper _output;
    private readonly LocalDbContextFixture _fixture;

    public TradeRepositoryTests(LocalDbContextFixture fixture, ITestOutputHelper output)
    {
        _context = fixture.Context;
        _repository = new TradeRepository(_context);
        _output = output;
        _fixture = fixture;
    }

    #region GetTrades

    [Fact]
    public async Task GetTrades_ReturnsTrades()
    {
        _fixture.ClearDatabase();
        _fixture.Seed(_context);

        // Act
        var result = await _repository.GetTrades();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Value?.Count());
    }
    [Fact]
    public async Task GetTrades_ReturnsNoContent()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.GetTrades();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Value);
    }
    
    #endregion
    
    #region GetTrade

    [Fact]
    public async Task GetTrade_ReturnsTrade()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        var result = await _repository.GetTrade(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.TradeId);
    }

    [Fact]
    public async Task GetTrade_ReturnsNull()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.GetTrade(1);
        
        // Assert
        Assert.Null(result);
    }
    
    #endregion
    
    #region UpdateTrade

    [Fact]
    public async Task UpdateTrade_ReturnsTrade()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        var trade = new TradeDto
        {
            TradeId = 1,
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
        };

        // Act
        var result = await _repository.UpdateTrade(1, trade);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.TradeId);
        Assert.Equal("Account1", result.Account);
    }
    
    [Fact]
    public async Task UpdateTrade_ReturnsNull()
    {
        // Arrange
        _fixture.ClearDatabase();
        var trade = new TradeDto
        {
            TradeId = 1,
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
        };

        // Act
        var result = await _repository.UpdateTrade(1, trade);

        // Assert
        Assert.Null(result);
    }
    
    #endregion

    #region PostTrade
    
    [Fact]
    public async Task PostTrade_ReturnsTrade()
    {
        // Arrange
        _fixture.ClearDatabase();
        var trade = new TradeDto
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
        };

        // Act
        var result = await _repository.PostTrade(trade);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Account1", result.Account);
    }
    
    #endregion
    
    #region DeleteTrade

    [Fact]
    public async Task DeleteTrade_ReturnsOk()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        await _repository.DeleteTrade(1);
        
        // Assert
        var result = await _repository.GetTrade(1);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteTrade_ReturnsFalse()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.DeleteTrade(1);
        
        // Assert
        Assert.False(result);
    }
    
    #endregion



}
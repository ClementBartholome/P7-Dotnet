
using Xunit.Abstractions;

namespace P7CreateRestApiTests.Repository;

public class BidListRepositoryTests : IClassFixture<LocalDbContextFixture>
{
    private readonly BidRepository _repository;
    private readonly LocalDbContext _context;
    private readonly ITestOutputHelper _output;
    private readonly LocalDbContextFixture _fixture;

    public BidListRepositoryTests(LocalDbContextFixture fixture, ITestOutputHelper output)
    {
        _context = fixture.Context;
        _repository = new BidRepository(_context);
        _output = output;
        _fixture = fixture;
    }

    #region GetBidLists

    [Fact]
    public async Task GetBidLists_ReturnsBidLists()
    {
        _fixture.Seed(_context);

        // Act
        var result = await _repository.GetBidLists();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Value?.Count());
    }
    [Fact]
    public async Task GetBidLists_ReturnsNoContent()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.GetBidLists();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Value);
    }
    
    #endregion
    
    #region GetBidList
    
    [Fact]
    public async Task GetBidList_ReturnsBidList()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        var result = await _repository.GetBidList(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.BidListId);
    }
    
    [Fact]
    public async Task GetBidList_ReturnsNull()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.GetBidList(1);

        // Assert
        Assert.Null(result);
    }
    
    #endregion
    
    #region UpdateBidList
    
    [Fact]
    public async Task UpdateBidList_ReturnsBidList()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        var bidListDto = new BidListDto
        {
            Account = "Account3",
            BidType = "Type3",
            BidQuantity = 300
        };
        
        // Act
        var result = await _repository.UpdateBidList(1, bidListDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.BidListId);
        Assert.Equal("Account3", result.Account);
        Assert.Equal("Type3", result.BidType);
        Assert.Equal(300, result.BidQuantity);
    }
    
    [Fact]
    public async Task UpdateBidList_ReturnsNull()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        var bidListDto = new BidListDto
        {
            Account = "Account3",
            BidType = "Type3",
            BidQuantity = 300
        };
        
        // Act
        var result = await _repository.UpdateBidList(1, bidListDto);

        // Assert
        Assert.Null(result);
    }
    
    #endregion
    
    #region PostBidList
    
    [Fact]
    public async Task PostBidList_ReturnsBidList()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        var bidListDto = new BidListDto
        {
            Account = "Account3",
            BidType = "Type3",
            BidQuantity = 300
        };
        
        // Act
        var result = await _repository.PostBidList(bidListDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.BidListId);
        Assert.Equal("Account3", result.Account);
        Assert.Equal("Type3", result.BidType);
        Assert.Equal(300, result.BidQuantity);
    }
    
    #endregion
    
    #region DeleteBidList
    
    [Fact]
    public async Task DeleteBidList_ReturnsOk()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        await _repository.DeleteBidList(1);

        // Assert
        var result = await _repository.GetBidList(1);
        Assert.Null(result);
    }
    
    #endregion
    
    #region BidListExists
    
    [Fact]
    public void BidListExists_ReturnsTrue()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        var result = _repository.BidListExists(1);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void BidListExists_ReturnsFalse()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = _repository.BidListExists(1);

        // Assert
        Assert.False(result);
    }
    
    #endregion
}
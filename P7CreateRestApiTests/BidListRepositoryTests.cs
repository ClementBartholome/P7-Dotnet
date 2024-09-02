
using Xunit.Abstractions;

namespace P7CreateRestApiTests;

public class BidListRepositoryTests : IClassFixture<LocalDbContextFixture>
{
    private readonly BidRepository _repository;
    private readonly LocalDbContext _context;
    private readonly ITestOutputHelper _output;

    public BidListRepositoryTests(LocalDbContextFixture fixture, ITestOutputHelper output)
    {
        _context = fixture.Context;
        _repository = new BidRepository(_context);
        _output = output;
    }
    
    private void ClearDatabase()
    {
        _context.BidLists.RemoveRange(_context.BidLists);
        _context.SaveChanges();
    }
    
    [Fact]
    public async Task GetBidLists_ReturnsBidLists()
    {
        ClearDatabase();
        
        // Arrange
        var bidList1 = new BidList
        {
            BidListId = 1,
            Account = "Account1",
            BidType = "Type1",
            BidQuantity = 100
        };

        var bidList2 = new BidList
        {
            BidListId = 2,
            Account = "Account2",
            BidType = "Type2",
            BidQuantity = 200
        };

        _context.BidLists.Add(bidList1);
        _context.BidLists.Add(bidList2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetBidLists();

        // Assert
        Assert.NotNull(result.Value);
        var bidListDtos = result.Value.ToList();
        Assert.Equal(2, bidListDtos.Count);

        var returnedBidList1 = bidListDtos.FirstOrDefault(b => b.BidListId == bidList1.BidListId);
        var returnedBidList2 = bidListDtos.FirstOrDefault(b => b.BidListId == bidList2.BidListId);

        
        Assert.NotNull(returnedBidList1);
        Assert.Equal(bidList1.BidListId, returnedBidList1.BidListId);
        _output.WriteLine($"ReturnedBidList1 id: {returnedBidList1.BidListId}");

        Assert.Equal(bidList1.Account, returnedBidList1.Account);
        _output.WriteLine($"Account: {returnedBidList1.Account}");
        
        Assert.Equal(bidList1.BidType, returnedBidList1.BidType);
        _output.WriteLine($"BidType: {returnedBidList1.BidType}");
        
        Assert.Equal(bidList1.BidQuantity, returnedBidList1.BidQuantity);
        _output.WriteLine($"BidQuantity: {returnedBidList1.BidQuantity}");

        Assert.NotNull(returnedBidList2);
        Assert.Equal(bidList2.BidListId, returnedBidList2.BidListId);
        _output.WriteLine($"ReturnedBidList2 id: {returnedBidList2.BidListId}");

        Assert.Equal(bidList2.Account, returnedBidList2.Account);
        _output.WriteLine($"Account: {returnedBidList2.Account}");
        
        Assert.Equal(bidList2.BidType, returnedBidList2.BidType);
        _output.WriteLine($"BidType: {returnedBidList2.BidType}");
        
        Assert.Equal(bidList2.BidQuantity, returnedBidList2.BidQuantity);
        _output.WriteLine($"BidQuantity: {returnedBidList2.BidQuantity}");
    }

    [Fact]
    public async Task GetBidList_ReturnsBidList()
    {
        ClearDatabase();
        
        // Arrange
        var bidList = new BidList
        {
            BidListId = 3,
            Account = "Account1",
            BidType = "Type1",
            BidQuantity = 100
        };

        _context.BidLists.Add(bidList);

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetBidList(3);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bidList.BidListId, result.BidListId);

        _output.WriteLine($"BidListId: {result.BidListId}");

        Assert.Equal(bidList.Account, result.Account);

        _output.WriteLine($"Account: {result.Account}");

        Assert.Equal(bidList.BidType, result.BidType);

        _output.WriteLine($"BidType: {result.BidType}");

        Assert.Equal(bidList.BidQuantity, result.BidQuantity);

        _output.WriteLine($"BidQuantity: {result.BidQuantity}");
    }
    
    [Fact]
    public async Task UpdateBidList_ReturnsUpdatedBidList()
    {
        ClearDatabase();
        
        // Arrange
        var bidList = new BidList
        {
            BidListId = 4,
            Account = "Account1",
            BidType = "Type1",
            BidQuantity = 100
        };

        _context.BidLists.Add(bidList);

        await _context.SaveChangesAsync();

        var updatedBidList = new BidListDto
        {
            BidListId = 4,
            Account = "Account2",
            BidType = "Type2",
            BidQuantity = 9999
        };

        // Act
        var result = await _repository.UpdateBidList(4, updatedBidList);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedBidList.BidListId, result.BidListId);

        _output.WriteLine($"BidListId: {result.BidListId}");

        Assert.Equal(updatedBidList.Account, result.Account);

        _output.WriteLine($"Account: {result.Account}");

        Assert.Equal(updatedBidList.BidType, result.BidType);

        _output.WriteLine($"BidType: {result.BidType}");

        Assert.Equal(updatedBidList.BidQuantity, result.BidQuantity);

        _output.WriteLine($"BidQuantity: {result.BidQuantity}");
    }
    
    [Fact]
    public async Task PostBidList_ReturnsCreatedBidList()
    {
        ClearDatabase();
        
        // Arrange
        var bidList = new BidListDto
        {
            Account = "Account1",
            BidType = "Type1",
            BidQuantity = 100
        };

        // Act
        var result = await _repository.PostBidList(bidList);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bidList.Account, result.Account);

        _output.WriteLine($"Account: {result.Account}");

        Assert.Equal(bidList.BidType, result.BidType);

        _output.WriteLine($"BidType: {result.BidType}");

        Assert.Equal(bidList.BidQuantity, result.BidQuantity);

        _output.WriteLine($"BidQuantity: {result.BidQuantity}");
    }
    
    [Fact]
    public async Task DeleteBidList_ReturnsNoContent()
    {
        ClearDatabase();
        
        // Arrange
        var bidList = new BidList
        {
            BidListId = 5,
            Account = "Account1",
            BidType = "Type1",
            BidQuantity = 100
        };

        _context.BidLists.Add(bidList);

        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteBidList(5);

        // Assert
        var result = await _repository.GetBidList(5);
        Assert.Null(result);
        _output.WriteLine("BidList deleted successfully.");
    }
}
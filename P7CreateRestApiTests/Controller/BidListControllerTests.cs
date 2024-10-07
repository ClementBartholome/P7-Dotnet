using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Interfaces;
using Xunit.Abstractions;

namespace P7CreateRestApiTests.Controller;

public class BidListControllerTests : IClassFixture<LocalDbContextFixture>
{
    private readonly BidListController _controller;
    private readonly Mock<IBidRepository> _repositoryMock = new();


    public BidListControllerTests()
    {
        Mock<ILogger<BidListController>> loggerMock = new();
        _controller = new BidListController(_repositoryMock.Object, loggerMock.Object);
    }
    
    #region GetBidLists

    [Fact]
    public async Task GetBidLists_ReturnsOkResult()
    {
        // Arrange
        var bidLists = new List<BidListDto> { new() { BidListId = 1, Account = "Account1", BidType = "Type1", BidQuantity = 100 } };
        _repositoryMock.Setup(repo => repo.GetBidLists()).ReturnsAsync(bidLists);

        // Act
        var result = await _controller.GetBidLists();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetBidLists_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetBidLists()).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetBidLists();

        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion

    #region GetBidList
    
    [Fact]
    public async Task GetBidList_ReturnsOkResult()
    {
        // Arrange
        var bidList = new BidListDto { BidListId = 1, Account = "Account1", BidType = "Type1", BidQuantity = 100 };
        _repositoryMock.Setup(repo => repo.GetBidList(1)).ReturnsAsync(bidList);

        // Act
        var result = await _controller.GetBidList(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetBidList_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetBidList(1))!.ReturnsAsync((BidListDto?)null);

        // Act
        var result = await _controller.GetBidList(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, (result.Result as NotFoundObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task GetBidList_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetBidList(1)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetBidList(1);

        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region UpdateBidList
    
    [Fact]
    public async Task UpdateBidList_ReturnsOkResult()
    {
        // Arrange
        var bidListDto = new BidListDto { BidListId = 1, Account = "Account1", BidType = "Type1", BidQuantity = 100 };
        var bidList = new BidList { BidListId = 1, Account = "Account1", BidType = "Type1", BidQuantity = 100 };
        _repositoryMock.Setup(repo => repo.UpdateBidList(1, bidListDto)).ReturnsAsync(bidList);
        _repositoryMock.Setup(repo => repo.BidListExists(1)).Returns(true);

        // Act
        var result = await _controller.UpdateBidList(1, bidListDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateBidList_ReturnsNotFound()
    {
        // Arrange
        var bidListDto = new BidListDto { BidListId = 1, Account = "Account1", BidType = "Type1", BidQuantity = 100 };
        _repositoryMock.Setup(repo => repo.UpdateBidList(1, bidListDto)).ReturnsAsync((BidList?)null);

        // Act
        var result = await _controller.UpdateBidList(1, bidListDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task UpdateBidList_ReturnsInternalServerError()
    {
        // Arrange
        var bidListDto = new BidListDto { BidListId = 1, Account = "Account1", BidType = "Type1", BidQuantity = 100 };
        _repositoryMock.Setup(repo => repo.UpdateBidList(1, bidListDto)).ThrowsAsync(new Exception("Test exception"));
        _repositoryMock.Setup(repo => repo.BidListExists(1)).Returns(true);

        // Act
        var result = await _controller.UpdateBidList(1, bidListDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);

        var value = objectResult.Value;
        var messageProperty = value?.GetType().GetProperty("message")?.GetValue(value, null);
        Assert.Equal("An error occurred while updating the BidList.", messageProperty);
    }
        
    
    #endregion
    
    #region PostBidList
    
    [Fact]
    public async Task PostBidList_ReturnsCreatedAtAction()
    {
        // Arrange
        var bidListDto = new BidListDto { Account = "Account1", BidType = "Type1", BidQuantity = 100 };
        var bidList = new BidList { BidListId = 1, Account = "Account1", BidType = "Type1", BidQuantity = 100 };
        _repositoryMock.Setup(repo => repo.PostBidList(bidListDto)).ReturnsAsync(bidList);

        // Act
        var result = await _controller.PostBidList(bidListDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }
    
    [Fact]
    public async Task PostBidList_ReturnsConflict()
    {
        // Arrange
        var bidListDto = new BidListDto { BidListId = 1, Account = "Account1", BidType = "Type1", BidQuantity = 100 };
        _repositoryMock.Setup(repo => repo.BidListExists(1)).Returns(true);

        // Act
        var result = await _controller.PostBidList(bidListDto);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal(409, conflictResult.StatusCode);
    }
    
    [Fact]
    public async Task PostBidList_ReturnsInternalServerError()
    {
        // Arrange
        var bidListDto = new BidListDto { Account = "Account1", BidType = "Type1", BidQuantity = 100 };
        _repositoryMock.Setup(repo => repo.PostBidList(bidListDto)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.PostBidList(bidListDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);

        var value = objectResult.Value;
        var messageProperty = value?.GetType().GetProperty("message")?.GetValue(value, null);
        Assert.Equal("An error occurred while creating the BidList.", messageProperty);
    }
    
    #endregion
    
    #region DeleteBidList

    [Fact]
    public async Task DeleteBidList_ReturnsOkResult()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.BidListExists(1)).Returns(true);

        // Act
        var result = await _controller.DeleteBidList(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteBidList_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.BidListExists(1)).Returns(false);

        // Act
        var result = await _controller.DeleteBidList(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteBidList_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.BidListExists(1)).Throws(new Exception());

        // Act
        var result = await _controller.DeleteBidList(1);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
    
    #endregion
    
}
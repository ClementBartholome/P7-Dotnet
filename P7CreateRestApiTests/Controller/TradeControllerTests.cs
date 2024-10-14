using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApiTests.Controller;

public class TradeControllerTests
{
    private readonly TradeController _controller;
    private readonly Mock<ITradeRepository> _repositoryMock = new();
    
    public TradeControllerTests()
    {
        Mock<ILogger<TradeController>> loggerMock = new();
        _controller = new TradeController(_repositoryMock.Object, loggerMock.Object);
    }
    
    #region GetTrades
    
    [Fact]
    public async Task GetTrades_ReturnsOkResult()
    {
        // Arrange
        var trades = new List<TradeDto> { new() { TradeId = 1, Account = "Account1", AccountType = "Type1" } };
        _repositoryMock.Setup(repo => repo.GetTrades()).ReturnsAsync(trades);
        
        // Act
        var result = await _controller.GetTrades();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetTrades_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetTrades()).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.GetTrades();
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region GetTrade
    
    [Fact]
    public async Task GetTrade_ReturnsOkResult()
    {
        // Arrange
        var trade = new TradeDto { TradeId = 1, Account = "Account1", AccountType = "Type1" };
        _repositoryMock.Setup(repo => repo.GetTrade(1)).ReturnsAsync(trade);
        
        // Act
        var result = await _controller.GetTrade(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetTrade_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetTrade(1)).ReturnsAsync((TradeDto)null);
        
        // Act
        var result = await _controller.GetTrade(1);
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, (result.Result as NotFoundObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task GetTrade_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetTrade(1)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.GetTrade(1);
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region UpdateTrade
    
    [Fact]
    public async Task UpdateTrade_ReturnsOkResult()
    {
        // Arrange
        var trade = new TradeDto { TradeId = 1, Account = "Account1", AccountType = "Type1" };
        _repositoryMock.Setup(repo => repo.UpdateTrade(1, trade)).ReturnsAsync(new Trade { TradeId = 1, Account = "Account1", AccountType = "Type1"});;
        
        // Act
        var result = await _controller.UpdateTrade(1, trade);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateTrade_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.UpdateTrade(1, It.IsAny<TradeDto>())).ReturnsAsync((Trade)null);
        
        // Act
        var result = await _controller.UpdateTrade(1, new TradeDto());
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, (result as NotFoundObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task UpdateTrade_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.UpdateTrade(1, It.IsAny<TradeDto>())).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.UpdateTrade(1, new TradeDto());
        
        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region PostTrade
    
    [Fact]
    public async Task PostTrade_ReturnsCreatedAtAction()
    {
        // Arrange
        var trade = new TradeDto { TradeId = 1, Account = "Account1", AccountType = "Type1" };
        _repositoryMock.Setup(repo => repo.PostTrade(It.IsAny<TradeDto>())).ReturnsAsync(new Trade { TradeId = 1, Account = "Account1", AccountType = "Type1"});;
        
        // Act
        var result = await _controller.PostTrade(new TradeDto());
        
        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }
    
    [Fact]
    public async Task PostTrade_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.PostTrade(It.IsAny<TradeDto>())).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.PostTrade(new TradeDto());
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region DeleteTrade
    
    [Fact]
    public async Task DeleteTrade_ReturnsOkResult()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteTrade(1)).ReturnsAsync(true);
        
        // Act
        var result = await _controller.DeleteTrade(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteTrade_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteTrade(1)).ReturnsAsync(false);
        
        // Act
        var result = await _controller.DeleteTrade(1);
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, (result as NotFoundObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task DeleteTrade_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteTrade(1)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.DeleteTrade(1);
        
        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    
}
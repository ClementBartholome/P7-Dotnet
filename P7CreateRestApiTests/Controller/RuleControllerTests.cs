using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApiTests.Controller;

public class RuleControllerTests
{
    private readonly RuleController _controller;
    private readonly Mock<IRuleRepository> _repositoryMock = new();
    
    public RuleControllerTests()
    {
        Mock<ILogger<RuleController>> loggerMock = new();
        _controller = new RuleController(_repositoryMock.Object, loggerMock.Object);
    }
    
    #region GetRules
    
    [Fact]
    public async Task GetRules_ReturnsOkResult()
    {
        // Arrange
        var rules = new List<RuleDto> { new() { Id = 1, Name = "Rule1", Description = "Description1" } };
        _repositoryMock.Setup(repo => repo.GetRules()).ReturnsAsync(rules);
        
        // Act
        var result = await _controller.GetRules();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetRules_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetRules()).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.GetRules();
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region GetRule
    
    [Fact]
    public async Task GetRule_ReturnsOkResult()
    {
        // Arrange
        var rule = new RuleDto { Id = 1, Name = "Rule1", Description = "Description1" };
        _repositoryMock.Setup(repo => repo.GetRule(1)).ReturnsAsync(rule);
        
        // Act
        var result = await _controller.GetRule(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetRule_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetRule(1)).ReturnsAsync((RuleDto)null);
        
        // Act
        var result = await _controller.GetRule(1);
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, (result.Result as NotFoundObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task GetRule_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetRule(1)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.GetRule(1);
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region UpdateRule
    
    [Fact]
    public async Task UpdateRule_ReturnsOkResult()
    {
        // Arrange
        var rule = new RuleDto { Id = 1, Name = "Rule1", Description = "Description1" };
        _repositoryMock.Setup(repo => repo.UpdateRule(1, rule)).ReturnsAsync(new RuleName{Id = 1, Name = "Rule1", Description = "Description1"});
        
        // Act
        var result = await _controller.UpdateRule(1, rule);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateRule_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.UpdateRule(1, It.IsAny<RuleDto>())).ReturnsAsync((RuleName)null);
        
        // Act
        var result = await _controller.UpdateRule(1, new RuleDto());
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, (result.Result as NotFoundObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task UpdateRule_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.UpdateRule(1, It.IsAny<RuleDto>())).ThrowsAsync(new DbUpdateConcurrencyException());
        
        // Act
        var result = await _controller.UpdateRule(1, new RuleDto());
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region PostRule
    
    [Fact]
    public async Task PostRule_ReturnsCreatedAtAction()
    {
        // Arrange
        var rule = new RuleDto { Id = 1, Name = "Rule1", Description = "Description1" };
        _repositoryMock.Setup(repo => repo.PostRule(rule)).ReturnsAsync(new RuleName { Id = 1, Name = "Rule1", Description = "Description1" });
        
        // Act
        var result = await _controller.PostRule(rule);
        
        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }
    
    [Fact]
    public async Task PostRule_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.PostRule(It.IsAny<RuleDto>())).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.PostRule(new RuleDto());
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region DeleteRule
    
    [Fact]
    public async Task DeleteRule_ReturnsOkResult()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteRule(1)).ReturnsAsync(true);
        
        // Act
        var result = await _controller.DeleteRule(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteRule_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteRule(1)).ReturnsAsync(false);
        
        // Act
        var result = await _controller.DeleteRule(1);
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, (result.Result as NotFoundObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task DeleteRule_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteRule(1)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.DeleteRule(1);
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
}
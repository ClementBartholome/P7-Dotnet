using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Interfaces;

namespace P7CreateRestApiTests.Controller;

public class CurveControllerTests
{
    private readonly CurveController _controller;
    private readonly Mock<ICurveRepository> _repositoryMock = new();
    
    public CurveControllerTests()
    {
        Mock<ILogger<CurveController>> loggerMock = new();
        _controller = new CurveController(_repositoryMock.Object, loggerMock.Object);
    }
    
    #region GetCurves
    
    [Fact]
    public async Task GetCurves_ReturnsOkResult()
    {
        // Arrange
        var curvePoints = new List<CurvePointDto> { new() { Id = 1, CurveId = 1, CurvePointValue = 100 } };
        _repositoryMock.Setup(repo => repo.GetCurves()).ReturnsAsync(curvePoints);
        
        // Act
        var result = await _controller.GetCurves();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetCurves_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetCurves()).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.GetCurves();
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region GetCurve
    
    [Fact]
    public async Task GetCurve_ReturnsOkResult()
    {
        // Arrange
        var curvePoint = new CurvePointDto { Id = 1, CurveId = 1, CurvePointValue = 100 };
        _repositoryMock.Setup(repo => repo.GetCurve(1)).ReturnsAsync(curvePoint);
        
        // Act
        var result = await _controller.GetCurve(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetCurve_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetCurve(1)).ReturnsAsync((CurvePointDto) null);
        
        // Act
        var result = await _controller.GetCurve(1);
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, (result.Result as NotFoundObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task GetCurve_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetCurve(1)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.GetCurve(1);
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region UpdateCurve
    
    [Fact]
    public async Task UpdateCurve_ReturnsOkResult()
    {
        // Arrange
        var curvePoint = new CurvePointDto { Id = 1, CurveId = 1, CurvePointValue = 100 };
        _repositoryMock.Setup(repo => repo.UpdateCurve(1, curvePoint)).ReturnsAsync(new CurvePoint { Id = 1, CurveId = 1, CurvePointValue = 100 });
        
        // Act
        var result = await _controller.UpdateCurve(1, curvePoint);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateCurve_ReturnsNotFound()
    {
        // Arrange
        var curvePoint = new CurvePointDto { Id = 1, CurveId = 1, CurvePointValue = 100 };
        _repositoryMock.Setup(repo => repo.UpdateCurve(1, curvePoint)).ReturnsAsync((CurvePoint) null);
        
        // Act
        var result = await _controller.UpdateCurve(1, curvePoint);
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, (result as NotFoundObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task UpdateCurve_ReturnsBadRequest()
    {
        // Arrange
        var curvePoint = new CurvePointDto { Id = 1, CurveId = 1, CurvePointValue = 100 };
        _repositoryMock.Setup(repo => repo.UpdateCurve(1, curvePoint)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.UpdateCurve(1, curvePoint);
        
        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region PostCurve
    
    [Fact]
    public async Task PostCurve_ReturnsCreatedAtAction()
    {
        // Arrange
        var curvePoint = new CurvePointDto { Id = 1, CurveId = 1, CurvePointValue = 100 };
        _repositoryMock.Setup(repo => repo.PostCurve(curvePoint)).ReturnsAsync(new CurvePoint { Id = 1, CurveId = 1, CurvePointValue = 100 });
        
        // Act
        var result = await _controller.PostCurve(curvePoint);
        
        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }
    
    [Fact]
    public async Task PostCurve_ReturnsConflict()
    {
        // Arrange
        var curvePoint = new CurvePointDto { Id = 1, CurveId = 1, CurvePointValue = 100 };
        _repositoryMock.Setup(repo => repo.PostCurve(curvePoint)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.PostCurve(curvePoint);
        
        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region DeleteCurve
    
    [Fact]
    public async Task DeleteCurve_ReturnsOkResult()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteCurve(1)).ReturnsAsync(true);
        
        // Act
        var result = await _controller.DeleteCurve(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteCurve_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteCurve(1)).ReturnsAsync(false);
        
        // Act
        var result = await _controller.DeleteCurve(1);
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, (result as NotFoundObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task DeleteCurve_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteCurve(1)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _controller.DeleteCurve(1);
        
        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    

    
}
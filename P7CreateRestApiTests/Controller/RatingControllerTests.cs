using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Controllers;

namespace P7CreateRestApiTests.Controller;

public class RatingControllerTests
{
    private readonly RatingController _controller;
    private readonly Mock<IRatingRepository> _repositoryMock = new();
    
    public RatingControllerTests()
    {
        Mock<ILogger<RatingController>> loggerMock = new();
        _controller = new RatingController(_repositoryMock.Object, loggerMock.Object);
    }
    
    #region GetRatings
    
    [Fact]
    public async Task GetRatings_ReturnsOkResult()
    {
        // Arrange
        var ratings = new List<RatingDto> { new() { Id = 1, MoodysRating = "Moodys1", SandPRating = "SandP1", FitchRating = "Fitch1", OrderNumber = 1 } };
        _repositoryMock.Setup(repo => repo.GetRatings()).ReturnsAsync(ratings);

        // Act
        var result = await _controller.GetRatings();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetRatings_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetRatings()).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetRatings();

        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region GetRating
    
    [Fact]
    public async Task GetRating_ReturnsOkResult()
    {
        // Arrange
        var rating = new RatingDto { Id = 1, MoodysRating = "Moodys1", SandPRating = "SandP1", FitchRating = "Fitch1", OrderNumber = 1 };
        _repositoryMock.Setup(repo => repo.GetRating(1)).ReturnsAsync(rating);

        // Act
        var result = await _controller.GetRating(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetRating_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetRating(1)).ReturnsAsync((RatingDto)null);

        // Act
        var result = await _controller.GetRating(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task GetRating_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetRating(1)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetRating(1);

        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }
    
    #endregion
    
    #region UpdateRating
    
    [Fact]
    public async Task UpdateRating_ReturnsOkResult()
    {
        // Arrange
        var ratingDto = new RatingDto { Id = 1, MoodysRating = "Moodys1", SandPRating = "SandP1", FitchRating = "Fitch1", OrderNumber = 1 };
        _repositoryMock.Setup(repo => repo.UpdateRating(1, ratingDto)).ReturnsAsync(new Rating
        {
            Id = 1, MoodysRating = "Moodys1", SandPRating = "SandP1", FitchRating = "Fitch1", OrderNumber = 1
        });

        // Act
        var result = await _controller.UpdateRating(1, ratingDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateRating_ReturnsNotFound()
    {
        // Arrange
        var ratingDto = new RatingDto { Id = 1, MoodysRating = "Moodys1", SandPRating = "SandP1", FitchRating = "Fitch1", OrderNumber = 1 };
        _repositoryMock.Setup(repo => repo.UpdateRating(1, ratingDto)).ReturnsAsync((Rating)null);

        // Act
        var result = await _controller.UpdateRating(1, ratingDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task UpdateRating_ReturnsInternalServerErrror()
    {
        // Arrange
        var ratingDto = new RatingDto { Id = 1, MoodysRating = "Moodys1", SandPRating = "SandP1", FitchRating = "Fitch1", OrderNumber = 1 };
        _repositoryMock.Setup(repo => repo.UpdateRating(1, ratingDto)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.UpdateRating(1, ratingDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        
        var value = objectResult.Value;
        var messageProperty = value?.GetType().GetProperty("message")?.GetValue(value, null);
        Assert.Equal("An error occurred while updating the Rating.", messageProperty);
    }
    
    #endregion

    #region PostRating

    [Fact]
    public async Task PostRating_ReturnsCreatedAtAction()
    {
        // Arrange
        var ratingDto = new RatingDto { MoodysRating = "Moodys1", SandPRating = "SandP1", FitchRating = "Fitch1", OrderNumber = 1 };
        var rating = new Rating { MoodysRating = "Moodys1", SandPRating = "SandP1", FitchRating = "Fitch1", OrderNumber = 1 };
        _repositoryMock.Setup(repo => repo.PostRating(ratingDto)).ReturnsAsync(rating);
        
        // Act
        var result = await _controller.PostRating(ratingDto);
        
        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }
    
    [Fact]
    public async Task PostRating_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.PostRating(It.IsAny<RatingDto>())).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.PostRating(new RatingDto());

        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, (result.Result as ObjectResult)?.StatusCode);
    }

    #endregion
    
    #region DeleteRating
    
    [Fact]
    public async Task DeleteRating_ReturnsOkResult()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteRating(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteRating(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteRating_ReturnsNotFound()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteRating(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteRating(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteRating_ReturnsBadRequest()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteRating(1)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.DeleteRating(1);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
    
    #endregion
}
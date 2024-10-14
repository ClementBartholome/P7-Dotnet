using Xunit.Abstractions;

namespace P7CreateRestApiTests.Repository;

public class RatingRepositoryTests : IClassFixture<LocalDbContextFixture>
{
    private readonly RatingRepository _repository;
    private readonly LocalDbContext _context;
    private readonly ITestOutputHelper _output;
    private readonly LocalDbContextFixture _fixture;
    

    public RatingRepositoryTests(LocalDbContextFixture fixture, ITestOutputHelper output)
    {
        _context = fixture.Context;
        _repository = new RatingRepository(_context);
        _output = output;
        _fixture = fixture;
    }

    #region GetRatings
    
    [Fact]
    public async Task GetRatings_ReturnsRatings()
    {
        _fixture.ClearDatabase();
        _fixture.Seed(_context);

        // Act
        var result = await _repository.GetRatings();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Value?.Count());
    }
    
    [Fact]
    public async Task GetRatings_ReturnsNoContent()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.GetRatings();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Value);
    }
    
    #endregion
    
    #region GetRating
    
    [Fact]
    public async Task GetRating_ReturnsRating()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        var result = await _repository.GetRating(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result?.Id);
    }
    
    [Fact]
    public async Task GetRating_ReturnsNull()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.GetRating(1);

        // Assert
        Assert.Null(result);
    }
    
    #endregion
    
    #region UpdateRating
    
    [Fact]
    public async Task UpdateRating_ReturnsRating()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        var ratingDto = new RatingDto
        {
            MoodysRating = "MoodysRating",
            SandPRating = "SandPRating",
            FitchRating = "FitchRating",
            OrderNumber = 1
        };
        
        // Act
        var result = await _repository.UpdateRating(1, ratingDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result?.Id);
    }
    
    [Fact]
    public async Task UpdateRating_ReturnsNull()
    {
        // Arrange
        _fixture.ClearDatabase();
        var ratingDto = new RatingDto
        {
            MoodysRating = "MoodysRating",
            SandPRating = "SandPRating",
            FitchRating = "FitchRating",
            OrderNumber = 1
        };
        
        // Act
        var result = await _repository.UpdateRating(1, ratingDto);

        // Assert
        Assert.Null(result);
    }
    
    #endregion
    
    #region PostRating
    
    [Fact]
    public async Task PostRating_ReturnsRating()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        var ratingDto = new RatingDto
        {
            MoodysRating = "MoodysRating",
            SandPRating = "SandPRating",
            FitchRating = "FitchRating",
            OrderNumber = 1
        };
        
        // Act
        var result = await _repository.PostRating(ratingDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("MoodysRating", ratingDto.MoodysRating);
        Assert.Equal("SandPRating", ratingDto.SandPRating);
        Assert.Equal("FitchRating", ratingDto.FitchRating);
        Assert.Equal(1, (int)ratingDto.OrderNumber);
    }
    
    #endregion
    
    #region DeleteRating
    
    [Fact]
    public async Task DeleteRating_ReturnsTrue()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        var result = await _repository.DeleteRating(1);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteRating_ReturnsFalse()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.DeleteRating(1);

        // Assert
        Assert.False(result);
    }
    
    #endregion
    

    
    
}
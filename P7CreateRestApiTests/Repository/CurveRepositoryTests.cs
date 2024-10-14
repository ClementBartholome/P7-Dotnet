using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace P7CreateRestApiTests.Repository;

public class CurveRepositoryTests : IClassFixture<LocalDbContextFixture>
{
    private readonly CurveRepository _repository;
    private readonly LocalDbContext _context;
    private readonly ITestOutputHelper _output;
    private readonly LocalDbContextFixture _fixture;

    public CurveRepositoryTests(LocalDbContextFixture fixture, ITestOutputHelper output)
    {
        _context = fixture.Context;
        _repository = new CurveRepository(_context);
        _output = output;
        _fixture = fixture;
    }

    #region GetCurves

    [Fact]
    public async Task GetCurves_ReturnsCurves()
    {
        _fixture.ClearDatabase();
        _fixture.Seed(_context);

        // Act
        var result = await _repository.GetCurves();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Value?.Count());
    }
    [Fact]
    public async Task GetCurves_ReturnsNoContent()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.GetCurves();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Value);
    }
    
    #endregion
    
    #region GetCurve

    [Fact]
    public async Task GetCurve_ReturnsCurve()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        var result = await _repository.GetCurve(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.CurveId);
    }

    [Fact]
    public async Task GetCurve_ReturnsNull()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.GetCurve(1);

        // Assert
        Assert.Null(result);
    }
    #endregion

    #region UpdateCurve

    [Fact]
    public async Task UpdateCurve_ReturnsUpdatedCurve()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        var curvePointDto = new CurvePointDto
        {
            Id = 1,
            CurveId = 1,
            Term = 1,
            CurvePointValue = 1
        };
        
        // Act
        var result = await _repository.UpdateCurve(1, curvePointDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.CurveId);
    }
    
    [Fact]
    public async Task UpdateCurve_ReturnsNull()
    {
        // Arrange
        _fixture.ClearDatabase();
        var curvePointDto = new CurvePointDto
        {
            Id = 1,
            CurveId = 1,
            Term = 1,
            CurvePointValue = 1
        };
        
        // Act
        var result = await _repository.UpdateCurve(1, curvePointDto);

        // Assert
        Assert.Null(result);
    }

    #endregion
    
    #region PostCurve
    
    [Fact]
    public async Task PostCurve_ReturnsCreatedCurve()
    {
        // Arrange
        _fixture.ClearDatabase();
        var curvePointDto = new CurvePointDto
        {
            CurveId = 1,
            Term = 1,
            CurvePointValue = 1
        };
        
        // Act
        var result = await _repository.PostCurve(curvePointDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.CurveId);
    }
    
    #endregion
    
    #region DeleteCurve
    
    [Fact]
    public async Task DeleteCurve_ReturnsTrue()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        var result = await _repository.DeleteCurve(1);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteCurve_ReturnsFalse()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.DeleteCurve(1);

        // Assert
        Assert.False(result);
    }
    
    #endregion
    
}
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace P7CreateRestApiTests.Repository;

public class RuleRepositoryTests : IClassFixture<LocalDbContextFixture>
{
    private readonly RuleRepository _repository;
    private readonly LocalDbContext _context;
    private readonly ITestOutputHelper _output;
    private readonly LocalDbContextFixture _fixture;

    public RuleRepositoryTests(LocalDbContextFixture fixture, ITestOutputHelper output)
    {
        _context = fixture.Context;
        _repository = new RuleRepository(_context);
        _output = output;
        _fixture = fixture;
    }

    #region GetRules

    [Fact]
    public async Task GetRules_ReturnsRules()
    {
        _fixture.ClearDatabase();
        _fixture.Seed(_context);

        // Act
        var result = await _repository.GetRules();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Value?.Count());
    }
    [Fact]
    public async Task GetRules_ReturnsNoContent()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.GetRules();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Value);
    }
    
    #endregion
    
    #region GetRule

    [Fact]
    public async Task GetRule_ReturnsRule()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        var result = await _repository.GetRule(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }
    [Fact]
    public async Task GetRule_ReturnsNull()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.GetRule(1);

        // Assert
        Assert.Null(result);
    }
    
    #endregion
    
    #region UpdateRule
    
    [Fact]
    public async Task UpdateRule_ReturnsRule()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        var ruleDto = new RuleDto
        {
            Name = "New Rule",
            Description = "New Description",
            Json = "New Json",
            Template = "New Template",
            SqlStr = "New SqlStr",
            SqlPart = "New SqlPart"
        };
        
        // Act
        var result = await _repository.UpdateRule(1, ruleDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Rule", result.Name);
        Assert.Equal("New Description", result.Description);
        Assert.Equal("New Json", result.Json);
        Assert.Equal("New Template", result.Template);
        Assert.Equal("New SqlStr", result.SqlStr);
        Assert.Equal("New SqlPart", result.SqlPart);
    }
    
    [Fact]
    public async Task UpdateRule_ReturnsNull()
    {
        // Arrange
        _fixture.ClearDatabase();
        var ruleDto = new RuleDto
        {
            Name = "New Rule",
            Description = "New Description",
            Json = "New Json",
            Template = "New Template",
            SqlStr = "New SqlStr",
            SqlPart = "New SqlPart"
        };
        
        // Act
        var result = await _repository.UpdateRule(1, ruleDto);

        // Assert
        Assert.Null(result);
    }
    
    #endregion
    
    #region PostRule
    
    [Fact]
    public async Task PostRule_ReturnsRule()
    {
        // Arrange
        _fixture.ClearDatabase();
        var ruleDto = new RuleDto
        {
            Name = "New Rule",
            Description = "New Description",
            Json = "New Json",
            Template = "New Template",
            SqlStr = "New SqlStr",
            SqlPart = "New SqlPart"
        };
        
        // Act
        var result = await _repository.PostRule(ruleDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Rule", result.Name);
        Assert.Equal("New Description", result.Description);
        Assert.Equal("New Json", result.Json);
        Assert.Equal("New Template", result.Template);
        Assert.Equal("New SqlStr", result.SqlStr);
        Assert.Equal("New SqlPart", result.SqlPart);
    }
    
    #endregion
    
    #region DeleteRule
    
    [Fact]
    public async Task DeleteRule_ReturnsTrue()
    {
        // Arrange
        _fixture.ClearDatabase();
        _fixture.Seed(_context);
        
        // Act
        var result = await _repository.DeleteRule(1);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteRule_ReturnsFalse()
    {
        // Arrange
        _fixture.ClearDatabase();
        
        // Act
        var result = await _repository.DeleteRule(1);

        // Assert
        Assert.False(result);
    }
    
    #endregion
}
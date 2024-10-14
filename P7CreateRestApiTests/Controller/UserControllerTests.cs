using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Controllers;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Interfaces;

namespace P7CreateRestApiTests.Controller;

public class UserControllerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<UserController>> _loggerMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<UserController>>();
        _controller = new UserController(_userRepositoryMock.Object, _loggerMock.Object);
    }

    #region  GetUser
    
    [Fact]
    public async Task GetUser_ReturnsOk()
    {
        // Arrange
        var userId = "testUserId";
        var user = new User();
        _userRepositoryMock.Setup(repo => repo.GetUser(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetUser_ReturnsNotFound()
    {
        // Arrange
        var userId = "testUserId";
        _userRepositoryMock.Setup(repo => repo.GetUser(userId))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task GetUser_ReturnsInternalServerError()
    {
        // Arrange
        var userId = "testUserId";
        _userRepositoryMock.Setup(repo => repo.GetUser(userId))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
        var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    #endregion
    
    #region GetUsers
    
    [Fact]
    public async Task GetUsers_ReturnsOk()
    {
        // Arrange
        var users = new List<User>();
        _userRepositoryMock.Setup(repo => repo.GetUsers())
            .ReturnsAsync(users);

        // Act
        var result = await _controller.GetUsers();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task GetUsers_ReturnsInternalServerError()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetUsers())
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetUsers();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
        var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
    
    #endregion

    #region AddRolesToUser
    
    [Fact]
    public async Task AddRolesToUser_UserIsAdmin_ReturnsOk()
    {
        // Arrange
        var userId = "testUserId";
        var roles = new List<string> { "Admin" };
        _userRepositoryMock.Setup(repo => repo.AddRolesToUser(userId, roles))
            .ReturnsAsync((true, new List<string>()));

        // Act
        var result = await _controller.AddRolesToUser(userId, roles);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }


    [Fact]
    public async Task AddRolesToUser_ReturnsBadRequest()
    {
        // Arrange
        var userId = "testUserId";
        var roles = new List<string> { "Admin" };
        _userRepositoryMock.Setup(repo => repo.AddRolesToUser(userId, roles))
            .ReturnsAsync((false, new List<string> { "User already has the provided role." }));

        // Act
        var result = await _controller.AddRolesToUser(userId, roles);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task AddRolesToUser_ReturnsInternalServerError()
    {
        // Arrange
        var userId = "testUserId";
        var roles = new List<string> { "Admin" };
        _userRepositoryMock.Setup(repo => repo.AddRolesToUser(userId, roles))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.AddRolesToUser(userId, roles);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    #endregion
    
    #region DeleteUser
    
    [Fact]
    public async Task DeleteUser_ReturnsOk()
    {
        // Arrange
        var userId = "testUserId";
        _userRepositoryMock.Setup(repo => repo.DeleteUser(userId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteUser_ReturnsNotFound()
    {
        // Arrange
        var userId = "testUserId";
        _userRepositoryMock.Setup(repo => repo.DeleteUser(userId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteUser_ReturnsInternalServerError()
    {
        // Arrange
        var userId = "testUserId";
        _userRepositoryMock.Setup(repo => repo.DeleteUser(userId))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
    
    #endregion
    
    #region UpdateUser
    
    [Fact]
    public async Task UpdateUser_ReturnsOk()
    {
        // Arrange
        var userId = "testUserId";
        var userDto = new UserDto { Id = userId };
        _userRepositoryMock.Setup(repo => repo.UpdateUser(userId, userDto))
            .ReturnsAsync(new User());

        // Act
        var result = await _controller.UpdateUser(userId, userDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateUser_ReturnsNotFound()
    {
        // Arrange
        var userId = "testUserId";
        var userDto = new UserDto { Id = userId };
        _userRepositoryMock.Setup(repo => repo.UpdateUser(userId, userDto))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _controller.UpdateUser(userId, userDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateUser_ReturnsBadRequest()
    {
        // Arrange
        var userId = "testUserId";
        var userDto = new UserDto { Id = "differentId" };

        // Act
        var result = await _controller.UpdateUser(userId, userDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateUser_ReturnsInternalServerError()
    {
        // Arrange
        var userId = "testUserId";
        var userDto = new UserDto { Id = userId };
        _userRepositoryMock.Setup(repo => repo.UpdateUser(userId, userDto))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.UpdateUser(userId, userDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
    
    #endregion

    #region RemoveRoleFromUser
    
    [Fact]
    public async Task RemoveRoleFromUser_ReturnsOk()
    {
        // Arrange
        var userId = "testUserId";
        var role = "Admin";
        _userRepositoryMock.Setup(repo => repo.RemoveRoleFromUser(userId, role))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.RemoveRoleFromUser(userId, role);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task RemoveRoleFromUser_ReturnsNotFound()
    {
        // Arrange
        var userId = "testUserId";
        var role = "Admin";
        _userRepositoryMock.Setup(repo => repo.RemoveRoleFromUser(userId, role))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.RemoveRoleFromUser(userId, role);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task RemoveRoleFromUser_ReturnsInternalServerError()
    {
        // Arrange
        var userId = "testUserId";
        var role = "Admin";
        _userRepositoryMock.Setup(repo => repo.RemoveRoleFromUser(userId, role))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.RemoveRoleFromUser(userId, role);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    #endregion
    
    #region UpdateRoleForUser
    
    [Fact]
    public async Task UpdateRoleForUser_ReturnsOk()
    {
        // Arrange
        var userId = "testUserId";
        var role = "Admin";
        _userRepositoryMock.Setup(repo => repo.UpdateRoleForUser(userId, role))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateRoleForUser(userId, role);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateRoleForUser_ReturnsNotFound()
    {
        // Arrange
        var userId = "testUserId";
        var role = "Admin";
        _userRepositoryMock.Setup(repo => repo.UpdateRoleForUser(userId, role))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateRoleForUser(userId, role);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task UpdateRoleForUser_ReturnsInternalServerError()
    {
        // Arrange
        var userId = "testUserId";
        var role = "Admin";
        _userRepositoryMock.Setup(repo => repo.UpdateRoleForUser(userId, role))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.UpdateRoleForUser(userId, role);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
    
    #endregion
    
    #region CreateRole
    
    [Fact]
    public async Task CreateRole_ReturnsOk()
    {
        // Arrange
        var roleName = "Admin";
        _userRepositoryMock.Setup(repo => repo.CreateRole(roleName))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.CreateRole(roleName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task CreateRole_ReturnsBadRequest()
    {
        // Arrange
        var roleName = "Admin";
        _userRepositoryMock.Setup(repo => repo.CreateRole(roleName))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.CreateRole(roleName);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task CreateRole_ReturnsInternalServerError()
    {
        // Arrange
        var roleName = "Admin";
        _userRepositoryMock.Setup(repo => repo.CreateRole(roleName))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.CreateRole(roleName);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
    
    #endregion
    
    #region DeleteRole
    
    [Fact]
    public async Task DeleteRole_ReturnsOk()
    {
        // Arrange
        var roleName = "Admin";
        _userRepositoryMock.Setup(repo => repo.DeleteRole(roleName))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteRole(roleName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteRole_ReturnsNotFound()
    {
        // Arrange
        var roleName = "Admin";
        _userRepositoryMock.Setup(repo => repo.DeleteRole(roleName))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteRole(roleName);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteRole_ReturnsInternalServerError()
    {
        // Arrange
        var roleName = "Admin";
        _userRepositoryMock.Setup(repo => repo.DeleteRole(roleName))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.DeleteRole(roleName);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
    
    #endregion
}
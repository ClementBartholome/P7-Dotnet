using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models.Dto;
using P7CreateRestApi.Repositories;
using Xunit;

namespace P7CreateRestApiTests.Repository
{
    public class UserRepositoryTests : IClassFixture<LocalDbContextFixture>
    {
        private readonly UserRepository _repository;
        private readonly LocalDbContext _context;
        private readonly LocalDbContextFixture _fixture;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRepositoryTests(LocalDbContextFixture fixture)
        {
            _context = fixture.Context;
            _passwordHasher = new PasswordHasher<User>();
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, _passwordHasher, null, null, null, null, null, null);
            _repository = new UserRepository(_context, _userManagerMock.Object);
            _fixture = fixture;
        }

        #region GetUsers
        [Fact]
        public async Task GetUsers_ReturnsUsers()
        {
            // Arrange
            _fixture.ClearDatabase();
            _context.Users.Add(new User { Id = "1", UserName = "User1", FullName = "Full Name 1" });
            _context.Users.Add(new User { Id = "2", UserName = "User2", FullName = "Full Name 2" });
            _context.SaveChanges();

            // Act
            var result = await _repository.GetUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Value.Count());
        }
        #endregion

        #region GetUser
        [Fact]
        public async Task GetUser_ReturnsUser()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1", FullName = "Full Name 1" };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetUser("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User1", result.UserName);
        }
        #endregion

        #region UpdateUser
        [Fact]
        public async Task UpdateUser_ReturnsUpdatedUser()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1", FullName = "Full Name 1" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var userDto = new UserDto { UserName = "UpdatedUser", FullName = "Updated Full Name" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.UpdateUser("1", userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedUser", result.UserName);
            Assert.Equal("Updated Full Name", result.FullName);
        }

        [Fact]
        public async Task UpdateUser_WithPassword_ReturnsUpdatedUser()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1", FullName = "Full Name 1" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var userDto = new UserDto { UserName = "UpdatedUser", FullName = "Updated Full Name", Password = "Password" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.UpdateUser("1", userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedUser", result.UserName);
            Assert.Equal("Updated Full Name", result.FullName);
            Assert.NotNull(result.PasswordHash);
        }

        [Fact]
        public async Task UpdateUser_WhenUserIsNull_ReturnsFalse()
        {
            // Arrange
            _fixture.ClearDatabase();
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync((User)null);

            // Act
            var result = await _repository.UpdateUser("1", new UserDto());

            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task UpdateUser_WhenUpdateFails_ReturnsFalse()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1", FullName = "Full Name 1" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var userDto = new UserDto { UserName = "UpdatedUser", FullName = "Updated Full Name" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            // Act
            var result = await _repository.UpdateUser("1", userDto);

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region DeleteUser
        [Fact]
        public async Task DeleteUser_ReturnsTrue()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1", FullName = "Full Name 1" };
            _context.Users.Add(user);
            _context.SaveChanges();
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.DeleteUser("1");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUser_UserIsNull_ReturnsFalse()
        {
            // Arrange
            _fixture.ClearDatabase();
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync((User)null);

            // Act
            var result = await _repository.DeleteUser("1");

            // Assert
            Assert.False(result);    
        }
        
        #endregion

        #region AddRolesToUser
        [Fact]
        public async Task AddRolesToUser_ReturnsSuccess()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.IsInRoleAsync(user, It.IsAny<string>())).ReturnsAsync(false);
            _userManagerMock.Setup(um => um.AddToRoleAsync(user, It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.AddRolesToUser("1", new List<string> { "Admin" });

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.AlreadyInRoles);
        }

        [Fact]
        public async Task AddRolesToUser_WhenUserIsNull_ReturnsFalse()
        {
            // Arrange
            _fixture.ClearDatabase();
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync((User)null);

            // Act
            var result = await _repository.AddRolesToUser("1", new List<string> { "Admin" });

            // Assert
            Assert.False(result.Success);
        }
        
        [Fact]
        public async Task AddRolesToUser_WhenRolesListIsNull_ThrowsArgumentException()
        {
            // Arrange
            _fixture.ClearDatabase();

            // Act
            async Task Act() => await _repository.AddRolesToUser("1", null);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async Task AddRolesToUser_WhenUserIsAlreadyInRole_ReturnsSuccess()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);

            // Act
            var result = await _repository.AddRolesToUser("1", new List<string> { "Admin" });

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.AlreadyInRoles);
            Assert.Equal("Admin", result.AlreadyInRoles.First());
        }
        
        [Fact]
        public async Task AddRolesToUser_WhenRoleIsNotAdded_ReturnsFalse()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);
            _userManagerMock.Setup(um => um.AddToRoleAsync(user, "Admin")).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            // Act
            var result = await _repository.AddRolesToUser("1", new List<string> { "Admin" });

            // Assert
            Assert.False(result.Success);
        }
        
        #endregion

        #region RemoveRoleFromUser
        [Fact]
        public async Task RemoveRoleFromUser_ReturnsTrue()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.RemoveFromRoleAsync(user, "Admin")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.RemoveRoleFromUser("1", "Admin");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveRoleFromUser_WhenUserIsNull_ReturnsFalse()
        {
            // Arrange
            _fixture.ClearDatabase();
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync((User)null);

            // Act
            var result = await _repository.RemoveRoleFromUser("1", "Admin");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RemoveRoleFromUser_WhenUserIsNotInRole_ReturnsTrue()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            // Act
            var result = await _repository.RemoveRoleFromUser("1", "Admin");

            // Assert
            Assert.True(result);
        }
        #endregion

        #region UpdateRoleForUser
        [Fact]
        public async Task UpdateRoleForUser_ReturnsTrue()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(um => um.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.AddToRoleAsync(user, "Admin")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.UpdateRoleForUser("1", "Admin");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateRoleForUser_WhenUserIsNull_ReturnsFalse()
        {
            // Arrange
            _fixture.ClearDatabase();
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync((User)null);

            // Act
            var result = await _repository.UpdateRoleForUser("1", "Admin");

            // Assert
            Assert.False(result);
        }
        
        [Fact]
        public async Task UpdateRoleForUser_WhenRolesListIsEmpty_ReturnsFalse()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string>());

            // Act
            var result = await _repository.UpdateRoleForUser("1", "Admin");

            // Assert
            Assert.False(result);
        }
        
        [Fact]
        public async Task UpdateRoleForUser_WhenRoleIsNotUpdated_ReturnsFalse()
        {
            // Arrange
            _fixture.ClearDatabase();
            var user = new User { Id = "1", UserName = "User1" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(um => um.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            // Act
            var result = await _repository.UpdateRoleForUser("1", "Admin");

            // Assert
            Assert.False(result);
        }
        #endregion

        #region CreateRole
        [Fact]
        public async Task CreateRole_ReturnsTrue()
        {
            // Arrange
            _context.Roles.Add(new IdentityRole("User"));
            _context.SaveChanges();

            // Act
            var result = await _repository.CreateRole("Admin");

            // Assert
            Assert.True(result);
        }
        
        [Fact]
        public async Task CreateRole_WhenRoleAlreadyExists_ReturnsFalse()
        {
            // Arrange
            _context.Roles.Add(new IdentityRole("Admin"));
            _context.SaveChanges();

            // Act
            var result = await _repository.CreateRole("Admin");

            // Assert
            Assert.False(result);
        }
        
        #endregion

        #region DeleteRole
        [Fact]
        public async Task DeleteRole_ReturnsTrue()
        {
            // Arrange
            var role = new IdentityRole("Admin");
            _context.Roles.Add(role);
            _context.SaveChanges();

            // Act
            var result = await _repository.DeleteRole("Admin");

            // Assert
            Assert.True(result);
        }
        
        [Fact]
        public async Task DeleteRole_WhenRoleDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _fixture.ClearDatabase();
            _context.Roles.Add(new IdentityRole("User"));
            _context.SaveChanges();

            // Act
            var result = await _repository.DeleteRole("Admin");

            // Assert
            Assert.False(result);
        }
        #endregion
    }
}
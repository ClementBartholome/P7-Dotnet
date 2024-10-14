using Microsoft.AspNetCore.Mvc;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authorization;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Interfaces;
using P7CreateRestApi.Models.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            _logger.LogInformation("Retrieving Users");
            try
            {
                var users = await _userRepository.GetUsers();
                return Ok(users);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Users." });
            }
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            try
            {
                _logger.LogInformation("Retrieving User");
                var userDto = await _userRepository.GetUser(id);

                if (userDto == null)
                {
                    return NotFound(new { message = "User not found with the provided id." });
                }

                return Ok(userDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the User." });
            }
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userDto">The updated user data.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest(new { message = "The provided id does not match the id in the request." });
            }

            try
            {
                _logger.LogInformation("Updating User");
                var updatedUser = await _userRepository.UpdateUser(id, userDto);
                if (updatedUser == null)
                {
                    return NotFound(new { message = "User not found with the provided id." });
                }

                return Ok(new { message = "User updated successfully.", updatedUser });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while updating the User." });
            }
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                _logger.LogInformation("Deleting User");
                var result = await _userRepository.DeleteUser(id);
                if (!result)
                {
                    return NotFound(new { message = "User not found with the provided id." });
                }
                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the User." });
            }
        }

        /// <summary>
        /// Adds a role to a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="role">The role to add.</param>
        [HttpPost("{userId}/roles")]
        public async Task<IActionResult> AddRolesToUser(string userId, [FromBody] List<string> roles)
        {
            try
            {
                _logger.LogInformation("Adding Roles to User");
                var (result, errors) = await _userRepository.AddRolesToUser(userId, roles);
                if (!result)
                {
                    return BadRequest(new { message = errors });
                }
                return Ok(new { message = "Roles added to User successfully." });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while adding the Roles to the User." });
            }
        }

        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="role">The role to remove.</param>
        [HttpDelete("{userId}/roles")]
        public async Task<IActionResult> RemoveRoleFromUser(string userId, [FromBody] string role)
        {
            try
            {
                _logger.LogInformation("Removing Role from User");
                var result = await _userRepository.RemoveRoleFromUser(userId, role);
                if (!result)
                {
                    return NotFound(new { message = "User not found with the provided id." });
                }

                return Ok(new { message = "Role removed from User successfully." });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while removing the Role from the User." });
            }
        }

        /// <summary>
        /// Updates the role for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="role">The new role to assign.</param>
        [HttpPut("{userId}/roles")]
        public async Task<IActionResult> UpdateRoleForUser(string userId, [FromBody] string role)
        {
            try
            {
                _logger.LogInformation("Updating Role for User");
                var result = await _userRepository.UpdateRoleForUser(userId, role);
                if (!result)
                {
                    return NotFound(new { message = "User not found with the provided id." });
                }

                return Ok(new { message = "Role updated for User successfully." });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while updating the Role for the User." });
            }
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            try
            {
                _logger.LogInformation("Creating Role");
                var result = await _userRepository.CreateRole(roleName);
                if (!result)
                {
                    return BadRequest(new { message = "Role already exists." });
                }
                return Ok(new { message = "Role created successfully." });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while creating the Role." });
            }
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="roleName">The name of the role to delete.</param>
        [HttpDelete("roles")]
        public async Task<IActionResult> DeleteRole([FromBody] string roleName)
        {
            try
            {
                _logger.LogInformation("Deleting Role");
                var result = await _userRepository.DeleteRole(roleName);
                if (!result)
                {
                    return NotFound(new { message = "Role not found." });
                }
                return Ok(new { message = "Role deleted successfully." });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the Role." });
            }
        }
    }
}
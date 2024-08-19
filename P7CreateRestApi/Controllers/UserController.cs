using Microsoft.AspNetCore.Mvc;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(UserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            _logger.LogInformation("Retrieving Users");
            try
            {
                return await _userRepository.GetUsers();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Users." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving User");
                var userDto = await _userRepository.GetUser(id);

                if (userDto == null)
                {
                    return NotFound(new { message = "User not found with the provided id." });
                }

                return userDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the User." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
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
                return NotFound(new { message = "An error occurred while updating the User." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDto userDto)
        {
            try
            {
                _logger.LogInformation("Creating new User successfully.");
                var user = await _userRepository.PostUser(userDto);
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the User." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
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
    }
}
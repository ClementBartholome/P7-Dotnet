using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Models.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly RatingRepository _ratingRepository;
        private readonly ILogger<RatingController> _logger;

        public RatingController(RatingRepository ratingRepository, ILogger<RatingController> logger)
        {
            _ratingRepository = ratingRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatings()
        {
            _logger.LogInformation("Retrieving Ratings");
            try
            {
                return await _ratingRepository.GetRatings();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Ratings." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RatingDto>> GetRating(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving Rating");
                var ratingDto = await _ratingRepository.GetRating(id);

                if (ratingDto == null)
                {
                    return NotFound(new { message = "Rating not found with the provided id." });
                }

                return ratingDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Rating." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating(int id, RatingDto ratingDto)
        {
            if (id != ratingDto.Id)
            {
                return BadRequest(new { message = "The provided id does not match the id in the request." });
            }

            try
            {
                _logger.LogInformation("Updating Rating");
                var updatedRating = await _ratingRepository.UpdateRating(id, ratingDto);
                if (updatedRating == null)
                {
                    return NotFound(new { message = "Rating not found with the provided id." });
                }

                return Ok(new { message = "Rating updated successfully.", updatedRating });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound(new { message = "An error occurred while updating the Rating." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Rating>> PostRating(RatingDto ratingDto)
        {
            try
            {
                _logger.LogInformation("Creating new Rating successfully.");
                var rating = await _ratingRepository.PostRating(ratingDto);
                return CreatedAtAction("GetRating", new { id = rating.Id }, rating);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the Rating." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            try
            {
                _logger.LogInformation("Deleting Rating");
                await _ratingRepository.DeleteRating(id);
                return Ok(new { message = "Rating deleted successfully." });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the Rating." });
            }
        }
    }
}
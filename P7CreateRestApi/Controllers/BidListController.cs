using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authorization;
using P7CreateRestApi.Data;
using P7CreateRestApi.Models.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BidListController : ControllerBase
    {
        private readonly BidRepository _bidListRepository;
        private readonly ILogger<BidListController> _logger;

        public BidListController(BidRepository bidListRepository,
            ILogger<BidListController> logger)
        {
            _bidListRepository = bidListRepository;
            _logger = logger;
        }

        // GET: BidList
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BidListDto>>> GetBidLists()
        {
            _logger.LogInformation("Retrieving BidLists");
            try
            {
                var bidLists = await _bidListRepository.GetBidLists();
                _logger.LogInformation("Successfully retrieved BidLists");
                return bidLists;
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while retrieving the BidLists: {Message}", e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the BidLists." });
            }
        }

        // GET: BidList/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BidListDto>> GetBidList(int id)
        {
            _logger.LogInformation("Retrieving BidList with id: {Id}", id);
            try
            {
                var bidListDto = await _bidListRepository.GetBidList(id);

                if (bidListDto == null)
                {
                    _logger.LogWarning("BidList not found with id: {Id}", id);
                    return NotFound(new { message = "BidList not found with the provided id." });
                }

                _logger.LogInformation("Successfully retrieved BidList with id: {Id}", id);
                return bidListDto;
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while retrieving the BidList with id {Id}: {Message}", id,
                    e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the BidList." });
            }
        }

        // PUT: BidList/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBidList(int id, BidListDto bidListDto)
        {
            if (id != bidListDto.BidListId)
            {
                _logger.LogWarning(
                    "The provided id does not match the id in the request. Provided id: {Id}, Request id: {RequestId}",
                    id, bidListDto.BidListId);
                return BadRequest(new { message = "The provided id does not match the id in the request." });
            }

            if (!BidListExists(id))
            {
                _logger.LogWarning("BidList not found with id: {Id}", id);
                return NotFound(new { message = "BidList not found with the provided id." });
            }

            _logger.LogInformation("Updating BidList with id: {Id}", id);
            try
            {
                var updatedBidList = await _bidListRepository.UpdateBidList(id, bidListDto);
                if (updatedBidList == null)
                {
                    _logger.LogWarning("BidList not found with id: {Id}", id);
                    return NotFound(new { message = "BidList not found with the provided id." });
                }

                var updatedBidListDto = new BidListDto
                {
                    BidListId = updatedBidList.BidListId,
                    Account = updatedBidList.Account,
                    BidType = updatedBidList.BidType,
                    BidQuantity = updatedBidList.BidQuantity
                };

                _logger.LogInformation("Successfully updated BidList with id: {Id}", id);
                return Ok(new { message = "BidList updated successfully.", updatedBidList = updatedBidListDto });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BidListExists(id))
                {
                    _logger.LogWarning("BidList not found with id: {Id}", id);
                    return NotFound(new { message = "BidList not found with the provided id." });
                }

                throw;
            }
        }

        // POST: BidList
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<BidList>> PostBidList(BidListDto bidListDto)
        {
            _logger.LogInformation("Creating new BidList.");

            if (BidListExists(bidListDto.BidListId))
            {
                _logger.LogWarning("BidList with id: {Id} already exists.", bidListDto.BidListId);
                return Conflict(new { message = "A BidList with the provided id already exists." });
            }

            try
            {
                var bidList = await _bidListRepository.PostBidList(bidListDto);
                _logger.LogInformation("Successfully created new BidList with id: {Id}", bidList.BidListId);
                return CreatedAtAction("GetBidList", new { id = bidList.BidListId }, bidList);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating the BidList: {Message}", ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the BidList." });
            }
        }

        // DELETE: BidList/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBidList(int id)
        {
            if (!BidListExists(id))
            {
                _logger.LogWarning("BidList not found with id: {Id}", id);
                return NotFound(new { message = "BidList not found with the provided id." });
            }

            _logger.LogInformation("Deleting BidList with id: {Id}", id);
            try
            {
                await _bidListRepository.DeleteBidList(id);
                _logger.LogInformation("Successfully deleted BidList with id: {Id}", id);
                return Ok(new { message = "BidList deleted successfully." });
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while deleting the BidList with id {Id}: {Message}", id, e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the BidList." });
            }
        }

        private bool BidListExists(int id)
        {
            return _bidListRepository.BidListExists(id);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authorization;
using P7CreateRestApi.Data;
using P7CreateRestApi.Interfaces;
using P7CreateRestApi.Models.Dto;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BidListController : ControllerBase
    {
        private readonly IBidRepository _bidListRepository;
        private readonly ILogger<BidListController> _logger;

        public BidListController(IBidRepository bidListRepository,
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
                return Ok(bidLists);
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
                return Ok(bidListDto);
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
            try
            {
               if (!BidListExists(id))
               {
                   _logger.LogWarning("BidList not found with id: {Id}", id);
                   return NotFound(new { message = "BidList not found with the provided id." });
               }

               _logger.LogInformation("Updating BidList with id: {Id}", id);
               var updatedBidList = await _bidListRepository.UpdateBidList(id, bidListDto);
               _logger.LogInformation("Successfully updated BidList with id: {Id}", id);
               return Ok(updatedBidList);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while updating the BidList with id {Id}: {Message}", id, e.Message);
                return StatusCode(500, new { message = "An error occurred while updating the BidList." });
            }
        }

        // POST: BidList
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<BidList>> PostBidList(BidListDto bidListDto)
        {
            _logger.LogInformation("Creating new BidList.");

            try
            {
                if (BidListExists(bidListDto.BidListId))
                {
                    _logger.LogWarning("BidList with id: {Id} already exists.", bidListDto.BidListId);
                    return Conflict(new { message = "A BidList with the provided id already exists." });
                }

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
            try
            {
                if (!BidListExists(id))
                {
                    _logger.LogWarning("BidList not found with id: {Id}", id);
                    return NotFound(new { message = "BidList not found with the provided id." });
                }

                _logger.LogInformation("Deleting BidList with id: {Id}", id);
                await _bidListRepository.DeleteBidList(id);
                _logger.LogInformation("Successfully deleted BidList with id: {Id}", id);
                return Ok(new { message = "BidList deleted successfully." });
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while processing the request for BidList with id {Id}: {Message}", id, e.Message);
                return StatusCode(500, new { message = "An error occurred while processing the request." });
            }
        }

        private bool BidListExists(int id)
        {
            return _bidListRepository.BidListExists(id);
        }
    }
}
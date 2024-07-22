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
    public class BidListController : ControllerBase
    {
        private readonly LocalDbContext _context;
        private readonly BidRepository _bidListRepository;
        private readonly ILogger<BidListController> _logger;

        public BidListController(LocalDbContext context, BidRepository bidListRepository,
            ILogger<BidListController> logger)
        {
            _context = context;
            _bidListRepository = bidListRepository;
            _logger = logger;
        }

        // GET: BidList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidListDto>>> GetBidLists()
        {
            _logger.LogInformation("Retrieving BidLists");
            try {
                return await _bidListRepository.GetBidLists();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the BidLists." });
            }
        }

        // GET: BidList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BidListDto>> GetBidList(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving BidList");
                var bidListDto = await _bidListRepository.GetBidList(id);
                
                if (bidListDto == null)
                {
                    return NotFound(new { message = "BidList not found with the provided id." });
                }

                return bidListDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the BidList." });
            }
        }

        // PUT: BidList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBidList(int id, BidListDto bidListDto)
        {
            if (id != bidListDto.BidListId)
            {
                return BadRequest(new { message = "The provided id does not match the id in the request." });
            }

            try
            {
                _logger.LogInformation("Updating BidList");
                var updatedBidList = await _bidListRepository.UpdateBidList(id, bidListDto);
                if (updatedBidList == null)
                {
                    return NotFound(new { message = "BidList not found with the provided id." });
                }

                var updatedBidListDto = new BidListDto
                {
                    BidListId = updatedBidList.BidListId,
                    Account = updatedBidList.Account,
                    BidType = updatedBidList.BidType,
                    BidQuantity = updatedBidList.BidQuantity
                };

                return Ok(new { message = "BidList updated successfully.", updatedBidList = updatedBidListDto });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound(new { message = "An error occurred while updating the BidList." });
            }
        }

        // POST: BidList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BidList>> PostBidList(BidListDto bidListDto)
        {
            try
            {
                _logger.LogInformation("Creating new BidList successfully.");
                var bidList = await _bidListRepository.PostBidList(bidListDto);
                return CreatedAtAction("GetBidList", new { id = bidList.BidListId }, bidList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the BidList." });
            }
        }


        // DELETE: BidList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            try
            {
                _logger.LogInformation("Deleting BidList");
                var result = await _bidListRepository.DeleteBidList(id);
                if (!result)
                {
                    return NotFound(new { message = "BidList not found with the provided id." });
                }

                return Ok(new { message = "BidList deleted successfully." });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the BidList." });
            }
        }
    }
}
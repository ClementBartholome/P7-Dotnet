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

        public BidListController(LocalDbContext context, BidRepository bidListRepository)
        {
            _context = context;
            _bidListRepository = bidListRepository;
        }

        // GET: BidList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidListDto>>> GetBidLists()
        {
            return await _bidListRepository.GetBidLists();
        }

        // GET: BidList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BidListDto>> GetBidList(int id)
        {
            var bidListDto = await _bidListRepository.GetBidList(id);

            if (bidListDto == null)
            {
                return NotFound(new { message = "BidList not found with the provided id." });
            }

            return bidListDto;
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
                await _bidListRepository.UpdateBidList(id, bidListDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BidListExists(id))
                {
                    return NotFound(new { message = "BidList not found with the provided id." });
                }

                throw;
            }

            return NoContent();
        }

        // POST: BidList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BidList>> PostBidList(BidListDto bidListDto)
        {
            try
            {
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
        public async Task<IActionResult> DeleteBidList(int id)
        {
            try
            {
                await _bidListRepository.DeleteBidList(id);
                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the BidList." });
            }
        }

        private bool BidListExists(int id)
        {
            return (_context.BidLists?.Any(e => e.BidListId == id)).GetValueOrDefault();
        }
    }
}

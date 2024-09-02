using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models.Dto;
using P7CreateRestApi.Repositories;
using System;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authorization;

namespace P7CreateRestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly TradeRepository _tradeRepository;
        private readonly ILogger<TradeController> _logger;

        public TradeController(TradeRepository tradeRepository, ILogger<TradeController> logger)
        {
            _tradeRepository = tradeRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TradeDto>>> GetTrades()
        {
            _logger.LogInformation("Retrieving Trades");
            try
            {
                return await _tradeRepository.GetTrades();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Trades." });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TradeDto>> GetTrade(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving Trade");
                var tradeDto = await _tradeRepository.GetTrade(id);

                if (tradeDto == null)
                {
                    return NotFound(new { message = "Trade not found with the provided id." });
                }

                return tradeDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Trade." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTrade(int id, TradeDto tradeDto)
        {
            if (id != tradeDto.TradeId)
            {
                return BadRequest(new { message = "The provided id does not match the id in the request." });
            }

            try
            {
                _logger.LogInformation("Updating Trade");
                var updatedTrade = await _tradeRepository.UpdateTrade(id, tradeDto);
                if (updatedTrade == null)
                {
                    return NotFound(new { message = "Trade not found with the provided id." });
                }

                return Ok(new { message = "Trade updated successfully.", updatedTrade });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound(new { message = "An error occurred while updating the Trade." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Trade>> PostTrade(TradeDto tradeDto)
        {
            try
            {
                _logger.LogInformation("Creating new Trade.");
                var trade = await _tradeRepository.PostTrade(tradeDto);
                return CreatedAtAction("GetTrade", new { id = trade.TradeId }, trade);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the Trade." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            try
            {
                _logger.LogInformation("Deleting Trade");
                var result = await _tradeRepository.DeleteTrade(id);
                
                if (!result)
                {
                    return NotFound(new { message = "Trade not found with the provided id." });
                }
                
                return Ok(new { message = "Trade deleted successfully." });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the Trade." });
            }
        }
    }
}
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
        private readonly ITradeRepository _tradeRepository;
        private readonly ILogger<TradeController> _logger;

        public TradeController(ITradeRepository tradeRepository, ILogger<TradeController> logger)
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
                var trades = await _tradeRepository.GetTrades();
                return Ok(trades);
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

                return Ok(tradeDto);
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
            try
            {
                _logger.LogInformation("Updating Trade");
                var result = await _tradeRepository.UpdateTrade(id, tradeDto);

                if (result == null)
                {
                    return NotFound(new { message = "Trade not found with the provided id." });
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while updating the Trade." });
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
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models.Dto;
using P7CreateRestApi.Repositories;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly RuleRepository _ruleRepository;
        private readonly ILogger<RuleController> _logger;

        public RuleController(RuleRepository ruleRepository, ILogger<RuleController> logger)
        {
            _ruleRepository = ruleRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RuleDto>>> GetRules()
        {
            _logger.LogInformation("Retrieving Rules");
            try
            {
                return await _ruleRepository.GetRules();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Rules." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RuleDto>> GetRule(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving Rule");
                var ruleDto = await _ruleRepository.GetRule(id);

                if (ruleDto == null)
                {
                    return NotFound(new { message = "Rule not found with the provided id." });
                }

                return ruleDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Rule." });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RuleDto>> UpdateRule(int id, RuleDto ruleDto)
        {
            try
            {
                _logger.LogInformation("Updating Rule");
                var updatedRule = await _ruleRepository.UpdateRule(id, ruleDto);

                if (updatedRule == null)
                {
                    return NotFound(new { message = "Rule not found with the provided id." });
                }

                var updatedRuleDto = new RuleDto
                {
                    Id = updatedRule.Id,
                    Name = updatedRule.Name,
                    Description = updatedRule.Description,
                    Json = updatedRule.Json,
                    Template = updatedRule.Template,
                    SqlStr = updatedRule.SqlStr,
                    SqlPart = updatedRule.SqlPart
                };

                return Ok(new { message = "BidList updated successfully.", updatedBidList = updatedRuleDto });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "An error occurred while updating the Rule." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<RuleDto>> PostRule(RuleDto ruleDto)
        {
            try
            {
                _logger.LogInformation("Creating Rule");
                var rule = await _ruleRepository.PostRule(ruleDto);
                return CreatedAtAction("GetRule", new { id = rule.Id }, ruleDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while creating the Rule." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RuleDto>> DeleteRule(int id)
        {
            try
            {
                _logger.LogInformation("Deleting Rule");
                var result = await _ruleRepository.DeleteRule(id);

                if (!result)
                {
                    return NotFound(new { message = "Rule not found with the provided id." });
                }
                
                return Ok(new { message = "Rule deleted successfully." });

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the Rule." });
            }
        }
    }
}
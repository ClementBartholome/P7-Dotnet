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
    public class CurveController : ControllerBase
    {
        private readonly LocalDbContext _context;
        private readonly CurveRepository _curveRepository;
        private readonly ILogger<CurveController> _logger;

        public CurveController(LocalDbContext context, CurveRepository curveRepository,
            ILogger<CurveController> logger)
        {
            _context = context;
            _curveRepository = curveRepository;
            _logger = logger;
        }

        // GET: Curve
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurvePointDto>>> GetCurves()
        {
            _logger.LogInformation("Retrieving Curves");
            try
            {
                return await _curveRepository.GetCurves();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Curves." });
            }
        }

        // GET: Curve/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurvePointDto>> GetCurve(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving Curve");
                var curvePointDto = await _curveRepository.GetCurve(id);

                if (curvePointDto == null)
                {
                    return NotFound(new { message = "Curve not found with the provided id." });
                }

                return curvePointDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Curve." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurve(int id, CurvePointDto curvePointDto)
        {
            if (id != curvePointDto.Id)
            {
                return BadRequest(new { message = "The provided id does not match the id in the request." });
            }

            try
            {
                _logger.LogInformation("Updating CurvePoint");
                var updatedCurvePoint = await _curveRepository.UpdateCurve(id, curvePointDto);
                if (updatedCurvePoint == null)
                {
                    return NotFound(new { message = "CurvePoint not found with the provided id." });
                }

                return Ok(new { message = "CurvePoint updated successfully.", updatedCurvePoint });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound(new { message = "An error occurred while updating the CurvePoint." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CurvePoint>> PostCurve(CurvePointDto curvePointDto)
        {
            try
            {
                _logger.LogInformation("Creating new CurvePoint successfully.");
                var curvePoint = await _curveRepository.PostCurve(curvePointDto);
                return CreatedAtAction("GetCurve", new { id = curvePoint.Id }, curvePoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the CurvePoint." });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurve(int id)
        {
            try
            {
                _logger.LogInformation("Deleting CurvePoint");
                var result = await _curveRepository.DeleteCurve(id);
                if (!result)
                {
                    return NotFound(new { message = "CurvePoint not found with the provided id." });
                }

                return Ok(new { message = "CurvePoint deleted successfully." });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the CurvePoint." });
            }
        }
    }
}
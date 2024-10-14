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
    public class CurveController : ControllerBase
    {
        private readonly ICurveRepository _curveRepository;
        private readonly ILogger<CurveController> _logger;

        public CurveController(ICurveRepository curveRepository,
            ILogger<CurveController> logger)
        {
            _curveRepository = curveRepository;
            _logger = logger;
        }

        // GET: Curve
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CurvePointDto>>> GetCurves()
        {
            _logger.LogInformation("Retrieving Curves");
            try
            {
                var curves = await _curveRepository.GetCurves();
                _logger.LogInformation("Successfully retrieved Curves");
                return Ok(curves);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while retrieving the Curves: {Message}", e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Curves." });
            }
        }

        // GET: Curve/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CurvePointDto>> GetCurve(int id)
        {
            _logger.LogInformation("Retrieving Curve with id: {Id}", id);
            try
            {
                var curvePointDto = await _curveRepository.GetCurve(id);

                if (curvePointDto == null)
                {
                    _logger.LogWarning("Curve not found with id: {Id}", id);
                    return NotFound(new { message = "Curve not found with the provided id." });
                }

                _logger.LogInformation("Successfully retrieved Curve with id: {Id}", id);
                return Ok(curvePointDto);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while retrieving the Curve with id {Id}: {Message}", id, e.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving the Curve." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurve(int id, CurvePointDto curvePointDto)
        {
            _logger.LogInformation("Updating CurvePoint with id: {Id}", id);
            
            try
            {
                var result = await _curveRepository.UpdateCurve(id, curvePointDto);

                if (result == null)
                {
                    _logger.LogWarning("CurvePoint not found with id: {Id}", id);
                    return NotFound(new { message = "CurvePoint not found with the provided id." });
                }

                _logger.LogInformation("Successfully updated CurvePoint with id: {Id}", id);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while updating the CurvePoint with id {Id}: {Message}", id, e.Message);
                return StatusCode(500, new { message = "An error occurred while updating the CurvePoint." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CurvePoint>> PostCurve(CurvePointDto curvePointDto)
        {
            _logger.LogInformation("Creating CurvePoint");
            try
            {
                var result = await _curveRepository.PostCurve(curvePointDto);
                _logger.LogInformation("Successfully created CurvePoint");
                return CreatedAtAction("GetCurve", new { id = result.Id }, result);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while creating the CurvePoint: {Message}", e.Message);
                return StatusCode(500, new { message = "An error occurred while creating the CurvePoint." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurve(int id)
        {
            _logger.LogInformation("Deleting CurvePoint with id: {Id}", id);
            try
            {
                var result = await _curveRepository.DeleteCurve(id);

                if (!result)
                {
                    _logger.LogWarning("CurvePoint not found with id: {Id}", id);
                    return NotFound(new { message = "CurvePoint not found with the provided id." });
                }

                _logger.LogInformation("Successfully deleted CurvePoint with id: {Id}", id);
                return Ok(new { message = "CurvePoint deleted successfully." });
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while deleting the CurvePoint with id {Id}: {Message}", id, e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the CurvePoint." });
            }
        }
    }
}
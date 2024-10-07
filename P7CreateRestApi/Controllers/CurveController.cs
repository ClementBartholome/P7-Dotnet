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
    public class CurveController : ControllerBase
    {
        private readonly CurveRepository _curveRepository;
        private readonly ILogger<CurveController> _logger;

        public CurveController(CurveRepository curveRepository,
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
            if (id != curvePointDto.Id)
            {
                _logger.LogWarning("The provided id does not match the id in the request. Provided id: {Id}, Request id: {RequestId}", id, curvePointDto.Id);
                return BadRequest(new { message = "The provided id does not match the id in the request." });
            }

            if (!CurveExists(id))
            {
                _logger.LogWarning("CurvePoint not found with id: {Id}", id);
                return NotFound(new { message = "CurvePoint not found with the provided id." });
            }

            _logger.LogInformation("Updating CurvePoint with id: {Id}", id);
            try
            {
                var updatedCurvePoint = await _curveRepository.UpdateCurve(id, curvePointDto);
                _logger.LogInformation("Successfully updated CurvePoint with id: {Id}", id);
                return Ok(new { message = "CurvePoint updated successfully.", updatedCurvePoint });
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
            _logger.LogInformation("Creating new CurvePoint");

            if (CurveExists(curvePointDto.Id))
            {
                _logger.LogWarning("CurvePoint with id: {Id} already exists.", curvePointDto.Id);
                return Conflict(new { message = "A CurvePoint with the provided id already exists." });
            }

            try
            {
                var curvePoint = await _curveRepository.PostCurve(curvePointDto);
                _logger.LogInformation("Successfully created new CurvePoint with id: {Id}", curvePoint.Id);
                return CreatedAtAction("GetCurve", new { id = curvePoint.Id }, curvePoint);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating the CurvePoint: {Message}", ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the CurvePoint." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurve(int id)
        {
            if (!CurveExists(id))
            {
                _logger.LogWarning("CurvePoint not found with id: {Id}", id);
                return NotFound(new { message = "CurvePoint not found with the provided id." });
            }

            _logger.LogInformation("Deleting CurvePoint with id: {Id}", id);
            try
            {
                var result = await _curveRepository.DeleteCurve(id);
                _logger.LogInformation("Successfully deleted CurvePoint with id: {Id}", id);
                return Ok(new { message = "CurvePoint deleted successfully." });
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while deleting the CurvePoint with id {Id}: {Message}", id, e.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the CurvePoint." });
            }
        }
        
        private bool CurveExists(int id)
        {
            return _curveRepository.CurveExists(id);
        }
    }
}
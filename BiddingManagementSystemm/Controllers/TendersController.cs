using BiddingSystem.Application.DTOs.TenderDtos;
using BiddingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace BiddingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TendersController : ControllerBase
    {
        private readonly ITenderService _tenderService;
        private readonly ILogger<TendersController> _logger;

        public TendersController(ITenderService tenderService, ILogger<TendersController> logger)
        {
            _tenderService = tenderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TenderDto>>> GetTenders([FromQuery] TenderParameters parameters)
        {
            try
            {
                var tenders = await _tenderService.GetTendersAsync(parameters);
                return Ok(tenders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tenders");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TenderDto>> GetTender(Guid id)
        {
            try
            {
                var tender = await _tenderService.GetTenderByIdAsync(id);
                if (tender == null)
                {
                    return NotFound();
                }
                return Ok(tender);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tender with id {TenderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Procurement")]
        public async Task<ActionResult<TenderDto>> CreateTender(CreateTenderDto createTenderDto)
        {
            try
            {
                var result = await _tenderService.CreateTenderAsync(createTenderDto);
                return CreatedAtAction(nameof(GetTender), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tender");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Procurement")]
        public async Task<ActionResult> UpdateTender(Guid id, UpdateTenderDto updateTenderDto)
        {
            try
            {
                var result = await _tenderService.UpdateTenderAsync(id, updateTenderDto);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tender with id {TenderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Procurement")]
        public async Task<ActionResult> DeleteTender(Guid id)
        {
            try
            {
                var result = await _tenderService.DeleteTenderAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tender with id {TenderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Procurement")]
        public async Task<ActionResult> UpdateTenderStatus(Guid id, [FromBody] UpdateTenderStatusDto statusDto)
        {
            try
            {
                var result = await _tenderService.UpdateTenderStatusAsync(id, statusDto.Status);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tender status for id {TenderId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("categories/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TenderDto>>> GetTendersByCategory(Guid categoryId)
        {
            try
            {
                var tenders = await _tenderService.GetTendersByCategoryAsync(categoryId);
                return Ok(tenders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tenders by category {CategoryId}", categoryId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("organization/{organizationId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TenderDto>>> GetTendersByOrganization(Guid organizationId)
        {
            try
            {
                var tenders = await _tenderService.GetTendersByOrganizationAsync(organizationId);
                return Ok(tenders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tenders by organization {OrganizationId}", organizationId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

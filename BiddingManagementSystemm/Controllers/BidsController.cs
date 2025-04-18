using BiddingSystem.Application.DTOs.BidDtos;
using BiddingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using RouteAttribute = System.Web.Mvc.RouteAttribute;

namespace BiddingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : System.Web.Mvc.ControllerBase
    {
        private readonly IBidService _bidService;
        private readonly ILogger<BidsController> _logger;

        public BidsController(IBidService bidService, ILogger<BidsController> logger)
        {
            _bidService = bidService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Procurement")]
        public async Task<ActionResult<PagedResult<BidDto>>> GetBids([FromQuery] BidParameters parameters)
        {
            try
            {
                var bids = await _bidService.GetBidsAsync(parameters);
                return Ok(bids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bids");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BidDto>> GetBid(Guid id)
        {
            try
            {
                var bid = await _bidService.GetBidByIdAsync(id);
                if (bid == null)
                {
                    return NotFound();
                }
                return Ok(bid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bid with id {BidId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult<BidDto>> CreateBid(CreateBidDto createBidDto)
        {
            try
            {
                var result = await _bidService.CreateBidAsync(createBidDto);
                return CreatedAtAction(nameof(GetBid), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bid");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> UpdateBid(Guid id, UpdateBidDto updateBidDto)
        {
            try
            {
                var result = await _bidService.UpdateBidAsync(id, updateBidDto);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bid with id {BidId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Supplier,Admin")]
        public async Task<ActionResult> DeleteBid(Guid id)
        {
            try
            {
                var result = await _bidService.DeleteBidAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bid with id {BidId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("tender/{tenderId}")]
        [Authorize(Roles = "Admin,Procurement")]
        public async Task<ActionResult<PagedResult<BidDto>>> GetBidsByTender(Guid tenderId, [FromQuery] PaginationParameters parameters)
        {
            try
            {
                var bids = await _bidService.GetBidsByTenderAsync(tenderId, parameters);
                return Ok(bids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bids for tender {TenderId}", tenderId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("supplier/{supplierId}")]
        [Authorize]
        public async Task<ActionResult<PagedResult<BidDto>>> GetBidsBySupplier(Guid supplierId, [FromQuery] PaginationParameters parameters)
        {
            try
            {
                // Check if current user is the supplier or has admin/procurement role
                if (!User.IsInRole("Admin") && !User.IsInRole("Procurement") &&
                    User.FindFirst("userId")?.Value != supplierId.ToString())
                {
                    return Forbid();
                }

                var bids = await _bidService.GetBidsBySupplierAsync(supplierId, parameters);
                return Ok(bids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bids for supplier {SupplierId}", supplierId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

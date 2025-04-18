using System.ComponentModel.DataAnnotations;

namespace BiddingSystem.Application.DTOs.BidDtos
{
    // DTO for bid items (used in both create and update)
    public class BidItemDto
    {
        [Required]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 500 characters")]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero")]
        public decimal UnitPrice { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BiddingSystem.Application.DTOs.BidDtos
{
    // Request DTO for evaluating a bid
    public class EvaluateBidDto
    {
        [Required]
        public int BidId { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot exceed 1000 characters")]
        public string Comments { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one criteria score is required")]
        public List<CriteriaScoreDto> Scores { get; set; } = new List<CriteriaScoreDto>();
    }
}

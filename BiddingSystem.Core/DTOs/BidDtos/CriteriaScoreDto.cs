using System.ComponentModel.DataAnnotations;

namespace BiddingSystem.Application.DTOs.BidDtos
{
    // DTO for criteria scores in evaluation
    public class CriteriaScoreDto
    {
        [Required]
        public int CriteriaId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100")]
        public decimal Score { get; set; }

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string Comments { get; set; }
    }
}

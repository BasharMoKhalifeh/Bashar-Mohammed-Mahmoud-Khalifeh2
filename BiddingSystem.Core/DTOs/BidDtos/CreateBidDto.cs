using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.DTOs.BidDtos
{
    public class CreateBidDto
    {
        [Required]
        public int TenderId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MinLength(50, ErrorMessage = "Technical proposal must be at least 50 characters")]
        public string TechnicalProposal { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one bid item is required")]
        public List<BidItemDto> Items { get; set; } = new List<BidItemDto>();
    }
}

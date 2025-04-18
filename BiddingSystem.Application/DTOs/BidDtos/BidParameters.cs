using BiddingSystem.Domain.Enums;

namespace BiddingSystem.Application.DTOs.BidDtos
{
    // Parameters for bid filtering and pagination
    public class BidParameters 
    {
        public int? TenderId { get; set; }
        public int? BidderId { get; set; }
        public BidStatus? Status { get; set; }
        public string OrderBy { get; set; } = "SubmissionDateDesc";
    }
}

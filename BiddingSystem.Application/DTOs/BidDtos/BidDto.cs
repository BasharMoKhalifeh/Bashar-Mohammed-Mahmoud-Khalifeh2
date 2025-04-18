using BiddingSystem.Domain.Enums;

namespace BiddingSystem.Application.DTOs.BidDtos
{
    // Response DTO for bid information
    public class BidDto
    {
        public int Id { get; set; }
        public int TenderId { get; set; }
        public string TenderTitle { get; set; }
        public int BidderId { get; set; }
        public string BidderName { get; set; }
        public DateTime SubmissionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string TechnicalProposal { get; set; }
        public BidStatus Status { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public List<BidItemDto> Items { get; set; } = new List<BidItemDto>();
        public List<BidDocumentDto> Documents { get; set; } = new List<BidDocumentDto>();
    }
}

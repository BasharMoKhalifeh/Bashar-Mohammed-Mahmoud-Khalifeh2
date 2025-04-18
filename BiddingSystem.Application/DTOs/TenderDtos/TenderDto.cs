using BiddingSystem.Domain.Enums;

namespace BiddingSystem.Application.DTOs.TenderDtos
{
    public class TenderDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public string IssuedBy { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string BudgetRange { get; set; }
        public string ContactEmail { get; set; }
        public TenderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public int TenderTypeId { get; set; }
        public string TenderType { get; set; }
        public int TenderCategoryId { get; set; }
        public string TenderCategory { get; set; }
        public IEnumerable<TenderDocumentDto> Documents { get; set; }
    }
}

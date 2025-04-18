using BiddingSystem.Domain.Enums;

public class UpdateTenderDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ReferenceNumber { get; set; }
    public decimal? EstimatedBudget { get; set; }
    public DateTime? SubmissionDeadline { get; set; }
    public DateTime? CloseDate { get; set; }
    public TenderStatus? Status { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? EvaluationCriteria { get; set; }
    public List<Guid>? AttachmentIds { get; set; }
}

namespace BiddingSystem.Application.DTOs.BidDtos
{
    // Response DTO for bid evaluation results
    public class BidEvaluationDto
    {
        public int Id { get; set; }
        public int EvaluatorId { get; set; }
        public string EvaluatorName { get; set; }
        public decimal TotalScore { get; set; }
        public string Comments { get; set; }
        public DateTime EvaluationDate { get; set; }
        public List<CriteriaScoreDto> CriteriaScores { get; set; } = new List<CriteriaScoreDto>();
    }
}

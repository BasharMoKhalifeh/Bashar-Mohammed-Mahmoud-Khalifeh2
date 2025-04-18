using TMS.Domain.Entities;

public class BidEvaluation
{
    public int Id { get; private set; }
    public int BidId { get; private set; }
    public int EvaluatorId { get; private set; }
    public decimal TotalScore { get; private set; }
    public string Comments { get; private set; }
    public DateTime EvaluationDate { get; private set; }

    // Navigation properties
    public Bid Bid { get; private set; }
    public User Evaluator { get; private set; }
    public ICollection<CriteriaScore> CriteriaScores { get; private set; }

    // Constructor
    public BidEvaluation(int bidId, int evaluatorId, string comments)
    {
        BidId = bidId;
        EvaluatorId = evaluatorId;
        Comments = comments;
        EvaluationDate = DateTime.UtcNow;
        CriteriaScores = new List<CriteriaScore>();
    }

    // For EF Core
    private BidEvaluation() { }

    // Domain methods
    public void AddCriteriaScore(CriteriaScore score)
    {
        CriteriaScores.Add(score);
        UpdateTotalScore();
    }

    private void UpdateTotalScore()
    {
        // Calculate weighted score
        TotalScore = CriteriaScores.Sum(cs => cs.Score * cs.Criteria.Weight / 100);
    }
}
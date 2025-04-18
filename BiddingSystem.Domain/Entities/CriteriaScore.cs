public class CriteriaScore
{
    public int Id { get; private set; }
    public int EvaluationId { get; private set; }
    public int CriteriaId { get; private set; }
    public decimal Score { get; private set; }
    public string Comments { get; private set; }

    // Navigation properties
    public BidEvaluation Evaluation { get; private set; }
    public EvaluationCriteria Criteria { get; private set; }

    // Constructor
    public CriteriaScore(int evaluationId, int criteriaId, decimal score, string comments)
    {
        EvaluationId = evaluationId;
        CriteriaId = criteriaId;
        Score = score;
        Comments = comments;
    }

    // For EF Core
    private CriteriaScore() { }

    // Domain methods
    public void UpdateScore(decimal score, string comments)
    {
        Score = score;
        Comments = comments;
    }
}
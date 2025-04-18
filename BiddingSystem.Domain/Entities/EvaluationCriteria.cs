public class EvaluationCriteria
{
    public int Id { get; private set; }
    public int TenderId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Weight { get; private set; }

    // Navigation properties
    public Tender Tender { get; private set; }
    public ICollection<CriteriaScore> Scores { get; private set; }

    // Constructor
    public EvaluationCriteria(int tenderId, string name, string description, decimal weight)
    {
        TenderId = tenderId;
        Name = name;
        Description = description;
        Weight = weight;
        Scores = new List<CriteriaScore>();
    }

    // For EF Core
    private EvaluationCriteria() { }
}
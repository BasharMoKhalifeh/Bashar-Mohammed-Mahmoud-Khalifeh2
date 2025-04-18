public class TenderType
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    // Navigation properties
    public ICollection<Tender> Tenders { get; private set; }

    // Constructor
    public TenderType(string name, string description)
    {
        Name = name;
        Description = description;
        Tenders = new List<Tender>();
    }

    // For EF Core
    private TenderType() { }
}
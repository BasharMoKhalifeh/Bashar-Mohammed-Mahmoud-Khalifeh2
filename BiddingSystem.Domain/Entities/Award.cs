using TMS.Domain.Entities;

public class Award
{
    public int Id { get; private set; }
    public int TenderId { get; private set; }
    public int WinningBidId { get; private set; }
    public int AwardedById { get; private set; }
    public DateTime AwardDate { get; private set; }
    public string Notes { get; private set; }

    // Navigation properties
    public Tender Tender { get; private set; }
    public Bid WinningBid { get; private set; }
    public User AwardedBy { get; private set; }

    // Constructor
    public Award(int tenderId, int winningBidId, int awardedById, string notes = null)
    {
        TenderId = tenderId;
        WinningBidId = winningBidId;
        AwardedById = awardedById;
        AwardDate = DateTime.UtcNow;
        Notes = notes;
    }

    // For EF Core
    private Award() { }

    // Domain methods
    public void UpdateNotes(string notes)
    {
        Notes = notes;
    }
}
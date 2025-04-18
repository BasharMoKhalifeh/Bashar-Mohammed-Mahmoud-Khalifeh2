using BiddingSystem.Domain.Enums;
using TMS.Domain.Entities;

public class Bid
{
    public int Id { get; private set; }
    public int TenderId { get; private set; }
    public int BidderId { get; private set; }
    public DateTime SubmissionDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string TechnicalProposal { get; private set; }
    public BidStatus Status { get; private set; }
    public DateTime? LastModifiedDate { get; private set; }

    // Navigation properties
    public Tender Tender { get; private set; }
    public User Bidder { get; private set; }
    public ICollection<BidItem> Items { get; private set; }
    public ICollection<BidDocument> Documents { get; private set; }
    public ICollection<BidEvaluation> Evaluations { get; private set; }

    // Constructor
    public Bid(int tenderId, int bidderId, decimal totalAmount, string technicalProposal)
    {
        TenderId = tenderId;
        BidderId = bidderId;
        TotalAmount = totalAmount;
        TechnicalProposal = technicalProposal;
        Status = BidStatus.Submitted;
        SubmissionDate = DateTime.UtcNow;

        Items = new List<BidItem>();
        Documents = new List<BidDocument>();
        Evaluations = new List<BidEvaluation>();
    }

    // For EF Core
    private Bid() { }

    // Domain methods
    public void AddItem(BidItem item)
    {
        Items.Add(item);
        UpdateTotalAmount();
    }

    public void AddDocument(BidDocument document)
    {
        Documents.Add(document);
    }

    public void UpdateStatus(BidStatus status)
    {
        Status = status;
        LastModifiedDate = DateTime.UtcNow;
    }

    private void UpdateTotalAmount()
    {
        TotalAmount = Items.Sum(i => i.TotalPrice);
        LastModifiedDate = DateTime.UtcNow;
    }
}
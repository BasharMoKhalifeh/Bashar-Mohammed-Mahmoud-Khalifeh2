public class BidItem
{
    public int Id { get; private set; }
    public int BidId { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice { get; private set; }

    // Navigation properties
    public Bid Bid { get; private set; }

    // Constructor
    public BidItem(int bidId, string description, int quantity, decimal unitPrice)
    {
        BidId = bidId;
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = quantity * unitPrice;
    }

    // For EF Core
    private BidItem() { }

    // Domain methods
    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
        TotalPrice = Quantity * UnitPrice;
    }

    public void UpdateUnitPrice(decimal unitPrice)
    {
        UnitPrice = unitPrice;
        TotalPrice = Quantity * UnitPrice;
    }
}
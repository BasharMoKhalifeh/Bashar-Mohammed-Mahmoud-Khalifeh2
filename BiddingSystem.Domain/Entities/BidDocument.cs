public class BidDocument
{
    public int Id { get; private set; }
    public int BidId { get; private set; }
    public string DocumentType { get; private set; }
    public string FilePath { get; private set; }
    public string FileName { get; private set; }
    public DateTime UploadDate { get; private set; }

    // Navigation properties
    public Bid Bid { get; private set; }

    // Constructor
    public BidDocument(int bidId, string documentType, string filePath, string fileName)
    {
        BidId = bidId;
        DocumentType = documentType;
        FilePath = filePath;
        FileName = fileName;
        UploadDate = DateTime.UtcNow;
    }

    // For EF Core
    private BidDocument() { }
}

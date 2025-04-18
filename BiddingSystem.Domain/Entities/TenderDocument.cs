public class TenderDocument
{
    public int Id { get; private set; }
    public int TenderId { get; private set; }
    public string DocumentType { get; private set; }
    public string FilePath { get; private set; }
    public string FileName { get; private set; }
    public DateTime UploadDate { get; private set; }

    // Navigation properties
    public Tender Tender { get; private set; }

    // Constructor
    public TenderDocument(int tenderId, string documentType, string filePath, string fileName)
    {
        TenderId = tenderId;
        DocumentType = documentType;
        FilePath = filePath;
        FileName = fileName;
        UploadDate = DateTime.UtcNow;
    }

    // For EF Core
    private TenderDocument() { }
}
namespace BiddingSystem.Application.DTOs.TenderDtos
{
    public class TenderDocumentDto
    {
        public int Id { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
    }
}

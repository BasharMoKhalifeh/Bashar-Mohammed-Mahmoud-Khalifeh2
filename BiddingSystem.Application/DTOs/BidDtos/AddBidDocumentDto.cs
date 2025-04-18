using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BiddingSystem.Application.DTOs.BidDtos
{
    // Request DTO for adding documents to a bid
    public class AddBidDocumentDto
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Document type cannot exceed 100 characters")]
        public string DocumentType { get; set; }
    }
}

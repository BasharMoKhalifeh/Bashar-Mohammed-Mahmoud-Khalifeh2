using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BiddingSystem.Application.DTOs.TenderDtos
{
    public class AddDocumentDto
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string DocumentType { get; set; }
    }
}

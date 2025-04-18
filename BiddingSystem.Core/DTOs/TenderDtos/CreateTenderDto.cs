using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.DTOs.TenderDtos
{

    public class CreateTenderDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string IssuedBy { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime ClosingDate { get; set; }

        [Required]
        public int TenderTypeId { get; set; }

        [Required]
        public int TenderCategoryId { get; set; }

        public string BudgetRange { get; set; }

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.DTOs.TenderDtos
{
    public class AwardTenderDto
    {
        public Guid TenderId { get; set; }
        public Guid SupplierId { get; set; }
        public decimal AwardAmount { get; set; }
        public DateTime AwardDate { get; set; } = DateTime.UtcNow;
        public string? AwardNotes { get; set; }
        public bool IsAwarded { get; set; } = true;
    }
}

using BiddingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.DTOs.TenderDtos
{
    public class TenderParameters : PaginationParameters
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ReferenceNumber { get; set; }
        public TenderStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public string? SortBy { get; set; } = "CreatedDate";
        public bool IsDescending { get; set; } = true;
        public Guid? CategoryId { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}

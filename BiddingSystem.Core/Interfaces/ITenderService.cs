using BiddingSystem.Application.DTOs.TenderDtos;
using System;
using System.Collections.Generic;
using System.Fabric.Query;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.Interfaces
{
    public interface ITenderService
    {
        Task<TenderDto> CreateTenderAsync(CreateTenderDto dto, int userId);
        Task<TenderDto> UpdateTenderAsync(UpdateTenderDto dto, int userId);
        Task<bool> PublishTenderAsync(int tenderId, int userId);
        Task<bool> CloseTenderAsync(int tenderId, int userId);
        Task<bool> AddDocumentAsync(int tenderId, AddDocumentDto dto, int userId);
        Task<PagedList<TenderDto>> GetTendersAsync(TenderParameters parameters);
    }
}

using BiddingSystem.Application.DTOs.BidDtos;
using System;
using System.Collections.Generic;
using System.Fabric.Query;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.Interfaces
{
    public interface IBidService
    {
        Task<BidDto> SubmitBidAsync(CreateBidDto dto, int bidderId);
        Task<bool> UpdateBidAsync(UpdateBidDto dto, int bidderId);
        Task<bool> WithdrawBidAsync(int bidId, int bidderId);
        Task<bool> AddBidDocumentAsync(int bidId, AddBidDocumentDto dto, int bidderId);
        Task<PagedList<BidDto>> GetBidsForTenderAsync(int tenderId, BidParameters parameters);
    }
}

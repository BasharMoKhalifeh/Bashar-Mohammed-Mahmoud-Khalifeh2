using System.Linq.Dynamic.Core;

namespace BiddingSystem.Application.Repositories.Interfaces
{
    public interface IBidRepository : IBaseRepository<Bid>
    {
        Task<Bid> GetBidWithDetailsAsync(Guid id);
        Task<PagedResult<Bid>> GetBidsByTenderAsync(Guid tenderId, PaginationParameters parameters);
        Task<PagedResult<Bid>> GetBidsBySupplierAsync(Guid supplierId, PaginationParameters parameters);
        Task<Bid> GetWinningBidByTenderAsync(Guid tenderId);
        Task<decimal> GetLowestBidAmountByTenderAsync(Guid tenderId);
        Task<decimal> GetHighestBidAmountByTenderAsync(Guid tenderId);
        Task<int> GetBidCountByTenderAsync(Guid tenderId);
        Task<bool> HasSupplierBidOnTenderAsync(Guid tenderId, Guid supplierId);
    }
}
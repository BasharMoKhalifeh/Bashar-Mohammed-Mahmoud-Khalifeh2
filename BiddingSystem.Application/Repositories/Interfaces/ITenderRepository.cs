using BiddingSystem.Application.DTOs.TenderDtos;
using BiddingSystem.Application.Repositories.Interfaces;
using BiddingSystem.Domain.Enums;
using System.Linq.Dynamic.Core;

public interface ITenderRepository : IBaseRepository<Tender>
{
    Task<Tender> GetTenderWithDetailsAsync(Guid id);
    Task<PagedResult<Tender>> GetTendersAsync(TenderParameters parameters);
    Task<IReadOnlyList<Tender>> GetTendersByOrganizationAsync(Guid organizationId);
    Task<IReadOnlyList<Tender>> GetTendersByCategoryAsync(Guid categoryId);
    Task<IReadOnlyList<Tender>> GetOpenTendersAsync();
    Task<bool> UpdateTenderStatusAsync(Guid id, TenderStatus status);
    Task<bool> ExistsByReferenceNumberAsync(string referenceNumber);
}

using BiddingSystem.Application.Repositories.Interfaces;
using Fluent.Infrastructure.FluentModel;
using System.Linq.Dynamic.Core;

public interface IUserRepository : IBaseRepository<ApplicationUser>
{
    Task<ApplicationUser> GetUserByEmailAsync(string email);
    Task<ApplicationUser> GetUserWithRolesAsync(Guid userId);
    Task<PagedResult<ApplicationUser>> GetUsersAsync(UserParameters parameters);
    Task<IReadOnlyList<ApplicationUser>> GetUsersByRoleAsync(string role);
    Task<IReadOnlyList<ApplicationUser>> GetUsersByOrganizationAsync(Guid organizationId);
    Task<bool> IsUserInRoleAsync(Guid userId, string role);
    Task<bool> IsEmailUniqueAsync(string email);
}

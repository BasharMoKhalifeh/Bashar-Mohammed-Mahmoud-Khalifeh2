using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Infrastructure.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ITenderRepository Tenders { get; }
        IBidRepository Bids { get; }
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}

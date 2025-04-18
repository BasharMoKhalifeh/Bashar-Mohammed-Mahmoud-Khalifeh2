using BiddingSystem.Application.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Infrastructure.Persistence.Contexts;

namespace BiddingSystem.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ITenderRepository _tenderRepository;
        private IBidRepository _bidRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ITenderRepository Tenders =>
            _tenderRepository ??= new TenderRepository(_context);

        public IBidRepository Bids =>
            _bidRepository ??= new BidRepository(_context);

        public IUserRepository Users =>
            _userRepository ??= new UserRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}


using AccountingManagement.Core.Interface;
using AccountingManagement.Core.IUnitOfWork;
using AccountingManagement.Core.Models;
using AccountingManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields & Properties
        private readonly AccountingManagementContext _context;
        public IAuthRepository AuthRepository {get; private set;}

        #endregion

        #region Contractor
        public UnitOfWork(AccountingManagementContext _context)
        {
            this._context = _context;
            AuthRepository = new AuthRepository(_context);
        }
        #endregion

        #region Save Changes
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion

        
    }
}

﻿using AccountingManagement.Core.Interface;
using AccountingManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository AuthRepository { get; }
        IUserRepository UserRepository { get; }
        public Task<int> CommitAsync();

    }
}

using AccountingManagement.Core.DTOs.AccountDTOs;
using AccountingManagement.Core.DTOs.TransactionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<GetAllTransactionDTO>> GetAllTransactionAsync(int userId, int accountId, string? transactionStatus);
        Task<int> AddTransactionAsync(AddTransactionDTO transactionDTO);
        Task<string> ModifyTransactionAsync(int transactionId, EditTransactionDTO transactionDTO);
        Task<int> DeleteTransactionAsync(transactionDTO transactionDTO);
    }
}

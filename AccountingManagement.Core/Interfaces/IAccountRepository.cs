using AccountingManagement.Core.DTOs.AccountDTOs;
using AccountingManagement.Core.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<GetAllAccountsDTO>> GetAllAccountAsync(int userId, int accountId, int accountNumber);
        Task<int> AddAccountAsync(AddAccountDTO accountDTO);
        Task<string> ModifyAccountAsync(int accountId, EditAccountDTO editAccountDTO);
        Task<int> DeleteAccountAsync(List<int> accountIds);
    }
}

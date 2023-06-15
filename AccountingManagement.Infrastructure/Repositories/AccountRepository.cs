using AccountingManagement.Core.DTOs.AccountDTOs;
using AccountingManagement.Core.DTOs.AuthDTOs;
using AccountingManagement.Core.DTOs.UserDTOs;
using AccountingManagement.Core.Interfaces;
using AccountingManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AccountingManagement.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        #region Fields & Property
        private AccountingManagementContext _context;
        #endregion

        #region Constructor
        public AccountRepository(AccountingManagementContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region Get All Users Repository
        public async Task<IEnumerable<GetAllAccountsDTO>> GetAllAccountAsync(int userId, int accountId, int accountNumber)
        {
            try
            {
                var listOfAccounts = _context.AccountTables
                    .Select(x => new GetAllAccountsDTO
                    {
                        AccountId = x.AccountId.ToString(),
                        UserId = x.UserId.ToString(),
                        ServerDateTime = x.ServerDateTime,
                        DateTimeUtc = x.DateTimeUtc,
                        UpdateDateTimeUtc = x.UpdateDateTimeUtc,
                        AccountNumber = x.AccountNumber,
                        Balance = x.Balance.ToString(),
                        Currency = x.Currency,
                        Status = ((Status)x.Status).ToString()
                    });

                if (userId > 0)
                    listOfAccounts = listOfAccounts.Where(x => x.UserId == userId.ToString());

                if (accountId > 0)
                    listOfAccounts = listOfAccounts.Where(x => x.AccountId == accountId.ToString());

                if (accountNumber > 0)
                    listOfAccounts = listOfAccounts.Where(x => x.AccountNumber == accountNumber.ToString());

                return await listOfAccounts.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
        #endregion

        #region Add Account Repository
        public async Task<int> AddAccountAsync(AddAccountDTO accountDTO)
        {
            try
            {
                var isValidUser = await _context.UserTables.SingleOrDefaultAsync(x => x.UserId == accountDTO.UserId);
                if (isValidUser == null)
                    return 0;

                var addAccount = new AccountTable();
                addAccount.UserId = accountDTO.UserId;
                addAccount.ServerDateTime = DateTime.Now;
                addAccount.DateTimeUtc = DateTime.UtcNow;
                addAccount.UpdateDateTimeUtc = DateTime.UtcNow;

                Random random = new Random();
                string randomNumber;
                do
                {
                    randomNumber = random.Next(1000000, 9999999).ToString();
                } while (await _context.AccountTables.AnyAsync(x => x.AccountNumber == randomNumber));

                addAccount.AccountNumber = randomNumber;
                addAccount.Balance = accountDTO.Balance;

                if (Enum.IsDefined(typeof(Currencies), accountDTO.Currency.ToLower()))
                {
                    addAccount.Currency = accountDTO.Currency.ToLower();
                }

                addAccount.Status = _context.UserTables.SingleOrDefault(x => x.UserId == accountDTO.UserId).Status;

                await _context.AddAsync(addAccount);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
        #endregion

        #region Modify Account Repository
        public async Task<string> ModifyAccountAsync(int accountId, EditAccountDTO editAccountDTO)
        {
            try
            {
                var desiredAccount = await _context.AccountTables.SingleOrDefaultAsync(x => x.AccountId == accountId);

                if (string.IsNullOrEmpty(editAccountDTO.UserId) || editAccountDTO.UserId != "string")
                {
                    if (!Regex.IsMatch(editAccountDTO.UserId, @"^\d+$"))
                        return "Invalid AccountId.";

                    var isFoundUser = await _context.UserTables.SingleOrDefaultAsync(x => x.UserId == Int32.Parse(editAccountDTO.UserId));
                    if (isFoundUser == null)
                        return "User not found";

                    desiredAccount.UserId = Int32.Parse(editAccountDTO.UserId);
                    desiredAccount.UpdateDateTimeUtc = DateTime.UtcNow;
                }
                if (string.IsNullOrEmpty(editAccountDTO.Balance) || editAccountDTO.Balance != "string")
                {
                    if (!Regex.IsMatch(editAccountDTO.Balance, @"^\d+(\.\d+)?$"))
                        return "Invalid Balance.";

                    desiredAccount.Balance = Int32.Parse(editAccountDTO.Balance);
                    desiredAccount.UpdateDateTimeUtc = DateTime.UtcNow;
                }
                if (string.IsNullOrEmpty(editAccountDTO.Currency) || editAccountDTO.Currency != "string")
                {
                    if (!Enum.IsDefined(typeof(Currencies), editAccountDTO.Currency.ToLower()))
                    {
                        throw new ArgumentException("Invalid Currency value");
                    }

                    desiredAccount.Currency = editAccountDTO.Currency.ToLower();
                    desiredAccount.UpdateDateTimeUtc = DateTime.UtcNow;
                }
                if (string.IsNullOrEmpty(editAccountDTO.Status) || editAccountDTO.Status != "string")
                {
                    if (!Enum.IsDefined(typeof(Status), editAccountDTO.Status.ToLower()))
                    {
                        throw new ArgumentException("Invalid Status value");
                    }

                    desiredAccount.Status = (int)Enum.Parse(typeof(Status), editAccountDTO.Status.ToLower());
                    desiredAccount.UpdateDateTimeUtc = DateTime.UtcNow;
                }
                _context.Update(desiredAccount);
                await _context.SaveChangesAsync();
                return "Modified successfully.";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete the accounts Repository
        public async Task<int> DeleteAccountAsync(List<int> accountIds)
        {
            try
            {
                var accountToDelete = await _context.AccountTables.Where(x => accountIds.Contains(x.AccountId)).ToListAsync();

                foreach (var account in accountToDelete)
                {
                    account.Status = 1;
                    _context.Update(account);
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
        #endregion
    }
}

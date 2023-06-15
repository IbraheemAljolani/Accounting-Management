using AccountingManagement.Core.DTOs.AccountDTOs;
using AccountingManagement.Core.DTOs.TransactionDTOs;
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
    public class TransactionRepository : ITransactionRepository
    {
        #region Fields & Property
        private AccountingManagementContext _context;
        #endregion

        #region Constructor
        public TransactionRepository(AccountingManagementContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region Get All Transaction Repository
        public async Task<IEnumerable<GetAllTransactionDTO>> GetAllTransactionAsync(int userId, int accountId, string? transactionStatus)
        {
            try
            {
                var getAllTransaction = await _context.TransactionTables
                 .Select(x => new GetAllTransactionDTO
                 {
                     TransactionId = x.TransactionId,
                     AccountId = x.AccountId,
                     UserId = x.UserId,
                     Amount = x.Amount,
                     CreditType = x.CreditType,
                     TransactionStatus = x.TransactionStatus,
                     ServerDateTime = x.ServerDateTime,
                     DateTimeUtc = x.DateTimeUtc,
                     UpdateDateTimeUtc = x.UpdateDateTimeUtc
                 }).ToListAsync();

                if (userId > 0)
                    getAllTransaction = getAllTransaction.Where(x => x.UserId == userId).ToList();

                if (accountId > 0)
                    getAllTransaction = getAllTransaction.Where(x => x.AccountId == accountId).ToList();

                if (!string.IsNullOrEmpty(transactionStatus))
                    getAllTransaction = getAllTransaction.Where(x => x.TransactionStatus == transactionStatus).ToList();

                return getAllTransaction;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
        #endregion

        #region Add Transaction Repository
        public async Task<int> AddTransactionAsync(AddTransactionDTO transactionDTO)
        {
            try
            {

                var isValidAccount = await _context.AccountTables.SingleOrDefaultAsync(x => x.AccountId == transactionDTO.AccountId && x.Status != 1);
                if (isValidAccount == null)
                    return 0;

                var addTransaction = new TransactionTable();
                addTransaction.AccountId = transactionDTO.AccountId;
                addTransaction.UserId = isValidAccount.UserId;

                string amount = transactionDTO.Amount.ToString();
                char firstSymbol = amount[0];
                if (firstSymbol == '-')
                {
                    isValidAccount.Balance -= decimal.Parse(amount.Substring(1));
                    addTransaction.Amount = transactionDTO.Amount;
                }
                else if (firstSymbol == '+')
                {
                    isValidAccount.Balance += decimal.Parse(amount.Substring(1));
                    addTransaction.Amount = transactionDTO.Amount;
                }
                else
                {
                    isValidAccount.Balance += decimal.Parse(amount);
                    addTransaction.Amount = transactionDTO.Amount;
                }

                addTransaction.CreditType = transactionDTO.CreditType.ToLower();

                addTransaction.TransactionStatus = "pending";


                addTransaction.ServerDateTime = DateTime.Now;
                addTransaction.DateTimeUtc = DateTime.UtcNow;
                addTransaction.UpdateDateTimeUtc = DateTime.UtcNow;

                _context.Update(isValidAccount);
                await _context.AddAsync(addTransaction);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
        #endregion

        #region Modify Transaction Repository
        public async Task<string> ModifyTransactionAsync(int transactionId, EditTransactionDTO transactionDTO)
        {
            var desiredTransaction = await _context.TransactionTables.SingleOrDefaultAsync(x => x.TransactionId == transactionId);
            var accountInfo = _context.AccountTables.SingleOrDefault(x => x.AccountId == desiredTransaction.AccountId);
            if (transactionDTO.AccountId > 0)
            {
                if (!Regex.IsMatch(transactionDTO.AccountId.ToString(), @"^\d+$"))
                    return "Invalid AccountId.";

                var isFoundAccount = await _context.AccountTables.SingleOrDefaultAsync(x => x.AccountId == transactionDTO.AccountId);
                if (isFoundAccount == null)
                    return "Account not found";

                desiredTransaction.AccountId = transactionDTO.AccountId;
                desiredTransaction.UserId = isFoundAccount.UserId;
                desiredTransaction.UpdateDateTimeUtc = DateTime.UtcNow;
            }

            if (transactionDTO.Amount > 0)
            {

                string amount = transactionDTO.Amount.ToString();
                char firstSymbol = amount[0];
                if (firstSymbol == '-')
                {
                    accountInfo.Balance -= decimal.Parse(amount.Substring(1));
                    desiredTransaction.Amount = transactionDTO.Amount;
                    desiredTransaction.UpdateDateTimeUtc = DateTime.UtcNow;
                }
                else if (firstSymbol == '+')
                {
                    accountInfo.Balance += decimal.Parse(amount.Substring(1));
                    desiredTransaction.Amount = transactionDTO.Amount;
                    desiredTransaction.UpdateDateTimeUtc = DateTime.UtcNow;
                }
                else
                {
                    accountInfo.Balance += decimal.Parse(amount);
                    desiredTransaction.Amount = transactionDTO.Amount;
                    desiredTransaction.UpdateDateTimeUtc = DateTime.UtcNow;
                }
            }

            if (transactionDTO.CreditType != "string")
            {
                if (!Enum.IsDefined(typeof(CreditType), transactionDTO.CreditType.ToLower()))
                {
                    throw new ArgumentException("Invalid CreditType value");
                }

                transactionDTO.CreditType = transactionDTO.CreditType.ToLower();
                desiredTransaction.UpdateDateTimeUtc = DateTime.UtcNow;
            }

            if (transactionDTO.TransactionStatus != "string")
            {
                var isReversed = Enum.IsDefined(typeof(TransactionStatus), transactionDTO.TransactionStatus.ToLower());
                if (isReversed)
                {
                    if (transactionDTO.TransactionStatus.ToLower() == "reversed")
                    {
                        decimal lastBalance = accountInfo.Balance - desiredTransaction.Amount;
                        desiredTransaction.Amount = 0.00M;
                        accountInfo.Balance = lastBalance;
                        desiredTransaction.UpdateDateTimeUtc = DateTime.UtcNow;
                    }
                }
            }
            _context.Update(desiredTransaction);
            _context.Update(accountInfo);
            await _context.SaveChangesAsync();
            return "Modified successfully.";
        }
        #endregion

        #region Delete the Transactions Repository
        public async Task<int> DeleteTransactionAsync(List<int> transactionIds)
        {
            try
            {
                var transactionToDelete = await _context.TransactionTables.Where(x => transactionIds.Contains(x.TransactionId)).ToListAsync();

                foreach (var transaction in transactionToDelete)
                {
                    transaction.TransactionStatus = "delete";
                    _context.Update(transaction);
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

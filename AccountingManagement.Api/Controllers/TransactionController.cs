using AccountingManagement.Core.DTOs.AccountDTOs;
using AccountingManagement.Core.DTOs.TransactionDTOs;
using AccountingManagement.Core.IUnitOfWork;
using AccountingManagement.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel;

namespace AccountingManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        #region Fields & Property
        private readonly IUnitOfWork _unitOfWork;
        private readonly AccountingManagementContext _context;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public TransactionController(AccountingManagementContext _context, IUnitOfWork _unitOfWork, IConfiguration _configuration)
        {
            this._context = _context;
            this._unitOfWork = _unitOfWork;
            this._configuration = _configuration;
        }
        #endregion

        #region View All Transactions
        /// <summary>
        /// Retrieve a numbered view of all transactions.
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ViewAllTransactions([FromQuery, DefaultValue(10)] int pageSize, [FromQuery, DefaultValue(1)] int pageNumber,
                                                             [FromQuery, DefaultValue(0)] int userId, [FromQuery, DefaultValue(0)] int accountId,
                                                             [FromQuery] string? transactionStatus)
        {
            try
            {
                var getAllTransactions = await _unitOfWork.TransactionRepository.GetAllTransactionAsync(userId, accountId, transactionStatus);
                int skipAmount = pageSize * pageNumber - (pageSize);
                var pagedAccount = getAllTransactions.Skip(skipAmount).Take(pageSize);
                return Ok(pagedAccount);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add Transaction
        /// <summary>
        /// Adds a new transaction.
        /// </summary>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionDTO transactionDTO)
        {
            try
            {
                var added = await _unitOfWork.TransactionRepository.AddTransactionAsync(transactionDTO);
                if (added <= 0)
                    return BadRequest("Transaction registration failed.");

                return Ok(added);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Edit Transaction
        /// <summary>
        /// Edit transaction.
        /// </summary>
        [HttpPut]
        [Route("[action]/{transactionId}")]
        public async Task<IActionResult> EditTransaction([FromRoute] int transactionId, [FromBody] EditTransactionDTO transactionDTO)
        {
            try
            {
                var isExistTransaction = await _context.TransactionTables.SingleOrDefaultAsync(x => x.TransactionId == transactionId);
                if (isExistTransaction == null)
                    return NotFound("Account not found.");

                var modified = await _unitOfWork.TransactionRepository.ModifyTransactionAsync(transactionId, transactionDTO);
                return Ok(modified);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Transaction
        /// <summary>
        /// Deletes one or more transaction.
        /// </summary>
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteTransactions([FromBody] List<int> transactionIds)
        {
            var transactionToDelete = await _context.TransactionTables.Where(x => transactionIds.Contains(x.TransactionId)).ToListAsync();
            if (transactionToDelete.Count == 0)
                return NotFound("No transactions found with the provided IDs.");

            var isDelete = await _unitOfWork.TransactionRepository.DeleteTransactionAsync(transactionIds);
            if (isDelete <= 0)
                return BadRequest("No transactions were deleted.");

            return Ok("transactions have been deleted successfully.");
        }
        #endregion
    }
}

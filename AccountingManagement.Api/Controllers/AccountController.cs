using AccountingManagement.Core.DTOs.AccountDTOs;
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
    public class AccountController : ControllerBase
    {
        #region Fields & Property
        private readonly IUnitOfWork _unitOfWork;
        private readonly AccountingManagementContext _context;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public AccountController(AccountingManagementContext _context, IUnitOfWork _unitOfWork, IConfiguration _configuration)
        {
            this._context = _context;
            this._unitOfWork = _unitOfWork;
            this._configuration = _configuration;
        }
        #endregion

        #region View All Account
        /// <summary>
        /// Retrieve a numbered view of all accounts.
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ViewAllAccount([FromQuery, DefaultValue(10)] int pageSize, [FromQuery, DefaultValue(1)] int pageNumber,
                                                        [FromQuery, DefaultValue(0)] int userId, [FromQuery, DefaultValue(0)] int accountId,
                                                        [FromQuery, DefaultValue(0)] int accountNumber)
        {
            try
            {
                var getAllAccounts = await _unitOfWork.AccountRepository.GetAllAccountAsync(userId, accountId, accountNumber);
                int skipAmount = pageSize * pageNumber - (pageSize);
                var pagedAccount = getAllAccounts.Skip(skipAmount).Take(pageSize);
                return Ok(pagedAccount);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add Account
        /// <summary>
        /// Adds a new account.
        /// </summary>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddAccount([FromBody] AddAccountDTO accountDTO)
        {
            try
            {
                var added = await _unitOfWork.AccountRepository.AddAccountAsync(accountDTO);
                if (added <= 0)
                    return BadRequest("Account registration failed.");

                return Ok(added);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Edit Account
        /// <summary>
        /// Edit account.
        /// </summary>
        [HttpPut]
        [Route("[action]/{accountId}")]
        public async Task<IActionResult> EditAccount([FromRoute] int accountId, [FromBody] EditAccountDTO editAccountDTO)
        {
            try
            {
                var isExistAccount = await _context.AccountTables.SingleOrDefaultAsync(x => x.AccountId == accountId);
                if (isExistAccount == null)
                    return NotFound("Account not found.");

                var modified = await _unitOfWork.AccountRepository.ModifyAccountAsync(accountId, editAccountDTO);
                return Ok(modified);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete Accounts
        /// <summary>
        /// Deletes one or more account.
        /// </summary>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DeleteAccounts([FromBody] accountDTO accountDTO)
        {
            List<int> acccountIds = accountDTO.accountIds;
            var accountToDelete = await _context.AccountTables.Where(x => acccountIds.Contains(x.AccountId)).ToListAsync();
            if (accountToDelete.Count == 0)
                return NotFound("No accounts found with the provided IDs.");

            var isDelete = await _unitOfWork.AccountRepository.DeleteAccountAsync(accountDTO);
            if (isDelete <= 0)
                return BadRequest("No accounts were deleted.");

            return Ok("accounts have been deleted successfully.");
        }
        #endregion
    }
}

using AccountingManagement.Core.DTOs.AuthDTOs;
using AccountingManagement.Core.IUnitOfWork;
using AccountingManagement.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AccountingManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields & Property
        private readonly IUnitOfWork _unitOfWork;
        private readonly AccountingManagementContext _context;
        #endregion

        #region Constructor
        public AuthController(IUnitOfWork _unitOfWork, AccountingManagementContext _context)
        {
            this._unitOfWork = _unitOfWork;
            this._context = _context;
        }
        #endregion

        #region Registration
        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationDTO registration)
        {
            try
            {
                var checkRecurrenceLogin = await _context.LoginTables.SingleOrDefaultAsync(x => x.Email == registration.Email);
                if (checkRecurrenceLogin != null)
                    return BadRequest("Email is already registered");

                var checkRecurrenceUser = await _context.UserTables.SingleOrDefaultAsync(x => x.Username == registration.Username);
                if (checkRecurrenceUser != null)
                    return BadRequest("Username is already registered");

                var loseEffect = await _unitOfWork.AuthRepository.RegistrationAsync(registration);
                if (loseEffect != 2)
                    return BadRequest("An error occurred registering the account, try again");

                return Ok("Congratulations, the account has been registered successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex.InnerException);
                return BadRequest("An error occurred while registering the account. Please try again.");
            }
        }
        #endregion
    }
}

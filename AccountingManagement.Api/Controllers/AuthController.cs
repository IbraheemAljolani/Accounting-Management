using AccountingManagement.Core.DTOs.AuthDTOs;
using AccountingManagement.Core.Helpers;
using AccountingManagement.Core.IUnitOfWork;
using AccountingManagement.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Numerics;

namespace AccountingManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields & Property
        private readonly IUnitOfWork _unitOfWork;
        private readonly AccountingManagementContext _context;
        private readonly IConfiguration _configuration;
        private string JWTkey;
        #endregion

        #region Constructor
        public AuthController(AccountingManagementContext _context, IUnitOfWork _unitOfWork, IConfiguration _configuration)
        {
            this._context = _context;
            this._unitOfWork = _unitOfWork;
            JWTkey = _configuration.GetValue<String>("JWTKey");
        }
        #endregion

        #region Registration
        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationDTO registrationDTO)
        {
            try
            {
                var checkRecurrenceLogin = await _context.LoginTables.SingleOrDefaultAsync(x => x.Email == registrationDTO.Email);
                if (checkRecurrenceLogin != null)
                    return BadRequest("Email is already registered.");

                var checkRecurrenceUser = await _context.UserTables.SingleOrDefaultAsync(x => x.Username == registrationDTO.Username);
                if (checkRecurrenceUser != null)
                    return BadRequest("Username is already registered.");

                var loseEffect = await _unitOfWork.AuthRepository.RegistrationAsync(registrationDTO);
                if (loseEffect != 2)
                    return BadRequest("An error occurred registering the account, try again.");

                return Ok("Congratulations, the account has been registered successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest("An error occurred while registering the account. Please try again.");
            }
        }
        #endregion

        #region Login
        /// <summary>
        /// Logs in a user.
        /// </summary>
        [HttpPut]
        [Route("action")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var CheckEmail = await _context.LoginTables.SingleOrDefaultAsync(x => x.Email == loginDTO.Email);
                if (CheckEmail == null) 
                    return NotFound("The email does not exist. Please check the validity of the email and try again.");

                if (!HelperApi.VerifyPasswordHash(loginDTO.Password, CheckEmail.PasswordHash, CheckEmail.PasswordSalt))
                    return NotFound("Incorrect password.");

                if (CheckEmail.IsActive)
                    return BadRequest("User is already logged in.");

                var token = await _unitOfWork.AuthRepository.LoginAsync(loginDTO, JWTkey);
                if (token == null)
                    return BadRequest("An error occurred while logging in. Please try again.");

                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest("An error occurred while logging in. Please try again.");
            }
        }
        #endregion
    }
}

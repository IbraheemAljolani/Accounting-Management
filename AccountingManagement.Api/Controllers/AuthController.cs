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
            this._configuration = _configuration;
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
                var checkEmailUsage = await _context.LoginTables.SingleOrDefaultAsync(x => x.Email == registrationDTO.Email);
                if (checkEmailUsage != null)
                    return BadRequest("Email is already registered.");

                var checkUsernameUsage = await _context.UserTables.SingleOrDefaultAsync(x => x.Username == registrationDTO.Username);
                if (checkUsernameUsage != null)
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
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var isValidEmail = await _context.LoginTables.SingleOrDefaultAsync(x => x.Email == loginDTO.Email);
                if (isValidEmail == null)
                    return NotFound("The email does not exist. Please check the validity of the email and try again.");

                var isValidUser = await _context.UserTables.SingleOrDefaultAsync(x => x.Email == loginDTO.Email && x.Status == 2);
                if (isValidUser != null)
                    return Unauthorized("The user has been deleted.");

                if (!HelperApi.VerifyPasswordHash(loginDTO.Password, isValidEmail.PasswordHash, isValidEmail.PasswordSalt))
                    return NotFound("Incorrect password.");

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

        #region Logout
        /// <summary>
        /// Logs out a user.
        /// </summary>
        [HttpPut]
        [Route("Logout/{token}")]
        public async Task<IActionResult> Logout([FromRoute] string token)
        {
            try
            {
                var isLogout = await _unitOfWork.AuthRepository.LogoutAsync(token);
                if (isLogout <= 0)
                    return NotFound("Please log in first.");

                return Ok("Logout successful.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region ForgotPassword
        /// <summary>
        /// Initiates the password reset process for a user.
        /// </summary>
        [HttpPut]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO passwordDTO)
        {
            try
            {
                var checkAccount = await _context.LoginTables.SingleOrDefaultAsync(x => x.Email == passwordDTO.Email);
                if (checkAccount == null)
                    return NotFound("Email not found");

                var checkUser = await _context.UserTables.SingleOrDefaultAsync(x => x.Status == 2);
                if (checkUser != null)
                    return Unauthorized("The user has been deleted.");

                var changePassword = await _unitOfWork.AuthRepository.ForgotPasswordAsync(passwordDTO);
                if (changePassword <= 0)
                    return BadRequest("An error occurred while processing the password reset request.");

                return Ok("Password has been modified successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}

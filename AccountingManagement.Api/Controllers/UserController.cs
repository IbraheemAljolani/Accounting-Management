using AccountingManagement.Core.DTOs.AuthDTOs;
using AccountingManagement.Core.DTOs.UserDTOs;
using AccountingManagement.Core.Helpers;
using AccountingManagement.Core.IUnitOfWork;
using AccountingManagement.Core.Models;
using AccountingManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel;

namespace AccountingManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Fields & Property
        private readonly IUnitOfWork _unitOfWork;
        private readonly AccountingManagementContext _context;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public UserController(AccountingManagementContext _context, IUnitOfWork _unitOfWork, IConfiguration _configuration)
        {
            this._context = _context;
            this._unitOfWork = _unitOfWork;
            this._configuration = _configuration;
        }
        #endregion

        #region View All Users
        /// <summary>
        /// Retrieves a paginated view of all users.
        /// </summary>
        [HttpGet]
        [Route("ViewAllUsers")]
        public async Task<IActionResult> ViewAllUsers([FromQuery, DefaultValue(10)] int pageSize, [FromQuery, DefaultValue(1)] int pageNumber,
                                                      [FromQuery] string? userName, [FromQuery] string? email, [FromQuery, DefaultValue(0)] int userId)
        {
            try
            {
                var getAllUsers = await _unitOfWork.UserRepository.GetAllUsersAsync(userName, email, userId);
                int skipAmount = pageSize * pageNumber - (pageSize);
                var pagedUsers = getAllUsers.Skip(skipAmount).Take(pageSize);
                return Ok(pagedUsers);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add New User
        /// <summary>
        /// Adds a new user.
        /// </summary>
        [HttpPost]
        [Route("AddNewUser")]
        public async Task<IActionResult> AddNewUser([FromBody] RegistrationDTO registrationDTO)
        {
            try
            {
                var checkRecurrenceEmail = await _context.LoginTables.SingleOrDefaultAsync(x => x.Email == registrationDTO.Email);
                if (checkRecurrenceEmail != null)
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
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Edit User
        /// <summary>
        /// Edit user.
        /// </summary>
        [HttpPut]
        [Route("EditUser/{userId}")]
        public async Task<IActionResult> EditUser([FromBody] EditUserDTO editDTO, [FromRoute] int userId)
        {
            try
            {
                var isExistUser = await _context.UserTables.SingleOrDefaultAsync(x => x.UserId == userId);
                if (isExistUser == null)
                    return NotFound("User not found.");

                var modified = await _unitOfWork.UserRepository.ModifyUserAsync(editDTO, userId);
                return Ok(modified);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete Users
        /// <summary>
        /// Deletes one or more users.
        /// </summary>
        [HttpPost]
        [Route("DeleteUsers")]
        public async Task<IActionResult> DeleteUsers([FromBody] deleteDTO dto)
        {
            List<int> userIds = dto.userIds;
            try
            {
                var usersToDelete = await _context.UserTables.Where(x => userIds.Contains(x.UserId)).ToListAsync();
                if (usersToDelete.Count == 0)
                    return NotFound("No users found with the provided IDs.");

                var isDelete = await _unitOfWork.UserRepository.DeleteUsersAsync(dto);
                if (isDelete <= 0)
                    return BadRequest("No users were deleted.");

                return Ok("Users have been deleted successfully.");
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

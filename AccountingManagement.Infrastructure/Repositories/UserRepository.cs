using AccountingManagement.Core.DTOs.UserDTOs;
using AccountingManagement.Core.Helpers;
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
    public class UserRepository : IUserRepository
    {
        #region Fields & Property
        private AccountingManagementContext _context;
        #endregion

        #region Constructor
        public UserRepository(AccountingManagementContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region Get All Users Repository
        public async Task<IEnumerable<GetAllUsersDTO>> GetAllUsersAsync(string userName, string email, int userId)
        {
            try
            {
                var getlistedOfUsers = await _context.UserTables
                    .Select(x => new GetAllUsersDTO
                    {
                        UserId = x.UserId,
                        ServerDateTime = x.ServerDateTime,
                        DateTimeUtc = x.DateTimeUtc,
                        UpdateDateTimeUtc = x.UpdateDateTimeUtc,
                        Username = x.Username,
                        Email = x.Email,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Status = ((Status)x.Status).ToString(),
                        Gender = ((Gender)x.Gender).ToString(),
                        DateOfBirth = x.DateOfBirth
                    }).ToListAsync();

                if (!string.IsNullOrEmpty(userName))
                    getlistedOfUsers = getlistedOfUsers.Where(x => x.Username == userName).ToList();

                if (!string.IsNullOrEmpty(email))
                    getlistedOfUsers = getlistedOfUsers.Where(x => x.Email == email).ToList();

                if (userId > 0)
                    getlistedOfUsers = getlistedOfUsers.Where(x => x.UserId == userId).ToList();

                return getlistedOfUsers;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
        #endregion

        #region Modify User Repository
        public async Task<string> ModifyUserAsync(EditUserDTO editDTO, int userId)
        {
            try
            {
                var desiredUser = await _context.UserTables.SingleOrDefaultAsync(x => x.UserId == userId);
                var desiredLogin = await _context.LoginTables.SingleOrDefaultAsync(x => x.UserId == userId);
                if (desiredUser != null)
                {
                    if (string.IsNullOrEmpty(editDTO.Username) || editDTO.Username != "string")
                    {
                        var isValidUsername = Regex.IsMatch(editDTO.Username, @"^[a-zA-Z0-9][a-zA-Z0-9 ]*$");
                        if (!isValidUsername)
                            return "Invalid username. Only English letters and numbers are allowed.";

                        var isUsernameTaken = await _context.UserTables.AnyAsync(x => x.Username == editDTO.Username);
                        if (isValidUsername)
                            return "Username already exists. Please choose a different one.";

                        desiredUser.Username = editDTO.Username;
                        desiredUser.UpdateDateTimeUtc = DateTime.UtcNow;
                    }
                    if (string.IsNullOrEmpty(editDTO.Email) || editDTO.Email != "string")
                    {
                        var isValidEmail = Regex.IsMatch(editDTO.Email, @"^\w+([\.-]?\w+)*@(gmail\.com|yahoo\.com|hotmail\.com)$");
                        if (!isValidEmail)
                            return "Invalid email address, Ex: user@example.com";

                        var IsEmailTaken = await _context.UserTables.AnyAsync(x => x.Email == editDTO.Email);
                        if (IsEmailTaken)
                            return "Email address already exists. Please choose a different one.";

                        desiredUser.Email = editDTO.Email;
                        desiredUser.UpdateDateTimeUtc = DateTime.UtcNow;
                    }
                    if (string.IsNullOrEmpty(editDTO.FirstName) || editDTO.FirstName != "string")
                    {
                        desiredUser.FirstName = editDTO.FirstName;
                        desiredUser.UpdateDateTimeUtc = DateTime.UtcNow;
                    }
                    if (string.IsNullOrEmpty(editDTO.LastName) || editDTO.LastName != "string")
                    {
                        desiredUser.LastName = editDTO.LastName;
                        desiredUser.UpdateDateTimeUtc = DateTime.UtcNow;
                    }
                    if (string.IsNullOrEmpty(editDTO.Status) || editDTO.Status != "string")
                    {
                        desiredUser.Status = (int)Enum.Parse(typeof(Status), editDTO.Status.ToLower());
                        desiredUser.UpdateDateTimeUtc = DateTime.UtcNow;
                    }
                    if (string.IsNullOrEmpty(editDTO.Gender) || editDTO.Gender != "string")
                    {
                        desiredUser.Gender = (int)Enum.Parse(typeof(Gender), editDTO.Gender.ToLower());
                        desiredUser.UpdateDateTimeUtc = DateTime.UtcNow;
                    }
                    if (string.IsNullOrEmpty(editDTO.DateOfBirth) || editDTO.DateOfBirth != "string")
                    {
                        desiredUser.DateOfBirth = DateTime.Parse(editDTO.DateOfBirth);
                        desiredUser.UpdateDateTimeUtc = DateTime.UtcNow;
                    }

                    _context.Update(desiredUser);
                    await _context.SaveChangesAsync();
                }

                if (desiredLogin != null && string.IsNullOrEmpty(editDTO.Password) || editDTO.Password != "string")
                {
                    var isValidPassword = Regex.IsMatch(editDTO.Password, @"^(?=.*[A-Z])(?=.*\d)\S{8,}$");
                    if (!isValidPassword)
                        return "Invalid Password";

                    HelperApi.CreatePasswordHash(editDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    desiredLogin.PasswordHash = passwordHash;
                    desiredLogin.PasswordSalt = passwordSalt;
                    desiredUser.UpdateDateTimeUtc = DateTime.UtcNow;

                    _context.Update(desiredLogin);
                    _context.Update(desiredUser);
                    await _context.SaveChangesAsync();
                }
                return "Modified successfully.";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete the users Repository
        public async Task<int> DeleteUsersAsync(List<int> userIds)
        {
            try
            {
                var usersToDelete = await _context.UserTables.Where(x => userIds.Contains(x.UserId)).ToListAsync();

                foreach (var user in usersToDelete)
                {
                    var logoutForUser = await _context.LoginTables.SingleOrDefaultAsync(x => x.UserId == user.UserId && x.IsActive == true);

                    if (logoutForUser != null)
                    {
                        logoutForUser.IsActive = false;
                        _context.Update(logoutForUser);
                        await _context.SaveChangesAsync();
                    }
                    user.Status = 1;
                    _context.Update(user);
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

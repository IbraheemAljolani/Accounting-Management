using AccountingManagement.Core.DTOs.AuthDTOs;
using AccountingManagement.Core.Helpers;
using AccountingManagement.Core.Interface;
using AccountingManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace AccountingManagement.Infrastructure.Repositories
{
    #region enum User Status
    public enum UserStatus
    {
        Delete = 1,
        Active
    }
    #endregion

    #region enum User Gender
    public enum UserGender
    {
        Male = 1,
        Female,
        Other
    }
    #endregion

    public class AuthRepository : IAuthRepository
    {
        #region Fields & Property
        private AccountingManagementContext _context;
        #endregion

        #region Constructor
        public AuthRepository(AccountingManagementContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region Registration Repositories
        public async Task<int> RegistrationAsync(RegistrationDTO registrationDTO)
        {
            try
            {
                int rowsaffected = 0;

                UserTable user = new UserTable();
                user.ServerDateTime = DateTime.Now;
                user.DateTimeUtc = DateTime.UtcNow;
                user.UpdateDateTimeUtc = SqlDateTime.MinValue.Value;
                user.Username = registrationDTO.Username;
                user.Email = registrationDTO.Email;
                user.FirstName = registrationDTO.FirstName;
                user.LastName = registrationDTO.LastName;
                user.Status = 1;
                user.Gender = (int)Enum.Parse(typeof(UserGender), registrationDTO.Gender);
                user.DateOfBirth = registrationDTO.DateOfBirth;
                await _context.AddAsync(user);
                rowsaffected += await _context.SaveChangesAsync();

                HelperApi.CreatePasswordHash(registrationDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                LoginTable login = new LoginTable();
                login.Email = registrationDTO.Email;
                login.PasswordHash = passwordHash;
                login.PasswordSalt = passwordSalt;
                login.IsActive = false;
                login.UserId = user.UserId;
                //_context.UserTables.SingleOrDefault(x => x.Email == registration.Email).UserId;
                await _context.AddAsync(login);
                rowsaffected += await _context.SaveChangesAsync();
                return rowsaffected;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return 0;
            }
        }
        #endregion

        #region Login Repositories
        public async Task<string> LoginAsync(LoginDTO loginDTO, string JWTkey)
        {
            try
            {
                var checkAccount = await _context.LoginTables.SingleOrDefaultAsync(x => x.Email == loginDTO.Email);

                HelperApi helperApi = new HelperApi(_context);

                string tokin = helperApi.GenerateJwtToken(loginDTO, JWTkey);
                if (tokin == null)
                    return null;

                checkAccount.IsActive = true;
                _context.Update(checkAccount);
                await _context.SaveChangesAsync();

                return tokin;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return ex.Message;
            }
        }
        #endregion
    }
}




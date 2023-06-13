using AccountingManagement.Core.DTOs.AuthDTOs;
using AccountingManagement.Core.Helpers;
using AccountingManagement.Core.Interface;
using AccountingManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<int> RegistrationAsync(RegistrationDTO registration)
        {
            try
            {
                int rowsaffected = 0;

                UserTable user = new UserTable();
                user.ServerDateTime = DateTime.Now;
                user.DateTimeUtc = DateTime.UtcNow;
                user.UpdateDateTimeUtc = SqlDateTime.MinValue.Value;
                user.Username = registration.Username;
                user.Email = registration.Email;
                user.FirstName = registration.FirstName;
                user.LastName = registration.LastName;
                user.Status = 1;
                user.Gender = (int)Enum.Parse(typeof(UserGender), registration.Gender);
                user.DateOfBirth = registration.DateOfBirth;
                await _context.AddAsync(user);
                rowsaffected += await _context.SaveChangesAsync();

                HelperApi.CreatePasswordHash(registration.Password, out byte[] passwordHash, out byte[] passwordSalt);

                LoginTable login = new LoginTable();
                login.Email = registration.Email;
                login.PasswordHash = passwordHash;
                login.PasswordSalt = passwordSalt;
                login.IsActive = true;
                login.UserId = user.UserId;
                    //_context.UserTables.SingleOrDefault(x => x.Email == registration.Email).UserId;
                await _context.AddAsync(login);
                rowsaffected += await _context.SaveChangesAsync();
                return rowsaffected;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex.InnerException);
                return 0;
            }
        }
        #endregion
    }
}




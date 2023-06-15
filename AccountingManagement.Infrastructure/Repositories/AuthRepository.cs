using AccountingManagement.Core.DTOs.AccountDTOs;
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

        #region RegistrationAsync Repository
        public async Task<int> RegistrationAsync(RegistrationDTO registrationDTO)
        {
            try
            {
                int rowsaffected = 0;

                UserTable user = new UserTable();
                user.ServerDateTime = DateTime.Now;
                user.DateTimeUtc = DateTime.UtcNow;
                user.UpdateDateTimeUtc = DateTime.UtcNow;
                user.Username = registrationDTO.Username;
                user.Email = registrationDTO.Email;
                user.FirstName = registrationDTO.FirstName;
                user.LastName = registrationDTO.LastName;
                user.Status = 1;
                if (Enum.IsDefined(typeof(Gender), registrationDTO.Gender.ToLower()))
                {
                    user.Gender = (int)Enum.Parse(typeof(Gender), registrationDTO.Gender.ToLower());
                }
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

        #region LoginAsync Repository
        public async Task<string> LoginAsync(LoginDTO loginDTO, string JWTkey)
        {
            try
            {
                var checkAccount = await _context.LoginTables.SingleOrDefaultAsync(x => x.Email == loginDTO.Email);
                var user = await _context.UserTables.SingleOrDefaultAsync(x => x.Email == loginDTO.Email);

                LoginResponseDTO responseDTO = new LoginResponseDTO();
                responseDTO.LoginId = checkAccount.LoginId;
                responseDTO.UserId = user.UserId;
                responseDTO.Email = loginDTO.Email;

                HelperApi helperApi = new HelperApi(_context);
                string tokin = helperApi.GenerateJwtToken(responseDTO, JWTkey);
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

        #region LogoutAsync Repository
        public async Task<int> LogoutAsync(string token)
        {
            try
            {
                LoginResponseDTO loginResponseDTO = new LoginResponseDTO();
                if (HelperApi.ValidateJWTtoken(token, out loginResponseDTO))
                {
                    if (loginResponseDTO.LoginId != 0)
                    {
                        var login = await _context.LoginTables.SingleOrDefaultAsync(x => x.LoginId == loginResponseDTO.LoginId);

                        if (login != null)
                        {
                            if (login.IsActive == true)
                            {
                                login.IsActive = false;
                                _context.Update(login);
                                return await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return 0;
            }
        }
        #endregion

        #region ForgotPasswordAsync Repository
        public async Task<int> ForgotPasswordAsync(ForgotPasswordDTO passwordDTO)
        {
            try
            {
                var login = await _context.LoginTables.SingleOrDefaultAsync(x => x.Email == passwordDTO.Email);
                if (passwordDTO.NewPassword == passwordDTO.ConfirmPassword)
                {
                    HelperApi.CreatePasswordHash(passwordDTO.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                    login.PasswordHash = passwordHash;
                    login.PasswordSalt = passwordSalt;
                    _context.Update(login);
                    return await _context.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return 0;
            }
        }
        #endregion
    }
}




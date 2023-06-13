using AccountingManagement.Core.DTOs.AuthDTOs;
using AccountingManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.Helpers
{
    public class HelperApi
    {
        #region Fields & Property
        private readonly IConfiguration _configuration;
        private readonly AccountingManagementContext _context;
         private string JWTkey;
        #endregion

        #region Constructor
        public HelperApi(AccountingManagementContext _Context)
        {
            this._context = _Context;
        }
        public HelperApi(AccountingManagementContext _Context, IConfiguration _configuration)
        {
            this._context = _Context;
            this._configuration = _configuration;
            JWTkey = _configuration.GetValue<String>("JWTKey");
        }
        #endregion

        #region Create Password Hash
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion

        #region Verify Password Hash
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        #endregion

        #region Generate Jwt Token
        public string GenerateJwtToken(LoginDTO login, string JWTkey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(JWTkey);

            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Email,login.Email),
                        new Claim(ClaimTypes.Role, "user")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey)
                , SecurityAlgorithms.HmacSha512Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptior);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}

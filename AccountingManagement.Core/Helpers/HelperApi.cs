using AccountingManagement.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.Helpers
{
    public class HelperApi
    {
        //#region Fields & Property
        //private readonly IConfiguration _configuration;
        //private readonly AccountingManagementContext _Context;
        //#endregion

        //#region Constructor
        //public HelperApi(IConfiguration _configuration, AccountingManagementContext _Context)
        //{
        //    this._configuration = _configuration;
        //    this._Context = _Context;
        //}
        //#endregion

       

        #region Password Hash
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion
    }
}

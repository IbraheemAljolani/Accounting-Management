﻿using AccountingManagement.Core.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.Interface
{
    public interface IAuthRepository
    {
        Task<int> RegistrationAsync(RegistrationDTO registrationDTO);
        Task<string> LoginAsync(LoginDTO loginDTO,string JWTkey);
        Task<int> LogoutAsync(string token);
        Task<int> ForgotPasswordAsync(ForgotPasswordDTO passwordDTO);
    }
}

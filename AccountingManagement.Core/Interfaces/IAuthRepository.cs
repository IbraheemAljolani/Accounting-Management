using AccountingManagement.Core.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.Interface
{
    public interface IAuthRepository
    {
        Task<int> RegistrationAsync(RegistrationDTO registration);
    }
}

using AccountingManagement.Core.DTOs.UserDTOs;
using AccountingManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<GetAllUsersDTO>> GetAllUsersAsync(string userName, string email, int userId);
        Task<int> DeleteUsersAsync(List<int> userIds);
        Task<string> ModifyUserAsync(EditUserDTO editDTO, int userId);
    }
}

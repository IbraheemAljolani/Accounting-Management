using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.DTOs.AuthDTOs
{
    public class LoginResponseDTO
    {
        public int LoginId { get; set; }
        public int UserId { get; set; }
        public string? Email { get; set; }
    }
}

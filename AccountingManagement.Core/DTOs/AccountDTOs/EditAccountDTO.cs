using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.DTOs.AccountDTOs
{
    public class EditAccountDTO
    {
        public string? UserId { get; set; }
        public string? Balance { get; set; }
        public string? Currency { get; set; } = null!;
        public string? Status { get; set; }
    }
}

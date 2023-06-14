using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.DTOs.UserDTOs
{
    public class EditUserDTO
    {
        public DateTime UpdateDateTimeUtc { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public string? Password { get; set; }
        public string DateOfBirth { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.DTOs.AuthDTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email field is required.")]
        [RegularExpression(@"^\w+([\.-]?\w+)*@(gmail\.com|yahoo\.com|hotmail\.com)$", ErrorMessage = "The entered syntax must be valid eg.\"user@example.com\".")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password field is required.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)\S{8,}$", ErrorMessage = ("Incorrect password should:\r\n1 - At least one capital letter.\r\n2 - at least one number.\r\n3 - It does not contain any spaces.\r\n4- It must be at least 8 characters long."))]
        public string Password { get; set; }
    }
}

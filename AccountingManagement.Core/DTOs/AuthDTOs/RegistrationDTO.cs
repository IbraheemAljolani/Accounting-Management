using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.DTOs.AuthDTOs
{
    public class RegistrationDTO
    {
        [Required(ErrorMessage = "Name field is required.")]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9 ]*$", ErrorMessage = "This field contains letters and numbers only.")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Email field is required.")]
        [RegularExpression(@"^\w+([\.-]?\w+)*@(gmail\.com|yahoo\.com|hotmail\.com)$",ErrorMessage = "The entered syntax must be valid eg.\"user@example.com\".")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "First name field is required.")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Last name field is required.")]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "The gender field is required. You must choose from the dropdown list.")]
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Password field is required.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)\S{8,}$",ErrorMessage =("Incorrect password should: 1- At least one capital letter. 2- at least one number. 3- It does not contain any spaces. 4- It must be at least 8 characters long."))]
        public string Password { get; set; }
    }
}

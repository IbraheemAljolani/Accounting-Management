using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.DTOs.AccountDTOs
{
    public class AddAccountDTO
    {
        [Required(ErrorMessage = "User ID field is required"),RegularExpression(@"^\d+$",ErrorMessage = "Only valid numbers are allowed")]
        public int? UserId { get; set; }
        [Required(ErrorMessage = "Balance field is required"),RegularExpression(@"^\d+(\.\d+)?$",ErrorMessage =("It only accepts numbers, you must enter the decimal place"))]
        public decimal Balance { get; set; }
        [Required(ErrorMessage = "Currency field is required")]
        public string Currency { get; set; } = null!;
    }
}

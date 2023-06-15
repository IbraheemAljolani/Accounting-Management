using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.DTOs.TransactionDTOs
{
    public class AddTransactionDTO
    {
        [Required(ErrorMessage = "Account ID field is required"), RegularExpression(@"^\d+$", ErrorMessage = "Only valid numbers are allowed")]
        public int? AccountId { get; set; }
        [Required(ErrorMessage = "Amount field is required"), RegularExpression(@"^[+-]?\d+(\.\d+)?$", ErrorMessage = ("It only accepts numbers, you must enter the decimal place"))]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "CreditType field is required")]
        public string CreditType { get; set; } = null!;
    }
}

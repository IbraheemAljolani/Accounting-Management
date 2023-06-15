using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.DTOs.TransactionDTOs
{
    public class EditTransactionDTO
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string CreditType { get; set; } = null!;
        public string TransactionStatus { get; set; } = null!;
    }
}

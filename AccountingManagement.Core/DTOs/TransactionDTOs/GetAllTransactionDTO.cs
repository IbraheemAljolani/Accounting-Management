using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.DTOs.TransactionDTOs
{
    public class GetAllTransactionDTO
    {
        public int TransactionId { get; set; }
        public int? UserId { get; set; }
        public int? AccountId { get; set; }
        public decimal Amount { get; set; }
        public string CreditType { get; set; } = null!;
        public string TransactionStatus { get; set; } = null!;
        public DateTime ServerDateTime { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public DateTime UpdateDateTimeUtc { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace AccountingManagement.Core.Models
{
    public partial class TransactionTable
    {
        public int TransactionId { get; set; }
        public int? UserId { get; set; }
        public int? AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime ServerDateTime { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public DateTime UpdateDateTimeUtc { get; set; }

        public virtual AccountTable? Account { get; set; }
        public virtual UserTable? User { get; set; }
    }
}

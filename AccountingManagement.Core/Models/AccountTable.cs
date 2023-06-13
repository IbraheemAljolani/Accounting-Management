using System;
using System.Collections.Generic;

namespace AccountingManagement.Core.Models
{
    public partial class AccountTable
    {
        public AccountTable()
        {
            TransactionTables = new HashSet<TransactionTable>();
        }

        public int AccountId { get; set; }
        public int? UserId { get; set; }
        public DateTime ServerDateTime { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public DateTime UpdateDateTimeUtc { get; set; }
        public string AccountNumber { get; set; } = null!;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = null!;
        public int Status { get; set; }

        public virtual UserTable? User { get; set; }
        public virtual ICollection<TransactionTable> TransactionTables { get; set; }
    }
}

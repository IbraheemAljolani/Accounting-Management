using System;
using System.Collections.Generic;

namespace AccountingManagement.Core.Models
{
    public partial class UserTable
    {
        public UserTable()
        {
            AccountTables = new HashSet<AccountTable>();
            LoginTables = new HashSet<LoginTable>();
            TransactionTables = new HashSet<TransactionTable>();
        }

        public int UserId { get; set; }
        public DateTime ServerDateTime { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public DateTime UpdateDateTimeUtc { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Status { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<AccountTable> AccountTables { get; set; }
        public virtual ICollection<LoginTable> LoginTables { get; set; }
        public virtual ICollection<TransactionTable> TransactionTables { get; set; }
    }
}

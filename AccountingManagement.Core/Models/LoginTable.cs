using System;
using System.Collections.Generic;

namespace AccountingManagement.Core.Models
{
    public partial class LoginTable
    {
        public int LoginId { get; set; }
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public bool IsActive { get; set; }
        public int? UserId { get; set; }

        public virtual UserTable? User { get; set; }
    }
}

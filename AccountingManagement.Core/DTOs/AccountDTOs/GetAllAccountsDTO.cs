using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Core.DTOs.AccountDTOs
{
    public class GetAllAccountsDTO
    {
        public string AccountId { get; set; }
        public string UserId { get; set; }
        public DateTime ServerDateTime { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public DateTime UpdateDateTimeUtc { get; set; }
        public string AccountNumber { get; set; }
        public string Balance { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
    }
}

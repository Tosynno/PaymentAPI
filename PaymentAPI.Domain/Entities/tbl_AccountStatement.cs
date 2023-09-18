using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Entities
{
    public class tbl_Account
    {
        [Key]
        public Guid Id { get; set; }
        public string ProfileId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public double ClosingBalance { get; set; }
        public double AvailableBalance { get; set; }
        public double TotalCredit { get; set; }
        public double TotalDebit { get; set; }
        public DateTime Date { get; set; }

       // public List<tbl_PaymentTransaction> tblPaymentInwardTransaction { get; set; } = new List<tbl_PaymentTransaction>();
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Entities
{
    public class tbl_AccountStatement
    {
        public Guid Id { get; set; }
        public Guid PaymentProfileId { get; set; }
        public double ClosingBalance { get; set; }
        public double AvailableBalance { get; set; }
        public double TotalCredit { get; set; }
        public double TotalDebit { get; set; }
        public DateTime Date { get; set; }

        public tbl_PaymentProfile tblPaymentProfile { get; set; }
       // public List<tbl_PaymentTransaction> tblPaymentInwardTransaction { get; set; } = new List<tbl_PaymentTransaction>();
       
    }
}

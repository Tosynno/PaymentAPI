using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Entities
{
    public class tbl_NIPTransaction
    {
        public Guid Id { get; set; }
        public Guid PaymentTransactionId { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public char TransactionType { get; set; }
        public string CreditMerchantNumber { get; set; }
        public string DebitMerchantNumber { get; set; }
        public string Narration { get; set; }
        public double TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
       // public tbl_AccountStatement tblAccountStatement { get; set; }
        public tbl_PaymentTransaction tblPaymentInwardTransaction { get; set; } 
    }
}

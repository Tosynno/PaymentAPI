using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Entities
{
    public class tbl_PaymentTransaction
    {
        public Guid Id { get; set; }
        //public Guid AccountStatementId { get; set; }
        public Guid ProfileId { get; set; }
        public string Tran_id { get; set; }
        public string Part_tran_srl_num { get; set; }
        public string CreditAccountNumber { get; set; }
        public char TransactionType { get; set; }
        public string DebitAccountNumber { get; set; }
        public string Narration { get; set; }
        public double TransactionAmount { get; set; }
        public double Balance { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public tbl_Marchant tblPaymentProfile { get; set; }
        //public tbl_AccountStatement tblAccountStatement { get; set; }
        public List<tbl_NIPTransaction> tbl_NIPTransaction { get; set; } = new List<tbl_NIPTransaction>();
    }
}

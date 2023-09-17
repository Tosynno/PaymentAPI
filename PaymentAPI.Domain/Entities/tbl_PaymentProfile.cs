using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Entities
{
    public class tbl_PaymentProfile
    {
        public Guid Id { get; set; }
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string ContactName{ get; set; }
        public string ContactSurname { get; set; }
        public DateTime DateOfEstablishment { get; set; }
        public string MerchantNumber { get; set; }
        public double AverageTransaction { get; set; }
        public string NationalIDNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public List<tbl_PaymentTransaction> tblPaymentInwardTransaction { get; set; } = new List<tbl_PaymentTransaction>();
        public List<tbl_NIPTransaction> tblPaymentOutwardTransaction { get; set; } = new List<tbl_NIPTransaction>();
        public List<tbl_AccountStatement> tblAccountStatement { get; set; } = new List<tbl_AccountStatement>();
    }
}

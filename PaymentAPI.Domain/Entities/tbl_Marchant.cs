using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Entities
{
    public class tbl_Marchant
    {
        public Guid Id { get; set; }
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string ContactName{ get; set; }
        public string ContactSurname { get; set; }
        public DateTime DateOfEstablishment { get; set; }
       // public string AccountNumber { get; set; }
        public double AverageTransaction { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
       
    }
}

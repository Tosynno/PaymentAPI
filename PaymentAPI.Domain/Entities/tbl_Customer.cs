using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Entities
{
    public class tbl_Customer
    {
        public Guid Id { get; set; } 
        public string NationalIDNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CustomerNumber { get; set; }
        //public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateofBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}

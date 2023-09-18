using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Models
{
    public class CreateCustomerRequest
    {
        public string NationalIDNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CustomerNumber { get; set; }
        public string AccountNumber { get; set; }
        public DateTime DateofBirth { get; set; }
    }
}

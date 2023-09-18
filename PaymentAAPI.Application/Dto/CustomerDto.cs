using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Dto
{
    public class CustomerDto
    {
        public string NationalIDNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CustomerNumber { get; set; }
        public string Status { get; set; }
        public string DateofBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}

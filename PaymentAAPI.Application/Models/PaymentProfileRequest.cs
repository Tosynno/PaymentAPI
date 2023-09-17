using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Models
{
    public class PaymentProfileRequest
    {
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string ContactName { get; set; }
        public string ContactSurname { get; set; }
        public DateTime DateOfEstablishment { get; set; }
        public string NationalIDNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
    public class UpdatePaymentProfileRequest
    {
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string MarchantNumber { get; set; }
        public string ContactName { get; set; }
        public string ContactSurname { get; set; }
        public DateTime DateOfEstablishment { get; set; }
        public string NationalIDNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Models
{
    public class IntraBankTransferRequest
    {
        public string CreditMerchantNumber { get; set; }
        public string DebitMerchantNumber { get; set; }
        public string? Narration { get; set; }
        public double TransactionAmount { get; set; }
    }
}

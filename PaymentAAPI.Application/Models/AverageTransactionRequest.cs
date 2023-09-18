using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Models
{
    public class AverageTransactionRequest
    {
        public string BusinessId { get; set; }
        public double TransactionAmountlimit { get; set; }
    }
}

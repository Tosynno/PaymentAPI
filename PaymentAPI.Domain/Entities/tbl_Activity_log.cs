using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Entities
{
    public class tbl_Activity_log
    {
        public long Id { get; set; }
        public string Request { get; set; }
        public string ClientName { get; set; }
        public string Action { get; set; }
        public string IPAddress { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

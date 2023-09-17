using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Models
{
    public class GetAllMarchantRequest
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public bool previous { get; set; }
        public bool next { get; set; }
    }
}

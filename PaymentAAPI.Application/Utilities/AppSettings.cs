using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Utilities
{
    public class AppSettings
    {
        public string? Swagger { get; set; }
        public string ClientKey { get; internal set; }
    }
}

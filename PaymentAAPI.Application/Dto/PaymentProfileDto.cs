﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Dto
{
    public class PaymentProfileDto
    {
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string ContactName { get; set; }
        public string ContactSurname { get; set; }
        public DateTime DateOfEstablishment { get; set; }
      
        public double AverageTransaction { get; set; }
       
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}

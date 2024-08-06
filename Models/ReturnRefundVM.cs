using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class ReturnRefundVM
    {
        public Sale sale { get; set; }

        public ReturnAndRefund ReturnAndRefund { get; set; }    

        public Delivery delivery { get; set; }

        public int? ProducId { get; set; }

        public int? ReasonId { get; set; }   
        public int? RNRId { get; set; }
    }
}
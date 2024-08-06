using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Coupon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodeId { get; set; }
        public string CouponCode { get; set; }
        public int? Discount { get; set; }
        public int CouponCounter { get; set; }

        // Email list will be stored in controller 

        public bool CouponIsActive { get; set; }
    }
}
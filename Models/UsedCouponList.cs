using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class UsedCouponList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string CouponEntry { get; set; }
        //Id from Coupon Code

        public Coupon Coupon;

        //User of the Coupon
        public string userId { get; set; }

        //Date Coupon was used
        public DateTime CouponUsed { get; set; }

        //Checks if Coupon must discount Total
        public bool CouponUserIsValid { get; set; }
        
        public double? CouponDiscountAmount { get; set; }

        

    }
}
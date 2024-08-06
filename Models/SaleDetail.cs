using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class SaleDetail
    {
        [Key]
        public int SaleDetailId { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double ProductPrice { get; set; }
        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
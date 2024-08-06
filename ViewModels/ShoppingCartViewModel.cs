using BookStore.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;

namespace BookStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        [Key]
        public List<Cart> CartItems { get; set; }
        public double? CartTotal
        {
            get; set;
        }
        public int? PaymentMethodId { get; set; }

        public BankingDetails bankingDetails { get; set; }

        public int? BankingInfo { get; set; }
    
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class WishListsProducts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListedProductsId { get; set; }
        public int? ListId { get; set; }
        [ForeignKey("ListId")]
        public WishLists WishLists { get; set; }

        public int TotalItems { get; set; }
        public string ListUrl { get; set; }


        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Products Products { get; set; }




    }
}
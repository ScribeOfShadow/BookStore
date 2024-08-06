using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.ViewModels
{
    public class FeedbackVM
    {
        public int Id { get; set; }
        public DateTime? Created { get; set; }
        public int? BookId { get; set; }
        public int? SaleId { get; set; }
        public string Comment { get; set; }
        public int? Rating { get; set; }
       

        
        [NotMapped]
        public string Bookname { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
       

        public int SaleId { get; set; }
        public int BookId { get; set; }


        public DateTime? Created { get; set; }
       

        public string Comment { get; set; }
        public int? Rating { get; set; }

        [NotMapped]
        public string Bookname { get; set; }
    }
}
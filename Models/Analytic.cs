using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Analytic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? BookId { get; set; }

        [Display(Name = "Name")]
        public string BookName { get; set; }

        [Display(Name = "Description")]
        public string BookDescription { get; set; }

        [Display(Name = "Sold On")]
        public DateTime? SoldDateTime { get; set; }

        public string Comment { get; set; }
        public int? Rating { get; set; }
    }
}
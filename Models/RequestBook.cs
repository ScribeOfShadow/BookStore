using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class RequestBook
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Book_RequestId { get; set; }
        [Display(Name = "Book Requested")]
        [Required]
        public string Book_Request { get; set; }
        [Display(Name = "Date Requested")]
        public DateTime Date_Requested { get; set; }
        [Display(Name = "Admin Decision")]
        public string Book_Author { get; set; }
        [Display(Name = "Author of Book")]
        public bool Admindecision { get; set; }
    }

}
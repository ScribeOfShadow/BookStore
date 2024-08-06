using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class BorrowedBooks
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }


        public int? BookId { get; set; }
        [ForeignKey("BookId")]
        public Library Library { get; set; }
        
        public DateTime? DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; }


        [NotMapped]
        public BookQrCodes BookQrCodes { get; set; }




    }
}
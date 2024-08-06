using BookStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.ViewModels
{
    [NotMapped]
    public class BorrowViewModel
    {
        public Library Library { get; set; }
        public BorrowedBooks BorrowedBooks { get; set; }

        public BookQrCodes BookQrCodes { get; set; }
    }
}
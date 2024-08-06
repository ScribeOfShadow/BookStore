using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace BookStore.Models
{
    [Table("Library")]
    public class Library
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryId { get; set; }
        [Display(Name = "Name")]
        public string LibraryProductName { get; set; }
        [Display(Name = "Author")]
        public string LibraryAuthor { get; set; }
        [Display(Name = "Description")]
        public string LibraryDescription { get; set; }
        [Display(Name = "Image")]
        public String LibraryURL { get; set; }
        [Display(Name = "Was Created At")]
        public DateTime CreatedAt { get; set; }
        


        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public int Assignment_Id { get; set; }
        [Display(Name = "Date Book Was Borrowed")]
        public bool IsAssigned { get; set; }
        public DateTime? Borrowed { get; set; }
        [Display(Name = "Date Returned")]
        public DateTime? ExpectedReturn { get; set; }
       
        public DateTime? ReturnedDate { get; set; }

        public int TotalStock { get; set; }




        public int? LibraryCatergoryId { get; set; }
        [ForeignKey("LibraryCatergoryId")]
        public LibraryCatergory LibraryCatergory { get; set; }

       
    }
} 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace BookStore.Models
{
    public class eBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string BookName { get; set; }
        [Display(Name = "Description")]
        public string BookDescription { get; set; }
        [Display(Name = "Genre")]
        public string BookGenre { get; set; }
        [Display(Name = "Book")]
        public string FileName { get; set; }
        [Display(Name = "File Path")]
        public string FileContents { get; set; }
        [Display(Name = "mimetype")]
        public string mimetype { get; set; }
        [Display(Name = "Book Mark Index")]
        public int LastReadPosition { get; set; }
        [Display(Name = "Image")]
        public string BookImageName { get; set; }
        public byte[] BookImageContent { get; set; }
        public string BookImageExtension { get; set; }
        public double BookPrice { get; set; }
        public string UserId { get; set; }

        //[Display(Name = "Created On")]
      //  public DateTime? CreatedDateTime { get; set; }

        [NotMapped]
        public Byte[] FileContent { get; set; }
        [NotMapped]
        public string ContentType { get; set; }

        [NotMapped]
        public object Content { get; set; }
    }
}
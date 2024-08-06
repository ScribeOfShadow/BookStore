using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class BookQrCodes
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int LibraryBookId { get; set; }
        [ForeignKey("LibraryBookId")]
        public Library Library { get; set; }

        [Column(Order = 3)]
        [Display(Name = "Code Text")]
        public string PlainText { get; set; }

        //[Column(Order = 3)]
        //[Display(Name = "Encrypted Text")]
        //public string EncryptedText { get; set; }

        //[Column(Order = 4)]
        //[Display(Name = "Public Key")]
        //public string EncryptedPublicKey { get; set; }

        //[Column(Order = 5)]
        //[Display(Name = "Private Key")]
        //public string EncryptedPrivateKey { get; set; }
        [Column(Order = 5)]
        public int ScannedCount { get; set; }

        [Column(Order = 6)]
        [Display(Name = "Rendered Code Image")]
        public string RenderedImagePath { get; set; }

        [Column(Order = 7)]
        [Display(Name = "Rendered Bytes")]
        public byte[] RenderedBytes { get; set; }

        [Column(Order = 9)]
        public DateTime? CreatedDateTime { get; set; }
        [Column(Order = 10)]
        public DateTime? ModifiedDateTime { get; set; }
    }
}

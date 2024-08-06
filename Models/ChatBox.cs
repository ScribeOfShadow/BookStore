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
    [Table("ChatBox")]
    public class ChatBox
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Chat_Id { get; set; }
        public string Subject { get; set; }
        [Display(Name = "Post")]
        public string ChatPost { get; set; }
        public DateTime Posted_Date { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class ReturnAndRefund
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }

        [Required]
        public int? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Sale Sale { get; set; }

        [Display(Name = "Refund / Return")]
        public int? RNRId { get; set; }
        [ForeignKey("RNRId")]
        public virtual RNRData RNRData { get; set; }


        [Display(Name = "Reason For Returning Item")]
        public int? ReasonId { get; set; }
        [ForeignKey("ReasonId")]
        public virtual ReasonDrop ReasonDrop { get; set; }



        public string AdditionalComments { get; set; }


        public string CustomerName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }


        [Display(Name = "Status For Item on Returning Or Refunding Approved or declined")]
        public int? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        public string AdminComments { get; set; }

        public int? ProductionId { get; set; }
        [ForeignKey("ProductionId")]
        public Products Products { get; set; }


        public bool? IsCompleted { get; set; }
    }
}
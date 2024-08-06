using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Evaluation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Comments { get; set; }
        [Display(Name = "Status For Item on Returning Or Refunding Approved or declined")]
        public int? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }


        public string CustomerName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }


        public int? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Sale Sale { get; set; }



        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }


        public bool? IsActive { get; set; }


        public bool? IsCompleted { get; set; }
        public int? RRIdValue { get; set; }
        [ForeignKey("RRIdValue")]
        public ReturnAndRefund returnAndRefund { get; set; }

        public bool? PickedUpAndReadyForEval { get; set; }

        public int? RNRIdE { get; set; }
        [ForeignKey("RNRIdE")]
        public RNRData rNRData { get; set; }



        public int? SeverityId { get; set; }
        [ForeignKey("SeverityId")]
        public RNRData Severity { get; set; }


        public int? ProductionId { get; set; }
        [ForeignKey("ProductionId")]
        public Products Products { get; set; }


        public byte[] ProductImage { get; set; }

        [NotMapped]
        public int? FinalRdValue { get; set; }

        [NotMapped]
        public int? FinalStatusId { get; set; }

        public int? DeliveryId { get; set; }
        [ForeignKey("DeliveryId")]
        public Delivery Delivery { get; set; }

        public bool? DeliveryPickUpForReturn { get; set; }








    }
}
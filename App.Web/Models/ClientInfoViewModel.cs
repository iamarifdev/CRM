using System;
using System.ComponentModel.DataAnnotations;
using App.Entity.Models;

namespace App.Web.Models
{
    public class ClientInfoViewModel
    {

        [Display(Name = "Client")]
        public int Id { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        [Display(Name = "Agent")]
        public int? AgentId { get; set; }

        [Display(Name = "Service")]
        public int? ServiceId { get; set; }

        [Display(Name = "Air Line")]
        public int? AirLineId { get; set; }

        [Display(Name = "From Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Served By")]
        public int? ServedBy { get; set; }

        [Display(Name = "Working Status")]
        public WorkingStatus? WorkingStatus { get; set; }

        [Display(Name = "Delivery Status")]
        public DeliveryStatus? DeliveryStatus { get; set; }

        [Display(Name = "Information Update")]
        public InformationUpdate? InfoStatus { get; set; }
    }
}
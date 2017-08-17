using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class ClientInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }

        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [StringLength(255)]
        public string Sn { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Referral Type")]
        public string ReferralType { get; set; }

        [Display(Name = "Agent")]
        [ForeignKey("AgentInfo")]
        public int? AgentId { get; set; }

        [Display(Name = "Supplier")]
        [ForeignKey("SuppliersInfo")]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(25)]
        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }

        [StringLength(100)]
        public string Referral { get; set; }

        [StringLength(25)]
        [Display(Name = "Referral Contact No")]
        public string ReferralContactNo { get; set; }

        [Display(Name = "Service")]
        [ForeignKey("ServiceInfo")]
        public int ServiceId { get; set; }

        [Display(Name = "Air Line")]
        [ForeignKey("AirLineInfo")]
        public int AirLineId { get; set; }

        [Display(Name = "Old Flight Date")]
        public DateTime? OldFlightDate { get; set; }

        [Display(Name = "Change Flight Date")]
        public DateTime? ChangeFlightDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Air Line PNR")]
        public string AirLinePnr { get; set; }

        [StringLength(50)]
        [Display(Name = "GDS PNR")]
        public string GdsPnr { get; set; }

        [Display(Name = "New Flight Date")]
        public DateTime NewFlightDate { get; set; }

        [StringLength(150)]
        [Display(Name = "Collage Name")]
        public string CollageName { get; set; }

        [StringLength(150)]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Service Charge")]
        public double? ServiceCharge { get; set; }

        public double? Cost { get; set; }

        public double? Profit { get; set; }

        public double Discount { get; set; }

        [Display(Name = "Served By")]
        [ForeignKey("UserServedBy")]
        public int? ServedBy { get; set; }

        [Display(Name = "Done By")]
        [ForeignKey("UserDoneBy")]
        public int? DoneBy { get; set; }

        [Display(Name = "Working Status")]
        public int? WorkingStatus { get; set; }

        [Display(Name = "Delivery Status")]
        public int? DeliveryStatus { get; set; }

        [Display(Name = "Information Update")]
        public int? InfoStatus { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }

        public int? Status { get; set; }

        [Display(Name = "Delete Status")]
        public bool? DelStatus { get; set; }

        [Display(Name = "Entry By")]
        [ForeignKey("UserEntryBy")]
        public int EntryBy { get; set; }

        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Venue From")]
        [ForeignKey("SectorFrom")]
        public int? VenueFromId { get; set; }
        
        [Display(Name = "Venue To")]
        [ForeignKey("SectorTo")]
        public int? VenueToId { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "SMS Number")]
        public string SmsNo { get; set; }

        [Display(Name = "Country")]
        [ForeignKey("CountryList")]
        public int? CountryId { get; set; }

        [StringLength(10)]
        public string Finger { get; set; }

        [StringLength(10)]
        public string Manpower { get; set; }

        [Display(Name = "Ticket Issue")]
        [StringLength(10)]
        public string TicketIssue { get; set; }

        [Display(Name = "Flight Status")]
        [StringLength(10)]
        public string FlightStatus { get; set; }

        public virtual AgentInfo AgentInfo { get; set; }
        public virtual User UserServedBy { get; set; }
        public virtual User UserDoneBy { get; set; }
        public virtual User UserEntryBy { get; set; }

        public virtual SectorInfo SectorFrom { get; set; }
        public virtual SectorInfo SectorTo { get; set; }
        public virtual CountryList CountryList { get; set; }
        public virtual AirLineInfo AirLineInfo { get; set; }
        public virtual SuppliersInfo SuppliersInfo { get; set; }
        public virtual ServiceInfo ServiceInfo { get; set; }
    }
}
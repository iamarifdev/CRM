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

        //[Required]
        //[StringLength(255)]
        //public string Sn { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "Referral Type")]
        public string ReferralType { get; set; }

        [Display(Name = "Agent")]
        [ForeignKey("AgentInfo")]
        public int? AgentId { get; set; }
        public virtual AgentInfo AgentInfo { get; set; }

        [Display(Name = "Supplier")]
        [ForeignKey("SuppliersInfo")]
        public int SupplierId { get; set; }
        public virtual SuppliersInfo SuppliersInfo { get; set; }

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
        public virtual ServiceInfo ServiceInfo{ get; set; }

        [Display(Name = "Air Line")]
        [ForeignKey("SectorInfo")]
        public int AirLineId { get; set; }
        public virtual SectorInfo SectorInfo { get; set; }

        public DateTime? OldFlightDate { get; set; }

        public DateTime? ChangeFlightDate { get; set; }

        [StringLength(50)]
        public string AirLinePnr { get; set; }

        [StringLength(50)]
        public string GdsPnr { get; set; }

        public DateTime NewFlightDate { get; set; }

        [Required]
        [StringLength(150)]
        public string CollageName { get; set; }

        [Required]
        [StringLength(150)]
        public string CourseName { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        public double? ServiceCharge { get; set; }

        public double? Cost { get; set; }

        public double? Profit { get; set; }

        public double Discount { get; set; }

        [StringLength(20)]
        public string ServedBy { get; set; }

        [StringLength(100)]
        public string DoneBy { get; set; }

        [StringLength(20)]
        public string WorkingStatus { get; set; }

        [StringLength(20)]
        public string DeliveryStatus { get; set; }

        [StringLength(20)]
        public string InfoStatus { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public bool? DelStatus { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }

        [Required]
        [StringLength(20)]
        public string VenueFromId { get; set; }

        [Required]
        [StringLength(20)]
        public string VenueToId { get; set; }

        [Required]
        [StringLength(25)]
        public string SmsNo { get; set; }

        [Required]
        [StringLength(5)]
        public string CountryId { get; set; }

        [Required]
        [StringLength(10)]
        public string Finger { get; set; }

        [Required]
        [StringLength(10)]
        public string Manpower { get; set; }

        [Required]
        [StringLength(10)]
        public string TicketIssue { get; set; }

        [Required]
        [StringLength(10)]
        public string FlightStatus { get; set; }
    }
}
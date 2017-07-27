using System;
using System.ComponentModel.DataAnnotations;

public class ClientInfo
{
    public int Id { get; set; }

    [StringLength(20)]
    public string CustomerId { get; set; }

    [StringLength(20)]
    public string BranchId { get; set; }

    [Required]
    [StringLength(255)]
    public string Sn { get; set; }

    [StringLength(10)]
    public string ReferralType { get; set; }

    [StringLength(20)]
    public string AgentId { get; set; }

    [Required]
    [StringLength(20)]
    public string SupplierId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(25)]
    public string ContactNo { get; set; }

    [StringLength(100)]
    public string Referral { get; set; }

    [StringLength(25)]
    public string ReferralContactNo { get; set; }

    [StringLength(20)]
    public string ServiceId { get; set; }

    [StringLength(20)]
    public string AirLineId { get; set; }

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
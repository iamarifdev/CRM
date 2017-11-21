using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class EmployeeBasicInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Employee ID")]
        public string EmployeeId { get; set; }

        [StringLength(100)]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [StringLength(100)]
        [Display(Name = "Father's Name")]
        public string FatherName { get; set; }

        [StringLength(100)]
        [Display(Name = "Mothers Name")]
        public string MotherName { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime? Dob { get; set; }

        public Gender Gender { get; set; }

        [Display(Name = "Marital Status")]
        public Ack MaritalStatus { get; set; }

        [StringLength(100)]
        [Display(Name = "Spouse Name")]
        public string SpouseName { get; set; }

        [StringLength(50)]
        [Display(Name="NID Number")]
        public string NidNo { get; set; }

        [Display(Name = "Employee Designation")]
        [ForeignKey("EmployeeDesignation")]
        public int EmployeeDesignationId { get; set; }

        [StringLength(255)]
        [Display(Name = "Profile Photo")]
        [FileExtensions(Extensions = "jpg,png,jpeg,gif,bmp", ErrorMessage = "Only .jpg, .png, .jpeg, .gif, .bmp file types are allowed.")]
        public string ImageUrl { get; set; }

        [Display(Name = "Basic Salary")]
        public double? BasicSalary { get; set; }

        [Display(Name = "Other Allownce")]
        public double? OtherAllowance { get; set; }

        [Display(Name = "Date Of Joining")]
        public DateTime? DateOfJoining { get; set; }

        [Display(Name = "Next Increment Date")]
        public DateTime? IncrementDate { get; set; }

        [Display(Name = "Blood Group")]
        public BloodGroup? BloodGroup { get; set; }

        [StringLength(16)]
        [Display(Name = "Land Line Number")]
        public string LandLineNumber { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [StringLength(15)]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [Display(Name = "User Level")]
        public EmployeeLevel? UserLevel { get; set; }

        [StringLength(20)]
        public string ZoneId { get; set; }

        [StringLength(20)]
        public string TerritoryId { get; set; }

        [StringLength(20)]
        public string WarehouseId { get; set; }

        public Status Status { get; set; }

        [Display(Name = "Delete Status")]
        public bool DelStatus { get; set; }

        [Display(Name = "Entry By")]
        [ForeignKey("User")]
        public int EntryBy { get; set; }

        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        public virtual EmployeeDesignation EmployeeDesignation { get; set; }
        public virtual User User { get; set; }
    }
}
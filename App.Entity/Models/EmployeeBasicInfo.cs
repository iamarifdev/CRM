using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models
{
    public class EmployeeBasicInfo
    {
        [Key]
        public int RowId { get; set; }

        [StringLength(20)]
        [Index("IX_Employee_Id", 1, IsUnique = true)]
        public string EmployeeId { get; set; }

        [StringLength(100)]
        public string EmployeeName { get; set; }

        [StringLength(100)]
        public string FatherName { get; set; }

        [StringLength(100)]
        public string MotherName { get; set; }

        public DateTime? Dob { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [StringLength(10)]
        public string MaritalStatus { get; set; }

        [StringLength(100)]
        public string SpouseName { get; set; }

        [StringLength(50)]
        public string NidNo { get; set; }

        [StringLength(20)]
        public string EmployeeDesignation { get; set; }

        [StringLength(25)]
        public string ImageUrl { get; set; }

        public double? BasicSalary { get; set; }

        public double? OtherAllowance { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public DateTime? IncrementDate { get; set; }

        [StringLength(15)]
        public string BloodGroup { get; set; }

        [StringLength(16)]
        public string LandLineNumber { get; set; }

        [StringLength(11)]
        public string MobileNumber { get; set; }

        [StringLength(100)]
        public string ContactPerson { get; set; }

        [StringLength(11)]
        public string ContactNumber { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Address { get; set; }

        [StringLength(20)]
        public string UserLevel { get; set; }

        [StringLength(20)]
        public string ZoneId { get; set; }

        [StringLength(20)]
        public string TerritoryId { get; set; }

        [StringLength(20)]
        public string WarehouseId { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public int DelStatus { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class EmployeeDesignation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string DesignationId { get; set; }

        [StringLength(100)]
        public string DesignationTitleEn { get; set; }

        [StringLength(100)]
        public string DesignationTitleBn { get; set; }

        [StringLength(20)]
        public string DesignationDepertment { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public int DelStatus { get; set; }

        public DateTime? EntryDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class AgentInfo
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string AgentId { get; set; }

        [StringLength(150)]
        public string OfficeName { get; set; }

        [StringLength(150)]
        public string AgentName { get; set; }

        [StringLength(50)]
        public string ContactName { get; set; }

        [StringLength(25)]
        public string MobileNo { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(15)]
        public string OfficeNo { get; set; }

        [StringLength(15)]
        public string FaxNo { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public int ResetStatus { get; set; }

        [StringLength(10)]
        public string Channel { get; set; }

        [Required]
        [StringLength(30)]
        public string AgentPhoto { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public int DelStatus { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }
    }
}
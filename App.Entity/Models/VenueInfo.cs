using System;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class VenueInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string VenueId { get; set; }

        [StringLength(100)]
        public string VenueName { get; set; }

        [StringLength(100)]
        public string VenueCode { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public int DelStatus { get; set; }

        public DateTime? EntryDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }
    }
}
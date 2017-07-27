using System;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class SmsSentHistory
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string SmsSendId { get; set; }

        [StringLength(20)]
        public string CustomerId { get; set; }

        [StringLength(20)]
        public string MobileNumber { get; set; }

        [StringLength(250)]
        public string SmsBody { get; set; }

        [StringLength(20)]
        public string SendStatus { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public int DelStatus { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }

        //TimeStamp
        public DateTime CurrentTime { get; set; }
    }
}
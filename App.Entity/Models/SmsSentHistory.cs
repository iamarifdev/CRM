using System;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public class SmsSentHistory : BaseModel
    {

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

        public Status Status { get; set; }

        //TimeStamp
        public DateTime CurrentTime { get; set; }
    }
}
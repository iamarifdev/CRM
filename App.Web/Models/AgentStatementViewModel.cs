using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Foolproof;

namespace App.Web.Models
{
    public class AgentStatementViewModel
    {
        [Display(Name = "Agent")]
        public int AgentId { get; set; }

        [Display(Name = "From Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //[LessThan("FromDate",ErrorMessage = "To Date cannot be less than from date.")]
        public DateTime? ToDate { get; set; }
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models
{
    public enum Status
    {
        [Display(Name = "Inactive"), Description("Inactive")]
        Inactive,
        [Display(Name = "Active"), Description("Active")]
        Active
    }

    public enum WorkingStatus
    {
        [Description("Pending")]
        Pending,
        [Description("Done")]
        Done
    }

    public enum DeliveryStatus
    {
        [Description("Not Delivery")]
        NotDelivery,
        [Description("Delivery")]
        Delivery
    }

    public enum InformationUpdate
    {
        [Description("Not Updated")]
        NotUpdated,
        [Description("Updated")]
        Updated
    }

    public enum SmsConfirmation
    {
        [Display(Name = "Not Sending SMS"), Description("Not Sending SMS")]
        NotSendingSms,
        [Display(Name = "Sending SMS"), Description("Sending SMS")]
        SendingSms
    }

    public enum RequireSuppiler
    {
        [Description("No")]
        No,
        [Description("Yes")]
        Yes
    }

    public enum ReferralsType
    {
        [Description("Agent")]
        Agent=1,
        [Description("Referrals")]
        Referrals=2,
        [Description("Office")]
        Office=3
    }
}
using System.ComponentModel;

namespace App.Entity.Models
{
    public enum Status
    {
        [Description("Inactive")]
        Inactive,
        [Description("Active")]
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
        [Description("Not Sending SMS")]
        NotSendingSms,
        [Description("Sending SMS")]
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
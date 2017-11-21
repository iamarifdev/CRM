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
        Pending,
        Done
    }

    public enum DeliveryStatus
    {
        [Display(Name = "Not Delivery")]
        NotDelivery,
        [Display(Name = "Delivery")]
        Delivery
    }

    public enum InformationUpdate
    {
        [Display(Name = "Not Updated")]
        NotUpdated,
        [Display(Name = "Updated")]
        Updated
    }

    public enum SmsConfirmation
    {
        [Display(Name = "Not Sending SMS")]
        NotSendingSms,
        [Display(Name = "Sending SMS")]
        SendingSms
    }

    public enum RequireSuppiler
    {
        No,
        Yes
    }

    public enum ReferralsType
    {
        Agent=1,
        Referrals=2,
        Office=3
    }

    public enum UserType
    {
        IsAgent,
        IsAdmin
    }

    public enum Channel
    {
        IsCustomer = 1,
        IsAgent = 2,
        IsSupplier = 3
    }

    public enum TransactionType
    {
        Transfer=1,
        Deposit=2,
        Expense=3
    }

    public enum PayerType
    {
        Agent=1,
        Client=2,
        Officer=3,
        Other=4
    }

    public enum BalanceMode
    {
        Increment,
        Decrement
    }

    public enum Flag
    {
        IsOff = 0,
        IsOn = 1,
    }

    public enum UserLevel
    {
        [Display(Name = "Head Office")]
        HeadOffice = 1,
        [Display(Name = "Branch Office")]
        BranchOffice = 2
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }

    public enum Ack
    {
        Yes = 1,
        No = 0
    }

    public enum BloodGroup
    {
        [Display(Name = "A+")]
        APositive = 1,
        [Display(Name = "A-")]
        ANegative = 2,
        [Display(Name = "B+")]
        BPositive = 3,
        [Display(Name = "B-")]
        BNegative = 4,
        [Display(Name = "AB+")]
        AbPositive = 6,
        [Display(Name = "AB-")]
        AbNegative = 7,
        [Display(Name = "O+")]
        OPositive = 6,
        [Display(Name = "O-")]
        ONegative = 7,
    }

    public enum EmployeeLevel
    {
        [Display(Name = "Head Office")]
        HeadOffice = 1,
        [Display(Name = "Bank Incharge")]
        BankIncharge = 2,
        [Display(Name = "Zone Incharge")]
        ZoneIncharge = 3,
        [Display(Name = "Service Officer")]
        ServiceOfficer = 4,
        [Display(Name = "Branch Manager")]
        BranchManager = 5,
    }

    public enum Module
    {
        Crm = 1,
        Billing = 2,
        Account = 3,
        Report = 4,
        Hrm = 5,
        Setup = 6
    }
}
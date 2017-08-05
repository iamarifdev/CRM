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
}
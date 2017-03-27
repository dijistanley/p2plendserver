using System.ComponentModel;
namespace Model.Models
{
    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    }
    
    public enum Gender
    {
        Male,
        Female,
        Other,
        Unknown
    }
    
    public enum AddressType
    {
        [Description("Postal")]
        postal,
        [Description("Physical")]
        physical,
        [Description("Both")]
        both
    }

    public enum AddressUse
    {
        [Description("Home")]
        home,
        [Description("Work")]
        work,
        [Description("Temporary")]
        temp,
        [Description("Old / Incorrect")]
        old
    }
    
}

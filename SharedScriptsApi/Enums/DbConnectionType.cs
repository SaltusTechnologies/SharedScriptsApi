using System.ComponentModel.DataAnnotations;

namespace SharedScriptsApi.Enums
{
    //[Flags]
    //public enum DbConnectionType
    //{
    //    [Display(Name = "Customer")]
    //    Customer = 0x001,
    //    [Display(Name = "State")]
    //    State = 0x002,
    //    [Display(Name = "Core")]
    //    Core = 0x004,
    //}

    public enum DbConnectionType
    {
        None = 0,
        [Display(Name = "Customer")]
        Customer = 1,
        [Display(Name = "Core")]
        Core = 2,
        [Display(Name = "Oklahoma")]
        Oklahoma = 3,
    }
}

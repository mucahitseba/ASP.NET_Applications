using System.ComponentModel;

namespace Admin.MODELS.Enums
{
    public enum ProductTypes
    {
        [Description("Toptan")]
        Bulk =10,
        [Description("Perakende")]
        Retail =100
    }
}

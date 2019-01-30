using System.ComponentModel;

namespace Admin.MODELS.Enums
{
    public enum OrderType
    {
        [Description("Alış")]
        Buying=10,
        [Description("Satış")]
        Selling =20
    }
}

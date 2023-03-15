using Wholesale.Desktop.Attributes;

namespace Wholesale.Desktop.Models.MarketPlaza.Lessees
{
    /// <summary>
    /// Базовый арендатор (свойства этой штуки есть во всех потомках).
    /// </summary>
    public class Lessee
    {
        [HiddenColumn]
        public short АрендаторID { get; set; }
    }
}

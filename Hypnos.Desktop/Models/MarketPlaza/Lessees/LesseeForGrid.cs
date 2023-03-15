using System.ComponentModel;
using Wholesale.Desktop.Attributes;

namespace Wholesale.Desktop.Models.MarketPlaza.Lessees
{
    /// <summary>
    /// Арендатор для гриды
    /// </summary>
    public class LesseeForGrid : Lessee
    {
        [DisplayName("ФИО")]
        public string FullName { get; set; }
    }
}

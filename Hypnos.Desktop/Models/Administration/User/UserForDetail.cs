using System.ComponentModel;

namespace Wholesale.Desktop.Models.Administration.User
{
    public class UserForDetail : UserForGrid
    {
        [DisplayName("Описание")]
        public string Description { get; set; }
    }
}

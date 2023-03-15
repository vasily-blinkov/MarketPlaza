using System.ComponentModel;

namespace Wholesale.Desktop.Models.Administration.User
{
    public class UserForGrid : Entity
    {
        [DisplayName("ФИО")]
        public string FullName { get; set; }

        [DisplayName("Логин")]
        public string LoginName { get; set; }
    }
}

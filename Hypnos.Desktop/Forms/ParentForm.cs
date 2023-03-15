using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Wholesale.Desktop.Models.Administration;
using Wholesale.Desktop.Repositories;
using Wholesale.Desktop.Utils;

namespace Wholesale.Desktop.Forms
{
    public partial class ParentForm : Form
    {
        public ParentForm()
        {
            InitializeComponent();
        }

        private void Exit(object sender, FormClosedEventArgs e)
        {
            if (ExceptionsUtility.IsTerminating)
            {
                return; // because called from another form
            }

            Program.Exit();
        }

        public void ApplyRoles()
        {
            BindingList<Role> rolesCollection;

            using (var repository = new AdministrationRepository())
            {
                rolesCollection = repository.GetRoles(AuthenticationUtility.UserId.Value);
            }

            if (rolesCollection.Any(r => new[]
            {
                "Администратор ИС",
                "Администратор безопасности ИС"
            }.Contains(r.Name)))
            {
                administrationItem.Visible = true;
            }
        }

        private void OpenUsers(object sender, EventArgs e) => OpenChild<UsersForm>();

        private void OpenLessees(object sender, EventArgs e) => OpenChild<АрендаторForm>();

        private void OpenChild<T>()
            where T : Form, new()
        {
            var form = new T();
            form.MdiParent = this;
            form.Show();
        }
    }
}

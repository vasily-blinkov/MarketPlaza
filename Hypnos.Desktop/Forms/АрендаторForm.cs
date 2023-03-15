using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Wholesale.Desktop.EqualityComparers;
using Wholesale.Desktop.Models.Administration;
using Wholesale.Desktop.Models.Administration.User;
using Wholesale.Desktop.Models.MarketPlaza.Lessees;
using Wholesale.Desktop.Repositories;
using Wholesale.Desktop.Utils;

namespace Wholesale.Desktop.Forms
{
    public partial class АрендаторForm : Form
    {
        private enum Mode { Main, Create };

        private short? entityID;

        private readonly ToolStripModeUtility<Mode> modeUtility;

        public АрендаторForm()
        {
            InitializeComponent();
            modeUtility = InitializeModes();
        }

        private ToolStripModeUtility<Mode> InitializeModes()
        {
            return new ToolStripModeUtility<Mode>(toolStrip)
                .Map(Mode.Main, filterLabel, filterBox, readButton, masterDetailsSeparator, crudLabel, createButton);
        }

        private void Transpose(object sender, EventArgs e)
        {
            splitContainer.Orientation = splitContainer.Orientation == Orientation.Vertical
                ? Orientation.Horizontal
                : Orientation.Vertical;
        }

        private void Prepare(object sender, EventArgs e)
        {
            LoadRoles();
            FillGrid();
            LoadUser();
        }

        private void FillGrid()
        {
            using (var repository = new MarketPlazaRepository())
            {
                lesseesGrid.DataSource = repository.GetLessees(filterBox.Text);
                GridUtility.Setup(lesseesGrid, ReflectionUtility.GetHiddenNames<LesseeForGrid>());
            }
        }

        private void LoadUser()
        {
            var selectedRows = lesseesGrid.SelectedRows;

            if (selectedRows.Count == 0)
            {
                return;
            }

            LoadEntity(selectedRows[0].Index);
        }

        private void LoadUser(object sender, DataGridViewCellEventArgs e)
        {
            LoadEntity(e.RowIndex);
        }

        private void LoadEntity(int rowIndex)
        {
            var entityID = GetSelectedEntityID(rowIndex);

            if (!entityID.HasValue)
            {
                return;
            }

            ChangeEntity(entityID.Value);
        }

        /// <summary>
        /// Переключает детали на другую сущность.
        /// </summary>
        private void ChangeEntity(short entityID)
        {
            LoadEntity(entityID);
            this.entityID = entityID;
        }

        /// <summary>
        /// Загружает детали экземпляра сущности.
        /// </summary>
        private void LoadEntity(short entityID)
        {
            LesseeForDetail entity;

            using (var repository = new MarketPlazaRepository())
            {
                entity = repository.GetSingleLessee(entityID);
            }

            FillDetail(entity);
            FillRoles(entityID);
        }

        /// <returns>
        /// ID of a selected entity row in the grid view
        /// </returns>
        private short? GetSelectedEntityID(int rowIndex)
        {
            if (rowIndex < 0)
            {
                return null; // nothing selected
            }

            var idValue = lesseesGrid.Rows[rowIndex].Cells[nameof(LesseeForGrid.АрендаторID)].Value;

            return short.TryParse(idValue != null ? idValue.ToString() : string.Empty, out var userID)
                ? userID
                : (short?)null;
        }

        private void FillDetail(LesseeForDetail entity)
        {
            фамилия.Text = entity.Фамилия;
            имя.Text = entity.Имя;
            отчество.Text = entity.Отчество;
            телефон.Text = entity.Телефон;
            адрес.Text = entity.Адрес;
        }

        private void FillRoles(short userID)
        {
        }

        private void Refresh(object sender, EventArgs e) => Reload();

        private void Reload()
        {
            FillGrid();

            // Trying to select a row in the grid matching the currently displaying user in the detail panel.
            if (entityID.HasValue)
            {
                SelectRow(entityID.Value);
            }

            LoadRoles();
            LoadUser();

            if (entityID.HasValue)
            {
                FillRoles(entityID.Value);
            }
        }

        private void SelectRow(short userID)
        {
            var row = lesseesGrid.Rows.Find(nameof(LesseeForGrid.АрендаторID), userID, new IdEqualityComparer());

            if (row == null)
            {
                return;
            }

            row.Selected = true;
        }

        private void LoadRoles()
        {
        }

        private void CreateEntity(object sender, EventArgs e)
        {
            // Add a new row to the table.
            ((BindingList<LesseeForGrid>)lesseesGrid.DataSource).Add(new LesseeForGrid());

            // Select the created row.
            lesseesGrid.ClearSelection();
            lesseesGrid.Rows[lesseesGrid.RowCount - 1].Selected = true;

            // Clear details.
            FillDetail(new LesseeForDetail());

            // Change visible tool strip items.
            modeUtility.Switch(Mode.Create);
        }

        private void CancelCreateEntity()
        {
            modeUtility.Mode = Mode.Main;
            ((BindingList<LesseeForGrid>)lesseesGrid.DataSource).RemoveAt(lesseesGrid.RowCount - 1);
        }

        private void DeleteUser(object sender, EventArgs e)
        {
            if (modeUtility.Mode == Mode.Create && ConfirmCancelCreateUser() == DialogResult.Yes)
            {
                CancelCreateEntity();

                // Select previously selected row.
                if (this.entityID.HasValue)
                {
                    SelectRow(entityID.Value);
                    LoadEntity(entityID.Value);
                }
            }
        }

        private void SelectUser(object sender, EventArgs e)
        {
            if (modeUtility.Mode == Mode.Create && !lesseesGrid.Rows[lesseesGrid.RowCount - 1].Selected)
            {
                if (ConfirmCancelCreateUser() == DialogResult.Yes) 
                {
                    CancelCreateEntity();
                    LoadEntity(lesseesGrid.SelectedRows[0].Index);
                }
                else
                {
                    lesseesGrid.Rows[lesseesGrid.RowCount - 1].Selected = true;
                }
            }
        }

        private DialogResult ConfirmCancelCreateUser() => MessageBox.Show(
            "Форма содержит данные, которые пока не были сохранены в базу. При продолжении без сохранения они будут безвозвратно утеряны.",
            "Отменить создание пользователя?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        private void HandleClosing(object sender, FormClosingEventArgs e)
        {
            if (modeUtility.Mode == Mode.Create && ConfirmCancelCreateUser() != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void SaveUser(object sender, EventArgs e)
        {
            if (Mode.Create == modeUtility.Mode)
            {
                AddEntity();
            }
            else
            {
                EditUser();
            }
        }

        private void AddEntity()
        {
            using (var repository = new MarketPlazaRepository())
            {
                UpsertUser(repository.AddLessee);
            }
        }

        private void UpsertUser(Upsert upsert)
        {
            var entityJson = ReadDetail().ToJsonString();

            bool reload = true;
            string message = string.Empty;

            try
            {
                upsert(entityJson);
            }
            catch (SqlException ex)
            {
                reload = false;
                message = ex.Message;
            }
            catch (Exception ex)
            {
                ExceptionsUtility.Handle(ex);
            }

            if (reload)
            {
                modeUtility.Switch(Mode.Main);
                Reload();
            }
            else
            {
                MessageBox.Show(message, "Арендатор не сохранен", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditUser()
        {
            using (var repository = new MarketPlazaRepository())
            {
                UpsertUser(repository.EditLessee);
            }
        }

        private LesseeForUpsert ReadDetail() => new LesseeForUpsert
        {
            АрендаторID = entityID.Value,
            Фамилия = фамилия.Text,
            Имя = имя.Text,
            Отчество = отчество.Text,
            Телефон = телефон.Text,
            Адрес = адрес.Text,
        };
    }
}

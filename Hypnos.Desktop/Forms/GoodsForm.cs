using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;
using Wholesale.Desktop.EqualityComparers;
using Wholesale.Desktop.Models.MarketPlaza.Goods;
using Wholesale.Desktop.Models.MarketPlaza.Lessees;
using Wholesale.Desktop.Repositories;
using Wholesale.Desktop.Repositories.MarketPlaza;
using Wholesale.Desktop.Utils;
using Wholesale.Desktop.Utils.Forms;
using Wholesale.Desktop.Utils.Forms.Abstractions;
using Wholesale.Desktop.Utils.Forms.Configurations;
using Wholesale.Desktop.Utils.Forms.Events;

namespace Wholesale.Desktop.Forms
{
    public partial class GoodsForm : Form, IEntityForm
    {
        private readonly ToolStripModeUtility<Mode> modeUtility;

        private readonly EntityCreationUtility<GoodForGrid, GoodForDetail> entityCreator;
        private readonly MasterFillingUtility<GoodRepository, GoodForGrid> masterFiller;

        private short? entityID;
        public short? EntityID => entityID;

        public GoodsForm()
        {
            InitializeComponent();
            modeUtility = InitializeModes();
            entityCreator = InitializeCreator();
            masterFiller = InitializeMasterFiller();
        }

        private ToolStripModeUtility<Mode> InitializeModes()
        {
            return new ToolStripModeUtility<Mode>(toolStrip)
                .Map(Mode.Main, filterLabel, filterBox, readButton, masterDetailsSeparator, crudLabel, createButton);
        }

        private EntityCreationUtility<GoodForGrid, GoodForDetail> InitializeCreator() =>
            new EntityCreationUtility<GoodForGrid, GoodForDetail>(
                new EntityCreationConfiguration<GoodForGrid, GoodForDetail>
                {
                    MasterGrid = masterGrid,
                    FillDetails = FillDetail,
                    ModeUtility = modeUtility
                });

        private MasterFillingUtility<GoodRepository, GoodForGrid> InitializeMasterFiller()
        {
            var configuration = new MasterFillingConfiguration<GoodRepository, GoodForGrid>
            {
                EntityForm = this,
                FilterBox = filterBox,
                MasterGrid = masterGrid
            };

            configuration.Filled += MasterGridFilled;

            return new MasterFillingUtility<GoodRepository, GoodForGrid>(configuration);
        }

        private void Transpose(object sender, EventArgs e)
        {
            splitContainer.Orientation = splitContainer.Orientation == Orientation.Vertical
                ? Orientation.Horizontal
                : Orientation.Vertical;
        }

        private void Prepare(object sender, EventArgs e)
        {
            masterFiller.FillGrid();
            LoadEntity();
        }

        /// <summary>
        /// Заливка гриды меняет выделение, так как данные удаляются; выбираем созданный/отредактированный экземпляр.
        /// </summary>
        /// <remarks>
        /// TODO: Зарефакторить.
        /// </remarks>
        private void MasterGridFilled(object sender, MasterGridFilledEventAgrs e)
        {
            if (e.EntityID.HasValue)
            {
                ChangeEntity(e.EntityID.Value);
            }
        }

        private void LoadEntity()
        {
            var selectedRows = masterGrid.SelectedRows;

            if (selectedRows.Count == 0)
            {
                return;
            }

            LoadEntity(selectedRows[0].Index);
        }

        private void LoadEntity(object sender, DataGridViewCellEventArgs e)
        {
            if (modeUtility.Mode == Mode.Create)
            {
                // потому что надо сначала получить подтверждение, после чего (при утвердительном ответе) вручную перечитается экземпляр
                return;
            }

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
            GoodForDetail entity;

            using (var repository = new GoodRepository())
            {
                entity = repository.GetSingleGood(entityID);
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

            var idValue = masterGrid.Rows[rowIndex].Cells[nameof(GoodForGrid.ТоварID)].Value;

            return short.TryParse(idValue != null ? idValue.ToString() : string.Empty, out var userID)
                ? userID
                : (short?)null;
        }

        private void FillDetail(GoodForDetail entity)
        {
            titleBox.Text = entity.Название;
            priceBox.Text = entity.Цена.ToString("F2");
        }

        private void FillRoles(short userID)
        {
        }

        private void Refresh(object sender, EventArgs e) => Reload();

        private void Reload()
        {
            masterFiller.FillGrid();

            // Trying to select a row in the grid matching the currently displaying user in the detail panel.
            if (entityID.HasValue)
            {
                SelectRow(entityID.Value);
            }

            LoadEntity();

            if (entityID.HasValue)
            {
                FillRoles(entityID.Value);
            }
        }

        private void SelectRow(short userID)
        {
            var row = masterGrid.Rows.Find(nameof(GoodForGrid.ТоварID), userID, new IdEqualityComparer());

            if (row == null)
            {
                return;
            }

            row.Selected = true;
        }

        private void CreateEntity(object sender, EventArgs e) => entityCreator.PrepareNewEntity();

        private void DeleteEntity()
        {
            if (!entityID.HasValue || ConfirmDeleteEntity() != DialogResult.Yes)
            {
                return;
            }

            using (var repository = new GoodRepository())
            {
                repository.DeleteGood(entityID.Value);
            }

            entityID = null;
            Reload();
        }

        private void CancelCreateEntity()
        {
            // Cancelling entity creation itself.
            modeUtility.Mode = Mode.Main;
            ((BindingList<GoodForGrid>)masterGrid.DataSource).RemoveAt(masterGrid.RowCount - 1);
        }

        private void DeleteUnsavedEntity()
        {
            CancelCreateEntity();

            // Select previously selected row.
            if (this.entityID.HasValue)
            {
                SelectRow(entityID.Value);
                LoadEntity(entityID.Value);
            }
        }

        private void DeleteEntity(object sender, EventArgs e)
        {
            switch (modeUtility.Mode)
            {
                case Mode.Main:
                    DeleteEntity();
                    break;
                case Mode.Create when ConfirmCancelCreateEntity() == DialogResult.Yes:
                    DeleteUnsavedEntity();
                    break;
                default:
                    break;
            }
        }

        private void SelectEntity(object sender, EventArgs e)
        {
            if (modeUtility.Mode == Mode.Create && !masterGrid.Rows[masterGrid.RowCount - 1].Selected)
            {
                if (ConfirmCancelCreateEntity() == DialogResult.Yes) 
                {
                    CancelCreateEntity();
                    LoadEntity(masterGrid.SelectedRows[0].Index);
                }
                else
                {
                    masterGrid.Rows[masterGrid.RowCount - 1].Selected = true;
                }
            }
        }

        private DialogResult ConfirmCancelCreateEntity() => MessageBox.Show(
            "Форма содержит данные, которые пока не были сохранены в базу. При продолжении без сохранения они будут безвозвратно утеряны.",
            "Отменить создание товара?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        private DialogResult ConfirmDeleteEntity() => MessageBox.Show(
            "Вы удаляете товар. Отменить действие будет невозможно. Уверены, что хотите продолжить?",
            "Удалить товар?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        private void HandleClosing(object sender, FormClosingEventArgs e)
        {
            if (modeUtility.Mode == Mode.Create && ConfirmCancelCreateEntity() != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void SaveUser(object sender, EventArgs e)
        {
            switch (modeUtility.Mode)
            {
                case Mode.Main:
                    EditEntity();
                    break;
                case Mode.Create:
                    AddEntity();
                    break;
                default:
                    break;
            }
        }

        private void AddEntity()
        {
            using (var repository = new GoodRepository())
            {
                UpsertUser(repository.AddGood);
            }
        }

        private void UpsertUser(Upsert upsert)
        {
            var entityJson = ReadDetail().ToJsonString();

            bool reload = true;
            string message = string.Empty;

            try
            {
                entityID = upsert(entityJson);
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
                MessageBox.Show(message, "Товар не сохранен", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditEntity()
        {
            using (var repository = new GoodRepository())
            {
                UpsertUser(repository.EditGood);
            }
        }

        private GoodForUpsert ReadDetail() => new GoodForUpsert
        {
            ТоварID = entityID,
            Название = titleBox.Text,
            Цена = float.Parse(priceBox.Text),
        };
    }
}

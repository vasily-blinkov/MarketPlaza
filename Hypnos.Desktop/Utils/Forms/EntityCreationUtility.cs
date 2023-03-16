using System.ComponentModel;
using Wholesale.Desktop.Forms;
using Wholesale.Desktop.Utils.Forms.Configurations;

namespace Wholesale.Desktop.Utils.Forms
{
    public class EntityCreationUtility<TGridEntity, TMasterEntity>
        where TGridEntity : class, new()
        where TMasterEntity : class, new()
    {
        private readonly EntityCreationConfiguration<TGridEntity, TMasterEntity> configuration;

        public EntityCreationUtility(EntityCreationConfiguration<TGridEntity, TMasterEntity> configuration)
        {
            this.configuration = configuration;
        }

        public void PrepareNewEntity()
        {
            var grid = configuration.MasterGrid;

            // Add a new row to the table.
            ((BindingList<TGridEntity>)grid.DataSource).Add(configuration.CreateGridEntity());

            // Select the new row.
            grid.ClearSelection();
            grid.Rows[grid.RowCount - 1].Selected = true;

            // Clear details.
            configuration.FillDetails(configuration.CreateDetailEntity());

            // Change visible tool strip items.
            configuration.ModeUtility.Switch(Mode.Create);
        }
    }
}

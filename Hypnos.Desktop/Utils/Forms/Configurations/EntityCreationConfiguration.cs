using System.Windows.Forms;
using Wholesale.Desktop.Forms;

namespace Wholesale.Desktop.Utils.Forms.Configurations
{
    public class EntityCreationConfiguration<TGridEntity, TDetailEntity>
        where TGridEntity : class, new()
        where TDetailEntity : class, new()
    {
        public DataGridView MasterGrid { get; set; }

        public FillDetails<TDetailEntity> FillDetails { get; set; }

        public ToolStripModeUtility<Mode> ModeUtility { get; set; }

        public TGridEntity CreateGridEntity() => new TGridEntity();

        public TDetailEntity CreateDetailEntity() => new TDetailEntity();
    }
}

using System.Windows.Forms;
using Wholesale.Desktop.Utils.Forms.Abstractions;
using Wholesale.Desktop.Utils.Forms.Events;

namespace Wholesale.Desktop.Utils.Forms.Configurations
{
    public class MasterFillingConfiguration<TFilterRepository, TGridEntity>
        where TFilterRepository : IFilterRepository<TGridEntity>
    {
        public IEntityForm EntityForm { get; set; }

        public DataGridView MasterGrid { get; set; }

        public ToolStripTextBox FilterBox { get; set; }

        public event MasterGridFilledEventHandler Filled;

        public void OnFilled(short? entityID)
        {
            if (Filled == null)
            {
                return;
            }

            Filled(this, new MasterGridFilledEventAgrs { EntityID = entityID });
        }
    }
}

using Wholesale.Desktop.Models.MarketPlaza.Goods;
using Wholesale.Desktop.Repositories.MarketPlaza;
using Wholesale.Desktop.Utils.Forms.Abstractions;
using Wholesale.Desktop.Utils.Forms.Configurations;

namespace Wholesale.Desktop.Utils.Forms
{
    public class MasterFillingUtility<TFilterRepository, TGridEntity>
        where TFilterRepository : IFilterRepository<TGridEntity>
    {
        private readonly MasterFillingConfiguration<TFilterRepository, TGridEntity> configuration;

        public MasterFillingUtility(MasterFillingConfiguration<TFilterRepository, TGridEntity> configuration)
        {
            this.configuration = configuration;
        }

        public void FillGrid()
        {
            var entityID = configuration.EntityForm.EntityID;
            var masterGrid = configuration.MasterGrid;

            using (var repository = new GoodRepository())
            {
                masterGrid.DataSource = repository.Filter(configuration.FilterBox.Text);
            }

            GridUtility.Setup(masterGrid, ReflectionUtility.GetHiddenNames<GoodForGrid>());

            configuration.OnFilled(entityID);
        }
    }
}

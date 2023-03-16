using System.Data.SqlClient;
using Wholesale.Desktop.Models.MarketPlaza.Goods;
using Wholesale.Desktop.Models.MarketPlaza.Lessees;

namespace Wholesale.Desktop.Converters.MarketPlaza
{
    public static class ConvertGood
    {
        public static GoodForDetail ForDetail(SqlDataReader source)
        {
            var destination = ForGrid<GoodForDetail>(source);
            destination.Цена = (decimal)source[nameof(GoodForDetail.Цена)];
            return destination;
        }

        public static T ForGrid<T>(SqlDataReader source)
            where T: GoodForGrid, new()
        {
            var destination = ToCommon(source, new T());
            destination.Название = (string)source[nameof(GoodForGrid.Название)];
            return destination;
        }

        private static T ToCommon<T>(SqlDataReader source, T destination)
            where T: Good, new()
        {
            if (destination == null)
            {
                destination = new T();
            }

            destination.ТоварID = (short)source[nameof(Good.ТоварID)];

            return destination;
        }
    }
}

using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Wholesale.Desktop.Converters.MarketPlaza;
using Wholesale.Desktop.Models.MarketPlaza.Goods;
using Wholesale.Desktop.Utils.Forms.Abstractions;

namespace Wholesale.Desktop.Repositories.MarketPlaza
{
    public class GoodRepository : RepositoryBase, IFilterRepository<GoodForGrid>
    {
        protected override string SchemaName => "MarketPlaza";

        public BindingList<GoodForGrid> Filter(string query = null)
        {
            SqlParameter[] parameters = !string.IsNullOrWhiteSpace(query)
                ? new[] { new SqlParameter { ParameterName = "@query", Value = query } }
                : new SqlParameter[0];

            return ExecuteReaderAuth<GoodForGrid>("GetGoods", ConvertGood.ForGrid<GoodForGrid>, parameters);
        }

        public GoodForDetail GetSingleGood(short id) => ExecuteReaderAuth<GoodForDetail>(
            "GetSingleGood", ConvertGood.ForDetail,
            new SqlParameter { ParameterName = "@id", Value = id }
        ).Single();

        /// <returns>ID of the created entity.</returns>
        public short? AddGood(string json)
        {
            var id = new SqlParameter { ParameterName = "@id", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.SmallInt };
            ExecuteCommandAuth("AddGood", new SqlParameter("@json", json), id);
            return (short?)(id.Value != DBNull.Value ? id.Value : null);
        }

        public short? EditGood(string json)
        {
            var id = new SqlParameter { ParameterName = "@id", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.SmallInt };
            ExecuteCommandAuth("EditGood", new SqlParameter("@json", json), id);
            return (short?)(id.Value != DBNull.Value ? id.Value : null);
        }

        public int DeleteGood(short id) => ExecuteCommandAuth("DeleteGood", new SqlParameter("@id", id)
        );
    }
}

using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Wholesale.Desktop.Converters;
using Wholesale.Desktop.Models.MarketPlaza.Lessees;

namespace Wholesale.Desktop.Repositories
{
    public class MarketPlazaRepository : RepositoryBase
    {
        protected override string SchemaName => "MarketPlaza";

        public BindingList<LesseeForGrid> GetLessees(string query = null)
        {
            SqlParameter[] parameters = !string.IsNullOrWhiteSpace(query)
                ? new[] { new SqlParameter { ParameterName = "@query", Value = query } }
                : new SqlParameter[0];

            return ExecuteReaderAuth("GetLessees", ConvertLessee.ForGrid, parameters);
        }

        public LesseeForDetail GetSingleLessee(short id) => ExecuteReaderAuth(
            "GetSignleLessee", ConvertLessee.ForDetail,
            new SqlParameter { ParameterName = "@id", Value = id }
        ).Single();

        /// <returns>ID of the created entity.</returns>
        public short? AddLessee(string json)
        {
            var id = new SqlParameter { ParameterName = "@id", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.SmallInt };
            ExecuteCommandAuth("AddLessee", new SqlParameter("@json", json), id);
            return (short?)(id.Value != DBNull.Value ? id.Value : null);
        }

        public short? EditLessee(string json)
        {
            var id = new SqlParameter { ParameterName = "@id", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.SmallInt };
            ExecuteCommandAuth("EditLessee", new SqlParameter("@json", json), id);
            return (short?)(id.Value != DBNull.Value ? id.Value : null);
        }

        public int DeleteLessee(short id) => ExecuteCommandAuth("DeleteLessee", new SqlParameter("@id", id)
        );
    }
}

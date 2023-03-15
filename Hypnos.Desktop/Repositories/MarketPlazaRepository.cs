using Wholesale.Desktop.Models.MarketPlaza.Lessees;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using Wholesale.Desktop.Converters;
using Wholesale.Desktop.Models.Administration;
using Wholesale.Desktop.Models.Administration.User;

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

        public int AddLessee(string json) => ExecuteCommandAuth(
            "AddLessee",
            new SqlParameter("@json", json)
        );

        public int EditLessee(string json) => ExecuteCommandAuth(
            "EditLessee",
            new SqlParameter("@json", json)
        );
    }
}

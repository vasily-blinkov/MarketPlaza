using System.Data.SqlClient;
using Wholesale.Desktop.Models.MarketPlaza.Lessees;

namespace Wholesale.Desktop.Converters
{
    public class ConvertLessee
    {
        public static LesseeForGrid ForGrid(SqlDataReader lessee) => ForGrid(lessee, new LesseeForGrid());

        public static LesseeForDetail ForDetail(SqlDataReader lessee)
        {
            var detail = new LesseeForDetail();
            ToCommon<LesseeForDetail>(lessee, detail);

            detail.Фамилия = (string)lessee[nameof(LesseeForDetail.Фамилия)];
            detail.Имя = (string)lessee[nameof(LesseeForDetail.Имя)];
            detail.Отчество = (string)lessee[nameof(LesseeForDetail.Отчество)];
            detail.Телефон = (string)lessee[nameof(LesseeForDetail.Телефон)];
            detail.Адрес = (string)lessee[nameof(LesseeForDetail.Адрес)];

            return detail;
        }

        private static LesseeForGrid ForGrid(SqlDataReader source, LesseeForGrid destination)
        {
            if (destination == null)
            {
                return new LesseeForGrid();
            }

            ToCommon<LesseeForGrid>(source, destination);
            destination.FullName = (string)source[nameof(LesseeForGrid.FullName)];

            return destination;
        }

        private static Lessee ToCommon<T>(SqlDataReader source, Lessee destination)
            where T : Lessee, new()
        {
            if (destination == null)
            {
                return new T();
            }

            destination.АрендаторID = (short)source[nameof(LesseeForGrid.АрендаторID)];

            return destination;
        }
    }
}

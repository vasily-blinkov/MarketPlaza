using System;

namespace Wholesale.Desktop.Converters
{
    public static class ConvertData
    {
        public static string ToString(object data) => data == null || data == DBNull.Value ? string.Empty : (string)data;
    }
}

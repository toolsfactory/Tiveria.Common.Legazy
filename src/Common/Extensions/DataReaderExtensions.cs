using System.Data;

namespace Tiveria.Common.Extensions
{
    public static class DataReaderExtensions
    {
        public static string SaveGetString(this IDataReader reader, int column, string DefaultValue="")
        {
            if (reader.IsDBNull(column))
                return DefaultValue;

            return reader.GetString(column);
        }

        public static string SaveGetString(this IDataReader reader, string column, string DefaultValue = "")
        {
            return reader.SaveGetString(reader.GetOrdinal(column), DefaultValue);
        }

        public static int SaveGetInt32(this IDataReader reader, int column, int DefaultValue = 0)
        {
            if (reader.IsDBNull(column))
                return DefaultValue;

            return reader.GetInt32(column);
        }

        public static int SaveGetInt32(this IDataReader reader, string column, int DefaultValue = 0)
        {
            return reader.SaveGetInt32(reader.GetOrdinal(column), DefaultValue);
        }

        public static byte SaveGetByte(this IDataReader reader, int column, byte DefaultValue = 0)
        {
            if (reader.IsDBNull(column))
                return DefaultValue;

            return reader.GetByte(column);
        }
    }
}

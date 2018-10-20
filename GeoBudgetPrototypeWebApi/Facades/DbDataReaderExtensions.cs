using System;
using System.Data.Common;

namespace GeoBudgetPrototypeWebApi
{
    public static class DbDataReaderExtensions
    {
        public static string GetNullableString(this DbDataReader reader, string name) =>
            reader[name] == DBNull.Value ? "" : (string)reader[name];

        public static T GetNullable<T>(this DbDataReader reader, string name) =>
            reader[name] == DBNull.Value ? default(T) : (T)reader[name];
    }
}

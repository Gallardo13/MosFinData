using MySql.Data.MySqlClient;
using System.Data.Common;

namespace GeoBudgetPrototypeWebApi.Facades
{
    public class DbBaseFacade
    {
        public string ConnectionString { get; set; }

        // строки запросов
        public ISqlStrings SqlStrings { get; } = new SqlStrings();

        public DbBaseFacade()
        {
            ConnectionString = "Server=sv4.surayfer.com;Database=geobudget;Uid=geobudget_user;Pwd=Ze92pl4VCIWgb0hr;SslMode=none;";
        }

        /// <summary>
        /// Получить коннекшн к БД. Использовать через using
        /// </summary>
        /// <returns></returns>
        protected DbConnection GetDbConnection()
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            return conn;
        }
    }
}

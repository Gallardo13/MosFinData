using ClearSpendingData.Contracts;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GetAllContractsFromClearSpending
{
    class Program
    {
        protected static string _connectionString = "Server=sv4.surayfer.com;Database=geobudget;Uid=geobudget_user;Pwd=Ze92pl4VCIWgb0hr;SslMode=none;";


        public static void Main(string[] args)
        {
            Console.WriteLine("Get customers...");

            var customersInn = GetCustomers();

            Console.WriteLine($"Done! Customers count: {customersInn.Count}");

            ClearContractData();

            foreach (var customer in customersInn)
            {
                Console.WriteLine($"Starting loading contracts for customer: {customer}");

                var contracts = ProcessContracts(customer).Result;

                Console.WriteLine($"Contracts loaded... Total: {contracts.Count}");

                Console.WriteLine($"Starting storing contract in database...");

                var contractsFiltered = contracts.Select(e =>
                    (e.number, e.regNum, e.price, e.products,
                    e.execution?.startDate, e.execution?.endDate,
                    e.customer?.inn, e.status, e.contractUrl, e.scan));

                StoreToDatabase(contractsFiltered);

                Console.WriteLine($"Contracts stored in database... Total: {contracts.Count}");
            }

            Console.ReadLine();
        }

        private static IReadOnlyList<string> GetCustomers()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "select inn from customers";
                var reader = cmd.ExecuteReader();

                var retVal = new List<string>();
                while (reader.Read())
                {
                    retVal.Add((string)reader[0]);
                }

                return retVal;
            }
        }

        private static async Task<IReadOnlyList<Datum>> ProcessContracts(string customer)
        {
            var contracts = new List<Datum>();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Core Custom Client MosFinData Hackaton");

            var page = 1;
            while (true)
            {
                var stringTask = client.GetStringAsync($"http://openapi.clearspending.ru/restapi/v3/contracts/search/?daterange=01.01.2018-31.03.2018&customerinn={customer}&customerregion=77&page={page}");

                var json = await stringTask;

                var objects = JsonConvert.DeserializeObject<RootObject>(json);

                contracts.AddRange(objects.contracts.data);

                Console.WriteLine($"Get contracts... Page: {objects.contracts.page}, Per page: {objects.contracts.perpage}, Total: {objects.contracts.total}, Loaded: {contracts.Count}");

                if (objects.contracts.page * objects.contracts.perpage >= objects.contracts.total)
                    break;

                page++;
            }

            return contracts;
        }

        private static void StoreToDatabase(IEnumerable<(string number, string regNum, double price, List<Product> products, string startDate, string endDate, string inn, string status, string contractUrl, List<Scan> scan)> contractsFiltered)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                foreach (var contract in contractsFiltered.Where(e=>e.inn != null))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into contracts" +
                        " (price, inn, date_start, date_finish, status, url, contract, regnumber)" +
                        " values" +
                        " (@price, @inn, @date_start, @date_finish, @status, @url, @contract, @regNum);" +
                        " select last_insert_id();";

                    cmd.Parameters.AddWithValue("price", (decimal)contract.price);
                    cmd.Parameters.AddWithValue("inn", contract.inn);
                    cmd.Parameters.AddWithValue("date_start", contract.startDate == null ? DBNull.Value : (object)DateTime.Parse(contract.startDate));
                    cmd.Parameters.AddWithValue("date_finish", contract.endDate == null ? DBNull.Value : (object)DateTime.Parse(contract.endDate));
                    cmd.Parameters.AddWithValue("status", contract.status);
                    cmd.Parameters.AddWithValue("url", contract.contractUrl);
                    cmd.Parameters.AddWithValue("contract", contract.number);
                    cmd.Parameters.AddWithValue("regNum", contract.regNum);

                    var contractId = (ulong)cmd.ExecuteScalar();

                    AddContractProducts(conn, contract.products.Select(e=>e.OKPD2.code).Distinct(), contractId);

                    AddContractScans(conn, contract.scan, contractId);

                    Console.WriteLine($"Contract: {contract.number} stored.");
                }
            }
        }

        private static void ClearContractData()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();

                cmd.CommandText = "truncate table okpd2contracts";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "truncate table documents2contracts";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "truncate table contracts";
                cmd.ExecuteNonQuery();
            }
        }

        private static void AddContractScans(MySqlConnection conn, IEnumerable<Scan> scans, ulong contractId)
        {
            foreach (var scan in scans)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "insert into documents2contracts" +
                    " (contract, description, name, url)" +
                    " values" +
                    " (@contract, @description, @name, @url)";

                cmd.Parameters.AddWithValue("contract", contractId);
                cmd.Parameters.AddWithValue("description", scan.docDescription);
                cmd.Parameters.AddWithValue("name", scan.fileName);
                cmd.Parameters.AddWithValue("url", scan.url);

                cmd.ExecuteNonQuery();
            }
        }

        private static void AddContractProducts(MySqlConnection conn, IEnumerable<string> okpds, ulong contractId)
        {
            foreach (var okpd in okpds)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "insert into okpd2contracts" +
                    " (contract, okpd) select @contract, id from okpd where code like @okpd";

                cmd.Parameters.AddWithValue("contract", contractId);
                cmd.Parameters.AddWithValue("okpd", okpd);

                cmd.ExecuteNonQuery();
            }
        }
    }
}

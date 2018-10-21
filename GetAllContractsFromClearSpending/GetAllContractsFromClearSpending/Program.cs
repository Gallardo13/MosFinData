using ClearSpendingData.Contracts;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GetAllContractsFromClearSpending
{
    class Program
    {
        protected static string _connectionString = "Server=sv4.surayfer.com;Database=geobudget;Uid=geobudget_user;Pwd=Ze92pl4VCIWgb0hr;SslMode=none;";
        protected static Random _random;

        public static void Main(string[] args)
        {
            _random = new Random((int)DateTime.Now.Ticks);

            Console.WriteLine("Get customers...");

            var customersInn = GetCustomers();

            Console.WriteLine($"Done! Customers count: {customersInn.Count}");

            Parallel.ForEach(customersInn, new ParallelOptions { MaxDegreeOfParallelism = 2 }, customer =>
            {
                Console.WriteLine($"Customer: {customer}. Starting loading contracts");

                var dateFrom = new DateTime(2018, 1, 1);
                var dateTo = new DateTime(2018, 12, 31);

                var contracts = ProcessContracts(customer, dateFrom, dateTo).Result;

                Console.WriteLine($"Customer: {customer}. Contracts loaded... Total: {contracts.Count}");
            });

            Console.WriteLine("Done!");
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

        private static async Task<IReadOnlyList<Datum>> ProcessContracts(string customer, DateTime dateFrom, DateTime dateTo)
        {
            var contracts = new List<Datum>();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Core Custom Client MosFinData Hackaton");

            var page = 1;
            while (true)
            {
                var rawDataFileName = $@"..\..\..\..\rawdata\{customer}_{dateFrom.ToString("yyyy-MM-dd")}_{dateTo.ToString("yyyy-MM-dd")}_{page}.json";

                RootObject objects = null;
                if (File.Exists(rawDataFileName))
                {
                    objects = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(rawDataFileName));
                }
                else
                {
                    var stringTask = client.GetStringAsync($"http://openapi.clearspending.ru/restapi/v3/contracts/search/?daterange={dateFrom.ToString("dd.MM.yyyy")}-{dateTo.ToString("dd.MM.yyyy")}&customerinn={customer}&customerregion=77&page={page}");

                    var json = await stringTask;

                    File.WriteAllText(rawDataFileName, json);

                    objects = JsonConvert.DeserializeObject<RootObject>(json);

                    System.Threading.Thread.Sleep(_random.Next(50, 500));
                }

                contracts.AddRange(objects.contracts.data);

                Console.WriteLine($"Customer: {customer}. Get contracts... Page: {objects.contracts.page}, Per page: {objects.contracts.perpage}, Total: {objects.contracts.total}, Loaded: {contracts.Count}");

                if (objects.contracts.page * objects.contracts.perpage >= objects.contracts.total)
                    break;

                page++;
            }

            return contracts;
        }
    }
}

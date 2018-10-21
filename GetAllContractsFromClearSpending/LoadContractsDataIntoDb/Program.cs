using ClearSpendingData.Contracts;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoadContractsDataIntoDb
{
    class Program
    {
        protected static string _connectionString = "Server=sv4.surayfer.com;Database=geobudget;Uid=geobudget_user;Pwd=Ze92pl4VCIWgb0hr;SslMode=none;";
        protected static bool _storeToDb = true;

        static void Main(string[] args)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                ClearContractData(conn, transaction);

                var files = Directory.EnumerateFiles(@"..\..\..\..\rawdata", "*.json").ToArray();
                for (var i = 0; i < files.Length; i++)
                {
                    var fileOnlyName = Path.GetFileName(files[i]);

                    Console.WriteLine($"FileName: {fileOnlyName}. Starting storing contract in database...");

                    var contracts = LoadContracts(files[i]);

                    var contractsFiltered = contracts.Select(e =>
                        (e.number, e.regNum, e.price, e.products,
                        e.execution?.startDate, e.execution?.endDate,
                        e.customer?.inn, e.status, e.contractUrl, e.scan)).ToArray();

                    StoreToDatabase(conn, transaction, contractsFiltered);

                    Console.WriteLine($"{i}/{files.Length}. FileName: {fileOnlyName}. Contracts stored in database... Total: {contractsFiltered.Length}");
                }

                transaction.Commit();
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static IReadOnlyList<Datum> LoadContracts(string fileName)
        {
            var contracts = new List<Datum>();

            var objects = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(fileName));

            contracts.AddRange(objects.contracts.data);

            return contracts;
        }

        private static void StoreToDatabase(MySqlConnection conn, MySqlTransaction transaction, IEnumerable<(string number, string regNum, double price, List<Product> products, string startDate, string endDate, string inn, string status, string contractUrl, List<Scan> scan)> contractsFiltered)
        {
            foreach (var contract in contractsFiltered.Where(e => e.inn != null))
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = "insert into contracts" +
                    " (price, inn, date_start, date_finish, status, url, contract, regnumber)" +
                    " values" +
                    " (@price, @inn, @date_start, @date_finish, @status, @url, @contract, @regNum);";

                cmd.Parameters.AddWithValue("price", (decimal)contract.price);
                cmd.Parameters.AddWithValue("inn", contract.inn);
                cmd.Parameters.AddWithValue("date_start", contract.startDate == null ? DBNull.Value : (object)DateTime.Parse(contract.startDate));
                cmd.Parameters.AddWithValue("date_finish", contract.endDate == null ? DBNull.Value : (object)DateTime.Parse(contract.endDate));
                cmd.Parameters.AddWithValue("status", contract.status);
                cmd.Parameters.AddWithValue("url", contract.contractUrl);
                cmd.Parameters.AddWithValue("contract", contract.number);
                cmd.Parameters.AddWithValue("regNum", contract.regNum);

                ulong contractId = 0;

                if (_storeToDb)
                {
                    cmd.ExecuteNonQuery();
                    contractId = (ulong)cmd.LastInsertedId;
                }

                var okpdCodes = contract.products.Select(e => e.OKPD2?.code).Distinct().ToArray();

                if (okpdCodes.Count() == 1 && String.IsNullOrEmpty(okpdCodes[0]))
                    Console.WriteLine($"Error in contract: {contract.regNum}! OKPD is empty!");
                else
                    AddContractProducts(conn, transaction, okpdCodes, contractId);

                AddContractScans(conn, transaction, contract.scan ?? Enumerable.Empty<Scan>(), contractId);
            }
        }

        private static void ClearContractData(MySqlConnection conn, MySqlTransaction transaction)
        {
            var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;

            cmd.CommandText = "truncate table okpd2contracts";
            if (_storeToDb)
                cmd.ExecuteNonQuery();

            cmd.CommandText = "truncate table documents2contracts";
            if (_storeToDb)
                cmd.ExecuteNonQuery();

            cmd.CommandText = "truncate table contracts";
            if (_storeToDb)
                cmd.ExecuteNonQuery();
        }

        private static void AddContractScans(MySqlConnection conn, MySqlTransaction transaction, IEnumerable<Scan> scans, ulong contractId)
        {
            foreach (var scan in scans)
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = "insert into documents2contracts" +
                    " (contract, description, name, url)" +
                    " values" +
                    " (@contract, @description, @name, @url)";

                cmd.Parameters.AddWithValue("contract", contractId);
                cmd.Parameters.AddWithValue("description", scan.docDescription);
                cmd.Parameters.AddWithValue("name", scan.fileName);
                cmd.Parameters.AddWithValue("url", scan.url);

                if (_storeToDb)
                    cmd.ExecuteNonQuery();
            }
        }

        private static void AddContractProducts(MySqlConnection conn, MySqlTransaction transaction, IEnumerable<string> okpds, ulong contractId)
        {
            foreach (var okpd in okpds)
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = "insert into okpd2contracts" +
                    " (contract, okpd) select @contract, id from okpd where code like @okpd";

                cmd.Parameters.AddWithValue("contract", contractId);
                cmd.Parameters.AddWithValue("okpd", okpd);

                if (_storeToDb)
                    cmd.ExecuteNonQuery();
            }
        }
    }
}

using GeoBudgetPrototypeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoBudgetPrototypeWebApi.Facades
{
    public class ContractFacade : DbBaseFacade
    {
        /// <summary>
        /// Загрузить все контракты
        /// </summary>
        public IEnumerable<Contract> GetContracts() 
        {
            var retVal = new List<Contract>();

            using (var connection = GetDbConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = SqlStrings.GetContracts;

                var dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var contract = new Contract()
                    {
                        Id = dataReader.GetNullable<int>("id"),
                        Price = dataReader.GetNullable<decimal>("price"),
                        INN = dataReader.GetNullableString("inn"),
                        DateStart = dataReader.GetNullable<DateTime>("date_start"),
                        DateFinish = dataReader.GetNullable<DateTime>("date_finish"),
                        Status = dataReader.GetNullableString("status"),
                        Url = dataReader.GetNullableString("url"),
                        Number = dataReader.GetNullableString("contract"),
                        OKPDs = new List<string>()
                    };

                    retVal.Add(contract);
                }

                dataReader.Close();

                foreach (var contract in retVal) 
                {
                    var okpdCommand = connection.CreateCommand();
                    okpdCommand.CommandText = SqlStrings.GetOKPDsByContractId;
                    okpdCommand.AddParameter("@contractid", contract.Id);

                    var okpdDataReader = okpdCommand.ExecuteReader();

                    while (okpdDataReader.Read())
                    {
                        contract.OKPDs.Add(okpdDataReader.GetNullable<string>("description"));
                    }

                    okpdDataReader.Close();
                }
            }

            return retVal;
        }

        /// <summary>
        /// Загрузить все контракты с соответствующим ОКАТО
        /// </summary>
        public IEnumerable<Contract> GetContractsByOKATO(long okato) 
        {
            var retVal = new List<Contract>();

            using (var connection = GetDbConnection()) 
            {
                var command = connection.CreateCommand();
                command.CommandText = SqlStrings.GetContractsByOkato;

                command.AddParameter("@okato", okato + "%");

                var dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var contract = new Contract()
                    {
                        Id = dataReader.GetNullable<int>("id"),
                        Price = dataReader.GetNullable<decimal>("price"),
                        DateStart = dataReader.GetNullable<DateTime>("date_start"),
                        DateFinish = dataReader.GetNullable<DateTime>("date_finish"),
                        Status = dataReader.GetNullableString("status"),
                        Url = dataReader.GetNullableString("url"),
                        OKPDs = new List<string>()
                    };

                    retVal.Add(contract);
                }

                dataReader.Close();

                foreach (var contract in retVal)
                {
                    var okpdCommand = connection.CreateCommand();
                    okpdCommand.CommandText = SqlStrings.GetOKPDsByContractId;
                    okpdCommand.AddParameter("@contractid", contract.Id);

                    var okpdDataReader = okpdCommand.ExecuteReader();

                    while (okpdDataReader.Read())
                    {
                        contract.OKPDs.Add((string)okpdDataReader["description"]);
                    }

                    okpdDataReader.Close();
                }
            }

            return retVal;
        }

        public IEnumerable<Contract> GetContractsByYearAndOKATO(int year, long okato)
        {
            var retVal = new List<Contract>();

            using (var connection = GetDbConnection()) 
            {

            }

            return retVal;
        }
    }
}

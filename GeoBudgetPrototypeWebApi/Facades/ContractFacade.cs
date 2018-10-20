using GeoBudgetPrototypeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;

namespace GeoBudgetPrototypeWebApi.Facades
{
    public class ContractFacade : DbBaseFacade
    {
        /// <summary>
        /// Загрузить все контракты
        /// </summary>
        public IEnumerable<Contract> GetContracts() 
        {
            return GetContractsWithParams(SqlStrings.GetContracts, new List<Param>());
        }

        /// <summary>
        /// Загрузить все контракты с соответствующим ОКАТО
        /// </summary>
        public IEnumerable<Contract> GetContractsByOkato(long okato) 
        {
            var parameters = new List<Param>() { new Param() { ParamName = "@okato", ParamValue = okato + "%" } };

            return GetContractsWithParams(SqlStrings.GetContractsByOkato, parameters);
        }

        /// <summary>
        /// Загрузить все контракты с соответствующим окато за переданный год
        /// </summary>
        public IEnumerable<Contract> GetContractsByOkatoAndYear(long okato, int year)
        {
            var parameters = new List<Param>() {
                new Param() { ParamName = "@okato", ParamValue = okato + "%" },
                new Param() { ParamName = "@dateFrom", ParamValue = new DateTime(year, 1, 1) },
                new Param() { ParamName = "@dateTo", ParamValue = new DateTime(year, 12, 31) }
            };

            return GetContractsWithParams(SqlStrings.GetContractsByOkatoAndPeriod, parameters);
        }

        public List<Contract> GetContractsWithParams(string commandText, List<Param> parameters, bool loadOkpds = true) 
        {
            var retVal = new List<Contract>();

            using (var connection = GetDbConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = commandText;

                foreach (var param in parameters) 
                {
                    command.AddParameter(param.ParamName, param.ParamValue);
                }

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
                        Number = dataReader.GetNullableString("contract")
                    };

                    retVal.Add(contract);
                }

                dataReader.Close();

                if (loadOkpds)
                {
                    foreach (var contract in retVal)
                    {
                        contract.OKPDs = GetOkpdsByContractId(connection, contract.Id);
                    }
                }
            }

            return retVal;

        } 

        public List<string> GetOkpdsByContractId(DbConnection connection, int contractId)
        {
            var retVal = new List<string>();

            var okpdCommand = connection.CreateCommand();
            okpdCommand.CommandText = SqlStrings.GetOKPDsByContractId;
            okpdCommand.AddParameter("@contractid", contractId);

            var okpdDataReader = okpdCommand.ExecuteReader();

            while (okpdDataReader.Read())
            {
                retVal.Add(okpdDataReader.GetNullableString("description"));
            }

            okpdDataReader.Close();

            return retVal;
        }
    }
}

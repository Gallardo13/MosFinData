using GeoBudgetPrototypeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;

namespace GeoBudgetPrototypeWebApi.Facades
{
    public class ChartFacade : DbBaseFacade
    {
        /// <summary>
        /// Загрузить сумму всех контрактов
        /// </summary>
        public decimal GetSumOfAllContracts()
        {
            return GetSumOfContractsWithParams(SqlStrings.GetSumOfContracts, new List<Param>());
        }

        /// <summary>
        /// Загрузить сумму всех контрактов за год
        /// </summary>
        public decimal GetSumOfAllContractsByYear(int year)
        {
            var parameters = new List<Param>() {
                new Param() { ParamName = "@dateFrom", ParamValue = new DateTime(year, 1, 1) },
                new Param() { ParamName = "@dateTo", ParamValue = new DateTime(year, 12, 31) }
            };

            return GetSumOfContractsWithParams(SqlStrings.GetSumOfContractsByPeriod, parameters);
        }

        /// <summary>
        /// Загрузить сумму всех контрактов по окато
        /// </summary>
        public decimal GetSumOfContractsByOkato(long okato)
        {
            var parameters = new List<Param>() { new Param() { ParamName = "@okato", ParamValue = okato + "%" } };

            return GetSumOfContractsWithParams(SqlStrings.GetSumOfContractsByOkato, parameters);
        }

        /// <summary>
        /// Загрузить сумму всех конрактов по окато и году
        /// </summary>
        public decimal GetSumOfContractsByOkatoAndYear(long okato, int year) 
        {
            var parameters = new List<Param>() { 
                new Param() { ParamName = "@okato", ParamValue = okato + "%" },
                new Param() { ParamName = "@dateFrom", ParamValue = new DateTime(year, 1, 1) },
                new Param() { ParamName = "@dateTo", ParamValue = new DateTime(year, 12, 31) }
            };

            return GetSumOfContractsWithParams(SqlStrings.GetSumOfContractsByOkatoAndPeriod, parameters);
        }

        public decimal GetSumOfContractsWithParams(string commandText, List<Param> parameters) 
        {
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
                    return dataReader.GetNullable<decimal>(0);
                }

            }

            return 0;
        }
    }
}

using GeoBudgetPrototypeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;

namespace GeoBudgetPrototypeWebApi.Facades
{
    public class PopulationFacade : DbBaseFacade
    {
        public IEnumerable<AccomplishmentKoefficient> GetKoefficientsByYear(int year) 
        {
            var retVal = new List<AccomplishmentKoefficient>();

            using (var connection = GetDbConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = SqlStrings.GetPopulationByYear;

                command.AddParameter("@dateFrom", new DateTime(year, 1, 1));
                command.AddParameter("@dateTo", new DateTime(year, 12, 31));
                command.AddParameter("@year", year);

                var dataReader = command.ExecuteReader();

                while (dataReader.Read()) 
                {
                    var accKoef = new AccomplishmentKoefficient()
                    {
                        Okato = dataReader.GetNullableString("ocato")
                    };

                    var population = dataReader.GetNullable<int>("count");
                    var contractsSum = dataReader.GetNullable<decimal>("contractsSum");

                    if (population != 0)
                    {
                        accKoef.Koefficient = contractsSum / population;
                        retVal.Add(accKoef);
                    }
                }
            }

            return retVal;
        }

        public IEnumerable<AccomplishmentKoefficient> GetKoefficientsByYearAndOkpd(int year, string okpd)
        {
            var retVal = new List<AccomplishmentKoefficient>();

            using (var connection = GetDbConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = SqlStrings.GetPopulationByYearAndOkpd;

                command.AddParameter("@dateFrom", new DateTime(year, 1, 1));
                command.AddParameter("@dateTo", new DateTime(year, 12, 31));
                command.AddParameter("@year", year);
                command.AddParameter("@code", okpd + ".%");

                var dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var accKoef = new AccomplishmentKoefficient()
                    {
                        Okato = dataReader.GetNullableString("ocato")
                    };

                    var population = dataReader.GetNullable<int>("count");
                    var contractsSum = dataReader.GetNullable<decimal>("contractsSum");

                    if (population != 0)
                    {
                        accKoef.Koefficient = contractsSum / population;
                        retVal.Add(accKoef);
                    }
                }
            }

            return retVal;
        }


    }
}

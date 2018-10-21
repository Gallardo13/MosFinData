using GeoBudgetPrototypeWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;

namespace GeoBudgetPrototypeWebApi.Facades
{
    public class OkpdFacade : DbBaseFacade
    {
        public IEnumerable<OKPD> GetAllOkpds() 
        {
            var retVal = new List<OKPD>();

            using (var connection = GetDbConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = SqlStrings.GetOKPDsDictionary;

                var dataReader = command.ExecuteReader();

                while (dataReader.Read()) 
                {
                    var okpd = new OKPD()
                    {
                        Code = dataReader.GetNullableString("code"),
                        Name = dataReader.GetNullableString("description")
                    };

                    retVal.Add(okpd);

                }
            }

            return retVal;
        }
    }
}

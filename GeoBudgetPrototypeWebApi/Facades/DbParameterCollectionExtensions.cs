﻿using System.Data.Common;

namespace GeoBudgetPrototypeWebApi
{
    public static class DbParameterCollectionExtensions
    {
        public static void AddParameter(this DbCommand command, string name, object value)
        {
            var param = command.CreateParameter();
            param.ParameterName = name;
            param.Value = value;

            command.Parameters.Add(param);
        }
    }
}

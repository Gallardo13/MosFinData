namespace GeoBudgetPrototypeWebApi.Facades
{
    public interface ISqlStrings
    {
        string GetContracts { get; }

        string GetContractsByOkato { get; }

        string GetContractsByOkatoAndPeriod { get; }

        string GetSumOfContracts { get; }

        string GetSumOfContractsByPeriod { get; }

        string GetSumOfContractsByOkato { get; }

        string GetSumOfContractsByOkatoAndPeriod { get; }

    }

    public class SqlStrings : ISqlStrings
    {
        public string GetContracts => "select * from contracts";

        public string GetContractsByOkato => @"select
 price,
 date_start,
 date_finish,
 status,
 url,
 (
     select
      group_concat(t2.description separator '; ') text
     from
      okpd2contracts t1
       left join okpd t2 on t2.id = t1.okpd
     where t1.contract = contracts.id
 ) okpdtext
from
 contracts
  left join customers on contracts.inn = customers.inn
where customers.ocato like @okato";

        public string GetContractsByOkatoAndPeriod => @"select
 price,
 date_start,
 date_finish,
 status,
 url,
 (
     select
      group_concat(t2.description separator '; ') text
     from
      okpd2contracts t1
       left join okpd t2 on t2.id = t1.okpd
     where t1.contract = contracts.id
 ) okpdtext
from
 contracts
  left join customers on contracts.inn = customers.inn
where customers.ocato like @okato and (contracts.date_start between @dateFrom and @dateTo)";

        public string GetSumOfContracts => "select SUM(price) from contracts";

        public string GetSumOfContractsByPeriod => "select SUM(price) from contracts where contracts.date_start between @dateFrom and @dateTo";

        public string GetSumOfContractsByOkato => "select SUM(price) from contracts left join customers on contracts.inn = customers.inn where customers.ocato like @okato";

        public string GetSumOfContractsByOkatoAndPeriod => "select SUM(price) from contracts left join customers on contracts.inn = customers.inn where customers.ocato like @okato and (contracts.date_start between @dateFrom and @dateTo)";
    
        
    }
}

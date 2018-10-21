namespace GeoBudgetPrototypeWebApi.Facades
{
    public interface ISqlStrings
    {
        string GetContracts { get; }

        string GetContractsByOkato { get; }

        string GetContractsByOkatoAndPeriod { get; }

        string GetContractsByOkatoAndPeriodAndOkpd { get; }

        string GetSumOfContracts { get; }

        string GetSumOfContractsByPeriod { get; }

        string GetSumOfContractsByPeriodAndOkpd { get; }

        string GetSumOfContractsByOkato { get; }

        string GetSumOfContractsByOkatoAndPeriod { get; }

        string GetSumOfContractsByOkatoAndPeriodAndOkpd { get; }

        string GetPopulationByYear { get; }

        string GetPopulationByYearAndOkpd { get; }

        string GetOKPDsDictionary { get; }

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

        public string GetContractsByOkatoAndPeriodAndOkpd => @"select
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
where
 customers.ocato like @okato
 and contracts.date_start between @dateFrom and @dateTo
 and contracts.id in
 (
     select c.id from contracts c
      left join okpd2contracts oc on c.id = oc.contract
      left join okpd o on o.id = oc.okpd
     where
      o.code like @code
 )
order by
 contracts.id";


        public string GetSumOfContracts => "select SUM(price) from contracts";

        public string GetSumOfContractsByPeriod => "select SUM(price) from contracts where contracts.date_start between @dateFrom and @dateTo";

        public string GetSumOfContractsByPeriodAndOkpd => @"select SUM(price) from contracts where (contracts.date_start between @dateFrom and @dateTo) 
                and contracts.id in
         (
             select c.id from contracts c
              left join okpd2contracts oc on c.id = oc.contract
              left join okpd o on o.id = oc.okpd
             where
              o.code like @code
         )
        ";

        public string GetSumOfContractsByOkato => "select SUM(price) from contracts left join customers on contracts.inn = customers.inn where customers.ocato like @okato";

        public string GetSumOfContractsByOkatoAndPeriod => "select SUM(price) from contracts left join customers on contracts.inn = customers.inn where customers.ocato like @okato and (contracts.date_start between @dateFrom and @dateTo)";

        public string GetSumOfContractsByOkatoAndPeriodAndOkpd => @"select SUM(price) from contracts left join customers on contracts.inn = customers.inn where customers.ocato like @okato and (contracts.date_start between @dateFrom and @dateTo)
                  and contracts.id in
         (
             select c.id from contracts c
              left join okpd2contracts oc on c.id = oc.contract
              left join okpd o on o.id = oc.okpd
             where
              o.code like @code
         )";



        public string GetPopulationByYear => @"select
 t3.municipality,
 t3.count,
 t2.ocato,
 sum(t1.price) contractsSum
from
 contracts t1
 left join customers t2 on t2.inn = t1.inn
 left join population t3 on t3.ocato = t2.ocato
where
 (t1.date_start between @dateFrom and @dateTo)
 and t3.year = @year
group by
 t3.municipality,
 t3.count,
 t2.ocato";

        public string GetPopulationByYearAndOkpd => @"select
 t3.municipality,
 t3.count,
 t2.ocato,
 sum(t1.price) contractsSum
from
 contracts t1
 left join customers t2 on t2.inn = t1.inn
 left join population t3 on t3.ocato = t2.ocato
where
 (t1.date_start between @dateFrom and @dateTo)
 and t3.year = @year
  and t1.id in
     (
         select c.id from contracts c
          left join okpd2contracts oc on c.id = oc.contract
          left join okpd o on o.id = oc.okpd
         where
          o.code like @code
     )
group by
 t3.municipality,
 t3.count,
 t2.ocato";

        public string GetOKPDsDictionary => "select * from okpd where code not like '%.%'";
    }
}

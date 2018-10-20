namespace GeoBudgetPrototypeWebApi.Facades
{
    public interface ISqlStrings
    {
        string GetContracts { get; }

        string GetContractsByOkato { get; }

        string GetContractsByOkatoAndPeriod { get; }

        string GetOKPDsByContractId { get; }

        string GetSumOfContracts { get; }

        string GetSumOfContractsByPeriod { get; }

        string GetSumOfContractsByOkato { get; }

        string GetSumOfContractsByOkatoAndPeriod { get; }

    }

    public class SqlStrings : ISqlStrings
    {
        public string GetContracts => "select * from contracts";

        public string GetContractsByOkato => @"select * from contracts left join customers on contracts.inn = customers.inn where customers.ocato like @okato";

        public string GetContractsByOkatoAndPeriod => "select * from contracts left join customers on contracts.inn = customers.inn where customers.ocato like @okato and (contracts.date_start between @dateFrom and @dateTo)";

        public string GetOKPDsByContractId => @"select * from okpd2contracts left join okpd on okpd2contracts.okpd = okpd.id where contract = @contractid";

        public string GetSumOfContracts => "select SUM(price) from contracts";

        public string GetSumOfContractsByPeriod => "select SUM(price) from contracts where contracts.date_start between @dateFrom and @dateTo";

        public string GetSumOfContractsByOkato => "select SUM(price) from contracts left join customers on contracts.inn = customers.inn where customers.ocato like @okato";

        public string GetSumOfContractsByOkatoAndPeriod => "select SUM(price) from contracts left join customers on contracts.inn = customers.inn where customers.ocato like @okato and (contracts.date_start between @dateFrom and @dateTo)";
    }
}

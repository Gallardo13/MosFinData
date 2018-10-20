namespace GeoBudgetPrototypeWebApi.Facades
{
    public interface ISqlStrings
    {
        string GetContracts { get; }

        string GetContractsByOkato { get; }

        string GetOKPDsByContractId { get; }
    }

    public class SqlStrings : ISqlStrings
    {
        public string GetContracts => "select * from contracts";

        public string GetContractsByOkato => @"select * from contracts left join customers on contracts.inn = customers.inn where customers.ocato like @okato";

        public string GetOKPDsByContractId => "select * from okpd2contracts left join okpd on okpd2contracts.okpd = okpd.id where contract = @contractid";
    }
}

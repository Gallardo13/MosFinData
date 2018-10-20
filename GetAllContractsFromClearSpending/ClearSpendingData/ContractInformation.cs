using System;
using System.Collections.Generic;
using System.Text;

namespace ClearSpendingData.Contracts
{
    public class Attachment
    {
        public string url { get; set; }
        public string guid { get; set; }
        public string docDescription { get; set; }
        public string registrationNumber { get; set; }
        public string fileName { get; set; }
    }

    public class Attachments
    {
        public List<Attachment> attachment { get; set; }
    }

    public class Scan
    {
        public string url { get; set; }
        public string docDescription { get; set; }
        public string fileName { get; set; }
    }

    public class Currency
    {
        public string code { get; set; }
        public string digitalCode { get; set; }
        public string name { get; set; }
    }

    public class PurchaseInfo
    {
        public string purchaseCodeName { get; set; }
        public string purchaseMethodCode { get; set; }
        public string purchaseNoticeNumber { get; set; }
        public string name { get; set; }
    }

    public class Execution
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class Customer
    {
        public string kpp { get; set; }
        public string fax { get; set; }
        public string shortName { get; set; }
        public string OKATO { get; set; }
        public string OGRN { get; set; }
        public string regNum { get; set; }
        public string inn { get; set; }
        public string legalAddress { get; set; }
        public string fullName { get; set; }
        public string iko { get; set; }
        public string postalAddress { get; set; }
    }

    public class MainInfo
    {
        public string kpp { get; set; }
        public string fax { get; set; }
        public string shortName { get; set; }
        public string okpo { get; set; }
        public string ogrn { get; set; }
        public string okato { get; set; }
        public string phone { get; set; }
        public string inn { get; set; }
        public string postalAddress { get; set; }
        public string legalAddress { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
    }

    public class Placer
    {
        public MainInfo mainInfo { get; set; }
    }

    public class Supplier
    {
        public string kpp { get; set; }
        public string organizationName { get; set; }
        public string inn { get; set; }
        public string type { get; set; }
    }

    public class OKEI
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Country
    {
        public string countryCode { get; set; }
        public string countryFullName { get; set; }
    }

    public class OKPD2
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Product
    {
        public string name { get; set; }
        public OKEI OKEI { get; set; }
        public Country country { get; set; }
        public string ordinalNumber { get; set; }
        public OKPD2 OKPD2 { get; set; }
        public string quantity { get; set; }
    }

    public class Modification
    {
        public string description { get; set; }
    }

    public class Budget
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Budgetary
    {
        public string KBK { get; set; }
        public double price { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }

    public class Extrabudget
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Extrabudgetary
    {
        public double price { get; set; }
        public string month { get; set; }
        public string KOSGU { get; set; }
        public string year { get; set; }
    }

    public class Finances
    {
        public string financeSource { get; set; }
        public Budget budget { get; set; }
        public List<Budgetary> budgetary { get; set; }
        public Extrabudget extrabudget { get; set; }
        public List<Extrabudgetary> extrabudgetary { get; set; }
    }

    public class EconomicSector
    {
        public string code { get; set; }
        public string title { get; set; }
    }

    public class OosOrder
    {
        public string notificationNumber { get; set; }
        public string lotNumber { get; set; }
        public string placing { get; set; }
    }

    public class Foundation
    {
        public string singleCustomer { get; set; }
        public OosOrder oosOrder { get; set; }
    }

    public class Datum
    {
        public Attachments attachments { get; set; }
        public List<Scan> scan { get; set; }
        public string number { get; set; }
        public Currency currency { get; set; }
        public string id { get; set; }
        public string fileVersion { get; set; }
        public string regionCode { get; set; }
        public DateTime signDate { get; set; }
        public double price { get; set; }
        public string status { get; set; }
        public PurchaseInfo purchaseInfo { get; set; }
        public string fz { get; set; }
        public DateTime protocolDate { get; set; }
        public DateTime publishDate { get; set; }
        public string regNum { get; set; }
        public int versionNumber { get; set; }
        public string schemaVersion { get; set; }
        public Execution execution { get; set; }
        public int searchRank { get; set; }
        public Customer customer { get; set; }
        public string name { get; set; }
        public Placer placer { get; set; }
        public List<Supplier> suppliers { get; set; }
        public int loadId { get; set; }
        public List<string> misuses { get; set; }
        public DateTime createDateTime { get; set; }
        public string mongo_id { get; set; }
        public List<Product> products { get; set; }
        public string printFormUrl { get; set; }
        public Modification modification { get; set; }
        public string currentContractStage { get; set; }
        public string placing { get; set; }
        public string contractUrl { get; set; }
        public string documentBase { get; set; }
        public Foundation foundation { get; set; }
        public Finances finances { get; set; }
        public List<EconomicSector> economic_sectors { get; set; }
        public string placingWayCode { get; set; }
    }

    public class Contracts
    {
        public int total { get; set; }
        public List<Datum> data { get; set; }
        public int page { get; set; }
        public int perpage { get; set; }
    }

    public class RootObject
    {
        public Contracts contracts { get; set; }
    }
}

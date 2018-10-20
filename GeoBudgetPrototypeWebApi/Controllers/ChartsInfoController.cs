using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoBudgetPrototypeWebApi.Facades;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoBudgetPrototypeWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ChartsInfoController : Controller
    {
        [HttpGet]
        public decimal Get()
        {
            var facade = new ContractFacade();
            return facade.GetSumOfAllContracts();
        }

        [HttpGet("{okato}")]
        public decimal Get(long okato)
        {
            var facade = new ContractFacade();
            return facade.GetSumOfContractsByOkato(okato);
        }
    }
}

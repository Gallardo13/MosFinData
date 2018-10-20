using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoBudgetPrototypeWebApi.Facades;
using GeoBudgetPrototypeWebApi.Models;

namespace GeoBudgetPrototypeWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ContractsController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<Contract> Get()
        {
            var facade = new ContractFacade();
            return facade.GetContracts();
        }

        // GET api/values/5
        [HttpGet("{okato}")]
        public IEnumerable<Contract> Get(long okato)
        {
            var facade = new ContractFacade();
            return facade.GetContractsByOKATO(okato);
        }
    }
}

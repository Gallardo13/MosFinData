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
        [HttpGet]
        public IEnumerable<Contract> Get()
        {
            var facade = new ContractFacade();
            return facade.GetContracts();
        }

        [HttpGet("{okato}")]
        public IEnumerable<Contract> Get(long okato)
        {
            var facade = new ContractFacade();
            return facade.GetContractsByOkato(okato);
        }

        [HttpGet("{okato}/{year}")]
        public IEnumerable<Contract> Get(long okato, int year)
        {
            var facade = new ContractFacade();
            return facade.GetContractsByOkatoAndYear(okato, year);
        }
    }
}

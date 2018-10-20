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
            var facade = new ChartFacade();
            return facade.GetSumOfAllContracts();
        }

        [HttpGet("{okato}/{year}")]
        public decimal Get(long okato, int year)
        {
            var facade = new ChartFacade();

            if (okato != 0 && year == 0) 
            {
                return facade.GetSumOfContractsByOkato(okato);
            }
            else if (okato == 0 && year != 0) 
            {
                return facade.GetSumOfAllContractsByYear(year);
            }
            else 
            {
                return facade.GetSumOfContractsByOkatoAndYear(okato, year);
            }

        }
    }
}

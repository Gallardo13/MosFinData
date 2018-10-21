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
    public class PopulationController : Controller
    {
        [HttpGet("{year}")]
        public IEnumerable<AccomplishmentKoefficient> Get(int year)
        {

            var facade = new PopulationFacade();
            return facade.GetKoefficientsByYear(year);
        }

        [HttpGet("{year}/{okpd}")]
        public IEnumerable<AccomplishmentKoefficient> Get(int year, string okpd)
        {
            var facade = new PopulationFacade();
            return facade.GetKoefficientsByYearAndOkpd(year, okpd);
        }
    }
}

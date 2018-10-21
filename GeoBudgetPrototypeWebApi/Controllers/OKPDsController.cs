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
    public class OKPDsController : Controller
    {
        [HttpGet]
        public IEnumerable<OKPD> Get()
        {
            var facade = new OkpdFacade();
            return facade.GetAllOkpds();
        }
    }
}

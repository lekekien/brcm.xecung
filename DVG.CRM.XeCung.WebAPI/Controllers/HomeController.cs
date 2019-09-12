using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Globalization;
using DVG.CRM.XeCung.WebAPI.Filter;
using DVG.CRM.XeCung.InfrastructureLayer.Core.Enum;

namespace DVG.CRM.XeCung.Web.API.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        // GET: api/values
        [Route("index")]
        [HttpPost]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public JsonResult Index()
        {
            return Json(new Response(SystemCode.Success, "Okie", null));
        }
        [Route("test")]
        public JsonResult Test()
        {
            return Json(new Response(SystemCode.Success, "Okie running success", null));
        }
    }
}

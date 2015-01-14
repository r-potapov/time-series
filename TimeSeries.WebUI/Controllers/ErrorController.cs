using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeSeries.WebUI.Infrastructure;

namespace TimeSeries.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
            return new NotFoundResult();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeSeries.WebUI.Infrastructure
{
    public class NotFoundResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 404;
            new ViewResult { ViewName = "NotFound" }.ExecuteResult(context);
        }
    }
}
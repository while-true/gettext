using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GettextLib;
using GettextMvcLib;

namespace GettextMvcSample.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
            /*
            if (false)
            {
                var gp = gf.GetPseudoContext();
                GettextMvcIntegration.SetAsContextForCurrentRequest(gp, this);
                return;
            }
            */

            var gc = MvcApplication.GettextFactory().GetContext("sl-SI");
            GettextMvcIntegration.SetAsContextForCurrentRequest(gc, this);
        }
    }
}
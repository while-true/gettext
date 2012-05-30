using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GettextMvcLib;

namespace GettextMvcSample.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
            var gf = new GettextFactory();

            var mapPath = System.Web.HttpContext.Current.Server.MapPath("~/po/sl_SI/messages.po");

            gf.AddTranslation("sl_SI", System.IO.File.ReadAllText(mapPath));

            if (false)
            {
                var gp = gf.GetPseudoContext();
                gf.SetAsContextForCurrentRequest(gp, ViewData);
                return;
            }

            var gc = gf.GetContext("sl_SI");
            gf.SetAsContextForCurrentRequest(gc, ViewData);
        }
    }
}
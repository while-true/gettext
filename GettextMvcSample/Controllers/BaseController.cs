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
            var gf = new GettextFactory();

            var mapPath = System.Web.HttpContext.Current.Server.MapPath("~/po/sl_SI/messages.po");

            using (var file = System.IO.File.OpenRead(mapPath))
            {
                gf.AddTranslation("sl_SI", file);
            }

            if (false)
            {
                var gp = gf.GetPseudoContext();
                GettextMvcIntegration.SetAsContextForCurrentRequest(gp, this);
                return;
            }
            
            var gc = gf.GetContext("sl_SI");
            GettextMvcIntegration.SetAsContextForCurrentRequest(gc, this);
        }
    }
}
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
        protected GettextTranslationContext gc;

        public BaseController()
        {
            var gf = new GettextFactory();

            var mapPath = System.Web.HttpContext.Current.Server.MapPath("~/po/sl_SI/messages.po");

            gf.AddTranslation("sl_SI", System.IO.File.ReadAllText(mapPath));

            gc = gf.GetContext("sl_SI");

            //gc = gf.GetPseudoContext();

            ViewData.Add(Consts.GettextContextKey, gc);
            System.Web.HttpContext.Current.Items[Consts.GettextContextKey] = gc;
        }
    }
}
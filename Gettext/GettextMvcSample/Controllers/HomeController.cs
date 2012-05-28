using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GettextMvcSample.Models.Home;

namespace GettextMvcSample.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var model = new IndexModel();

            model.Message = "Hello world from Controller code!";
            model.Email = "";
            model.TooLong = "stringy";

            TryValidateModel(model);
            
            return View(model);
        }
    }
}
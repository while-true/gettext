using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GettextMvcLib.HttpContextHelper;
using GettextMvcSample.Models.Home;

namespace GettextMvcSample.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var model = new IndexModel();

            model.Message = S._("Hello world from Controller code!");
            model.Email = "";
            model.TooLong = "stringy";
            model.AnotherModel = new IndexModel.IndexAnotherModel
                                     {
                                         Password = "abcd"
                                     };

            TryValidateModel(model);
            
            
            return View(model);
        }
    }
}
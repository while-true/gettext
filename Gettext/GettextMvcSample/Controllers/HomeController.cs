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

            for (var i = 0; i < 4; i++)
            {
                var plural = string.Format(S._("{0} file", "{0} files", i), i);
                model.PluralStrings.Add(plural);
            }

            TryValidateModel(model);
            
            
            return View(model);
        }
    }
}
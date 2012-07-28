using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GettextLib;
using GettextMvcLib;
using GettextMvcLib.HttpContextHelper;
using GettextMvcLib.JavaScript;
using GettextMvcSample.Models.Home;

namespace GettextMvcSample.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var str = S.PGettext("context 1", "test1");
            var str2 = S.PNGettext("context 2", "test", "tests", 234);


            var model = new IndexModel();

            model.Message = S._("Hello world from Controller code! čćžđš");
            model.Email = "";
            model.TooLong = "stringy";
            model.Message2 = S._("Hello {username}!").FormatWithNamed(new {username = "Bob"});

            model.AnotherModel = new IndexModel.IndexAnotherModel
                                     {
                                         Password = "abcd"
                                     };

            for (var i = 0; i < 4; i++)
            {
                var plural = S._("{fileCount} file", "{fileCount} files", i).FormatWithNamed(new {fileCount = i});
                model.PluralStrings.Add(plural);
            }

            TryValidateModel(model);
            
            
            return View(model);
        }

        public ActionResult GettextJavascript()
        {
            return ControllerHelper.HandleRequest();
        }
    }
}
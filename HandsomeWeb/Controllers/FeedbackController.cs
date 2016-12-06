using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandsomeWeb.Controllers
{
    public class FeedbackController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult FeedbackParsing(string Name1)
        {
            string testValue = Name1;

            return Json(testValue, JsonRequestBehavior.AllowGet);
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feedback.ContentsFeeder;
using Feedback.ContentsFeeder.Converter;
using Feedback.ContentsFeeder.BizDac;

namespace HandsomeWeb.Controllers
{
	public class FeedbackController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		[ValidateInput(false)]
		public JsonResult FeedbackParsing(string Name1)
		{
			FeedbackDocument test = new FeedbackDocument(FbSite.Gmarket, String.Empty, Name1, FbInputChannel.PC, DateTime.Now);

			string sensitive = "긍정도 부정도 아닌";

			if (test.SensitiveScore > 0)
			{
				sensitive = "긍정적인 평가의";
			}
			else if (test.SensitiveScore < 0)
			{
				sensitive = "부정적인 평가의 ";
			}



			return Json(String.Format("당신의 점수는 {0}점입니다.\n당신의 글은 {1} 상품평입니다. \n상품평의 감성점수는 {2}점 입니다.", test.FbQuality, sensitive, test.SensitiveScore), JsonRequestBehavior.AllowGet);
		}


	}
}
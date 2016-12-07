using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feedback.ContentsFeeder;
using Feedback.ContentsFeeder.Converter;
using Feedback.ContentsFeeder.BizDac;
using MongoDB.Bson;
using MongoDB.Driver;

using MyTaskListApp.Models;
using HandsomeWeb.Models;

namespace HandsomeWeb.Controllers
{
	public class FeedbackSearchController : Controller
	{

		public ActionResult Index()
		{

			return View();
		}
	}
}
using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Util;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Feedback.ContentsFeeder;
using Feedback.ContentsFeeder.Converter;
using Feedback.ContentsFeeder.BizDac;
using MongoDB.Bson;
using MongoDB.Driver;

using MyTaskListApp.Models;
using HandsomeWeb.Models;
using Feedback.ContentsFeeder.AzureWebservice;

using HandsomeWeb.Models;
using System.Text;

namespace HandsomeWeb.Controllers
{
	public class FeedbackController : Controller
	{
        // private MongoServer mongoServer = null;
        private bool disposed = false;

        // To do: update the connection string with the DNS name
        // or IP address of your server. 
        //For example, "mongodb://testlinux.cloudapp.net
        private string connectionString = "mongodb://handsome-test:7vjFMv1AbtMMRzuUwetQTZ9taZSOFb1j8UGs9Uj2YwuBZWXY0shu8QKCqz8u3Wk1iChWQAW1RF6U58HxFl2CWg==@handsome-test.documents.azure.com:10250/?ssl=true&sslverifycertificate=false";
        private string userName = "handsome-test";
        private string host = "handsome-test.documents.azure.com";
        private string password = "7vjFMv1AbtMMRzuUwetQTZ9taZSOFb1j8UGs9Uj2YwuBZWXY0shu8QKCqz8u3Wk1iChWQAW1RF6U58HxFl2CWg==";

        // This sample uses a database named "Tasks" and a 
        //collection named "TasksList".  The database and collection 
        //will be automatically created if they don't already exist.

        #region task
        // private string dbName = "Tasks";
        // private string collectionName = "TasksList";
        #endregion

        #region
        private string dbName = "Feedback";
        private string collectionName = "FeedbackList";
        #endregion

        public ActionResult Index()
		{
			return View();
		}

        public ActionResult RealFeedbackAnalyze()
        {
            return View();
        }

        public ActionResult RealFeedbackAnalyzeStep2(string orderNo)
        {
            string prvwNo = orderNo;
            // http://bamboo.auction.co.kr/Feedback/Feedback/GetFeedbackSecondDetail?orderno=1223317364
            string url = String.Format("http://bamboo.gmarket.co.kr/Feedback/Premium/GetPremiumFeedbackDetail?prvwNo={0}", prvwNo);

            HandsomeWeb.Models.APIResponseT model = new HandsomeWeb.Models.APIResponseT();
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;

                var json = client.DownloadString(url);
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                model = serializer.Deserialize<HandsomeWeb.Models.APIResponseT>(json);
                // TODO: do something with the model
            }

            RealFeedbackSearchModel data = new RealFeedbackSearchModel();
            data.Title = model.Result.model.PRvwNm;
            data.Contents = model.Result.model.PRvwTxt;
            data.ImageUrl = model.Result.model.PhtUrl;

            return View(data);
        }

        public JsonResult GetRealFeedback(string orderno)
        {
            string prvwNo = orderno;
            // http://bamboo.auction.co.kr/Feedback/Feedback/GetFeedbackSecondDetail?orderno=1223317364
            string url = String.Format("http://bamboo.gmarket.co.kr/Feedback/Premium/GetPremiumFeedbackDetail?prvwNo={0}", prvwNo);

            HandsomeWeb.Models.APIResponseT model = new HandsomeWeb.Models.APIResponseT();
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;

                var json = client.DownloadString(url);
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                model = serializer.Deserialize<HandsomeWeb.Models.APIResponseT>(json);
                // TODO: do something with the model
            }

            FeedbackDocument test = new FeedbackDocument(FbSite.Auction, String.Empty, 0, String.Empty, String.Empty, model.Result.model.PRvwTxt, FbInputChannel.PC, DateTime.Now);

            string sensitive = "";

            if (test.SensitiveScore > 0)
            {
                sensitive = "긍정적인 내용의 상품평";
            }
            else if (test.SensitiveScore < 0)
            {
				sensitive = "부정적인 내용의 상품평";
            }
            
			RealFeedbackSearchModel data = new RealFeedbackSearchModel();
            data.Title = model.Result.model.PRvwNm;
            data.Contents = model.Result.model.PRvwTxt;
            data.ImageUrl = model.Result.model.PhtUrl;


            data.FbQuality = test.FbQuality;
            data.Sensitive = sensitive;
            data.SensitiveScore = test.SensitiveScore;
			data.PositiveScore = test.PositiveScore;
			data.NgativeScore = test.NgativeScore;
            data.AnalysisedText = test.AnalysisedText;

			switch (test.FbQuality)
			{
				case FeedbackQuality.Junk:
					data.FbMsg = "<h1>정크 상품평입니다 (0점)<h1/>";
					data.PositiveScore = 0;
					data.NgativeScore = 0;
					data.SensitiveScore = 0;
					data.Sensitive = String.Empty;
					break;

				case FeedbackQuality.BelowAverage:
					data.FbMsg = "<h1>품질은 평균이하입니다 (1점)<h1/>";
					break;

				case FeedbackQuality.Average:
					data.FbMsg = "<h1>품질은 보통입니다 (2점)<h1/>";
					break;

				case FeedbackQuality.Good:
					data.FbMsg = "<h1>품질은 좋은 상품평입니다 (3점)<h1/>";
					break;

				case FeedbackQuality.Excellent:
					data.FbMsg = "<h1>품질이 훌륭합니다 (4점)<h1/>";
					break;

				case FeedbackQuality.MostExcellent:
					data.FbMsg = "<h1/>품질이 대단히 매우 훌륭합니다. 사랑합니다. 고객님 (5점)<h1/>";
					break;
			}

            return Json(data, JsonRequestBehavior.AllowGet);
            // return RedirectToAction("RealFeedbackAnalyzeStep2", "Feedback");
        }

        [ValidateInput(false)]
		public JsonResult FeedbackParsing(string Name1)
		{
			FeedbackDocument test = new FeedbackDocument(FbSite.Gmarket, String.Empty, 0,  String.Empty, String.Empty, Name1, FbInputChannel.PC, DateTime.Now);

			return Json(String.Format("당신의 점수는 {0}점입니다.\n\n{1}", test.FbQuality, test.AnalysisedText), JsonRequestBehavior.AllowGet);
		}

        [ValidateInput(false)]
        public JsonResult FeedbackParsingData(string Name1)
        {
			FeedbackDocument test = new FeedbackDocument(FbSite.Gmarket, String.Empty, 0, String.Empty, String.Empty, Name1, FbInputChannel.PC, DateTime.Now);

            string sensitive = "긍정도 부정도 아닌";

            if (test.SensitiveScore > 0)
            {
                sensitive = "긍정적인 평가의";
            }
            else if (test.SensitiveScore < 0)
            {
                sensitive = "부정적인 평가의 ";
            }

            //test.FbQuality; // 점수
            //sensitive; // 상품평
            //test.SensitiveScore; // 감성점수
            //test.AnalysisedText; //
            //test.PositiveScore; // 긍정점수
            //test.NgativeScore; // 부정점수

            return Json(String.Format("{0} / {1} / {2} / {3} / {4} / {5}", test.FbQuality, sensitive, test.SensitiveScore, test.AnalysisedText, test.PositiveScore, test.NgativeScore), JsonRequestBehavior.AllowGet);
        }

        public JsonResult MongoDBTest(string Name1)
        {
            #region feedback

            //List<FeedbackModel> feedbackRequest = new List<FeedbackModel>();
            //FeedbackModel reqeust = new FeedbackModel()
            //{
            //    Title = string.IsNullOrEmpty(Name1) ? "TestInsert_" + Convert.ToString(DateTime.Now) : Name1,
            //    OriginHtmlContents = "",
            //    Contents = "",
            //    AnaysisedContents = "",
            //    SiteId = 0,
            //    InputChannel = 0,
            //    ImageCount = 0,
            //    CountNPM = 0,
            //    RateOfValid = 0,
            //    QualityScore = 0,
            //    PositiveScore = 0,
            //    NgativeScore = 0,
            //    SensitiveScore = 0,
            //    ImageUrl = 
            //    ImageUrl1 = "",
            //    ImageUrl2 = "",
            //    ImageUrl3 = "",
            //    ImageUrl4 = "",
            //    ImageUrl5 = "",
            //    ImageUrl6 = "",
            //    ImageUrl7 = "",
            //    ImageUrl8 = "",
            //    ImageUrl9 = "",
            //    ImageUrl10 = "",
            //    FbDate = DateTime.Now
            //};

            // InsertFeedback(reqeust);

            List<FeedbackModel> Feedbacks = new List<FeedbackModel>();
            Feedbacks = GetAllFeedbacks();

            return Json(Feedbacks);
            // var test = Convert.ToString(Feedbacks.Count);

            #endregion

            #region task

            /*
            List<MyTask> test = new List<MyTask>();
            test = GetAllTasks();

            MyTask testTask = new MyTask()
            {
                Name = "1234",
                Category = "TestCategory",
                Date = DateTime.Now,
                CreatedDate = DateTime.Now
            };

            CreateTask(testTask);

            test = GetAllTasks();
            */
            #endregion

            // return Json(test);
        }

        #region Task
        // Gets all Task items from the MongoDB server.        
        public List<MyTask> GetAllTasks()
        {
            try
            {
                var collection = GetTasksCollection();
                return collection.Find(new BsonDocument()).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<MyTask>();
            }
        }

        // Creates a Task and inserts it into the collection in MongoDB.
        public void CreateTask(MyTask task)
        {
            var collection = GetTasksCollectionForEdit();
            try
            {
                collection.InsertOne(task);
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
            }
        }

        private IMongoCollection<MyTask> GetTasksCollection()
        {
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            var todoTaskCollection = database.GetCollection<MyTask>(collectionName);
            return todoTaskCollection;
        }

        private IMongoCollection<MyTask> GetTasksCollectionForEdit()
        {
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            var todoTaskCollection = database.GetCollection<MyTask>(collectionName);
            return todoTaskCollection;
        }

        #endregion

        #region Feedback
        public List<FeedbackModel> GetAllFeedbacks()
        {
            try
            {
                var collection = GetFeedbacksCollection();         
                return collection.Find(new BsonDocument()).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<FeedbackModel>();
            }
        }

        public void InsertFeedback(FeedbackModel request)
        {
            var collection = GetFeedbacksCollectionForEdit();
            try
            {
                collection.InsertOne(request);
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
            }
        }

        private IMongoCollection<FeedbackModel> GetFeedbacksCollection()
        {
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            var todoFeedbackCollection = database.GetCollection<FeedbackModel>(collectionName);
            return todoFeedbackCollection;
        }

        private IMongoCollection<FeedbackModel> GetFeedbacksCollectionForEdit()
        {
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            var todoFeedbackCollection = database.GetCollection<FeedbackModel>(collectionName);
            return todoFeedbackCollection;
        }

        #endregion
    }
}
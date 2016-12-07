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

        public JsonResult MongoDBTest(string Name1)
        {
            #region feedback

            List<FeedbackModel> feedbackRequest = new List<FeedbackModel>();
            FeedbackModel reqeust = new FeedbackModel()
            {
                Title = string.IsNullOrEmpty(Name1) ? "TestInsert_" + Convert.ToString(DateTime.Now) : Name1,
                OriginHtmlContents = "",
                Contents = "",
                AnaysisedContents = "",
                SiteId = 0,
                InputChannel = 0,
                ImageCount = 0,
                CountNPM = 0,
                RateOfValid = 0,
                QualityScore = 0,
                PositiveScore = 0,
                NgativeScore = 0,
                SensitiveScore = 0,
                ImageUrl1 = "",
                ImageUrl2 = "",
                ImageUrl3 = "",
                ImageUrl4 = "",
                ImageUrl5 = "",
                ImageUrl6 = "",
                ImageUrl7 = "",
                ImageUrl8 = "",
                ImageUrl9 = "",
                ImageUrl10 = "",
                FbData = DateTime.Now
            };

            InsertFeedback(reqeust);

            // List<FeedbackModel> Feedbacks = new List<FeedbackModel>();
            // Feedbacks = GetAllFeedbacks();

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

            return Json("MongoDBTest");
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
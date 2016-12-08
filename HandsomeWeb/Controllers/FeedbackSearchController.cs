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

		public ActionResult Introduce()
		{

			return View();
		}

		public JsonResult MongoDBTest5(string Name1)
		{
			#region feedback
			List<FeedbackModel> Feedbacks = new List<FeedbackModel>();
			Feedbacks = GetAllFeedbacks(5, 5);
			return Json(Feedbacks);
			#endregion
		}

		public JsonResult MongoDBTest4(string Name1)
		{
			#region feedback
			List<FeedbackModel> Feedbacks = new List<FeedbackModel>();
			Feedbacks = GetAllFeedbacks(4, 3);
			return Json(Feedbacks);
			#endregion
		}



		public JsonResult MongoDBTest3(string Name1)
		{
			#region feedback
			List<FeedbackModel> Feedbacks = new List<FeedbackModel>();
			Feedbacks = GetAllFeedbacks2();
			return Json(Feedbacks);
			#endregion
		}

		public JsonResult MongoDBTest2(string Name1)
		{
			#region feedback
			List<FeedbackModel> Feedbacks = new List<FeedbackModel>();
			Feedbacks = GetAllFeedbacks3();
			return Json(Feedbacks);
			#endregion
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
		public List<FeedbackModel> GetAllFeedbacks(int quality, int imageCount)
		{
			try
			{
				var collection = GetFeedbacksCollection();


				return collection.Find<FeedbackModel>(p => p.QualityScore == quality && p.ImageCount > imageCount).ToList<FeedbackModel>();
			}
			catch (MongoConnectionException)
			{
				return new List<FeedbackModel>();
			}
		}

		public List<FeedbackModel> GetAllFeedbacks2()
		{
			try
			{
				var collection = GetFeedbacksCollection();


				return collection.Find<FeedbackModel>(p => p.QualityScore == 5 && p.SensitiveScore > 3).ToList<FeedbackModel>();
			}
			catch (MongoConnectionException)
			{
				return new List<FeedbackModel>();
			}
		}

		public List<FeedbackModel> GetAllFeedbacks3()
		{
			try
			{
				var collection = GetFeedbacksCollection();


				return collection.Find<FeedbackModel>(p => p.QualityScore == 5 && p.SensitiveScore < -3).ToList<FeedbackModel>();
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
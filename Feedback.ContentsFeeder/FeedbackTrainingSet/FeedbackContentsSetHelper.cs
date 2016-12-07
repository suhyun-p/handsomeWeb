using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Feedback.ContentsFeeder;
using Feedback.ContentsFeeder.BizDac;

using System.IO;


namespace Feedback.ContentsFeeder.FeedbackTrainingSet
{
	public class FeedbackContentsSetHelper
	{
		#region private Fields
		private DataTable feedbackTable = null;
		private Queue<OriginFeedbackContetnsT> MsgBusFeedback = new Queue<OriginFeedbackContetnsT>();
		private List<FeedbackDocument> feedbackDocument = new List<FeedbackDocument>();
		System.Windows.Forms.ProgressBar Progressbar = null;
		private object _lock = new object();
		private string fileName = String.Empty;

		#endregion

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		#region Constructor
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="tableName">DataTable Name</param>
		/// <param name="feedbackDs">상품평정보가 담긴 DataSet</param>
		public FeedbackContentsSetHelper(FbSite fbsite, string tableName, DataSet feedbackDs, System.Windows.Forms.ProgressBar progressbar, string filename)
		{
			StartDate = System.DateTime.Now;
			EndDate = System.DateTime.Now;
			FeedbackSite = fbsite;
			Progressbar = progressbar;
			fileName = filename;

			if (feedbackDs == null || feedbackDs.Tables.Count == 0) return;

			if (feedbackDs.Tables[tableName] == null || feedbackDs.Tables[tableName].Rows.Count == 0) return;

			this.feedbackTable = feedbackDs.Tables[tableName];

			MakeQueueData();

			FeedbackDocumenList = new List<FeedbackDocument>();
			FeedbackCsvList = new List<FeedbackCsvT>();



			Action MakeFeedbackEntity = () =>
			{
				if (MsgBusFeedback == null || MsgBusFeedback.Count == 0)
				{
					return;
				}

				while (true)
				{
					if (MsgBusFeedback.Count <= 0) break;
					OriginFeedbackContetnsT dequeueItem = MsgBusFeedback.Dequeue();
					if (dequeueItem == null) break;

					FbInputChannel ic = FbInputChannel.NotSet;
					switch (dequeueItem.InputChannel)
					{
						case "F":
						case "PC":
							ic = FbInputChannel.PC;
							break;
						case "M":
						case "Android":
						case "iOS":
							ic = FbInputChannel.Mobile;
							break;
					}

					FeedbackDocument newFeedbackDoc = new FeedbackDocument(FeedbackSite, dequeueItem.Title, dequeueItem.Contents, ic, dequeueItem.FbDate);


					FeedbackDocumenList.Add(newFeedbackDoc);
					FeedbackCsvList.Add(ConvertCsvFormat(newFeedbackDoc));

					Progressbar.Increment(1);
				}
			};



			var taskArray = new[]
			{
				new Task(MakeFeedbackEntity),
				new Task(MakeFeedbackEntity),
				new Task(MakeFeedbackEntity)
			};

			foreach (var task in taskArray)
			{
				task.Start();
			}

			Task.Factory.ContinueWhenAll(taskArray, completedTasks =>
			{
				EndDate = System.DateTime.Now;

				string path = fileName;
				string writeStr = String.Empty;
				writeStr = ToCsv(",", FeedbackCsvList);

				// This text is added only once to the file.
				if (!File.Exists(path))
				{
					// Create a file to write to.
					using (StreamWriter sw = File.CreateText(path))
					{
						sw.Write(writeStr);

					}
				}
				else
				{

					// This text is always added, making the file longer over time
					// if it is not deleted.
					using (StreamWriter sw = File.AppendText(path))
					{
						sw.Write(writeStr);
					}
				}

				MakeDitionaryWordCount();
			});
		}
		#endregion

		private string ToCsv(string separator, IEnumerable<object> objectList)
		{
			StringBuilder csvData = new StringBuilder();
			foreach (var obj in objectList)
			{
				csvData.AppendLine(ToCsvFields(separator, obj));
			}
			return csvData.ToString();
		}

		private string ToCsvFields(string separator, object obj)
		{
			var fields = obj.GetType().GetProperties();
			StringBuilder line = new StringBuilder();

			if (obj is string)
			{
				line.Append(obj as string);
				return line.ToString();
			}

			foreach (var field in fields)
			{
				var value = field.GetValue(obj);
				var fieldType = field.GetValue(obj).GetType();

				if (line.Length > 0)
				{
					line.Append(separator);
				}
				if (value == null)
				{
					line.Append("NULL");
				}
				if (value is string)
				{
					line.Append(value as string);
				}
				if (typeof(IEnumerable).IsAssignableFrom(fieldType))
				{
					var objectList = value as IEnumerable;
					StringBuilder row = new StringBuilder();

					foreach (var item in objectList)
					{
						if (row.Length > 0)
						{
							row.Append(separator);
						}
						row.Append(ToCsvFields(separator, item));
					}
					line.Append(row.ToString());
				}
				else
				{
					line.Append(value.ToString());
				}
			}
			return line.ToString();
		}

		private FeedbackCsvT ConvertCsvFormat(FeedbackDocument fd)
		{
			FeedbackCsvT returnValue = new FeedbackCsvT();

			returnValue.Title = fd.Title;
			returnValue.OriginHtmlContents = fd.OriginDocument;

			returnValue.Contents = fd.UnHtmlTempDocument;
			returnValue.AnaysisedContents = fd.AnalysisedText;
			returnValue.ImageCount = fd.ImageCount; // 이미지 갯수
			returnValue.CountNPM = fd.TotalCntNPM;  // 유효 품사 NPM갯수
			returnValue.RateOfValid = (int)fd.RateOfValid;  // 전체 Token대비 유효품사 갯수
			returnValue.InputChannel = (int)fd.InputChannel; // 입력채널 PC : 1, Mobile : 2
			returnValue.SiteId = (int)fd.FeedbackSite;  // 입력도메인 옥션 : 1, 지마켓 : 2
			returnValue.QualityScore = (int)fd.FbQuality;
			returnValue.NgativeScore = fd.NgativeScore;
			returnValue.PositiveScore = fd.PositiveScore;
			returnValue.SensitiveScore = returnValue.PositiveScore - returnValue.NgativeScore;
			returnValue.FbDate = fd.FbDate;

			int imageIdx = 1;
			foreach (string currUrl in fd.ImageList)
			{
				switch (imageIdx)
				{
					case 1:
						returnValue.ImageUrl1 = currUrl;
						break;
					case 2:
						returnValue.ImageUrl2 = currUrl;
						break;
					case 3:
						returnValue.ImageUrl3 = currUrl;
						break;
					case 4:
						returnValue.ImageUrl4 = currUrl;
						break;
					case 5:
						returnValue.ImageUrl5 = currUrl;
						break;
					case 6:
						returnValue.ImageUrl6 = currUrl;
						break;
					case 7:
						returnValue.ImageUrl7 = currUrl;
						break;
					case 8:
						returnValue.ImageUrl8 = currUrl;
						break;
					case 9:
						returnValue.ImageUrl9 = currUrl;
						break;
					case 10:
						returnValue.ImageUrl10 = currUrl;
						break;

				}
				imageIdx++;
			}

			return returnValue;

		}

		private void MakeQueueData()
		{
			if (this.feedbackTable == null || this.feedbackTable.Rows.Count == 0)
			{
				return;
			}
			Progressbar.Minimum = 0;
			Progressbar.Maximum = feedbackTable.Rows.Count;

			MsgBusFeedback.Clear();

			foreach (DataRow curritem in this.feedbackTable.Rows)
			{
				OriginFeedbackContetnsT newOrgFb = new OriginFeedbackContetnsT();
				newOrgFb.Title = curritem["title"].ToString();
				newOrgFb.Contents = curritem["contents"].ToString();
				newOrgFb.InputChannel = curritem["inputchannel"].ToString();
				DateTime fbdate = DateTime.MinValue;

				// 엑셀에서 상품평 작성일자 수립에 실패하면 1년전 상품평으로 넣어준다.
				if (DateTime.TryParse(curritem["fbdate"].ToString(), out fbdate) == false)
				{
					newOrgFb.FbDate = DateTime.Now.AddYears(-1);
				}
				else
				{
					newOrgFb.FbDate = fbdate;
				}

				MsgBusFeedback.Enqueue(newOrgFb);
			}

		}

		public void MakeDitionaryWordCount()
		{
			//UniqueWordTokenCollection
			Dictionary<string, int> dicWord = new Dictionary<string, int>();

			foreach (FeedbackDocument currDoc in FeedbackDocumenList)
			{
				foreach (WordTokenT currtoken in currDoc.UniqueWordTokenCollection)
				{
					if (dicWord.ContainsKey(currtoken.Keyword))
					{
						int updateCnt = dicWord[currtoken.Keyword] + currtoken.RepeatCount;
						dicWord[currtoken.Keyword] = updateCnt;
					}
					else
					{
						dicWord.Add(currtoken.Keyword, currtoken.RepeatCount);
					}
				}
			}

			List<string> keyList = dicWord.Keys.ToList();
			List<KeywordCount> keywordlist = new List<KeywordCount>();
			foreach (string key in keyList)
			{
				int value = dicWord[key];

				keywordlist.Add(new KeywordCount(key, value));
			}


			string path = fileName + "_Dic.csv";
			string writeStr = String.Empty;
			writeStr = ToCsv(",", keywordlist.Where<KeywordCount>(p => p.Count > 1));

			if (File.Exists(path))
			{
				File.Delete(path);
			}

			// Create a file to write to.
			using (StreamWriter sw = File.CreateText(path))
			{
				sw.Write(writeStr);
			}
		}

		/// <summary>
		/// 정크상품평 리스트를 조회한다.
		/// </summary>
		/// <returns></returns>
		public List<FeedbackDocument> GetJunkFeedbackList()
		{
			List<FeedbackDocument> returnValue = new List<FeedbackDocument>();
			var result = this.FeedbackDocumenList.Where<FeedbackDocument>(p => p.FbQuality == FeedbackQuality.Junk);

			if (result != null)
			{
				returnValue = result.ToList<FeedbackDocument>();
			}

			return returnValue;
		}

		#region Properties
		public List<FeedbackDocument> FeedbackDocumenList { get; set; }
		public List<FeedbackCsvT> FeedbackCsvList { get; set; }
		private FbSite FeedbackSite { get; set; }
		#endregion
	}
}

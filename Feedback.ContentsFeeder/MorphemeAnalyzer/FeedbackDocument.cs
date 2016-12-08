﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Feedback.ContentsFeeder.Converter;
using Feedback.ContentsFeeder.BizDac;
using Feedback.ContentsFeeder.AzureWebservice;
using System.Web.Script.Serialization;

namespace Feedback.ContentsFeeder
{
	public class FeedbackDocument
	{
		/// <summary>
		///  생성자
		/// </summary>
		/// <param name="document"></param>
		public FeedbackDocument(FbSite fbSite, string itemno, long orderno, string buyerid, string title, string document, FbInputChannel ic, DateTime fbdate)
		{
			ItemNo = itemno;
			OrderNo = orderno;
			BuyerID = buyerid;

			// 상품평 작성 도메인
			FeedbackSite = fbSite;
			FbQuality = FeedbackQuality.NotSet;
			InputChannel = ic;
			Title = title;
			FbDate = fbdate;

			// 상품평 내용은 제목과 내용의 합으로 구성한다.
			//OriginDocument = String.Format("{0} {1}", title, document);
			OriginDocument = document;

			#region 상품평내에 존재하는 상품평 이미지를 추출한다. 이미지 Url 채워넣기
			ImageList = ContentsParser.GetImgHtmlList(fbSite, document);

			int imageIdx = 1;
			foreach (string currUrl in ImageList)
			{
				switch (imageIdx)
				{
					case 1:
						ImageUrl1 = currUrl;
						break;
					case 2:
						ImageUrl2 = currUrl;
						break;
					case 3:
						ImageUrl3 = currUrl;
						break;
					case 4:
						ImageUrl4 = currUrl;
						break;
					case 5:
						ImageUrl5 = currUrl;
						break;
					case 6:
						ImageUrl6 = currUrl;
						break;
					case 7:
						ImageUrl7 = currUrl;
						break;
					case 8:
						ImageUrl8 = currUrl;
						break;
					case 9:
						ImageUrl9 = currUrl;
						break;
					case 10:
						ImageUrl10 = currUrl;
						break;
				}

				imageIdx++;
			}


			#endregion

			// 상품평내에 분석에 방해되는 Html과 템플릿 문구, 특수문자를 제거한다.
			UnHtmlTempDocument = new ContentsParser().vbUnHtml(fbSite, OriginDocument);

			if (false == String.IsNullOrWhiteSpace(UnHtmlTempDocument))
			{
				try
				{
					// Html등이 제거된 문자열의 형태소 분석을 시작한다.
					AnalysisedSenCollection = MorphemeAnalyzer.DoMorphemeAnalysis(UnHtmlTempDocument);

				}
				catch (Exception ex)
				{
					throw ex;
					//this.FbQuality = FeedbackQuality.NotSet;
					//return;
				}
			}

			UniqueWordTokenCollection = MakeUniqueWordTokenList();

			//this.FbQuality = EvaluateQualityByAzure();

			EvaluateQualityByAzure();

			//Thread worker = new Thread(EvaluateQualityByAzure);
			//worker.Start();
		}

		private List<WordTokenT> MakeUniqueWordTokenList()
		{
			List<WordTokenT> returnValue = new List<WordTokenT>();
			if (AnalysisedSenCollection == null || AnalysisedSenCollection.Count == 0) return returnValue;

			foreach (SentenceT curritem in AnalysisedSenCollection)
			{
				if (curritem.WordToken == null) continue;

				foreach (WordTokenT currword in curritem.WordToken)
				{
					if (currword.WordClass == "N" || currword.WordClass == "P")
					{
						if (returnValue.Contains(currword))
						{
							int wordIdx = returnValue.IndexOf(currword);
							returnValue[wordIdx].RepeatCount = returnValue[wordIdx].RepeatCount + currword.RepeatCount;
						}
						else
						{
							returnValue.Add(currword);
						}
					}
				}
			}

			return returnValue;

		}
		#region Properties

		/// <summary>
		/// 상품번호
		/// </summary>
		public string ItemNo { get; set; }

		/// <summary>
		/// 주문번호
		/// </summary>
		public long OrderNo { get; set; }

		/// <summary>
		/// 구매자ID
		/// </summary>
		public string BuyerID { get; set; }

		/// <summary>
		/// 상품평 제목
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 상품평 컨텐츠내 반복되는 단어의 키워드와 횟수를 문장 구분에 상관없이 총합을 구한다.
		/// </summary>
		public List<WordTokenT> UniqueWordTokenCollection { get; set; }

		/// <summary>
		/// 형태소 분석된 문장, 단어들의 반복체크까지 끝난 결과셋
		/// Collection내의 모든 문자의 합이 하나의 컨텐츠 분석 결과이다.
		/// </summary>
		public List<SentenceT> AnalysisedSenCollection { get; set; }

		/// <summary>
		/// Contents안에 포함된 상품평 이미지 갯수
		/// </summary>
		public List<String> ImageList { get; set; }
		public string ImageUrl1 { get; set; }
		public string ImageUrl2 { get; set; }
		public string ImageUrl3 { get; set; }
		public string ImageUrl4 { get; set; }
		public string ImageUrl5 { get; set; }
		public string ImageUrl6 { get; set; }
		public string ImageUrl7 { get; set; }
		public string ImageUrl8 { get; set; }
		public string ImageUrl9 { get; set; }
		public string ImageUrl10 { get; set; }

		/// <summary>
		/// 상품평 작성일자
		/// </summary>
		public DateTime FbDate { get; set; }

		/// <summary>
		/// 상품평 이미지 갯수
		/// </summary>
		public int ImageCount
		{
			get
			{
				return this.ImageList != null ? ImageList.Count : 0;
			}
		}

		/// <summary>
		/// 상품평에 반복되는 문자의 합계를 계산한다.
		/// </summary>
		public int RepeatSentenceCount
		{
			get
			{
				// 형태소 분석 결과가 없다는것은 분석한 유효한 문장 자체가 없다는 뜻이다.
				// 데이타의 오염을 방지하기 위해 -1로 넘긴다.
				if (AnalysisedSenCollection == null || AnalysisedSenCollection.Count == 0) return 0;

				int repeatCnt = 0;
				foreach (SentenceT curritem in AnalysisedSenCollection)
				{
					repeatCnt += curritem.RepeatCount;
				}

				return repeatCnt;
			}
		}

		public string AnalysisedText
		{
			get
			{
				try
				{
					StringBuilder sb = new StringBuilder();

					foreach (SentenceT curritem in AnalysisedSenCollection)
					{
						foreach (kr.ac.kaist.swrc.jhannanum.comm.Eojeol currEo in curritem.AnalysisTokenList)
						{
							for (int idx = 0; idx < currEo.Morphemes.Length; idx++)
							{
								sb.Append(String.Format("{0}({1}) ", currEo.Morphemes[idx], currEo.Tags[idx]));
							}
						}
					}

					return sb.ToString();
				}
				catch
				{
					return String.Empty;
				}
			}
		}

		/// <summary>
		/// 반복문장을 제외한 상품평내 문장갯수
		/// </summary>
		public int SentenceCount
		{
			get
			{
				// 형태소 분석 결과가 없다는것은 분석한 유효한 문장 자체가 없다는 뜻이다.
				// 데이타의 오염을 방지하기 위해 -1로 넘긴다.
				if (AnalysisedSenCollection == null || AnalysisedSenCollection.Count == 0) return 0;

				return AnalysisedSenCollection.Count;
			}
		}

		/// <summary>
		/// 상품평에 반복되는 단어의 합계를 계산한다.
		/// </summary>
		public int RepeatWordTokenCount
		{
			get
			{
				// 형태소 분석 결과가 없다는것은 분석한 유효한 문장 자체가 없다는 뜻이다.
				// 데이타의 오염을 방지하기 위해 -1로 넘긴다.
				if (AnalysisedSenCollection == null || AnalysisedSenCollection.Count == 0) return 0;

				int repeatCnt = 0;
				foreach (SentenceT curritem in AnalysisedSenCollection)
				{
					foreach (WordTokenT currword in curritem.WordToken)
					{
						repeatCnt += currword.RepeatCount;
					}
				}

				return repeatCnt;
			}
		}

		/// <summary>
		/// 형태소 분석 단위가 되는 총 단어의 갯수
		/// </summary>
		public int TokenCount
		{
			get
			{
				// 형태소 분석 결과가 없다는것은 분석한 유효한 문장 자체가 없다는 뜻이다.
				// 데이타의 오염을 방지하기 위해 -1로 넘긴다.
				if (AnalysisedSenCollection == null || AnalysisedSenCollection.Count == 0) return 0;

				int returnValue = 0;
				foreach (SentenceT curritem in AnalysisedSenCollection)
				{
					returnValue += curritem.PlainTokenList.Count;
				}

				return returnValue;
			}
		}

		/// <summary>
		/// 상품평내 체언 (명사)갯수
		/// </summary>
		public int CountN
		{
			get
			{
				// 형태소 분석 결과가 없다는것은 분석한 유효한 문장 자체가 없다는 뜻이다.
				// 데이타의 오염을 방지하기 위해 -1로 넘긴다.
				if (AnalysisedSenCollection == null || AnalysisedSenCollection.Count == 0) return 0;

				int returnValue = 0;
				foreach (SentenceT curritem in AnalysisedSenCollection)
				{
					returnValue += curritem.CountN;
				}

				return returnValue;
			}
		}

		/// <summary>
		/// 상품평내 용언 (형용사)갯수
		/// </summary>
		public int CountP
		{
			get
			{
				// 형태소 분석 결과가 없다는것은 분석한 유효한 문장 자체가 없다는 뜻이다.
				// 데이타의 오염을 방지하기 위해 -1로 넘긴다.
				if (AnalysisedSenCollection == null || AnalysisedSenCollection.Count == 0) return 0;

				int returnValue = 0;
				foreach (SentenceT curritem in AnalysisedSenCollection)
				{
					returnValue += curritem.CountP;
				}

				return returnValue;
			}
		}

		/// <summary>
		/// 상품평내 관형사/부사 갯수
		/// </summary>
		public int CountM
		{
			get
			{
				// 형태소 분석 결과가 없다는것은 분석한 유효한 문장 자체가 없다는 뜻이다.
				// 데이타의 오염을 방지하기 위해 -1로 넘긴다.
				if (AnalysisedSenCollection == null || AnalysisedSenCollection.Count == 0) return 0;

				int returnValue = 0;
				foreach (SentenceT curritem in AnalysisedSenCollection)
				{
					returnValue += curritem.CountM;
				}

				return returnValue;
			}
		}

		/// <summary>
		/// 유효품사 (N,P,M)의 총 갯수 - 상품평 분석시 나머지 품사는 유효하지 않다고 판단한다.
		/// </summary>
		public int TotalCntNPM
		{
			get
			{
				return CountN + CountP + CountM;
			}
		}

		/// <summary>
		/// tokenizer가 끝난 전체 Token Sum 갯수 대비 유효품사 (N,P,M)의 비율
		/// 이것이 높을수록 완성도가 높은 문자이다.
		/// </summary>
		public double RateOfValid
		{
			get
			{
				return TokenCount > 0 ? ((double)TotalCntNPM / (double)TokenCount) * 100 : 0;
			}
		}

		/// <summary>
		/// Contents 사용자 등록 원본
		/// </summary>
		public string OriginDocument { get; set; }

		/// <summary>
		/// 사용자가 등록한 원본의 템플릿, 불필요한 내용, 특수문자등이 제거된 문장
		/// </summary>
		public string UnHtmlTempDocument { get; set; }

		/// <summary>
		/// 상품평 등록 사이트 (Auction, Gmarket)
		/// </summary>
		public FbSite FeedbackSite { get; set; }



		/// <summary>
		/// PC, Mobile
		/// </summary>
		public FbInputChannel InputChannel { get; set; }

		/// <summary>
		/// 현재 상품평의 품질점수
		/// </summary>
		public FeedbackQuality FbQuality { get; set; }

		/// <summary>
		/// 상품평내 긍정 점수
		/// </summary>
		public double PositiveScore
		{
			get
			{
				double returnValue = 0;
				foreach (WordTokenT currword in UniqueWordTokenCollection)
				{
					if (PositiveWordDictionary.positivewordDic.ContainsKey(currword.Keyword))
					{
						returnValue += PositiveWordDictionary.positivewordDic[currword.Keyword] * ((double)currword.RepeatCount + 1);
					}
				}

				return returnValue;
			}
		}

		/// <summary>
		/// 상품평내 부정 점수
		/// </summary>
		public double NgativeScore
		{
			get
			{
				double returnValue = 0;
				foreach (WordTokenT currword in UniqueWordTokenCollection)
				{
					if (NgativeWordDictionary.ngativewordDic.ContainsKey(currword.Keyword))
					{
						returnValue += NgativeWordDictionary.ngativewordDic[currword.Keyword] * ((double)currword.RepeatCount + 1);
					}
				}

				return returnValue;
			}
		}

		/// <summary>
		/// 상품평내 감성 점수
		/// </summary>
		public double SensitiveScore
		{
			get
			{
				return PositiveScore - NgativeScore;
			}
		}

		/// <summary>
		/// 현재 상품평의 품질을 측정한다.
		/// </summary>
		/// <returns></returns>
		public FeedbackQuality EvaluateQuality()
		{
			FeedbackQuality currQuality = FeedbackQuality.NotSet;
			#region 정크 상품평 기준
			// 쓸데없는 문구를 제거했더니 내용이 없는 상품평이 된 경우에는 T.T (실제 이런경우가 많다)
			if (String.IsNullOrWhiteSpace(UnHtmlTempDocument))
			{
				return FeedbackQuality.Junk;  // 널 쓰레기로 명명한다.
			}

			// 반복적인 문장이 3번이상 나타나면
			if (RepeatSentenceCount >= 3) return FeedbackQuality.Junk;

			// NPM값이 3개 이하이면서 75% 미만이면 Junk
			if (TotalCntNPM <= 2) return FeedbackQuality.Junk;
			if (TotalCntNPM <= 3 && RateOfValid < 75) return FeedbackQuality.Junk;
			if (TotalCntNPM <= 3 && RateOfValid >= 75 && RepeatSentenceCount > 0) return FeedbackQuality.Junk;

			// NPM값이 10개 이하이면서 Valid 비율이 50%이하이면 반복적인 문구로 판단해서 정크
			if (TotalCntNPM <= 10 && RateOfValid <= 50 && ImageCount == 0) return FeedbackQuality.Junk;

			// NPM값이 10개 이하이면서 Valid 비율이 50%이하이면 반복적인 문구로 판단해서 정크
			if (TotalCntNPM <= 10 && RateOfValid <= 34) return FeedbackQuality.Junk;

			#endregion

			if (TotalCntNPM <= 3 && RateOfValid >= 75) return FeedbackQuality.BelowAverage;

			if (TotalCntNPM <= 5 && RateOfValid >= 50) return FeedbackQuality.BelowAverage;

			if (TotalCntNPM > 5 && TotalCntNPM <= 10) currQuality = FeedbackQuality.BelowAverage;

			if (TotalCntNPM > 5 && TotalCntNPM <= 10 && ImageCount > 1) currQuality = FeedbackQuality.Average;

			if (TotalCntNPM > 10 && RateOfValid <= 70) currQuality = FeedbackQuality.BelowAverage;

			if (TotalCntNPM > 15 && RateOfValid > 70 && ImageCount == 0) currQuality = FeedbackQuality.Average;

			if (TotalCntNPM > 15 && RateOfValid > 70 && ImageCount > 0) currQuality = FeedbackQuality.Good;

			if (TotalCntNPM > 25 && RateOfValid <= 70 && ImageCount == 0) currQuality = FeedbackQuality.Average;

			if (TotalCntNPM > 25 && RateOfValid > 70 && ImageCount <= 1) currQuality = FeedbackQuality.Good;

			if (TotalCntNPM > 25 && RateOfValid > 70 && ImageCount > 1) currQuality = FeedbackQuality.Excellent;

			if (TotalCntNPM > 35 && RateOfValid <= 70 && ImageCount <= 1) currQuality = FeedbackQuality.Good;

			if (TotalCntNPM > 35 && RateOfValid > 70 && ImageCount > 1) currQuality = FeedbackQuality.Excellent;

			if (TotalCntNPM > 35 && RateOfValid > 70 && ImageCount > 2) currQuality = FeedbackQuality.MostExcellent;

			if (TotalCntNPM > 45 && RateOfValid > 70 && ImageCount <= 1) currQuality = FeedbackQuality.Excellent;

			if (TotalCntNPM > 45 && RateOfValid > 70 && ImageCount > 1) currQuality = FeedbackQuality.MostExcellent;

			return currQuality;
		}
		#endregion

		/// <summary>
		/// Azure ML Ws를 호출하여 상품평의 품질을 측정한다.
		/// </summary>
		/// <returns></returns>
		public void  EvaluateQualityByAzure()
		{
			FeedbackQuality currQuality = FeedbackQuality.NotSet;
			string siteid = this.FeedbackSite == FbSite.Auction ? "1" : "2";
			string inputchannel = this.InputChannel == FbInputChannel.PC ? "1" : "2";
			string rateofValid = Convert.ToInt64(this.RateOfValid).ToString();
			
			StringTable req = new StringTable()
							{
								ColumnNames = new string[] { "SiteId", "InputChannel", "ImageCount", "CountNPM", "RateOfValid", "QualityScore" },
								Values = new string[,] { { siteid, inputchannel, this.ImageCount.ToString(), this.TotalCntNPM.ToString(), rateofValid, "0" } }
							};

			string evaluationStr = String.Empty;

			Task<string> result = AzureMLWsHelper.InvokeRequestResponseService(req);

		
			


			
			//Action InvokeAzureMLWs=() =>
			//{
			////	Task<string> result = AzureMLWsHelper.InvokeRequestResponseService(req);
			//	//evaluationStr =  result.Result; 

			//	//result = AzureMLWsHelper.InvokeRequestResponseService(req);

				
			//	//evaluationStr = result.Result; 
			//};

			//var taskAzureArray = new[]
			//{
			//	new Task(InvokeAzureMLWs)
			//};

			//foreach (var task in taskAzureArray)
			//{
			//	task.Start();
			//}
	

			//Task.Factory.ContinueWhenAll(taskAzureArray, completedTasks =>
			//{
			//	evaluationStr = result.Result;
			//	if(false == String.IsNullOrEmpty(evaluationStr)) currQuality =  FeedbackQuality.MostExcellent;
			//});

			evaluationStr = result.Result;

			dynamic json = new JavaScriptSerializer().DeserializeObject(evaluationStr);

			try
			{
				object[] values = json["Results"]["output1"]["value"]["Values"][0];
				FeedbackQuality mlQuality = (FeedbackQuality)Convert.ToInt32(values[12]);
				this.FbQuality = mlQuality;
			}
			catch
			{
			}

			//string test = json["test"];
			//if (false == String.IsNullOrEmpty(evaluationStr)) this.FbQuality = FeedbackQuality.MostExcellent;

			//this.FbQuality = FeedbackQuality.MostExcellent;
		}
	}

	/// <summary>
	/// 상품평 등록 사이트 (Auction, Gmarket)
	/// </summary>
	public enum FbSite
	{
		Auction = 1,
		Gmarket = 2
	}

	/// <summary>
	/// 상품평 등록채널 (PC, Mobile)
	/// </summary>
	public enum FbInputChannel
	{
		NotSet = 0,
		PC = 1,
		Mobile = 2

	}

	/// <summary>
	///  상품평 점수
	/// </summary>
	public enum FeedbackQuality
	{
		NotSet = -1,
		Junk = 0,
		BelowAverage = 1,
		Average = 2,
		Good = 3,
		Excellent = 4,
		MostExcellent = 5
	}
}

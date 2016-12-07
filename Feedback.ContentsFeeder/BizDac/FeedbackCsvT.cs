using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.ContentsFeeder.BizDac
{
	public class FeedbackCsvT
	{
		public string Title { get; set; }
		public string OriginHtmlContents { get; set; }
		public string Contents { get; set; }
		public string AnaysisedContents { get; set; }
		public int SiteId { get; set; }
		public int InputChannel { get; set; }
		public int ImageCount { get; set; }
		public int CountNPM { get; set; }
		public int RateOfValid { get; set; }
		public int QualityScore { get; set; }
		public double PositiveScore { get; set; }
		public double NgativeScore { get; set; }
		public double SensitiveScore { get; set; }
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
		public DateTime FbData { get; set; }
	}
}

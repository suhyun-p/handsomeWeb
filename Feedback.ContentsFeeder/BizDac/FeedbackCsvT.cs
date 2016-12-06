using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.ContentsFeeder.BizDac
{
	public class FeedbackCsvT
	{
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
	}
}

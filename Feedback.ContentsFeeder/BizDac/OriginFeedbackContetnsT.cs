using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedback.ContentsFeeder;

namespace Feedback.ContentsFeeder.BizDac
{
	public class OriginFeedbackContetnsT
	{
		public string ItemNo { get; set; }
		public long OrderNo { get; set; }
		public string BuyerID { get; set; }
		public string Title { get; set; }
		public string Contents { get; set; }
		public string InputChannel { get; set; }
		public DateTime FbDate { get; set; }
	}
}

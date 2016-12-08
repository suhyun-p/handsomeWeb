using Feedback.ContentsFeeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandsomeWeb.Models
{
    public class RealFeedbackSearchModel
    {
        public string Title { get; set; }
        public string Contents { get; set; }
        public string ImageUrl { get; set; }

        public FeedbackQuality FbQuality { get; set; }
		public string FbMsg { get; set; }
        public string Sensitive { get; set; }
        public double SensitiveScore { get; set; }
        public string AnalysisedText { get; set; }
        public double PositiveScore { get; set; }
        public double NgativeScore { get; set; }

    }

    public partial class APIResponseT
    {
        public string Message { get; set; }
        public string Success { get; set; }
        public ResultT Result { get; set; }
    }

    public partial class ResultT
    {
        public PRvwExT model { get; set; }
    }

    public partial class PRvwExT
    {
        public int PRvwNo { get; set; }

        public long ContrNo { get; set; }

        public string RcdmGdNo { get; set; }

        public string CustNo { get; set; }

        public string RegId { get; set; }

        public string WrtrNm { get; set; }

        public string PRvwNm { get; set; }

        public string PRvwTxt { get; set; }

        public string PhtRvwYn { get; set; }

        public int ReadCnt { get; set; }

        public DateTime RegDt { get; set; }

        public string SelInfo { get; set; }

        public string PhtUrl { get; set; }

        public string PhtUrl2 { get; set; }

        public string PhtUrl3 { get; set; }

        public string PhtUrl4 { get; set; }

        public string PhtUrl5 { get; set; }

        public string PhtUrl6 { get; set; }

        public string PhtUrl7 { get; set; }

        public string PhtUrl8 { get; set; }


        public string PhtUrl9 { get; set; }


        public string PhtUrl10 { get; set; }


        public string PhtUrlDesc { get; set; }


        public string PhtUrl2Desc { get; set; }


        public string PhtUrl3Desc { get; set; }


        public string PhtUrl4Desc { get; set; }


        public string PhtUrl5Desc { get; set; }


        public string PhtUrl6Desc { get; set; }


        public string PhtUrl7Desc { get; set; }


        public string PhtUrl8Desc { get; set; }


        public string PhtUrl9Desc { get; set; }


        public string PhtUrl10Desc { get; set; }


        public string WinYn { get; set; }


        public string IsPowerReviewer { get; set; }


        public string IsAgreedOut { get; set; }


        public bool IsBlockContent { get; set; }


        public string FromWhere { get; set; }


        public int TotalPoint { get; set; }


        public int DeliverySatisPoint { get; set; }


        public string ReplyComment { get; set; }


        public Nullable<DateTime> ReplyCommentRegDt { get; set; }

        public int ShopGroupCd { get; set; }

        public int RNum { get; set; }

    }
}
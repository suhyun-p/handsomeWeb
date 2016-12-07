using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace HandsomeWeb.Models
{
    public class ImageModel
    {
        //[BsonElement("ImageUrl1")]
        public string ImageUrl1 { get; set; }

        //[BsonElement("ImageUrl2")]
        public string ImageUrl2 { get; set; }

        //[BsonElement("ImageUrl3")]
        public string ImageUrl3 { get; set; }

        //[BsonElement("ImageUrl4")]
        public string ImageUrl4 { get; set; }

        //[BsonElement("ImageUrl5")]
        public string ImageUrl5 { get; set; }

        //[BsonElement("ImageUrl6")]
        public string ImageUrl6 { get; set; }


        //[BsonElement("ImageUrl7")]
        public string ImageUrl7 { get; set; }

        //[BsonElement("ImageUrl8")]
        public string ImageUrl8 { get; set; }

        //[BsonElement("ImageUrl9")]
        public string ImageUrl9 { get; set; }

        //[BsonElement("ImageUrl10")]
        public string ImageUrl10 { get; set; }
    }

    public class FeedbackModel
    {
        //[BsonId(IdGenerator = typeof(CombGuidGenerator))]
        //public Guid No { get; set; }

        public ObjectId _id { get; set; }

		//[BsonElement("ItemNo")]
		public string ItemNo { get; set; }

		//[BsonElement("BuyerID")]
		public string BuyerID { get; set; }

		//[BsonElement("OrderNo")]
		public long OrderNo { get; set; }

        //[BsonElement("Title")]
        public string Title { get; set; }

        //[BsonElement("OriginHtmlContents")]
        public string OriginHtmlContents { get; set; }

        //[BsonElement("Contents")]
        public string Contents { get; set; }

        //[BsonElement("AnaysisedContents")]
        public string AnaysisedContents { get; set; }

        //[BsonElement("SiteId")]
        public int SiteId { get; set; }

        //[BsonElement("InputChannel")]
        public int InputChannel { get; set; }

        //[BsonElement("ImageCount")]
        public int ImageCount { get; set; }

        //[BsonElement("CountNPM")]
        public int CountNPM { get; set; }

        //[BsonElement("RateOfValid")]
        public int RateOfValid { get; set; }

        //[BsonElement("QualityScore")]
        public int QualityScore { get; set; }

        //[BsonElement("PositiveScore")]
        public double PositiveScore { get; set; }

        //[BsonElement("NgativeScore")]
        public double NgativeScore { get; set; }

        //[BsonElement("SensitiveScore")]
        public double SensitiveScore { get; set; }

        //[BsonElement("ImageUrl")]
        public ImageModel ImageUrl { get; set; }

        //[BsonElement("FbDate")]
        public string FbDate { get; set; }
    }
}
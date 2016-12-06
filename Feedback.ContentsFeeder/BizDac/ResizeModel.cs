using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.ContentsFeeder.BizDac
{
	public class ResizeModel
	{
		public ResizeModel(int quality, int resizeWidth, int crop, Boolean metaRemoveFlag, int thumbnailWidth = 0, int thumbnailQuality = 0 )
		{
			this.Quality = quality;
			this.ResizeWidth = resizeWidth;
			this.Crop = crop;
			this.MetaRemoveFlag = metaRemoveFlag;
			this.ThumbnailWidth = thumbnailWidth;
			this.ThumbnailQuality = thumbnailQuality;
			this.FileUrl = "";
		}

		public ResizeModel(){
			Quality = 0;
			ResizeWidth = 0;
			Crop = 0;
			MetaRemoveFlag = false;
			ThumbnailWidth = 0;
			ThumbnailQuality = 0;
			this.FileUrl = "";
		}

		public int Quality { get; set; }
		public int ResizeWidth { get; set; }
		public int ThumbnailWidth { get; set; }
		public int ThumbnailQuality { get; set; }
		public int Crop { get; set; }
		public Boolean MetaRemoveFlag { get; set; }
		public String FilePath { get; set; }
		public String FileName { get; set; }
		public String FileUrl { get; set; }
	}
	public class ResizeLogModel
	{
		public String ImageColumn { get; set; }
		public String ImageUrl { get; set; }
		public String Path { get; set; }
		public int OrgImageSize { get; set; }
		public int ChgImageSize { get; set; }
		public String UrlErrorYn { get; set; }
		public String PathErrorYn { get; set; }
		public String ErrorCd { get; set; }
		public String ErrorContent { get; set; }
	}

	public class ResizeLogModelGMKT : ResizeLogModel
	{
		public long PRvwNo { get; set; }
		public String RcdmGdNo { get; set; }
		
		public void CopyResizerLog(ResizeLogModel model){
			this.OrgImageSize = model.OrgImageSize;
			this.ChgImageSize = model.ChgImageSize;
			this.ErrorCd = model.ErrorCd;
			this.ErrorContent = model.ErrorContent;
		}
	}

	public class ResizeLogModelIAC : ResizeLogModel
	{
		public long OrderNo { get; set; }
		public String ItemNo { get; set; }

		public void CopyResizerLog(ResizeLogModel model)
		{
			this.OrgImageSize = model.OrgImageSize;
			this.ChgImageSize = model.ChgImageSize;
			this.ErrorCd = model.ErrorCd;
			this.ErrorContent = model.ErrorContent;
		}
	}

	public class ResizerErrorCode
	{
		public static String NoError = "0000";
		public static String UrlError = "0100";
		public static String UrlParsingError = "0200";

		public static String ResizerError = "0300"; // Resize중 에러.
		public static String ResizerWriteError = "0301"; // Resize Write중 에러.
		public static String ResizerOriginWriteError = "0302"; // Resize Origin이미지 Write중 에러.
		public static String FileNotFuound = "0303";
		public static String ThumbnailError = "0304";
		
		public static String ResizeSuccess = "0900"; 
	}
}

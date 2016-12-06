using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Configuration;

using Feedback.ContentsFeeder.BizDac;

namespace Feedback.ContentsFeeder.Converter
{
	public class IacConverter
	{
		private string iacImageUrl = Convert.ToString(ConfigurationManager.AppSettings["IacImageUrl"]);
		private string iacImageUrl1 = Convert.ToString(ConfigurationManager.AppSettings["IacImageUrl1"]);
		private string iacImageUrl2 = Convert.ToString(ConfigurationManager.AppSettings["IacImageUrl2"]);

		public string GetFullPathImgUrl(string url)
		{
			string result = url;

			if (url.ToLower().IndexOf("http") < 0)
			{
				return string.Empty;
			}

			if (result.ToLower().IndexOf(iacImageUrl1) >= 0 ||
				result.ToLower().IndexOf(iacImageUrl2) >= 0)
				return result;
			else
				return string.Empty;
		}

		public List<string> GetImgHtmlList(string content)
		{
			List<string> imgList = new List<string>();
			RegexOptions rxOpt = RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase;

			//주석 제거
			content = new Regex("<!--[^>](.*?)-->", RegexOptions.IgnoreCase | RegexOptions.Multiline).Replace(content, string.Empty);
			Regex regex = new Regex("(?i)<img[^>]*src=[\"']?([^>\"']+)[\"']?[^>]*>", rxOpt);

			foreach (Match rm in regex.Matches(content))
			{
				Regex regexOnclick = new Regex("(?i)(http|https)[a-zA-Z0-9_.\\-%&=?!:;@\"'/]*(?i)(.gif|.jpg|.png|.jpeg|.bmp)", rxOpt);

				foreach (Match rm2 in regexOnclick.Matches(rm.Groups[0].ToString()))
				{
					imgList.Add(rm2.Groups[0].ToString());
				}
			}

			return imgList;
		}
				

		/// <summary>
		/// html 태그 제거
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public string vbUnHtml(string content)
		{
			content = this.vbRecover(content);
			content = content.Replace("제목 없음", string.Empty);
			content = content.Replace("<br/><font size=2 color=\"#FFFFFF\">[@Gmarket Android Application]</font><br/><br/>", string.Empty);
			content = content.Replace("<br/><font size=2 color='#FFFFFF'>[@Gmarket Android Application]</font><br/><br/>", string.Empty);

			content = content.Replace("<br/><font size=2 color=\"#FFFFFF\">[@Gmarket iPhone Application]</font><br/><br/>", string.Empty);
			content = content.Replace("<br/><font size=2 color='#FFFFFF'>[@Gmarket iPhone Application]</font><br/><br/>", string.Empty);

			content = content.Replace("<br><br><br><font size=2 color=\"#FFFFFF\"><b>[지마켓 아이폰 앱에서 작성]</b></font><br>", string.Empty);
			content = content.Replace("<br><br><br><font size=2 color=\"#FFFFFF\"><b>[지마켓 안드로이드 앱에서 작성]</b></font><br>", string.Empty);
			content = content.Replace("<br><br>지마켓 안드로이드 앱으로 작성", string.Empty);

			content = content.Replace("[@Gmarket Android Application]", string.Empty);
			content = content.Replace("[G마켓 안드로이드 앱에서 작성]", string.Empty);
			content = content.Replace("[@Gmarket iPhone Application]", string.Empty);
			content = content.Replace("[G마켓 아이폰 앱에서 작성]", string.Empty);

			content = Regex.Replace(content, @"(<br>|<br />|<br/>|</ br>|</br>|\r\n)", "\n", RegexOptions.IgnoreCase);
			content = content.Replace("&nbsp;", " ");
			content = content.Replace("\"", "'");
			content = content.Replace("\\", string.Empty);
			content = content.Replace("Javascript", string.Empty);
			content = content.Replace("script", string.Empty);
			content = content.Replace("expression", string.Empty);
			content = content.Replace("iframe", string.Empty);
			content = content.Replace("/*", string.Empty);
			content = content.Replace("*/", string.Empty);
			content = content.Replace("document.cookie", string.Empty);
			content = content.Replace("%00", string.Empty);
			content = content.Replace("0x", string.Empty);
			content = content.Replace("\\u", string.Empty);

			content = this.vbLimitText(content);
			//content = vbReplaceDuplStr(content, "\n\n", "\n");
			content = Regex.Replace(content, @"[\n]+", "\n", RegexOptions.IgnoreCase);

			return content;
		}

		/// <summary>
		/// 중첩 문자열 변환
		/// </summary>
		/// <param name="str"></param>
		/// <param name="targetStr"></param>
		/// <param name="replaceStr"></param>
		/// <returns></returns>
		public string vbReplaceDuplStr(string str, string targetStr, string replaceStr)
		{
			while (str.IndexOf(targetStr) > -1)
			{
				str = str.Replace(targetStr, replaceStr);
			}

			return str;
		}

		public string vbLimitText(string content, int length = 110)
		{
			Regex re = new Regex("<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			string h = re.Replace(content, string.Empty);
			if (h.Length < length) length = h.Length;

			return h;
		}

		protected string vbRecover(string content)
		{
			content = content.Replace(@"&amp;nbsp;", @"&nbsp;");
			content = content.Replace(@"&lt;DIV", @"<DIV");
			content = content.Replace(@"&lt;/DIV", @"</DIV");
			content = content.Replace(@"&lt;div", @"<div");
			content = content.Replace(@"&lt;/div", @"</div");
			content = content.Replace(@"`", @"'");
			content = content.Replace(@"&amp;lt;", "&lt;");
			content = content.Replace(@"&amp;gt;", "&gt;");
			return content;
		}

		/// <summary>
		/// PRvwExT 변환 후 리턴
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public PRvwExT ConvertToPRvwExT(PRvwExT data)
		{
			if (data != null)
			{	
				data.PhtUrl = string.IsNullOrEmpty(data.PhtUrl) ? data.PhtUrl : GetFullPathImgUrl(data.PhtUrl);
				data.PhtUrl2 = string.IsNullOrEmpty(data.PhtUrl2) ? data.PhtUrl2 : GetFullPathImgUrl(data.PhtUrl2);
				data.PhtUrl3 = string.IsNullOrEmpty(data.PhtUrl3) ? data.PhtUrl3 : GetFullPathImgUrl(data.PhtUrl3);
				data.PhtUrl4 = string.IsNullOrEmpty(data.PhtUrl4) ? data.PhtUrl4 : GetFullPathImgUrl(data.PhtUrl4);
				data.PhtUrl5 = string.IsNullOrEmpty(data.PhtUrl5) ? data.PhtUrl5 : GetFullPathImgUrl(data.PhtUrl5);
				data.PhtUrl6 = string.IsNullOrEmpty(data.PhtUrl6) ? data.PhtUrl6 : GetFullPathImgUrl(data.PhtUrl6);
				data.PhtUrl7 = string.IsNullOrEmpty(data.PhtUrl7) ? data.PhtUrl7 : GetFullPathImgUrl(data.PhtUrl7);
				data.PhtUrl8 = string.IsNullOrEmpty(data.PhtUrl8) ? data.PhtUrl8 : GetFullPathImgUrl(data.PhtUrl8);
				data.PhtUrl9 = string.IsNullOrEmpty(data.PhtUrl9) ? data.PhtUrl9 : GetFullPathImgUrl(data.PhtUrl9);
				data.PhtUrl10 = string.IsNullOrEmpty(data.PhtUrl10) ? data.PhtUrl10 : GetFullPathImgUrl(data.PhtUrl10);
				data.PhtUrl_Thum = string.IsNullOrEmpty(data.PhtUrl_Thum) ? data.PhtUrl10 : GetFullPathImgUrl(data.PhtUrl_Thum);

				List<string> imgList = GetImgHtmlList(data.PRvwTxt);

				string IMG_TAG = "<img src=\"{0}\" />";
				StringBuilder sbImg = new StringBuilder();

				if (imgList != null)
				{
					foreach (string img in imgList)
					{
						if (string.IsNullOrEmpty(data.PhtUrl))
						{
							data.PhtUrl = GetFullPathImgUrl(img);
						}
						else if (string.IsNullOrEmpty(data.PhtUrl2))
						{
							data.PhtUrl2 = GetFullPathImgUrl(img);
						}
						else if (string.IsNullOrEmpty(data.PhtUrl3))
						{
							data.PhtUrl3 = GetFullPathImgUrl(img);
						}
						else if (string.IsNullOrEmpty(data.PhtUrl4))
						{
							data.PhtUrl4 = GetFullPathImgUrl(img);
						}
						else if (string.IsNullOrEmpty(data.PhtUrl5))
						{
							data.PhtUrl5 = GetFullPathImgUrl(img);
						}
						else if (string.IsNullOrEmpty(data.PhtUrl6))
						{
							data.PhtUrl6 = GetFullPathImgUrl(img);
						}
						else if (string.IsNullOrEmpty(data.PhtUrl7))
						{
							data.PhtUrl7 = GetFullPathImgUrl(img);
						}
						else if (string.IsNullOrEmpty(data.PhtUrl8))
						{
							data.PhtUrl8 = GetFullPathImgUrl(img);
						}
						else if (string.IsNullOrEmpty(data.PhtUrl9))
						{
							data.PhtUrl9 = GetFullPathImgUrl(img);
						}
						else if (string.IsNullOrEmpty(data.PhtUrl10))
						{
							data.PhtUrl10 = GetFullPathImgUrl(img);
						}
						else
						{
							break;
						}

						sbImg.AppendFormat(IMG_TAG, img);
					}
				}

				string pureContents = vbUnHtml(data.PRvwTxt);

				data.PRvwTxt = sbImg.ToString() + pureContents;
				data.CONTENTS_V2 = pureContents;
			}

			return data;
		}
	}
}

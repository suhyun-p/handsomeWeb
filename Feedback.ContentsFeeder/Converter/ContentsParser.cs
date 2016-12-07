using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Feedback.ContentsFeeder.BizDac;

namespace Feedback.ContentsFeeder.Converter
{
	public class ContentsParser
	{
		private string gmktImageUrl = Convert.ToString(ConfigurationManager.AppSettings["GmktImageUrl"]);

		#region GMKT Convertor


		public string GetFullPathImgUrl(string url)
		{
			string result = url;

			if (url.ToLower().IndexOf("http") < 0)
			{
				result = gmktImageUrl + url;
			}
			return result;
		}

		public static List<string> GetImgHtmlList(FbSite fbsite, string content)
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
		public string vbUnHtml(FbSite fbSite, string content)
		{
			content = this.vbRecover(content);

			if (fbSite == FbSite.Auction)
			{
				foreach (string curritem in AuctionSpamText.SpamTextList)
				{
					content = content.Replace(curritem, String.Empty);
				}
			}
			else
			{
				foreach (string curritem in GmarketSpamText.SpamTextList)
				{
					content = content.Replace(curritem, String.Empty);
				}
			}
			content = content.Replace("\n", ".");
			content = Regex.Replace(content, @"(<br>|<br />|<br/>|</ br>|</br>|\r\n)", " ", RegexOptions.IgnoreCase);
			content = content.Replace("&nbsp;", " ");
			content = content.Replace("\"", "'");
			content = this.vbLimitText(content);

			content = Regex.Replace(content, @"[\n]+", "\n", RegexOptions.IgnoreCase);

			content = 특수문자제거(content);

			return content.Trim();
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

		private string 특수문자제거(string content)
		{
			return Regex.Replace(content, @"[^a-zA-Z가-힣. ]", "", RegexOptions.Singleline);
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

		
		#endregion
	}
}

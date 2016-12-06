using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedback.ContentsFeeder.BizDac;
using System.Reflection;
using System.Web;
using System.Net;

namespace Feedback.ContentsFeeder.Converter
{
	public class AuctionSpamText
	{
		public static List<String> SpamTextList = new List<String>();
		static string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		static AuctionSpamText()
		{
			string txtLocation = HttpContext.Current.Server.MapPath(@"~/bin/data/auctionspamtext.txt");
			//SpamTextList = File.ReadAllLines(".\\data\\auctionspamtext.txt").ToList<String>();
			SpamTextList = File.ReadAllLines(txtLocation).ToList<String>();
		}
	}

	public class GmarketSpamText
	{
		public static List<String> SpamTextList = new List<String>();
		static string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		static GmarketSpamText()
		{
			string txtLocation = HttpContext.Current.Server.MapPath(@"~/bin/data/gmarketspamtext.txt");
			//SpamTextList = File.ReadAllLines(".\\data\\gmarketspamtext.txt").ToList<String>();
			SpamTextList = File.ReadAllLines(txtLocation).ToList<String>();
		}
	}

	public class NgativeWordDictionary
	{
		public static Dictionary<string, double> ngativewordDic = new Dictionary<string, double>();
		static string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		static NgativeWordDictionary()
		{
			
			string txtLocation = HttpContext.Current.Server.MapPath(@"~/bin/data/ngativeWord.txt");
			List<String> NgativeTextList = new List<String>();
			//NgativeTextList = File.ReadAllLines(".\\data\\ngativeWord.txt").ToList<String>();
			NgativeTextList = File.ReadAllLines(txtLocation).ToList<String>();

			foreach (string curritem in NgativeTextList)
			{
				try
				{
					string[] token = curritem.Split(",");

					if (token == null || token.Length != 2) continue;


					if (ngativewordDic.ContainsKey(token[0]) == false)
					{
						ngativewordDic.Add(token[0], Double.Parse(token[1]));
					}
				}
				catch(Exception ex) 
				{
					throw ex;
				}
			}
		}
	}

	public class PositiveWordDictionary
	{
		public static Dictionary<string, double> positivewordDic = new Dictionary<string, double>();
		static string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		static PositiveWordDictionary()
		{
			string txtLocation = HttpContext.Current.Server.MapPath(@"~/bin/data/positiveWord.txt");
			List<String> PositiveTextList = new List<String>();
			PositiveTextList = File.ReadAllLines(txtLocation).ToList<String>();
			foreach (string curritem in PositiveTextList)
			{
				try
				{
					string[] token = curritem.Split(",");

					if (token == null || token.Length != 2) continue;

					if (positivewordDic.ContainsKey(token[0]) == false)
					{
						positivewordDic.Add(token[0], Double.Parse(token[1]));
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}
	}
}

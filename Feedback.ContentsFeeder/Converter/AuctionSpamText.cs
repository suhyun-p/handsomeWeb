using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedback.ContentsFeeder.BizDac;


namespace Feedback.ContentsFeeder.Converter
{
	public class AuctionSpamText
	{
		public static List<String> SpamTextList = new List<String>();

		static AuctionSpamText()
		{
			SpamTextList = File.ReadAllLines("auctionspamtext.txt").ToList<String>();
		}
	}

	public class GmarketSpamText
	{
		public static List<String> SpamTextList = new List<String>();

		static GmarketSpamText()
		{
			SpamTextList = File.ReadAllLines("gmarketspamtext.txt").ToList<String>();
		}
	}

	public class NgativeWordDictionary
	{
		public static Dictionary<string, double> ngativewordDic = new Dictionary<string, double>();

		static NgativeWordDictionary()
		{
			List<String> NgativeTextList = new List<String>();
			NgativeTextList = File.ReadAllLines("ngativeWord.txt").ToList<String>();

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

		static PositiveWordDictionary()
		{
			List<String> PositiveTextList = new List<String>();
			PositiveTextList = File.ReadAllLines("positiveWord.txt").ToList<String>();

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

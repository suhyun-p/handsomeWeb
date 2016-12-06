using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.ContentsFeeder.BizDac
{
	public class WordTokenT
	{
		public string Keyword { get; set; }
		public string WordClass { get; set; }
		public int RepeatCount { get; set; }

		#region Constructor
		public WordTokenT(string keyword, string wordclass)
		{
			this.Keyword = keyword;
			this.WordClass = wordclass;
		}

		#endregion
		#region Equals 연산자 override
		public override bool Equals(System.Object obj)
		{
			if (obj == null)
			{
				return false;
			}

			WordTokenT p = obj as WordTokenT;
			if ((System.Object)p == null)
			{
				return false;
			}

			// Plain text가 같으면 동일한 Entitiy로 인정한다.
			return Keyword.CompareTo(p.Keyword) != 0 ? false : true;
		}
		#endregion
	}

	public class KeywordCount
	{
		public string Keyword { get; set; }
		public int Count { get; set; }

		#region Constructor
		public KeywordCount(string keyword, int cnt)
		{
			this.Keyword = keyword;
			this.Count = cnt;
		}

		#endregion
		
		#region Equals 연산자 override
		public override bool Equals(System.Object obj)
		{
			if (obj == null)
			{
				return false;
			}

			WordTokenT p = obj as WordTokenT;
			if ((System.Object)p == null)
			{
				return false;
			}

			// Plain text가 같으면 동일한 Entitiy로 인정한다.
			return Keyword.CompareTo(p.Keyword) != 0 ? false : true;
		}
		#endregion
	}
}

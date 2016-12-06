using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kr.ac.kaist.swrc.jhannanum.comm;

namespace Feedback.ContentsFeeder.BizDac
{
	public class SentenceT
	{
		#region Constructor
		public SentenceT(string[] plainTokenList, Eojeol[] analysisTokenList)
		{
			if (plainTokenList == null || analysisTokenList == null) return;

			PlainTokenList = plainTokenList.ToList<String>();
			AnalysisTokenList = analysisTokenList.ToList<Eojeol>();
			WordToken = new List<WordTokenT>();
			MakeWordToken();
		}
		#endregion

		

		#region MakeWordToken
		private void MakeWordToken()
		{
			if (this.AnalysisTokenList.Count == 0) return;

			for (int idx = 0; idx < AnalysisTokenList.Count; idx++)
			{
				List<String> morphemes = AnalysisTokenList[idx].Morphemes.ToList<String>();
				List<String> tags = AnalysisTokenList[idx].Tags.ToList<String>();

				for (int tIdx = 0; tIdx < morphemes.Count; tIdx++)
				{
					/*
						N : 명사 (체언)
						P : 형용사 / 동사 (용언)
						M : 관형사 / 부사
						I : 감탄사
						J : 조사
						E : 어말어미
						X : 접미사 / 접두사 / 어근
						S : 부호
						F : 한글이외
					*/
					if (tags[tIdx] != "N" && tags[tIdx] != "P" && tags[tIdx] != "M")
					{
						continue;
					}

					WordTokenT currWord = new WordTokenT(morphemes[tIdx], tags[tIdx]);

					if (WordToken.Contains(currWord))
					{
						int curridx = WordToken.IndexOf(currWord);
						WordToken[curridx].RepeatCount++;
					}
					else
					{
						WordToken.Add(currWord);
					}
				}

			}
		}
		#endregion

		#region Properties
		public List<String> PlainTokenList { get; set; }
		public List<Eojeol> AnalysisTokenList { get; set; }
		public int RepeatCount { get; set; }
		public List<WordTokenT> WordToken { get; set; }

		public int CountN
		{
			get
			{
				if (WordToken.Count == 0) return 0;

				var result = WordToken.Where<WordTokenT>(p => p.WordClass == "N");
				return result.Count<WordTokenT>();
			}
		}

		public int CountP
		{
			get
			{
				if (WordToken.Count == 0) return 0;

				var result = WordToken.Where<WordTokenT>(p => p.WordClass == "P");
				return result.Count<WordTokenT>();
			}
		}

		public int CountM
		{
			get
			{
				if (WordToken.Count == 0) return 0;

				var result = WordToken.Where<WordTokenT>(p => p.WordClass == "M");
				return result.Count<WordTokenT>();
			}
		}
		#endregion

		#region Equals 연산자 override
		public override bool Equals(System.Object obj)
		{
			if (obj == null)
			{
				return false;
			}

			SentenceT p = obj as SentenceT;
			if ((System.Object)p == null)
			{
				return false;
			}

			// 플레인텍스트안의 갯수와 순서, 내용이 동일해야 동일한 Entity로 인정
			if (PlainTokenList.Count != p.PlainTokenList.Count) return false;
			for (int idx = 0; idx < PlainTokenList.Count; idx++)
			{
				if (PlainTokenList[idx].CompareTo(p.PlainTokenList[idx]) != 0) return false;
			}

			return true;
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feedback.ContentsFeeder.BizDac;
using kr.ac.kaist.swrc.jhannanum.comm;
using kr.ac.kaist.swrc.jhannanum.hannanum;

namespace Feedback.ContentsFeeder
{
	public class MorphemeAnalyzer
	{
		public static List<SentenceT> DoMorphemeAnalysis(string document)
		{
			List<SentenceT> returnValue = new List<SentenceT>();
			StringBuilder sb = new StringBuilder();

			//Workflow workflow = WorkflowFactory.getPredefinedWorkflow(WorkflowFactory.WORKFLOW_NOUN_EXTRACTOR);  // 품사태그가 너무 구체적
			//Workflow workflow = WorkflowFactory.getPredefinedWorkflow(WorkflowFactory.WORKFLOW_MORPH_ANALYZER); // 데이타 안나옴
			//Workflow workflow = WorkflowFactory.getPredefinedWorkflow(WorkflowFactory.WORKFLOW_HMM_POS_TAGGER);  // 품사태그가 너무 구체적
			//Workflow workflow = WorkflowFactory.getPredefinedWorkflow(WorkflowFactory.WORKFLOW_POS_SIMPLE_22);
			WorkflowForWeb workflow = WorkflowFactoryForWeb.getPredefinedWorkflow(WorkflowFactoryForWeb.WORKFLOW_POS_SIMPLE_09);  
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

			try
			{
				/* Activate the work flow in the thread mode */
				workflow.activateWorkflow(true);

				/* Analysis using the work flow */
				workflow.analyze(document);

				LinkedList<Sentence> resultList = workflow.getResultOfDocument(new Sentence(0, 0, false));
				foreach (Sentence s in resultList)
				{
					SentenceT curritem = new SentenceT(s.PlainEojeols, s.Eojeols);
					

					if (returnValue.Contains(curritem))
					{
						int idx = returnValue.IndexOf(curritem);
						returnValue[idx].RepeatCount++;
					}
					else
					{
						returnValue.Add(curritem);
					}
				}

				workflow.close();

			}
			catch (Exception e)
			{
				throw e;
			}

			/* Shutdown the work flow */
			//workflow.close();

			return returnValue;
		}
	}
}

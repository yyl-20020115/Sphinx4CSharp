using System;

using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class Result : java.lang.Object
	{
		
		[LineNumberTable(new byte[]
		{
			159,
			40,
			162,
			103,
			99,
			134,
			104,
			137
		})]
		
		public virtual List getTimedBestResult(bool withFillers)
		{
			Token bestToken = this.getBestToken();
			if (bestToken == null)
			{
				return Collections.emptyList();
			}
			if (this.wordTokenFirst)
			{
				return this.getTimedWordPath(bestToken, withFillers);
			}
			return this.getTimedWordTokenLastPath(bestToken, withFillers);
		}

		public virtual bool toCreateLattice()
		{
			return this.toCreateLattice;
		}

		[LineNumberTable(new byte[]
		{
			160,
			246,
			103,
			99,
			134
		})]
		
		public virtual string getBestResultNoFiller()
		{
			Token bestToken = this.getBestToken();
			if (bestToken == null)
			{
				return "";
			}
			return bestToken.getWordPathNoFiller();
		}

		public virtual bool isFinal()
		{
			return this.isFinal;
		}

		public virtual void setReferenceText(string @ref)
		{
			this.reference = @ref;
		}

		[LineNumberTable(new byte[]
		{
			160,
			97,
			135,
			99,
			167
		})]
		
		public virtual Token getBestToken()
		{
			Token token = this.getBestFinalToken();
			if (token == null)
			{
				token = this.getBestActiveToken();
			}
			return token;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			120,
			105,
			104,
			103,
			103,
			103,
			103,
			103,
			103,
			107
		})]
		
		public Result(ActiveList activeList, List resultList, long collectTime, bool isFinal, bool wordTokenFirst, bool toCreateLattice)
		{
			this.activeList = activeList;
			this.resultList = resultList;
			this.currentCollectTime = collectTime;
			this.isFinal = isFinal;
			this.toCreateLattice = toCreateLattice;
			this.wordTokenFirst = wordTokenFirst;
			this.logMath = LogMath.getLogMath();
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			125,
			137,
			111,
			103
		})]
		
		public Result(AlternateHypothesisManager alternateHypothesisManager, ActiveList activeList, List resultList, long collectTime, bool isFinal, bool wordTokenFirst, bool toCreateLattice) : this(activeList, resultList, collectTime, isFinal, wordTokenFirst, toCreateLattice)
		{
			this.alternateHypothesisManager = alternateHypothesisManager;
		}

		public virtual string getReferenceText()
		{
			return this.reference;
		}

		[LineNumberTable(new byte[]
		{
			161,
			120,
			103,
			99,
			134
		})]
		
		public override string toString()
		{
			Token bestToken = this.getBestToken();
			if (bestToken == null)
			{
				return "";
			}
			return bestToken.getWordPath();
		}

		[LineNumberTable(new byte[]
		{
			160,
			79,
			98,
			127,
			1,
			113,
			130,
			98
		})]
		
		public virtual Token getBestFinalToken()
		{
			Token token = null;
			Iterator iterator = this.resultList.iterator();
			while (iterator.hasNext())
			{
				Token token2 = (Token)iterator.next();
				if (token == null || token2.getScore() > token.getScore())
				{
					token = token2;
				}
			}
			return token;
		}

		public virtual bool getWordTokenFirst()
		{
			return this.wordTokenFirst;
		}

		public virtual AlternateHypothesisManager getAlternateHypothesisManager()
		{
			return this.alternateHypothesisManager;
		}

		[LineNumberTable(new byte[]
		{
			160,
			112,
			98,
			104,
			127,
			1,
			113,
			130,
			130
		})]
		
		public virtual Token getBestActiveToken()
		{
			Token token = null;
			if (this.activeList != null)
			{
				Iterator iterator = this.activeList.iterator();
				while (iterator.hasNext())
				{
					Token token2 = (Token)iterator.next();
					if (token == null || token2.getScore() > token.getScore())
					{
						token = token2;
					}
				}
			}
			return token;
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			150,
			102,
			104,
			127,
			1,
			110,
			136,
			98
		})]
		
		public virtual List findPartialMatchingTokens(string text)
		{
			ArrayList arrayList = new ArrayList();
			text = java.lang.String.instancehelper_trim(text);
			Iterator iterator = this.activeList.iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				if (java.lang.String.instancehelper_startsWith(text, token.getWordPathNoFiller()))
				{
					arrayList.add(token);
				}
			}
			return arrayList;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			34,
			66,
			99,
			134,
			134,
			101,
			135,
			104,
			103,
			107,
			115,
			155,
			135,
			173,
			102
		})]
		
		private List getTimedWordPath(Token predecessor, bool flag)
		{
			long num = -1L;
			ArrayList arrayList = new ArrayList();
			while (predecessor != null)
			{
				if (num < 0L)
				{
					num = predecessor.getCollectTime();
				}
				if (predecessor.isWord())
				{
					Word word = predecessor.getWord();
					if (flag || !word.isFiller())
					{
						TimeFrame.__<clinit>();
						TimeFrame timeFrame = new TimeFrame(predecessor.getCollectTime(), num);
						arrayList.add(new WordResult(word, timeFrame, (double)predecessor.getScore(), (double)1f));
					}
					num = predecessor.getCollectTime();
				}
				predecessor = predecessor.getPredecessor();
			}
			Collections.reverse(arrayList);
			return arrayList;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			27,
			162,
			99,
			99,
			130,
			103,
			102,
			104,
			104,
			107,
			105,
			188,
			103,
			135,
			103,
			173,
			103
		})]
		
		private List getTimedWordTokenLastPath(Token predecessor, bool flag)
		{
			long num = -1L;
			long start = -1L;
			Word word = null;
			ArrayList arrayList = new ArrayList();
			while (predecessor != null)
			{
				if (predecessor.isWord())
				{
					if (word != null && num >= 0L && (flag || !word.isFiller()))
					{
						TimeFrame timeFrame = new TimeFrame(start, num);
						arrayList.add(new WordResult(word, timeFrame, (double)predecessor.getScore(), (double)1f));
					}
					num = predecessor.getCollectTime();
					word = predecessor.getWord();
				}
				start = predecessor.getCollectTime();
				predecessor = predecessor.getPredecessor();
			}
			Collections.reverse(arrayList);
			return arrayList;
		}

		public virtual LogMath getLogMath()
		{
			return this.logMath;
		}

		public virtual ActiveList getActiveTokens()
		{
			return this.activeList;
		}

		
		public virtual List getResultTokens()
		{
			return this.resultList;
		}

		public virtual long getCollectTime()
		{
			return this.currentCollectTime;
		}

		[LineNumberTable(new byte[]
		{
			160,
			132,
			104,
			127,
			1,
			110,
			130,
			98
		})]
		
		public virtual Token findToken(string text)
		{
			text = java.lang.String.instancehelper_trim(text);
			Iterator iterator = this.resultList.iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				if (java.lang.String.instancehelper_equals(text, token.getWordPathNoFiller()))
				{
					return token;
				}
			}
			return null;
		}

		[LineNumberTable(new byte[]
		{
			160,
			169,
			104,
			98,
			123,
			113,
			130,
			98
		})]
		
		public virtual Token getBestActiveParitalMatchingToken(string text)
		{
			List list = this.findPartialMatchingTokens(text);
			Token token = null;
			Iterator iterator = list.iterator();
			while (iterator.hasNext())
			{
				Token token2 = (Token)iterator.next();
				if (token == null || token2.getScore() > token.getScore())
				{
					token = token2;
				}
			}
			return token;
		}

		public virtual FrameStatistics[] getFrameStatistics()
		{
			return null;
		}

		public virtual int getStartFrame()
		{
			return 0;
		}

		public virtual int getEndFrame()
		{
			return 0;
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			218,
			135,
			99,
			130,
			166,
			103,
			99,
			136,
			103,
			131
		})]
		
		public virtual List getDataFrames()
		{
			Token token = this.getBestToken();
			if (token == null)
			{
				return null;
			}
			LinkedList linkedList = new LinkedList();
			do
			{
				Data data = token.getData();
				if (data != null)
				{
					linkedList.add(0, data);
				}
				token = token.getPredecessor();
			}
			while (token != null);
			return linkedList;
		}

		[LineNumberTable(new byte[]
		{
			161,
			8,
			103,
			99,
			134
		})]
		
		public virtual string getBestFinalResultNoFiller()
		{
			Token bestFinalToken = this.getBestFinalToken();
			if (bestFinalToken == null)
			{
				return "";
			}
			return bestFinalToken.getWordPathNoFiller();
		}

		[LineNumberTable(new byte[]
		{
			161,
			25,
			103,
			99,
			134
		})]
		
		public virtual string getBestPronunciationResult()
		{
			Token bestFinalToken = this.getBestFinalToken();
			if (bestFinalToken == null)
			{
				return "";
			}
			return bestFinalToken.getWordPath(false, true);
		}

		internal virtual void setFinal(bool flag)
		{
			this.isFinal = flag;
		}

		[LineNumberTable(new byte[]
		{
			161,
			144,
			98,
			127,
			1,
			104,
			98,
			134,
			98
		})]
		
		public virtual bool validate()
		{
			int result = 1;
			Iterator iterator = this.activeList.iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				if (!token.validate())
				{
					result = 0;
					token.dumpTokenPath();
				}
			}
			return result != 0;
		}

		
		private ActiveList activeList;

		
		
		private List resultList;

		private AlternateHypothesisManager alternateHypothesisManager;

		private bool isFinal;

		private bool wordTokenFirst;

		
		private long currentCollectTime;

		private string reference;

		
		private LogMath logMath;

		
		private bool toCreateLattice;
	}
}

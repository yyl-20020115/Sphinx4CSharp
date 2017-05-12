using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.result
{
	public class Result : Object
	{
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
			return this._toCreateLattice;
		}
		
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
			return this._isFinal;
		}

		public virtual void setReferenceText(string @ref)
		{
			this.reference = @ref;
		}
		
		public virtual Token getBestToken()
		{
			Token token = this.getBestFinalToken();
			if (token == null)
			{
				token = this.getBestActiveToken();
			}
			return token;
		}

		public Result(ActiveList activeList, List resultList, long collectTime, bool isFinal, bool wordTokenFirst, bool toCreateLattice)
		{
			this.activeList = activeList;
			this.resultList = resultList;
			this.currentCollectTime = collectTime;
			this._isFinal = isFinal;
			this._toCreateLattice = toCreateLattice;
			this.wordTokenFirst = wordTokenFirst;
			this.logMath = LogMath.getLogMath();
		}
		
		public Result(AlternateHypothesisManager alternateHypothesisManager, ActiveList activeList, List resultList, long collectTime, bool isFinal, bool wordTokenFirst, bool toCreateLattice) : this(activeList, resultList, collectTime, isFinal, wordTokenFirst, toCreateLattice)
		{
			this.alternateHypothesisManager = alternateHypothesisManager;
		}

		public virtual string getReferenceText()
		{
			return this.reference;
		}
		
		public override string toString()
		{
			Token bestToken = this.getBestToken();
			if (bestToken == null)
			{
				return "";
			}
			return bestToken.getWordPath();
		}
		
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
		
		public virtual List findPartialMatchingTokens(string text)
		{
			ArrayList arrayList = new ArrayList();
			text = String.instancehelper_trim(text);
			Iterator iterator = this.activeList.iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				if (String.instancehelper_startsWith(text, token.getWordPathNoFiller()))
				{
					arrayList.add(token);
				}
			}
			return arrayList;
		}
		
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
		
		public virtual Token findToken(string text)
		{
			text = String.instancehelper_trim(text);
			Iterator iterator = this.resultList.iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				if (String.instancehelper_equals(text, token.getWordPathNoFiller()))
				{
					return token;
				}
			}
			return null;
		}
		
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
		
		public virtual string getBestFinalResultNoFiller()
		{
			Token bestFinalToken = this.getBestFinalToken();
			if (bestFinalToken == null)
			{
				return "";
			}
			return bestFinalToken.getWordPathNoFiller();
		}
		
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
			this._isFinal = flag;
		}
		
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

		private bool _isFinal;

		private bool wordTokenFirst;

		
		private long currentCollectTime;

		private string reference;

		
		private LogMath logMath;

		
		private bool _toCreateLattice;
	}
}

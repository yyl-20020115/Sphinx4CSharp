using java.io;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class HypothesisFile : Object
	{		
		internal HypothesisFile(string text)
		{
			this.reader = new BufferedReader(new FileReader(text));
		}
		
		internal HypothesisWord nextWord()
		{
			if (this.iterator == null || !this.iterator.hasNext())
			{
				HypothesisUtterance hypothesisUtterance = this.nextUtterance();
				if (hypothesisUtterance != null)
				{
					this.iterator = hypothesisUtterance.getWords().iterator();
				}
				else
				{
					this.iterator = null;
				}
			}
			if (this.iterator == null)
			{
				return null;
			}
			return (HypothesisWord)this.iterator.next();
		}

		public int getUtteranceCount()
		{
			return this.utteranceCount;
		}
		
		private HypothesisUtterance nextUtterance()
		{
			string text = this.reader.readLine();
			if (text == null)
			{
				return null;
			}
			this.utteranceCount++;
			HypothesisUtterance hypothesisUtterance = new HypothesisUtterance(text);
			if (hypothesisUtterance.getWordCount() <= 0)
			{
				return this.nextUtterance();
			}
			return hypothesisUtterance;
		}

		private BufferedReader reader;
		
		private Iterator iterator;

		private int utteranceCount;
	}
}

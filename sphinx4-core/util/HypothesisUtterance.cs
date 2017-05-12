using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	internal sealed class HypothesisUtterance : Object
	{
		internal List getWords()
		{
			LinkedList linkedList = new LinkedList();
			linkedList.addAll(this.words);
			return linkedList;
		}
		
		internal HypothesisUtterance(string text)
		{
			this.words = new LinkedList();
			StringTokenizer stringTokenizer = new StringTokenizer(text, " \t\n\r\f(),");
			while (stringTokenizer.hasMoreTokens())
			{
				string text2 = stringTokenizer.nextToken();
				try
				{
					float num = Float.parseFloat(stringTokenizer.nextToken());
					float num2 = Float.parseFloat(stringTokenizer.nextToken());
					HypothesisWord hypothesisWord = new HypothesisWord(text2, num, num2);
					this.words.add(hypothesisWord);
				}
				catch (NumberFormatException ex)
				{
					Throwable.instancehelper_printStackTrace(ex);
				}
				continue;
			}
			if (!this.words.isEmpty())
			{
				HypothesisWord hypothesisWord2 = (HypothesisWord)this.words.get(0);
				this.startTime = hypothesisWord2.getStartTime();
				HypothesisWord hypothesisWord3 = (HypothesisWord)this.words.get(this.words.size() - 1);
				this.endTime = hypothesisWord3.getEndTime();
			}
		}
		
		internal int getWordCount()
		{
			return this.words.size();
		}

		internal float getStartTime()
		{
			return this.startTime;
		}

		internal float getEndTime()
		{
			return this.endTime;
		}
		
		private List words;

		private float startTime;

		private float endTime;
	}
}

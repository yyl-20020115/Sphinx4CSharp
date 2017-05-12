using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.linguist.dictionary;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search.stats
{
	public class WordTracker : Object
	{		
		private WordSequence getWordSequence(Token predecessor)
		{
			LinkedList linkedList = new LinkedList();
			while (predecessor != null)
			{
				if (predecessor.isWord())
				{
					WordSearchState wordSearchState = (WordSearchState)predecessor.getSearchState();
					Word word = wordSearchState.getPronunciation().getWord();
					linkedList.add(0, word);
				}
				predecessor = predecessor.getPredecessor();
			}
			return new WordSequence(linkedList);
		}
		
		internal virtual void dumpSummary()
		{
			java.lang.System.@out.println(new StringBuilder().append("Frame: ").append(this.frameNumber).append(" states: ").append(this.stateCount).append(" histories ").append(this.statMap.size()).toString());
		}
	
		public WordTracker(int frameNumber)
		{
			this.statMap = new HashMap();
			this.frameNumber = frameNumber;
		}
		
		public virtual void add(Token t)
		{
			this.stateCount++;
			WordSequence wordSequence = this.getWordSequence(t);
			WordTracker.WordStats wordStats = (WordTracker.WordStats)this.statMap.get(wordSequence);
			if (wordStats == null)
			{
				wordStats = new WordTracker.WordStats(wordSequence);
				this.statMap.put(wordSequence, wordStats);
			}
			wordStats.update(t);
		}
		
		public virtual void dump()
		{
			this.dumpSummary();
			ArrayList arrayList = new ArrayList(this.statMap.values());
			Collections.sort(arrayList, WordTracker.WordStats.COMPARATOR);
			Iterator iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				WordTracker.WordStats wordStats = (WordTracker.WordStats)iterator.next();
				java.lang.System.@out.println(new StringBuilder().append("   ").append(wordStats).toString());
			}
		}		
		
		internal Map statMap;
		
		internal int frameNumber;

		internal int stateCount;

		internal int maxWordHistories;

		internal sealed class WordStats : java.lang.Object
		{
			internal static float access_000(WordTracker.WordStats wordStats)
			{
				return wordStats.maxScore;
			}
		
			internal WordStats(WordSequence wordSequence)
			{
				this.size = 0;
				this.maxScore = float.MinValue;
				this.minScore = float.MaxValue;
				this.ws = wordSequence;
			}
		
			internal void update(Token token)
			{
				this.size++;
				if (token.getScore() > this.maxScore)
				{
					this.maxScore = token.getScore();
				}
				if (token.getScore() < this.minScore)
				{
					this.minScore = token.getScore();
				}
			}
		
			public override string toString()
			{
				return new StringBuilder().append("states:").append(this.size).append(" max:").append(this.maxScore).append(" min:").append(this.minScore).append(' ').append(this.ws).toString();
			}
		
			public static Comparator COMPARATOR = new WordTracker_WordStats_1();

			private int size;

			private float maxScore;

			private float minScore;
			
			private WordSequence ws;
		}
	}
}

using edu.cmu.sphinx.linguist;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search.stats
{
	public class StateHistoryTracker : java.lang.Object
	{
		private WordSequence getWordSequence(Token token)
		{
			return token.getSearchState().getWordHistory();
		}
	
		internal virtual void dumpSummary()
		{
			java.lang.System.@out.println(new StringBuilder().append("Frame: ").append(this.frameNumber).append(" states: ").append(this.stateCount).append(" histories ").append(this.statMap.size()).toString());
		}
	
		public StateHistoryTracker(int frameNumber)
		{
			this.statMap = new HashMap();
			this.frameNumber = frameNumber;
		}
		
		public virtual void add(Token t)
		{
			this.stateCount++;
			WordSequence wordSequence = this.getWordSequence(t);
			StateHistoryTracker.WordStats wordStats = (StateHistoryTracker.WordStats)this.statMap.get(wordSequence);
			if (wordStats == null)
			{
				wordStats = new StateHistoryTracker.WordStats(wordSequence);
				this.statMap.put(wordSequence, wordStats);
			}
			wordStats.update(t);
		}
		
		public virtual void dump()
		{
			this.dumpSummary();
			ArrayList arrayList = new ArrayList(this.statMap.values());
			Collections.sort(arrayList, StateHistoryTracker.WordStats.COMPARATOR);
			Iterator iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				StateHistoryTracker.WordStats wordStats = (StateHistoryTracker.WordStats)iterator.next();
				java.lang.System.@out.println(new StringBuilder().append("   ").append(wordStats).toString());
			}
		}	
		
		internal Map statMap;

		internal int frameNumber;

		internal int stateCount;

		internal int maxWordHistories;
		
		internal sealed class WordStats : java.lang.Object
		{		
			internal static float access_000(StateHistoryTracker.WordStats wordStats)
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
			
			public static Comparator COMPARATOR = new StateHistoryTracker_WordStats_1();

			private int size;

			private float maxScore;

			private float minScore;

			private WordSequence ws;
		}
	}
}

using java.util;
using java.lang;

namespace edu.cmu.sphinx.decoder.search.stats
{
	internal sealed class StateHistoryTracker_WordStats_1 : Object, Comparator
	{
		public int compare(StateHistoryTracker.WordStats wordStats, StateHistoryTracker.WordStats wordStats2)
		{
			if (StateHistoryTracker.WordStats.access_000(wordStats) > StateHistoryTracker.WordStats.access_000(wordStats2))
			{
				return -1;
			}
			if (StateHistoryTracker.WordStats.access_000(wordStats) == StateHistoryTracker.WordStats.access_000(wordStats2))
			{
				return 0;
			}
			return 1;
		}	
		
		internal StateHistoryTracker_WordStats_1()
		{
		}				
		
		public int compare(object obj, object obj2)
		{
			return this.compare((StateHistoryTracker.WordStats)obj, (StateHistoryTracker.WordStats)obj2);
		}
		
		bool Comparator.equals(object obj)
		{
			return Object.instancehelper_equals(this, obj);
		}
	}
}

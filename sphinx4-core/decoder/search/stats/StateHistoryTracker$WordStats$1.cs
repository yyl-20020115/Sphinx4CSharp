using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search.stats
{
	
	[Implements(new string[]
	{
		"java.util.Comparator"
	})]
	
	[EnclosingMethod("edu.cmu.sphinx.decoder.search.stats.StateHistoryTracker$WordStats", null, null)]
	[SourceFile("StateHistoryTracker.java")]
	internal sealed class StateHistoryTracker$WordStats$1 : java.lang.Object, Comparator
	{
		[LineNumberTable(new byte[]
		{
			34,
			110,
			98,
			110,
			130
		})]
		
		public int compare(StateHistoryTracker.WordStats wordStats, StateHistoryTracker.WordStats wordStats2)
		{
			if (StateHistoryTracker.WordStats.access$000(wordStats) > StateHistoryTracker.WordStats.access$000(wordStats2))
			{
				return -1;
			}
			if (StateHistoryTracker.WordStats.access$000(wordStats) == StateHistoryTracker.WordStats.access$000(wordStats2))
			{
				return 0;
			}
			return 1;
		}

		
		
		internal StateHistoryTracker$WordStats$1()
		{
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((StateHistoryTracker.WordStats)obj, (StateHistoryTracker.WordStats)obj2);
		}

		
		bool Comparator.Object;)Zequals(object obj)
		{
			return Object.instancehelper_equals(this, obj);
		}
	}
}

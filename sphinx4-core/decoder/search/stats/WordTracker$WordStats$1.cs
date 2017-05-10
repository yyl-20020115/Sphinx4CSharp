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
	
	[EnclosingMethod("edu.cmu.sphinx.decoder.search.stats.WordTracker_WordStats", null, null)]
	[SourceFile("WordTracker.java")]
	internal sealed class WordTracker_WordStats_1 : java.lang.Object, Comparator
	{
		[LineNumberTable(new byte[]
		{
			49,
			110,
			98,
			110,
			130
		})]
		
		public int compare(WordTracker.WordStats wordStats, WordTracker.WordStats wordStats2)
		{
			if (WordTracker.WordStats.access_000(wordStats) > WordTracker.WordStats.access_000(wordStats2))
			{
				return -1;
			}
			if (WordTracker.WordStats.access_000(wordStats) == WordTracker.WordStats.access_000(wordStats2))
			{
				return 0;
			}
			return 1;
		}

		
		
		internal WordTracker_WordStats_1()
		{
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((WordTracker.WordStats)obj, (WordTracker.WordStats)obj2);
		}

		
		bool Comparator.equals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}
	}
}

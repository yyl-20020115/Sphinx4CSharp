using System;

using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search.stats
{
	public class WordTracker : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			29,
			134,
			99,
			104,
			97,
			107,
			108,
			136,
			138
		})]
		
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

		[LineNumberTable(new byte[]
		{
			17,
			127,
			37,
			47,
			133
		})]
		
		internal virtual void dumpSummary()
		{
			java.lang.System.@out.println(new StringBuilder().append("Frame: ").append(this.frameNumber).append(" states: ").append(this.stateCount).append(" histories ").append(this.statMap.size()).toString());
		}

		[LineNumberTable(new byte[]
		{
			159,
			173,
			104,
			107,
			103
		})]
		
		public WordTracker(int frameNumber)
		{
			this.statMap = new HashMap();
			this.frameNumber = frameNumber;
		}

		[LineNumberTable(new byte[]
		{
			159,
			185,
			110,
			104,
			114,
			99,
			103,
			142,
			103
		})]
		
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

		[LineNumberTable(new byte[]
		{
			6,
			102,
			113,
			107,
			123,
			127,
			5,
			98
		})]
		
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

		
		[SourceFile("WordTracker.java")]
		
		internal sealed class WordStats : java.lang.Object
		{
			
			public static void __<clinit>()
			{
			}

			
			
			internal static float access$000(WordTracker.WordStats wordStats)
			{
				return wordStats.maxScore;
			}

			[LineNumberTable(new byte[]
			{
				69,
				104,
				103,
				107,
				107,
				103
			})]
			
			internal WordStats(WordSequence wordSequence)
			{
				this.size = 0;
				this.maxScore = float.MinValue;
				this.minScore = float.MaxValue;
				this.ws = wordSequence;
			}

			[LineNumberTable(new byte[]
			{
				83,
				110,
				110,
				140,
				110,
				140
			})]
			
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

			
			static WordStats()
			{
			}

			
			
			public static Comparator COMPARATOR = new WordTracker$WordStats$1();

			private int size;

			private float maxScore;

			private float minScore;

			
			private WordSequence ws;
		}
	}
}

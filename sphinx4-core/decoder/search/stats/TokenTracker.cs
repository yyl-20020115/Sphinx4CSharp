using System;

using edu.cmu.sphinx.linguist;
using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search.stats
{
	public class TokenTracker : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			33,
			107,
			102,
			105,
			145,
			111,
			159,
			25,
			108,
			127,
			25,
			127,
			10,
			191,
			5,
			138
		})]
		
		public virtual void dumpSummary()
		{
			if (this.enabled)
			{
				float num = 0f;
				if (this.utteranceStateCount > 0)
				{
					num = (float)this.utteranceSumStates / (float)this.utteranceStateCount;
				}
				java.lang.System.@out.print("# Utterance stats ");
				PrintStream @out = java.lang.System.@out;
				StringBuilder stringBuilder = new StringBuilder().append(" States: ");
				int num2 = this.utteranceStateCount;
				int num3 = this.frame;
				@out.print(stringBuilder.append((num3 != -1) ? (num2 / num3) : (-num2)).toString());
				if (this.utteranceStateCount > 0)
				{
					PrintStream out2 = java.lang.System.@out;
					StringBuilder stringBuilder2 = new StringBuilder().append(" Paths: ");
					int num4 = this.utteranceSumStates;
					int num5 = this.frame;
					out2.print(stringBuilder2.append((num5 != -1) ? (num4 / num5) : (-num4)).toString());
					java.lang.System.@out.print(new StringBuilder().append(" Max: ").append(this.utteranceMaxStates).toString());
					java.lang.System.@out.print(new StringBuilder().append(" Avg: ").append(num).toString());
				}
				java.lang.System.@out.println();
			}
		}

		[LineNumberTable(new byte[]
		{
			102,
			108,
			37,
			139,
			99,
			103,
			152
		})]
		
		private TokenTracker.TokenStats getStats(Token token)
		{
			TokenTracker.TokenStats tokenStats = (TokenTracker.TokenStats)this.stateMap.get(token.getSearchState().getLexState());
			if (tokenStats == null)
			{
				tokenStats = new TokenTracker.TokenStats(this);
				this.stateMap.put(token.getSearchState().getLexState(), tokenStats);
			}
			return tokenStats;
		}

		[LineNumberTable(new byte[]
		{
			54,
			107,
			102,
			98,
			130,
			127,
			10,
			105,
			132,
			106,
			116,
			106,
			168,
			111,
			141,
			133,
			152,
			103,
			109,
			146,
			127,
			10,
			159,
			15,
			112,
			127,
			5,
			127,
			5,
			127,
			6,
			191,
			5,
			138
		})]
		
		public virtual void dumpDetails()
		{
			if (this.enabled)
			{
				int num = -2147483647;
				int num2 = 0;
				int num3 = 0;
				Iterator iterator = this.stateMap.values().iterator();
				while (iterator.hasNext())
				{
					TokenTracker.TokenStats tokenStats = (TokenTracker.TokenStats)iterator.next();
					if (tokenStats.isHMM)
					{
						num2++;
					}
					num3 += tokenStats.count;
					this.utteranceSumStates += tokenStats.count;
					if (tokenStats.count > num)
					{
						num = tokenStats.count;
					}
					if (tokenStats.count > this.utteranceMaxStates)
					{
						this.utteranceMaxStates = tokenStats.count;
					}
				}
				this.utteranceStateCount += this.stateMap.size();
				float num4 = 0f;
				if (!this.stateMap.isEmpty())
				{
					num4 = (float)num3 / (float)this.stateMap.size();
				}
				java.lang.System.@out.print(new StringBuilder().append("# Frame ").append(this.frame).toString());
				java.lang.System.@out.print(new StringBuilder().append(" States: ").append(this.stateMap.size()).toString());
				if (!this.stateMap.isEmpty())
				{
					java.lang.System.@out.print(new StringBuilder().append(" Paths: ").append(num3).toString());
					java.lang.System.@out.print(new StringBuilder().append(" Max: ").append(num).toString());
					java.lang.System.@out.print(new StringBuilder().append(" Avg: ").append(num4).toString());
					java.lang.System.@out.print(new StringBuilder().append(" HMM: ").append(num2).toString());
				}
				java.lang.System.@out.println();
			}
		}

		
		
		public TokenTracker()
		{
		}

		internal virtual void setEnabled(bool flag)
		{
			this.enabled = flag;
		}

		internal virtual void startUtterance()
		{
			if (this.enabled)
			{
				this.frame = 0;
				this.utteranceStateCount = 0;
				this.utteranceMaxStates = -2147483647;
				this.utteranceSumStates = 0;
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			187,
			104,
			134
		})]
		
		internal virtual void stopUtterance()
		{
			if (this.enabled)
			{
				this.dumpSummary();
			}
		}

		[LineNumberTable(new byte[]
		{
			3,
			104,
			139
		})]
		
		internal virtual void startFrame()
		{
			if (this.enabled)
			{
				this.stateMap = new HashMap();
			}
		}

		[LineNumberTable(new byte[]
		{
			15,
			104,
			104,
			135
		})]
		
		public virtual void add(Token t)
		{
			if (this.enabled)
			{
				TokenTracker.TokenStats stats = this.getStats(t);
				stats.update(t);
			}
		}

		[LineNumberTable(new byte[]
		{
			24,
			104,
			110,
			134
		})]
		
		internal virtual void stopFrame()
		{
			if (this.enabled)
			{
				this.frame++;
				this.dumpDetails();
			}
		}

		
		private Map stateMap;

		private bool enabled;

		private int frame;

		private int utteranceStateCount;

		private int utteranceMaxStates;

		private int utteranceSumStates;

		
		[SourceFile("TokenTracker.java")]
		
		internal sealed class TokenStats : java.lang.Object
		{
			[LineNumberTable(new byte[]
			{
				123,
				111,
				103,
				107,
				107
			})]
			
			internal TokenStats(TokenTracker tokenTracker)
			{
				this.count = 0;
				this.maxScore = float.MinValue;
				this.minScore = float.Epsilon;
			}

			[LineNumberTable(new byte[]
			{
				160,
				69,
				110,
				110,
				172,
				110,
				172,
				116
			})]
			
			public void update(Token token)
			{
				this.count++;
				if (token.getScore() > this.maxScore)
				{
					this.maxScore = token.getScore();
				}
				if (token.getScore() < this.minScore)
				{
					this.minScore = token.getScore();
				}
				this.isHMM = (token.getSearchState() is HMMSearchState);
			}

			internal int count;

			internal float maxScore;

			internal float minScore;

			internal bool isHMM;

			
			internal TokenTracker this$0 = tokenTracker;
		}
	}
}

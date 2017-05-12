using edu.cmu.sphinx.linguist;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search.stats
{
	public class TokenTracker : Object
	{	
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
		
		internal virtual void stopUtterance()
		{
			if (this.enabled)
			{
				this.dumpSummary();
			}
		}
		
		internal virtual void startFrame()
		{
			if (this.enabled)
			{
				this.stateMap = new HashMap();
			}
		}
		
		public virtual void add(Token t)
		{
			if (this.enabled)
			{
				TokenTracker.TokenStats stats = this.getStats(t);
				stats.update(t);
			}
		}
		
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

		internal sealed class TokenStats : java.lang.Object
		{			
			internal TokenStats(TokenTracker tokenTracker)
			{
				this_0 = tokenTracker;
				this.count = 0;
				this.maxScore = float.MinValue;
				this.minScore = float.Epsilon;
			}

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

			internal TokenTracker this_0;
		}
	}
}

using System;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.util
{
	public class LinguistTimer : java.lang.Object
	{
		
		[LineNumberTable(new byte[]
		{
			57,
			103,
			142,
			106,
			136,
			115,
			105,
			105,
			110,
			110,
			105,
			110,
			105,
			113,
			159,
			11,
			142,
			142,
			238,
			49,
			233,
			81
		})]
		
		private void expandState(int num, List list, SearchState searchState)
		{
			SearchStateArc[] successors = searchState.getSuccessors();
			this.totalStates++;
			if (successors.Length > this.maxSuccessors)
			{
				this.maxSuccessors = successors.Length;
			}
			SearchStateArc[] array = successors;
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				SearchStateArc searchStateArc = array[i];
				SearchState state = searchStateArc.getState();
				if (state.isEmitting())
				{
					this.totalEmittingStates++;
					list.add(state);
				}
				else if (!state.isFinal())
				{
					this.totalNonEmittingStates++;
					list.add(state);
					if (this.details && state.isFinal())
					{
						java.lang.System.@out.println(new StringBuilder().append("result ").append(state.toPrettyString()).toString());
					}
					this.expandState(num + 1, list, state);
				}
				else
				{
					this.totalFinalStates++;
				}
				this.totalStates++;
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			132,
			162,
			104,
			103,
			103
		})]
		
		public LinguistTimer(Linguist linguist, bool details)
		{
			this.linguist = linguist;
			this.details = details;
		}

		[LineNumberTable(new byte[]
		{
			12,
			108,
			108,
			172,
			159,
			37,
			102,
			105,
			99,
			103,
			125,
			107,
			107,
			100,
			107,
			102,
			127,
			20,
			113,
			98,
			103,
			109,
			111,
			240,
			54,
			235,
			77,
			235,
			46,
			233,
			84,
			103,
			127,
			10,
			127,
			10,
			127,
			10,
			127,
			10,
			127,
			10
		})]
		
		public virtual void timeLinguist(int numRuns, int numFrames, int maxBeam)
		{
			Random random = new Random((long)((ulong)1000));
			Timer timer = TimerPool.getTimer(this, "frameTimer");
			Timer timer2 = TimerPool.getTimer(this, "totalTimer");
			java.lang.System.@out.println(new StringBuilder().append("TestLinguist: runs ").append(numRuns).append(" frames ").append(numFrames).append(" beam ").append(maxBeam).toString());
			timer2.start();
			for (int i = 0; i < numRuns; i++)
			{
				int num = 0;
				object obj = new ArrayList();
				((ArrayList)obj).add(this.linguist.getSearchGraph().getInitialState());
				this.linguist.startRecognition();
				for (int j = 0; j < numFrames; j++)
				{
					object obj2 = obj;
					obj = new ArrayList(maxBeam * 10);
					timer.start();
					object obj3 = obj2;
					List list;
					if (obj3 != null)
					{
						if ((list = (obj3 as List)) == null)
						{
							throw new IncompatibleClassChangeError();
						}
					}
					else
					{
						list = null;
					}
					Iterator iterator = list.iterator();
					while (iterator.hasNext())
					{
						SearchState searchState = (SearchState)iterator.next();
						this.expandState(num, (ArrayList)obj, searchState);
					}
					timer.stop();
					Collections.shuffle((ArrayList)obj, random);
					if (((ArrayList)obj).size() > maxBeam)
					{
						obj = ((ArrayList)obj).subList(0, maxBeam);
					}
				}
				this.linguist.stopRecognition();
			}
			timer2.stop();
			java.lang.System.@out.println(new StringBuilder().append(" MaxSuccessors : ").append(this.maxSuccessors).toString());
			java.lang.System.@out.println(new StringBuilder().append(" TotalStates   : ").append(this.totalStates).toString());
			java.lang.System.@out.println(new StringBuilder().append(" TotalEmitting : ").append(this.totalEmittingStates).toString());
			java.lang.System.@out.println(new StringBuilder().append("   NonEmitting : ").append(this.totalNonEmittingStates).toString());
			java.lang.System.@out.println(new StringBuilder().append("  Final States : ").append(this.totalFinalStates).toString());
		}

		
		private Linguist linguist;

		
		private bool details;

		internal int totalStates;

		internal int totalEmittingStates;

		internal int totalNonEmittingStates;

		internal int totalFinalStates;

		internal int maxSuccessors;
	}
}

using System;

using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.decoder.search.stats
{
	public class TokenTypeTracker : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			27,
			116
		})]
		
		private string pc(int num)
		{
			int num2 = 100 * num;
			int num3 = this.numTokens;
			int num4 = (num3 != -1) ? (num2 / num3) : (-num2);
			return new StringBuilder().append(" (").append(num4).append("%)").toString();
		}

		
		
		public TokenTypeTracker()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			176,
			110,
			135,
			104,
			115,
			104,
			115,
			107,
			113,
			127,
			11,
			112,
			112,
			112,
			176,
			98,
			142
		})]
		
		public virtual void add(Token t)
		{
			this.numTokens++;
			SearchState searchState = t.getSearchState();
			if (searchState is WordSearchState)
			{
				this.numWords++;
			}
			else if (searchState is UnitSearchState)
			{
				this.numUnits++;
			}
			else if (searchState is HMMSearchState)
			{
				HMM hmm = ((HMMSearchState)searchState).getHMMState().getHMM();
				switch (TokenTypeTracker_1._SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition[hmm.getPosition().ordinal()])
				{
				case 1:
					this.numHMMBegin++;
					break;
				case 2:
					this.numHMMEnd++;
					break;
				case 3:
					this.numHMMSingle++;
					break;
				case 4:
					this.numHMMInternal++;
					break;
				}
			}
			else
			{
				this.numOthers++;
			}
		}

		[LineNumberTable(new byte[]
		{
			8,
			127,
			10,
			127,
			27,
			127,
			27,
			127,
			27,
			127,
			27,
			127,
			27,
			127,
			7,
			47,
			133,
			127,
			27
		})]
		
		public virtual void dump()
		{
			java.lang.System.@out.println(new StringBuilder().append("TotalTokens: ").append(this.numTokens).toString());
			java.lang.System.@out.println(new StringBuilder().append("      Words: ").append(this.numWords).append(this.pc(this.numWords)).toString());
			java.lang.System.@out.println(new StringBuilder().append("      Units: ").append(this.numUnits).append(this.pc(this.numUnits)).toString());
			java.lang.System.@out.println(new StringBuilder().append("      HMM-b: ").append(this.numHMMBegin).append(this.pc(this.numHMMBegin)).toString());
			java.lang.System.@out.println(new StringBuilder().append("      HMM-e: ").append(this.numHMMEnd).append(this.pc(this.numHMMEnd)).toString());
			java.lang.System.@out.println(new StringBuilder().append("      HMM-s: ").append(this.numHMMSingle).append(this.pc(this.numHMMSingle)).toString());
			java.lang.System.@out.println(new StringBuilder().append("      HMM-i: ").append(this.numHMMInternal).append(this.pc(this.numHMMInternal)).toString());
			java.lang.System.@out.println(new StringBuilder().append("     Others: ").append(this.numOthers).append(this.pc(this.numOthers)).toString());
		}

		private int numWords;

		private int numUnits;

		private int numOthers;

		private int numHMMBegin;

		private int numHMMEnd;

		private int numHMMSingle;

		private int numHMMInternal;

		private int numTokens;
	}
}

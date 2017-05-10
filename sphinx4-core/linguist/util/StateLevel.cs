using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.util
{
	[SourceFile("LinguistDumper.java")]
	
	internal sealed class StateLevel : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			160,
			126,
			104,
			103,
			103
		})]
		
		internal StateLevel(SearchState searchState, int num)
		{
			this.state = searchState;
			this.level = num;
		}

		internal int getLevel()
		{
			return this.level;
		}

		internal SearchState getState()
		{
			return this.state;
		}

		[LineNumberTable(new byte[]
		{
			160,
			171,
			104,
			134,
			104,
			134,
			104,
			134
		})]
		public string getTypeLabel(SearchState searchState)
		{
			if (searchState is UnitSearchState)
			{
				return "Unit";
			}
			if (searchState is WordSearchState)
			{
				return "Word";
			}
			if (searchState is HMMSearchState)
			{
				return "HMM";
			}
			return "other";
		}

		[LineNumberTable(new byte[]
		{
			160,
			159,
			127,
			30,
			47
		})]
		
		public override string toString()
		{
			return new StringBuilder().append(java.lang.String.valueOf(this.level)).append(' ').append(this.state.getSignature()).append(" 1 ").append(this.getTypeLabel(this.state)).toString();
		}

		
		private int level;

		
		private SearchState state;
	}
}

using java.lang;

namespace edu.cmu.sphinx.linguist.util
{
	internal sealed class StateLevel : java.lang.Object
	{		
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
		
		public override string toString()
		{
			return new StringBuilder().append(java.lang.String.valueOf(this.level)).append(' ').append(this.state.getSignature()).append(" 1 ").append(this.getTypeLabel(this.state)).toString();
		}

		private int level;
		
		private SearchState state;
	}
}

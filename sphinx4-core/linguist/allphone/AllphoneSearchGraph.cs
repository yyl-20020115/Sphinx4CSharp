using System;

using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.allphone
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.SearchGraph"
	})]
	public class AllphoneSearchGraph : java.lang.Object, SearchGraph
	{
		[LineNumberTable(new byte[]
		{
			159,
			156,
			104,
			103
		})]
		
		public AllphoneSearchGraph(AllphoneLinguist linguist)
		{
			this.linguist = linguist;
		}

		[LineNumberTable(new byte[]
		{
			159,
			161,
			127,
			2
		})]
		
		public virtual SearchState getInitialState()
		{
			HMMState initialState = this.linguist.getAcousticModel().lookupNearestHMM(UnitManager.__SILENCE, HMMPosition.__UNDEFINED, true).getInitialState();
			return new PhoneHmmSearchState(initialState, this.linguist, 0f, 0f);
		}

		public virtual int getNumStateOrder()
		{
			return 2;
		}

		public virtual bool getWordTokenFirst()
		{
			return false;
		}

		private AllphoneLinguist linguist;
	}
}

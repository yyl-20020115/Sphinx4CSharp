using edu.cmu.sphinx.linguist.acoustic;
using java.lang;

namespace edu.cmu.sphinx.linguist.allphone
{
	public class AllphoneSearchGraph : Object, SearchGraph
	{		
		public AllphoneSearchGraph(AllphoneLinguist linguist)
		{
			this.linguist = linguist;
		}
		
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

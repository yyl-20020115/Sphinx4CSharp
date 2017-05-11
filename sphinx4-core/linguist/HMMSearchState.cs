using edu.cmu.sphinx.linguist.acoustic;

namespace edu.cmu.sphinx.linguist
{
	public interface HMMSearchState : SearchState
	{
		HMMState getHMMState();
	}
}

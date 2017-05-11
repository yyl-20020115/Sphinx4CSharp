using edu.cmu.sphinx.linguist.dictionary;

namespace edu.cmu.sphinx.linguist
{
	public interface WordSearchState : SearchState
	{
		Pronunciation getPronunciation();

		bool isWordStart();
	}
}

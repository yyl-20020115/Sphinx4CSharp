using System;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.SearchState"
	})]
	public interface WordSearchState : SearchState
	{
		Pronunciation getPronunciation();

		bool isWordStart();
	}
}

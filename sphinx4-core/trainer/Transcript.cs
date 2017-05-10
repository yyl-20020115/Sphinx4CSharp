using System;
using edu.cmu.sphinx.linguist.dictionary;

namespace edu.cmu.sphinx.trainer
{
	public interface Transcript
	{
		Dictionary getDictionary();

		void startWordIterator();

		bool isExact();

		bool hasMoreWords();

		string nextWord();

		string getTranscriptText();

		int numberOfWords();
	}
}

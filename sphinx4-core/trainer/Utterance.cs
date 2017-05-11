using edu.cmu.sphinx.linguist.dictionary;

namespace edu.cmu.sphinx.trainer
{
	public interface Utterance
	{
		void add(string str1, Dictionary d, bool b, string str2);

		void startTranscriptIterator();

		bool hasMoreTranscripts();

		Transcript nextTranscript();
	}
}

namespace edu.cmu.sphinx.result
{
	public interface Path
	{
		double getScore();

		double getConfidence();

		WordResult[] getWords();

		string getTranscription();

		string getTranscriptionNoFiller();

		string toString();
	}
}

namespace edu.cmu.sphinx.linguist
{
	public interface SearchState
	{
		bool isEmitting();

		int getOrder();

		SearchStateArc[] getSuccessors();

		bool isFinal();

		string toPrettyString();

		WordSequence getWordHistory();

		object getLexState();

		string getSignature();
	}
}

namespace edu.cmu.sphinx.linguist
{
	public interface SearchGraph
	{
		bool getWordTokenFirst();

		SearchState getInitialState();

		int getNumStateOrder();
	}
}

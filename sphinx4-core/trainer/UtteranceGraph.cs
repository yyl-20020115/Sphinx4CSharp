namespace edu.cmu.sphinx.trainer
{
	public interface UtteranceGraph
	{
		int size();

		Node getInitialNode();

		int indexOf(Node n);

		Node getFinalNode();

		Node getNode(int i);

		bool isInitialNode(Node n);

		bool isFinalNode(Node n);

		void add(Graph g);

		bool validate();
	}
}

using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.frontend
{
	public interface DataProcessor : Configurable
	{
		void initialize();

		Data getData();

		DataProcessor getPredecessor();

		void setPredecessor(DataProcessor dp);
	}
}

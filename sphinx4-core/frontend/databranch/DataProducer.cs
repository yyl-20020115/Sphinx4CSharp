using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.frontend.databranch
{
	public interface DataProducer : Configurable
	{
		void addDataListener(DataListener dl);

		void removeDataListener(DataListener dl);
	}
}

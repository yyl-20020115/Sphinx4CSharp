using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.decoder.pruner
{
	public interface Pruner : Configurable
	{
		void startRecognition();

		ActiveList prune(ActiveList al);

		void stopRecognition();

		void allocate();

		void deallocate();
	}
}

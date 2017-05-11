using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.decoder.search
{
	public interface SearchManager : Configurable
	{
		void allocate();

		void deallocate();

		void startRecognition();

		Result recognize(int i);

		void stopRecognition();
	}
}

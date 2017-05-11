using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.decoder
{
	public interface ResultProducer : Configurable
	{
		void addResultListener(ResultListener rl);

		void removeResultListener(ResultListener rl);
	}
}

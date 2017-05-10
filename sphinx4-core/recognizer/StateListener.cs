using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.recognizer
{
	public interface StateListener : EventListener, Configurable
	{
		void statusChanged(Recognizer.State rs);
	}
}

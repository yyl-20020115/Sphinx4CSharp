using edu.cmu.sphinx.util.props;
using java.io;

namespace edu.cmu.sphinx.frontend.util
{
	public interface AudioFileProcessListener : Configurable
	{
		void audioFileProcStarted(File f);

		void audioFileProcFinished(File f);
	}
}

using System;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.io;

namespace edu.cmu.sphinx.frontend.util
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public interface AudioFileProcessListener : Configurable
	{
		void audioFileProcStarted(File f);

		void audioFileProcFinished(File f);
	}
}

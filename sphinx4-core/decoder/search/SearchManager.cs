using System;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;

namespace edu.cmu.sphinx.decoder.search
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public interface SearchManager : Configurable
	{
		void allocate();

		void deallocate();

		void startRecognition();

		Result recognize(int i);

		void stopRecognition();
	}
}

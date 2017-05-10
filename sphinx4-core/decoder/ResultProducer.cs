using System;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;

namespace edu.cmu.sphinx.decoder
{
	public interface ResultProducer : Configurable
	{
		void addResultListener(ResultListener rl);

		void removeResultListener(ResultListener rl);
	}
}

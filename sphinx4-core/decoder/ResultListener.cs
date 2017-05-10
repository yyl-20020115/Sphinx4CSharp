using System;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.decoder
{
	public interface ResultListener : EventListener, Configurable
	{
		void newResult(Result r);
	}
}

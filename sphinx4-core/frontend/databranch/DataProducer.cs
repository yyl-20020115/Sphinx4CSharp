using System;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;

namespace edu.cmu.sphinx.frontend.databranch
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public interface DataProducer : Configurable
	{
		void addDataListener(DataListener dl);

		void removeDataListener(DataListener dl);
	}
}

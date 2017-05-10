using System;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;

namespace edu.cmu.sphinx.frontend
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public interface DataProcessor : Configurable
	{
		void initialize();

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		Data getData();

		DataProcessor getPredecessor();

		void setPredecessor(DataProcessor dp);
	}
}

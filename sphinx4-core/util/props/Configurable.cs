using System;
using IKVM.Attributes;

namespace edu.cmu.sphinx.util.props
{
	public interface Configurable
	{
		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		void newProperties(PropertySheet ps);
	}
}

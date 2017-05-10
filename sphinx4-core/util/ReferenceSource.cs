using System;
using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.util
{
	public interface ReferenceSource
	{
		
		List getReferences();
	}
}

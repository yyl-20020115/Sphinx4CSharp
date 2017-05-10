using System;
using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.alignment
{
	public interface TextTokenizer
	{
		
		List expand(string str);
	}
}

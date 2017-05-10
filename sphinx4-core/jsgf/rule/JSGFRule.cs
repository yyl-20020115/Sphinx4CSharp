using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.jsgf.rule
{
	public class JSGFRule : java.lang.Object
	{
		public override string toString()
		{
			return this.ruleName;
		}

		
		
		public JSGFRule()
		{
		}

		public string ruleName;

		public JSGFRule parent;
	}
}

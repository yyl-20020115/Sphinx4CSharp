using java.lang;

namespace edu.cmu.sphinx.jsgf.rule
{
	public class JSGFRule : Object
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

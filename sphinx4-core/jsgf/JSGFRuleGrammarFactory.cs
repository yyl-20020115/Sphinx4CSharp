using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.jsgf
{
	public class JSGFRuleGrammarFactory : Object
	{
		public JSGFRuleGrammarFactory(JSGFRuleGrammarManager manager)
		{
			this.manager = manager;
		}

		public virtual JSGFRuleGrammar newGrammar(string name)
		{
			if (!JSGFRuleGrammarFactory.assertionsDisabled && this.manager == null)
			{
				
				throw new AssertionError();
			}
			JSGFRuleGrammar jsgfruleGrammar = new JSGFRuleGrammar(name, this.manager);
			this.manager.storeGrammar(jsgfruleGrammar);
			return jsgfruleGrammar;
		}

		internal JSGFRuleGrammarManager manager;
		
		internal static bool assertionsDisabled = !ClassLiteral<JSGFRuleGrammarFactory>.Value.desiredAssertionStatus();
	}
}

using System;

using IKVM.Attributes;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.jsgf
{
	public class JSGFRuleGrammarFactory : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			155,
			104,
			103
		})]
		
		public JSGFRuleGrammarFactory(JSGFRuleGrammarManager manager)
		{
			this.manager = manager;
		}

		[LineNumberTable(new byte[]
		{
			159,
			161,
			122,
			114,
			108
		})]
		
		public virtual JSGFRuleGrammar newGrammar(string name)
		{
			if (!JSGFRuleGrammarFactory.assertionsDisabled && this.manager == null)
			{
				
				throw new AssertionError();
			}
			JSGFRuleGrammar.__<clinit>();
			JSGFRuleGrammar jsgfruleGrammar = new JSGFRuleGrammar(name, this.manager);
			this.manager.storeGrammar(jsgfruleGrammar);
			return jsgfruleGrammar;
		}

		
		static JSGFRuleGrammarFactory()
		{
		}

		internal JSGFRuleGrammarManager manager;

		
		internal static bool assertionsDisabled = !ClassLiteral<JSGFRuleGrammarFactory>.Value.desiredAssertionStatus();
	}
}

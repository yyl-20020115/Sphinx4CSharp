using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.jsgf
{
	public class JSGFRuleGrammarManager : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			159,
			104,
			107
		})]
		
		public JSGFRuleGrammarManager()
		{
			this.grammars = new HashMap();
		}

		
		
		public virtual JSGFRuleGrammar retrieveGrammar(string name)
		{
			return (JSGFRuleGrammar)this.grammars.get(name);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.JSGFGrammarException"
		})]
		[LineNumberTable(new byte[]
		{
			4,
			127,
			6,
			102,
			98
		})]
		
		public virtual void linkGrammars()
		{
			Iterator iterator = this.grammars.values().iterator();
			while (iterator.hasNext())
			{
				JSGFRuleGrammar jsgfruleGrammar = (JSGFRuleGrammar)iterator.next();
				jsgfruleGrammar.resolveAllRules();
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			180,
			115
		})]
		
		protected internal virtual void storeGrammar(JSGFRuleGrammar grammar)
		{
			this.grammars.put(grammar.getName(), grammar);
		}

		
		
		
		public virtual Collection grammars()
		{
			return this.grammars.values();
		}

		[LineNumberTable(new byte[]
		{
			159,
			168,
			103,
			109
		})]
		
		public virtual void remove(JSGFRuleGrammar grammar)
		{
			string name = grammar.getName();
			this.grammars.remove(name);
		}

		[LineNumberTable(new byte[]
		{
			159,
			173,
			109
		})]
		
		public virtual void remove(string name)
		{
			this.grammars.remove(name);
		}

		
		protected internal Map grammars;
	}
}

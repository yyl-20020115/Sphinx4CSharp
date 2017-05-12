using java.util;
using java.lang;

namespace edu.cmu.sphinx.jsgf
{
	public class JSGFRuleGrammarManager : Object
	{		
		public JSGFRuleGrammarManager()
		{
			this._grammars = new HashMap();
		}
		
		public virtual JSGFRuleGrammar retrieveGrammar(string name)
		{
			return (JSGFRuleGrammar)this._grammars.get(name);
		}
		
		public virtual void linkGrammars()
		{
			Iterator iterator = this._grammars.values().iterator();
			while (iterator.hasNext())
			{
				JSGFRuleGrammar jsgfruleGrammar = (JSGFRuleGrammar)iterator.next();
				jsgfruleGrammar.resolveAllRules();
			}
		}
		
		protected internal virtual void storeGrammar(JSGFRuleGrammar grammar)
		{
			this._grammars.put(grammar.getName(), grammar);
		}
				
		public virtual Collection grammars()
		{
			return this._grammars.values();
		}
		
		public virtual void remove(JSGFRuleGrammar grammar)
		{
			string name = grammar.getName();
			this._grammars.remove(name);
		}
		
		public virtual void remove(string name)
		{
			this._grammars.remove(name);
		}
		
		protected internal Map _grammars;
	}
}

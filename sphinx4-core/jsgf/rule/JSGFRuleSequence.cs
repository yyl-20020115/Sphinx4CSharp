using java.lang;
using java.util;

namespace edu.cmu.sphinx.jsgf.rule
{
	public class JSGFRuleSequence : JSGFRule
	{
		public virtual List getRules()
		{
			return this.rules;
		}

		public JSGFRuleSequence(List rules)
		{
			this.setRules(rules);
		}
		
		public virtual void append(JSGFRule rule)
		{
			if (this.rules == null)
			{
				string text = "null rule to append";
				
				throw new NullPointerException(text);
			}
			this.rules.add(rule);
		}
		
		public virtual void setRules(List rules)
		{
			this.rules = rules;
		}
		
		public JSGFRuleSequence()
		{
			this.setRules(null);
		}
		
		public override string toString()
		{
			if (this.rules.size() == 0)
			{
				return "<NULL>";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.rules.size(); i++)
			{
				if (i > 0)
				{
					stringBuilder.append(' ');
				}
				JSGFRule jsgfrule = (JSGFRule)this.rules.get(i);
				if (jsgfrule is JSGFRuleAlternatives || jsgfrule is JSGFRuleSequence)
				{
					stringBuilder.append("( ").append(jsgfrule).append(" )");
				}
				else
				{
					stringBuilder.append(jsgfrule);
				}
			}
			return stringBuilder.toString();
		}
		
		protected internal List rules;
	}
}

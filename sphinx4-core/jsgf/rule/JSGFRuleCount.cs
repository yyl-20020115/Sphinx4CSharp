using java.lang;

namespace edu.cmu.sphinx.jsgf.rule
{
	public class JSGFRuleCount : JSGFRule
	{
		public virtual int getCount()
		{
			return this.count;
		}

		public virtual JSGFRule getRule()
		{
			return this.rule;
		}

		public JSGFRuleCount(JSGFRule rule, int count)
		{
			this.setRule(rule);
			this.setCount(count);
		}

		public virtual void setRule(JSGFRule rule)
		{
			this.rule = rule;
		}

		public virtual void setCount(int count)
		{
			if (count != 2 && count != 4 && count != 3)
			{
				return;
			}
			this.count = count;
		}
		
		public JSGFRuleCount()
		{
			this.setRule(null);
			this.setCount(2);
		}
		
		public override string toString()
		{
			if (this.count == 2)
			{
				return new StringBuilder().append('[').append(this.rule.toString()).append(']').toString();
			}
			string text;
			if (this.rule is JSGFRuleToken || this.rule is JSGFRuleName)
			{
				text = this.rule.toString();
			}
			else
			{
				text = new StringBuilder().append('(').append(this.rule.toString()).append(')').toString();
			}
			if (this.count == 4)
			{
				return new StringBuilder().append(text).append(" *").toString();
			}
			if (this.count == 3)
			{
				return new StringBuilder().append(text).append(" +").toString();
			}
			return new StringBuilder().append(text).append("???").toString();
		}

		protected internal JSGFRule rule;

		protected internal int count;

		public const int OPTIONAL = 2;

		public const int ONCE_OR_MORE = 3;

		public const int ZERO_OR_MORE = 4;
	}
}

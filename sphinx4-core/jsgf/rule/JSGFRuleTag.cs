﻿using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.jsgf.rule
{
	public class JSGFRuleTag : JSGFRule
	{
		public virtual JSGFRule getRule()
		{
			return this.rule;
		}

		public virtual string getTag()
		{
			return this.tag;
		}

		[LineNumberTable(new byte[]
		{
			159,
			161,
			104,
			103,
			103
		})]
		
		public JSGFRuleTag(JSGFRule rule, string tag)
		{
			this.setRule(rule);
			this.setTag(tag);
		}

		public virtual void setRule(JSGFRule rule)
		{
			this.rule = rule;
		}

		public virtual void setTag(string tag)
		{
			if (tag == null)
			{
				this.tag = "";
			}
			else
			{
				this.tag = tag;
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			167,
			135,
			121,
			104,
			109,
			104,
			111,
			234,
			61,
			230,
			71
		})]
		
		private string escapeTag(string text)
		{
			StringBuilder stringBuilder = new StringBuilder(text);
			if (java.lang.String.instancehelper_indexOf(text, 125) >= 0 || java.lang.String.instancehelper_indexOf(text, 92) >= 0 || java.lang.String.instancehelper_indexOf(text, 123) >= 0)
			{
				for (int i = stringBuilder.length() - 1; i >= 0; i += -1)
				{
					int num = (int)stringBuilder.charAt(i);
					if (num == 92 || num == 125 || num == 123)
					{
						stringBuilder.insert(i, '\\');
					}
				}
			}
			return stringBuilder.toString();
		}

		[LineNumberTable(new byte[]
		{
			159,
			156,
			104,
			103,
			103
		})]
		
		public JSGFRuleTag()
		{
			this.setRule(null);
			this.setTag(null);
		}

		[LineNumberTable(new byte[]
		{
			10,
			159,
			17,
			122,
			159,
			2
		})]
		
		public override string toString()
		{
			string text = new StringBuilder().append(" {").append(this.escapeTag(this.tag)).append("}").toString();
			if (this.rule is JSGFRuleToken || this.rule is JSGFRuleName)
			{
				return new StringBuilder().append(this.rule.toString()).append(text).toString();
			}
			return new StringBuilder().append("(").append(this.rule.toString()).append(")").append(text).toString();
		}

		protected internal JSGFRule rule;

		protected internal string tag;
	}
}

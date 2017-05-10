using System;

using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.jsgf.rule
{
	public class JSGFRuleAlternatives : JSGFRule
	{
		
		public static void __<clinit>()
		{
		}

		
		public virtual List getRules()
		{
			return this.rules;
		}

		
		public virtual List getWeights()
		{
			return this.weights;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			161,
			104,
			103,
			103
		})]
		
		public JSGFRuleAlternatives(List rules)
		{
			this.setRules(rules);
			this.weights = null;
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			117,
			109,
			104,
			118
		})]
		
		public virtual void append(JSGFRule rule)
		{
			if (!JSGFRuleAlternatives.assertionsDisabled && rule == null)
			{
				
				throw new AssertionError();
			}
			this.rules.add(rule);
			if (this.weights != null)
			{
				this.weights.add(Float.valueOf(1f));
			}
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		
		[LineNumberTable(new byte[]
		{
			5,
			107,
			103,
			161,
			115,
			176,
			134,
			126,
			109,
			112,
			109,
			144,
			110,
			176,
			106,
			133,
			105,
			176,
			103
		})]
		
		public virtual void setWeights(List newWeights)
		{
			if (newWeights == null || newWeights.size() == 0)
			{
				this.weights = null;
				return;
			}
			if (newWeights.size() != this.rules.size())
			{
				string text = "weights/rules array length mismatch";
				
				throw new IllegalArgumentException(text);
			}
			float num = 0f;
			Iterator iterator = newWeights.iterator();
			while (iterator.hasNext())
			{
				Float @float = (Float)iterator.next();
				if (Float.isNaN(@float.floatValue()))
				{
					string text2 = "illegal weight value: NaN";
					
					throw new IllegalArgumentException(text2);
				}
				if (Float.isInfinite(@float.floatValue()))
				{
					string text3 = "illegal weight value: infinite";
					
					throw new IllegalArgumentException(text3);
				}
				if ((double)@float.floatValue() < (double)0f)
				{
					string text4 = "illegal weight value: negative";
					
					throw new IllegalArgumentException(text4);
				}
				num += @float.floatValue();
			}
			if ((double)num <= (double)0f)
			{
				string text5 = "illegal weight values: all zero";
				
				throw new IllegalArgumentException(text5);
			}
			this.weights = newWeights;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			189,
			123,
			135,
			103
		})]
		
		public virtual void setRules(List rules)
		{
			if (this.weights != null && rules.size() != this.weights.size())
			{
				this.weights = null;
			}
			this.rules = rules;
		}

		[LineNumberTable(new byte[]
		{
			159,
			158,
			102
		})]
		
		public JSGFRuleAlternatives()
		{
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		
		[LineNumberTable(new byte[]
		{
			159,
			167,
			104,
			127,
			1,
			103,
			103
		})]
		
		public JSGFRuleAlternatives(List rules, List weights)
		{
			if (!JSGFRuleAlternatives.assertionsDisabled && rules.size() != weights.size())
			{
				
				throw new AssertionError();
			}
			this.setRules(rules);
			this.setWeights(weights);
		}

		[LineNumberTable(new byte[]
		{
			38,
			117,
			134,
			134,
			115,
			100,
			140,
			104,
			159,
			23,
			114,
			115,
			158,
			232,
			53,
			233,
			78
		})]
		
		public override string toString()
		{
			if (this.rules == null || this.rules.size() == 0)
			{
				return "<VOID>";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.rules.size(); i++)
			{
				if (i > 0)
				{
					stringBuilder.append(" | ");
				}
				if (this.weights != null)
				{
					stringBuilder.append(new StringBuilder().append("/").append(this.weights.get(i)).append("/ ").toString());
				}
				JSGFRule jsgfrule = (JSGFRule)this.rules.get(i);
				if (this.rules.get(i) is JSGFRuleAlternatives)
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

		
		static JSGFRuleAlternatives()
		{
		}

		
		protected internal List rules;

		
		protected internal List weights;

		
		internal static bool assertionsDisabled = !ClassLiteral<JSGFRuleAlternatives>.Value.desiredAssertionStatus();
	}
}

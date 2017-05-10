using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.jsgf.rule
{
	public class JSGFRuleToken : JSGFRule
	{
		public virtual string getText()
		{
			return this.text;
		}

		[LineNumberTable(new byte[]
		{
			159,
			159,
			104,
			103
		})]
		
		public JSGFRuleToken(string text)
		{
			this.setText(text);
		}

		public virtual void setText(string text)
		{
			this.text = text;
		}

		[LineNumberTable(new byte[]
		{
			159,
			164,
			107,
			110,
			2,
			198
		})]
		
		private bool containsWhiteSpace(string text)
		{
			for (int i = 0; i < java.lang.String.instancehelper_length(text); i++)
			{
				if (Character.isWhitespace(java.lang.String.instancehelper_charAt(text, i)))
				{
					return true;
				}
			}
			return false;
		}

		[LineNumberTable(new byte[]
		{
			159,
			155,
			104,
			103
		})]
		
		public JSGFRuleToken()
		{
			this.setText(null);
		}

		[LineNumberTable(new byte[]
		{
			159,
			181,
			127,
			7,
			107,
			140,
			141,
			104,
			106,
			234,
			60,
			230,
			71,
			106,
			137,
			135
		})]
		
		public override string toString()
		{
			if (this.containsWhiteSpace(this.text) || java.lang.String.instancehelper_indexOf(this.text, 92) >= 0 || java.lang.String.instancehelper_indexOf(this.text, 34) >= 0)
			{
				StringBuilder stringBuilder = new StringBuilder(this.text);
				for (int i = stringBuilder.length() - 1; i >= 0; i += -1)
				{
					int num = (int)stringBuilder.charAt(i);
					if (num == 34 || num == 92)
					{
						stringBuilder.insert(i, '\\');
					}
				}
				stringBuilder.insert(0, '"');
				stringBuilder.append('"');
				return stringBuilder.toString();
			}
			return this.text;
		}

		protected internal string text;
	}
}

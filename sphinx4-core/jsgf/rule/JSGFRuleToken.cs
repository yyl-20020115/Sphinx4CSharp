using java.lang;

namespace edu.cmu.sphinx.jsgf.rule
{
	public class JSGFRuleToken : JSGFRule
	{
		public virtual string getText()
		{
			return this.text;
		}
		
		public JSGFRuleToken(string text)
		{
			this.setText(text);
		}

		public virtual void setText(string text)
		{
			this.text = text;
		}
		
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
		
		public JSGFRuleToken()
		{
			this.setText(null);
		}
		
		public override string toString()
		{
			if (this.containsWhiteSpace(this.text) || java.lang.String.instancehelper_indexOf(this.text, 92) >= 0 || java.lang.String.instancehelper_indexOf(this.text, 34) >= 0)
			{
				StringBuilder stringBuilder = new StringBuilder(this.text);
				for (int i = stringBuilder.length() - 1; i >= 0; i --)
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

using java.lang;
using java.util;

namespace edu.cmu.sphinx.jsgf.rule
{
	public class JSGFRuleName : JSGFRule
	{
		public virtual string getRuleName()
		{
			return this.fullRuleName;
		}

		public virtual string getFullGrammarName()
		{
			if (this.packageName != null)
			{
				return new StringBuilder().append(this.packageName).append(".").append(this.simpleGrammarName).toString();
			}
			return this.simpleGrammarName;
		}

		public virtual string getSimpleRuleName()
		{
			return this.simpleRuleName;
		}

		public JSGFRuleName(string name)
		{
			this.setRuleName(name);
		}
		
		public override string toString()
		{
			return new StringBuilder().append("<").append(this.fullRuleName).append(">").toString();
		}

		public virtual string getSimpleGrammarName()
		{
			return this.simpleGrammarName;
		}

		public virtual string getPackageName()
		{
			return this.packageName;
		}
		
		public virtual void setRuleName(string ruleName)
		{
			string text = JSGFRuleName.stripRuleName(ruleName);
			this.fullRuleName = text;
			int num = String.instancehelper_lastIndexOf(text, 46);
			if (num < 0)
			{
				this.packageName = null;
				this.simpleGrammarName = null;
				this.simpleRuleName = text;
			}
			else
			{
				int num2 = String.instancehelper_lastIndexOf(text, 46, num - 1);
				if (num2 < 0)
				{
					this.packageName = null;
					this.simpleGrammarName = String.instancehelper_substring(text, 0, num);
					this.simpleRuleName = String.instancehelper_substring(text, num + 1);
				}
				else
				{
					this.packageName = String.instancehelper_substring(text, 0, num2);
					this.simpleGrammarName = String.instancehelper_substring(text, num2 + 1, num);
					this.simpleRuleName = String.instancehelper_substring(text, num + 1);
				}
			}
		}
		
		public static bool isLegalRuleName(string name)
		{
			if (name == null)
			{
				return false;
			}
			name = JSGFRuleName.stripRuleName(name);
			if (String.instancehelper_endsWith(name, ".*"))
			{
				name = String.instancehelper_substring(name, 0, String.instancehelper_length(name) - 2);
			}
			if (String.instancehelper_length(name) == 0)
			{
				return false;
			}
			if (String.instancehelper_startsWith(name, ".") || String.instancehelper_endsWith(name, ".") || String.instancehelper_indexOf(name, "..") >= 0)
			{
				return false;
			}
			StringTokenizer stringTokenizer = new StringTokenizer(name, ".");
			while (stringTokenizer.hasMoreTokens())
			{
				string text = stringTokenizer.nextToken();
				int num = String.instancehelper_length(text);
				if (num == 0)
				{
					return false;
				}
				for (int i = 0; i < num; i++)
				{
					if (!JSGFRuleName.isRuleNamePart(String.instancehelper_charAt(text, i)))
					{
						return false;
					}
				}
			}
			return true;
		}
		
		public static string stripRuleName(string name)
		{
			if (String.instancehelper_startsWith(name, "<") && String.instancehelper_endsWith(name, ">"))
			{
				return String.instancehelper_substring(name, 1, String.instancehelper_length(name) - 1);
			}
			return name;
		}
		
		public static bool isRuleNamePart(char c)
		{
			return Character.isJavaIdentifierPart(c) || (c == '!' || c == '#' || c == '%' || c == '&' || c == '(' || c == ')' || c == '+' || c == ',' || c == '-' || c == '/' || c == ':' || c == ';' || c == '=' || c == '@' || c == '[' || c == '\\' || c == ']' || c == '^' || c == '|' || c == '~');
		}
		
		public JSGFRuleName() : this("NULL")
		{
		}
		
		public virtual bool isLegalRuleName()
		{
			return JSGFRuleName.isLegalRuleName(this.fullRuleName);
		}
		
		public static JSGFRuleName NULL
		{
			get
			{
				return JSGFRuleName.__NULL;
			}
		}
		
		public static JSGFRuleName VOID
		{
			get
			{
				return JSGFRuleName.__VOID;
			}
		}

		protected internal string fullRuleName;

		protected internal string packageName;

		protected internal string simpleGrammarName;

		protected internal string simpleRuleName;

		public string resolvedRuleName;

		internal static JSGFRuleName __NULL = new JSGFRuleName("NULL");

		internal static JSGFRuleName __VOID = new JSGFRuleName("VOID");
	}
}

using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.jsgf.rule
{
	public class JSGFRuleName : JSGFRule
	{
		
		public static void __<clinit>()
		{
		}

		public virtual string getRuleName()
		{
			return this.fullRuleName;
		}

		[LineNumberTable(new byte[]
		{
			159,
			179,
			104,
			191,
			12
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			172,
			136,
			103
		})]
		
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

		[LineNumberTable(new byte[]
		{
			67,
			103,
			135,
			137,
			100,
			103,
			103,
			140,
			140,
			100,
			103,
			110,
			145,
			110,
			112,
			175
		})]
		
		public virtual void setRuleName(string ruleName)
		{
			string text = JSGFRuleName.stripRuleName(ruleName);
			this.fullRuleName = text;
			int num = java.lang.String.instancehelper_lastIndexOf(text, 46);
			if (num < 0)
			{
				this.packageName = null;
				this.simpleGrammarName = null;
				this.simpleRuleName = text;
			}
			else
			{
				int num2 = java.lang.String.instancehelper_lastIndexOf(text, 46, num - 1);
				if (num2 < 0)
				{
					this.packageName = null;
					this.simpleGrammarName = java.lang.String.instancehelper_substring(text, 0, num);
					this.simpleRuleName = java.lang.String.instancehelper_substring(text, num + 1);
				}
				else
				{
					this.packageName = java.lang.String.instancehelper_substring(text, 0, num2);
					this.simpleGrammarName = java.lang.String.instancehelper_substring(text, num2 + 1, num);
					this.simpleRuleName = java.lang.String.instancehelper_substring(text, num + 1);
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			15,
			99,
			130,
			136,
			109,
			177,
			104,
			162,
			127,
			1,
			104,
			162,
			140,
			104,
			135,
			135,
			99,
			130,
			102,
			110,
			2,
			198,
			98
		})]
		
		public static bool isLegalRuleName(string name)
		{
			if (name == null)
			{
				return false;
			}
			name = JSGFRuleName.stripRuleName(name);
			if (java.lang.String.instancehelper_endsWith(name, ".*"))
			{
				name = java.lang.String.instancehelper_substring(name, 0, java.lang.String.instancehelper_length(name) - 2);
			}
			if (java.lang.String.instancehelper_length(name) == 0)
			{
				return false;
			}
			if (java.lang.String.instancehelper_startsWith(name, ".") || java.lang.String.instancehelper_endsWith(name, ".") || java.lang.String.instancehelper_indexOf(name, "..") >= 0)
			{
				return false;
			}
			StringTokenizer stringTokenizer = new StringTokenizer(name, ".");
			while (stringTokenizer.hasMoreTokens())
			{
				string text = stringTokenizer.nextToken();
				int num = java.lang.String.instancehelper_length(text);
				if (num == 0)
				{
					return false;
				}
				for (int i = 0; i < num; i++)
				{
					if (!JSGFRuleName.isRuleNamePart(java.lang.String.instancehelper_charAt(text, i)))
					{
						return false;
					}
				}
			}
			return true;
		}

		[LineNumberTable(new byte[]
		{
			92,
			122,
			144
		})]
		
		public static string stripRuleName(string name)
		{
			if (java.lang.String.instancehelper_startsWith(name, "<") && java.lang.String.instancehelper_endsWith(name, ">"))
			{
				return java.lang.String.instancehelper_substring(name, 1, java.lang.String.instancehelper_length(name) - 1);
			}
			return name;
		}

		[LineNumberTable(new byte[]
		{
			159,
			117,
			130,
			104,
			130
		})]
		
		public static bool isRuleNamePart(char c)
		{
			return Character.isJavaIdentifierPart(c) || (c == '!' || c == '#' || c == '%' || c == '&' || c == '(' || c == ')' || c == '+' || c == ',' || c == '-' || c == '/' || c == ':' || c == ';' || c == '=' || c == '@' || c == '[' || c == '\\' || c == ']' || c == '^' || c == '|' || c == '~');
		}

		[LineNumberTable(new byte[]
		{
			159,
			169,
			107
		})]
		
		public JSGFRuleName() : this("NULL")
		{
		}

		
		
		public virtual bool isLegalRuleName()
		{
			return JSGFRuleName.isLegalRuleName(this.fullRuleName);
		}

		[LineNumberTable(new byte[]
		{
			159,
			164,
			143
		})]
		static JSGFRuleName()
		{
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

using System;

using edu.cmu.sphinx.jsgf.rule;
using IKVM.Attributes;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.regex;

namespace edu.cmu.sphinx.jsgf
{
	public class JSGFRuleGrammar : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.JSGFGrammarException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			151,
			145,
			103,
			103,
			103,
			168,
			102,
			191,
			6,
			114,
			166,
			114,
			198,
			109,
			223,
			14,
			100,
			111,
			100,
			138,
			255,
			10,
			74,
			231,
			69,
			109,
			191,
			18,
			159,
			4,
			105,
			105,
			169,
			100,
			223,
			6,
			111,
			100,
			127,
			26,
			197,
			121,
			127,
			26,
			197,
			249,
			71,
			110,
			141,
			255,
			22,
			71,
			173,
			255,
			22,
			72,
			100,
			229,
			70,
			110,
			141,
			255,
			22,
			72,
			106,
			127,
			17,
			133,
			165,
			152,
			130,
			142,
			103,
			127,
			19,
			127,
			1,
			116,
			98,
			112
		})]
		
		public virtual JSGFRuleName resolve(JSGFRuleName ruleName)
		{
			JSGFRuleName.__<clinit>();
			JSGFRuleName jsgfruleName = new JSGFRuleName(ruleName.getRuleName());
			string simpleRuleName = jsgfruleName.getSimpleRuleName();
			string simpleGrammarName = jsgfruleName.getSimpleGrammarName();
			string packageName = jsgfruleName.getPackageName();
			string fullGrammarName = jsgfruleName.getFullGrammarName();
			if (packageName != null && simpleGrammarName == null)
			{
				string message = new StringBuilder().append("Error: badly formed rulename ").append(jsgfruleName).toString();
				
				throw new JSGFGrammarException(message);
			}
			if (java.lang.String.instancehelper_equals(ruleName.getSimpleRuleName(), "NULL"))
			{
				return JSGFRuleName.__NULL;
			}
			if (java.lang.String.instancehelper_equals(ruleName.getSimpleRuleName(), "VOID"))
			{
				return JSGFRuleName.__VOID;
			}
			if (fullGrammarName == null && this.getRule(simpleRuleName) != null)
			{
				JSGFRuleName.__<clinit>();
				return new JSGFRuleName(new StringBuilder().append(this.name).append('.').append(simpleRuleName).toString());
			}
			if (fullGrammarName != null)
			{
				JSGFRuleGrammar jsgfruleGrammar = this.manager.retrieveGrammar(fullGrammarName);
				if (jsgfruleGrammar != null && jsgfruleGrammar.getRule(simpleRuleName) != null)
				{
					JSGFRuleName.__<clinit>();
					return new JSGFRuleName(new StringBuilder().append(fullGrammarName).append('.').append(simpleRuleName).toString());
				}
			}
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList(this.__imports);
			List list = arrayList2;
			JSGFRuleName.__<clinit>();
			list.add(new JSGFRuleName(new StringBuilder().append(this.name).append(".*").toString()));
			Iterator iterator = arrayList2.iterator();
			while (iterator.hasNext())
			{
				JSGFRuleName jsgfruleName2 = (JSGFRuleName)iterator.next();
				string simpleRuleName2 = jsgfruleName2.getSimpleRuleName();
				string simpleGrammarName2 = jsgfruleName2.getSimpleGrammarName();
				string fullGrammarName2 = jsgfruleName2.getFullGrammarName();
				if (fullGrammarName2 == null)
				{
					string message2 = new StringBuilder().append("Error: badly formed import ").append(ruleName).toString();
					
					throw new JSGFGrammarException(message2);
				}
				JSGFRuleGrammar jsgfruleGrammar2 = this.manager.retrieveGrammar(fullGrammarName2);
				if (jsgfruleGrammar2 == null)
				{
					java.lang.System.@out.println(new StringBuilder().append("Warning: import of unknown grammar ").append(ruleName).append(" in ").append(this.name).toString());
				}
				else if (!java.lang.String.instancehelper_equals(simpleRuleName2, "*") && jsgfruleGrammar2.getRule(simpleRuleName2) == null)
				{
					java.lang.System.@out.println(new StringBuilder().append("Warning: import of undefined rule ").append(ruleName).append(" in ").append(this.name).toString());
				}
				else if (java.lang.String.instancehelper_equals(fullGrammarName2, fullGrammarName) || java.lang.String.instancehelper_equals(simpleGrammarName2, fullGrammarName))
				{
					if (java.lang.String.instancehelper_equals(simpleRuleName2, "*"))
					{
						if (jsgfruleGrammar2.getRule(simpleRuleName) != null)
						{
							List list2 = arrayList;
							JSGFRuleName.__<clinit>();
							list2.add(new JSGFRuleName(new StringBuilder().append(fullGrammarName2).append('.').append(simpleRuleName).toString()));
						}
					}
					else if (java.lang.String.instancehelper_equals(simpleRuleName2, simpleRuleName))
					{
						List list3 = arrayList;
						JSGFRuleName.__<clinit>();
						list3.add(new JSGFRuleName(new StringBuilder().append(fullGrammarName2).append('.').append(simpleRuleName).toString()));
					}
				}
				else if (fullGrammarName == null)
				{
					if (java.lang.String.instancehelper_equals(simpleRuleName2, "*"))
					{
						if (jsgfruleGrammar2.getRule(simpleRuleName) != null)
						{
							List list4 = arrayList;
							JSGFRuleName.__<clinit>();
							list4.add(new JSGFRuleName(new StringBuilder().append(fullGrammarName2).append('.').append(simpleRuleName).toString()));
						}
					}
					else if (java.lang.String.instancehelper_equals(simpleRuleName2, simpleRuleName))
					{
						List list5 = arrayList;
						JSGFRuleName.__<clinit>();
						list5.add(new JSGFRuleName(new StringBuilder().append(fullGrammarName2).append('.').append(simpleRuleName).toString()));
					}
				}
			}
			int num = arrayList.size();
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return (JSGFRuleName)arrayList.get(0);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.append("Warning: ambiguous reference ").append(jsgfruleName).append(" in ").append(this.name).append(" to ");
			Iterator iterator2 = arrayList.iterator();
			while (iterator2.hasNext())
			{
				JSGFRuleName jsgfruleName3 = (JSGFRuleName)iterator2.next();
				stringBuilder.append(jsgfruleName3).append(" and ");
			}
			stringBuilder.setLength(stringBuilder.length() - 5);
			string message3 = stringBuilder.toString();
			
			throw new JSGFGrammarException(message3);
		}

		[LineNumberTable(new byte[]
		{
			160,
			85,
			114,
			99,
			130
		})]
		
		public virtual JSGFRule getRule(string ruleName)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			if (jsgfruleState == null)
			{
				return null;
			}
			return jsgfruleState.rule;
		}

		
		
		
		public virtual Set getRuleNames()
		{
			return this.__rules.keySet();
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			108,
			114,
			99,
			130
		})]
		
		public virtual bool isRulePublic(string ruleName)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			return jsgfruleState != null && jsgfruleState.isPublic;
		}

		[LineNumberTable(new byte[]
		{
			159,
			16,
			130,
			127,
			6,
			103,
			98
		})]
		
		public virtual void setEnabled(bool enabled)
		{
			Iterator iterator = this.__rules.values().iterator();
			while (iterator.hasNext())
			{
				JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)iterator.next();
				jsgfruleState.isEnabled = enabled;
			}
		}

		
		public virtual List getImports()
		{
			return this.__imports;
		}

		[LineNumberTable(new byte[]
		{
			160,
			66,
			114,
			99,
			159,
			6
		})]
		
		private JSGFRule getKnownRule(string text)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(text);
			if (jsgfruleState == null)
			{
				string text2 = new StringBuilder().append("Unknown Rule: ").append(text).toString();
				
				throw new IllegalArgumentException(text2);
			}
			return jsgfruleState.rule;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.JSGFGrammarException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			70,
			104,
			161,
			104,
			127,
			6,
			103,
			98,
			161,
			104,
			127,
			6,
			103,
			98,
			161,
			104,
			113,
			161,
			107,
			135,
			103,
			167,
			115,
			105,
			103,
			148,
			159,
			6,
			103,
			161,
			107,
			104,
			138,
			100,
			223,
			28,
			110,
			110,
			193
		})]
		
		protected internal virtual void resolveRule(JSGFRule r)
		{
			if (r is JSGFRuleToken)
			{
				return;
			}
			if (r is JSGFRuleAlternatives)
			{
				Iterator iterator = ((JSGFRuleAlternatives)r).getRules().iterator();
				while (iterator.hasNext())
				{
					JSGFRule jsgfrule = (JSGFRule)iterator.next();
					this.resolveRule(jsgfrule);
				}
				return;
			}
			if (r is JSGFRuleSequence)
			{
				Iterator iterator = ((JSGFRuleSequence)r).getRules().iterator();
				while (iterator.hasNext())
				{
					JSGFRule jsgfrule = (JSGFRule)iterator.next();
					this.resolveRule(jsgfrule);
				}
				return;
			}
			if (r is JSGFRuleCount)
			{
				this.resolveRule(((JSGFRuleCount)r).getRule());
				return;
			}
			if (r is JSGFRuleTag)
			{
				JSGFRuleTag jsgfruleTag = (JSGFRuleTag)r;
				JSGFRule jsgfrule = jsgfruleTag.getRule();
				string text = jsgfrule.toString();
				object obj = (Collection)this.__ruleTags.get(text);
				if ((Collection)obj == null)
				{
					obj = new HashSet();
					this.__ruleTags.put(text, (HashSet)obj);
				}
				object obj2 = obj;
				object tag = jsgfruleTag.getTag();
				Collection collection;
				if (obj2 != null)
				{
					if ((collection = (obj2 as Collection)) == null)
					{
						throw new IncompatibleClassChangeError();
					}
				}
				else
				{
					collection = null;
				}
				collection.add(tag);
				this.resolveRule(jsgfrule);
				return;
			}
			if (!(r is JSGFRuleName))
			{
				string message = "Unknown rule type";
				
				throw new JSGFGrammarException(message);
			}
			JSGFRuleName jsgfruleName = (JSGFRuleName)r;
			JSGFRuleName jsgfruleName2 = this.resolve(jsgfruleName);
			if (jsgfruleName2 == null)
			{
				string message2 = new StringBuilder().append("Unresolvable rulename in grammar ").append(this.name).append(": ").append(jsgfruleName).toString();
				
				throw new JSGFGrammarException(message2);
			}
			jsgfruleName.resolvedRuleName = jsgfruleName2.getRuleName();
			jsgfruleName.setRuleName(jsgfruleName2.getRuleName());
		}

		[LineNumberTable(new byte[]
		{
			161,
			233,
			107,
			99,
			103,
			127,
			11,
			127,
			13,
			118,
			126,
			105,
			63,
			0,
			168,
			108,
			135
		})]
		
		private string formatComment(string text)
		{
			StringBuilder stringBuilder = new StringBuilder("");
			if (text == null)
			{
				return stringBuilder.toString();
			}
			Pattern pattern = Pattern.compile("[\\n\\r\\f]+");
			CharSequence charSequence;
			charSequence.__ref = text;
			if (pattern.matcher(charSequence).find())
			{
				string[] array = java.lang.String.instancehelper_split(text, new StringBuilder().append('[').append(JSGFRuleGrammar.LINE_SEPARATOR).append("]+").toString());
				stringBuilder.append("/**").append(JSGFRuleGrammar.LINE_SEPARATOR);
				stringBuilder.append("  *").append(array[0]).append(JSGFRuleGrammar.LINE_SEPARATOR);
				for (int i = 1; i < array.Length; i++)
				{
					stringBuilder.append("  *").append(array[i]).append(JSGFRuleGrammar.LINE_SEPARATOR);
				}
				stringBuilder.append("  */");
				return stringBuilder.toString();
			}
			return new StringBuilder().append("//").append(text).toString();
		}

		[LineNumberTable(new byte[]
		{
			161,
			187,
			102,
			118,
			108,
			115,
			108,
			127,
			9,
			140,
			108,
			115,
			127,
			21,
			105,
			126,
			108,
			127,
			19,
			236,
			58,
			233,
			73,
			108,
			127,
			13,
			105,
			115,
			159,
			10,
			110,
			105,
			140,
			127,
			24,
			108,
			101
		})]
		
		public override string toString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.append("#JSGF V1.0;").append(JSGFRuleGrammar.LINE_SEPARATOR);
			stringBuilder.append(JSGFRuleGrammar.LINE_SEPARATOR);
			stringBuilder.append(this.formatComment(this.grammarDocComment));
			stringBuilder.append(JSGFRuleGrammar.LINE_SEPARATOR);
			stringBuilder.append("grammar ").append(this.name).append(';').append(JSGFRuleGrammar.LINE_SEPARATOR);
			stringBuilder.append(JSGFRuleGrammar.LINE_SEPARATOR);
			Set set = this.importDocComments.keySet();
			for (int i = 0; i < this.__imports.size(); i++)
			{
				string text = new StringBuilder().append('<').append(((JSGFRuleName)this.__imports.get(i)).getRuleName()).append('>').toString();
				if (set.contains(text))
				{
					stringBuilder.append(this.formatComment((string)this.importDocComments.get(text)));
					stringBuilder.append(JSGFRuleGrammar.LINE_SEPARATOR);
					stringBuilder.append("import ").append(new StringBuilder().append(text).append(';').toString()).append(JSGFRuleGrammar.LINE_SEPARATOR);
					stringBuilder.append(JSGFRuleGrammar.LINE_SEPARATOR);
				}
			}
			set = this.ruleDocComments.keySet();
			Iterator iterator = this.__rules.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				object key = entry.getKey();
				if (set.size() > 0 && set.contains(key))
				{
					stringBuilder.append(this.formatComment((string)this.ruleDocComments.get(key))).append(JSGFRuleGrammar.LINE_SEPARATOR);
				}
				JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)entry.getValue();
				if (jsgfruleState.isPublic)
				{
					stringBuilder.append("public ");
				}
				stringBuilder.append('<').append(key).append("> = ").append(jsgfruleState.rule).append(';').append(JSGFRuleGrammar.LINE_SEPARATOR);
				stringBuilder.append(JSGFRuleGrammar.LINE_SEPARATOR);
			}
			return stringBuilder.toString();
		}

		[LineNumberTable(new byte[]
		{
			34,
			232,
			21,
			107,
			107,
			139,
			235,
			70,
			171,
			235,
			95,
			103,
			103
		})]
		
		public JSGFRuleGrammar(string name, JSGFRuleGrammarManager manager)
		{
			this.__rules = new HashMap();
			this.__imports = new ArrayList();
			this.__importedRules = new ArrayList();
			this.__ruleTags = new HashMap();
			this.ruleDocComments = new Properties();
			this.importDocComments = new Properties();
			this.name = name;
			this.manager = manager;
		}

		public virtual void addGrammarDocComment(string comment)
		{
			this.grammarDocComment = comment;
		}

		[LineNumberTable(new byte[]
		{
			53,
			110,
			141
		})]
		
		public virtual void addImport(JSGFRuleName importName)
		{
			if (!this.__imports.contains(importName))
			{
				this.__imports.add(importName);
			}
		}

		[LineNumberTable(new byte[]
		{
			63,
			115
		})]
		
		public virtual void addImportDocComment(JSGFRuleName imp, string comment)
		{
			this.importDocComments.put(imp.toString(), comment);
		}

		[LineNumberTable(new byte[]
		{
			71,
			110
		})]
		
		public virtual void addRuleDocComment(string rname, string comment)
		{
			this.ruleDocComments.put(rname, comment);
		}

		[LineNumberTable(new byte[]
		{
			81,
			114,
			99,
			129,
			109
		})]
		
		public virtual void addSampleSentence(string ruleName, string sample)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			if (jsgfruleState == null)
			{
				return;
			}
			jsgfruleState.samples.add(sample);
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			95,
			120
		})]
		
		public virtual void deleteRule(string ruleName)
		{
			this.__rules.remove(this.getKnownRule(ruleName).ruleName);
		}

		public virtual string getGrammarDocComment()
		{
			return this.grammarDocComment;
		}

		
		
		public virtual string getImportDocComment(JSGFRuleName imp)
		{
			return this.importDocComments.getProperty(imp.toString(), null);
		}

		
		
		
		public virtual Collection getJSGFTags(string ruleName)
		{
			return (Collection)this.__ruleTags.get(ruleName);
		}

		public virtual string getName()
		{
			return this.name;
		}

		
		
		public virtual string getRuleDocComment(string rname)
		{
			return this.ruleDocComments.getProperty(rname, null);
		}

		[LineNumberTable(new byte[]
		{
			160,
			136,
			110,
			141
		})]
		
		public virtual void removeImport(JSGFRuleName importName)
		{
			if (this.__imports.contains(importName))
			{
				this.__imports.remove(importName);
			}
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.jsgf.JSGFGrammarException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			43,
			166,
			127,
			1,
			103,
			110,
			100,
			159,
			15,
			98,
			105,
			177,
			127,
			7,
			109,
			98
		})]
		
		public virtual void resolveAllRules()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Iterator iterator = this.__imports.iterator();
			while (iterator.hasNext())
			{
				JSGFRuleName jsgfruleName = (JSGFRuleName)iterator.next();
				string fullGrammarName = jsgfruleName.getFullGrammarName();
				if (this.manager.retrieveGrammar(fullGrammarName) == null)
				{
					stringBuilder.append("Undefined grammar ").append(fullGrammarName).append(" imported in ").append(this.name).append('\n');
				}
			}
			if (stringBuilder.length() > 0)
			{
				string message = stringBuilder.toString();
				
				throw new JSGFGrammarException(message);
			}
			iterator = this.__rules.values().iterator();
			while (iterator.hasNext())
			{
				JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)iterator.next();
				this.resolveRule(jsgfruleState.rule);
			}
		}

		[LineNumberTable(new byte[]
		{
			161,
			142,
			114,
			99,
			135
		})]
		
		public virtual bool isEnabled(string ruleName)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			return jsgfruleState != null && jsgfruleState.isEnabled;
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			10,
			66,
			114,
			105,
			135
		})]
		
		public virtual void setEnabled(string ruleName, bool enabled)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			if (jsgfruleState.isEnabled != enabled)
			{
				jsgfruleState.isEnabled = enabled;
			}
		}

		[Throws(new string[]
		{
			"java.lang.NullPointerException",
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			6,
			130,
			106,
			110
		})]
		
		public virtual void setRule(string ruleName, JSGFRule rule, bool isPublic)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = new JSGFRuleGrammar.JSGFRuleState(this, rule, true, isPublic);
			this.__rules.put(ruleName, jsgfruleState);
		}

		[Throws(new string[]
		{
			"java.net.URISyntaxException",
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			7,
			118,
			108,
			102,
			102
		})]
		
		public virtual void saveJSGF(URL url)
		{
			File.__<clinit>();
			PrintStream printStream = new PrintStream(new File(url.toURI()));
			printStream.print(this.toString());
			printStream.flush();
			printStream.close();
		}

		[LineNumberTable(new byte[]
		{
			162,
			14,
			114
		})]
		
		public virtual bool isRuleChanged(string ruleName)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			return jsgfruleState.isChanged;
		}

		[LineNumberTable(new byte[]
		{
			158,
			237,
			98,
			114,
			103
		})]
		
		public virtual void setRuleChanged(string ruleName, bool changed)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			jsgfruleState.isChanged = changed;
		}

		
		static JSGFRuleGrammar()
		{
		}

		
		protected internal Map rules
		{
			
			get
			{
				return this.__rules;
			}
			
			private set
			{
				this.__rules = value;
			}
		}

		
		protected internal List imports
		{
			
			get
			{
				return this.__imports;
			}
			
			private set
			{
				this.__imports = value;
			}
		}

		
		protected internal List importedRules
		{
			
			get
			{
				return this.__importedRules;
			}
			
			private set
			{
				this.__importedRules = value;
			}
		}

		
		protected internal Map ruleTags
		{
			
			get
			{
				return this.__ruleTags;
			}
			
			private set
			{
				this.__ruleTags = value;
			}
		}

		
		private static string LINE_SEPARATOR = java.lang.System.getProperty("line.separator");

		
		internal Map __rules;

		
		internal List __imports;

		
		internal List __importedRules;

		
		internal Map __ruleTags;

		private string name;

		private JSGFRuleGrammarManager manager;

		internal Properties ruleDocComments;

		internal Properties importDocComments;

		internal string grammarDocComment;

		
		[SourceFile("JSGFRuleGrammar.java")]
		
		internal sealed class JSGFRuleState : java.lang.Object
		{
			[LineNumberTable(new byte[]
			{
				159,
				125,
				69,
				111,
				103,
				103,
				103,
				107
			})]
			
			public JSGFRuleState(JSGFRuleGrammar jsgfruleGrammar, JSGFRule jsgfrule, bool flag, bool flag2)
			{
				this.rule = jsgfrule;
				this.isPublic = flag2;
				this.isEnabled = flag;
				this.samples = new ArrayList();
			}

			public bool isPublic;

			public bool isEnabled;

			public JSGFRule rule;

			
			public ArrayList samples;

			public bool isChanged;

			
			internal JSGFRuleGrammar this$0 = jsgfruleGrammar;
		}
	}
}

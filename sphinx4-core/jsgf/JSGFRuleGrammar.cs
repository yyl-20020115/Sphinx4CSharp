using edu.cmu.sphinx.jsgf.rule;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.regex;

namespace edu.cmu.sphinx.jsgf
{
	public class JSGFRuleGrammar : java.lang.Object
	{
		public virtual JSGFRuleName resolve(JSGFRuleName ruleName)
		{
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
				return new JSGFRuleName(new StringBuilder().append(this.name).append('.').append(simpleRuleName).toString());
			}
			if (fullGrammarName != null)
			{
				JSGFRuleGrammar jsgfruleGrammar = this.manager.retrieveGrammar(fullGrammarName);
				if (jsgfruleGrammar != null && jsgfruleGrammar.getRule(simpleRuleName) != null)
				{
					return new JSGFRuleName(new StringBuilder().append(fullGrammarName).append('.').append(simpleRuleName).toString());
				}
			}
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList(this.__imports);
			List list = arrayList2;
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
							list2.add(new JSGFRuleName(new StringBuilder().append(fullGrammarName2).append('.').append(simpleRuleName).toString()));
						}
					}
					else if (java.lang.String.instancehelper_equals(simpleRuleName2, simpleRuleName))
					{
						List list3 = arrayList;
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
							list4.add(new JSGFRuleName(new StringBuilder().append(fullGrammarName2).append('.').append(simpleRuleName).toString()));
						}
					}
					else if (java.lang.String.instancehelper_equals(simpleRuleName2, simpleRuleName))
					{
						List list5 = arrayList;
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
		
		public virtual bool isRulePublic(string ruleName)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			return jsgfruleState != null && jsgfruleState.isPublic;
		}
		
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
		
		private string formatComment(string text)
		{
			StringBuilder stringBuilder = new StringBuilder("");
			if (text == null)
			{
				return stringBuilder.toString();
			}
			Pattern pattern = Pattern.compile("[\\n\\r\\f]+");
			CharSequence charSequence = CharSequence.Cast(text);
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
		
		public virtual void addImport(JSGFRuleName importName)
		{
			if (!this.__imports.contains(importName))
			{
				this.__imports.add(importName);
			}
		}
		
		public virtual void addImportDocComment(JSGFRuleName imp, string comment)
		{
			this.importDocComments.put(imp.toString(), comment);
		}
		
		public virtual void addRuleDocComment(string rname, string comment)
		{
			this.ruleDocComments.put(rname, comment);
		}
		
		public virtual void addSampleSentence(string ruleName, string sample)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			if (jsgfruleState == null)
			{
				return;
			}
			jsgfruleState.samples.add(sample);
		}
		
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
		
		public virtual void removeImport(JSGFRuleName importName)
		{
			if (this.__imports.contains(importName))
			{
				this.__imports.remove(importName);
			}
		}
		
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
		
		public virtual bool isEnabled(string ruleName)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			return jsgfruleState != null && jsgfruleState.isEnabled;
		}

		public virtual void setEnabled(string ruleName, bool enabled)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			if (jsgfruleState.isEnabled != enabled)
			{
				jsgfruleState.isEnabled = enabled;
			}
		}
		
		public virtual void setRule(string ruleName, JSGFRule rule, bool isPublic)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = new JSGFRuleGrammar.JSGFRuleState(this, rule, true, isPublic);
			this.__rules.put(ruleName, jsgfruleState);
		}
		
		public virtual void saveJSGF(URL url)
		{
			PrintStream printStream = new PrintStream(new File(url.toURI()));
			printStream.print(this.toString());
			printStream.flush();
			printStream.close();
		}
		
		public virtual bool isRuleChanged(string ruleName)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			return jsgfruleState.isChanged;
		}
		
		public virtual void setRuleChanged(string ruleName, bool changed)
		{
			JSGFRuleGrammar.JSGFRuleState jsgfruleState = (JSGFRuleGrammar.JSGFRuleState)this.__rules.get(ruleName);
			jsgfruleState.isChanged = changed;
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
		
		internal sealed class JSGFRuleState : java.lang.Object
		{			
			public JSGFRuleState(JSGFRuleGrammar jsgfruleGrammar, JSGFRule jsgfrule, bool flag, bool flag2)
			{
				this.this_0 = jsgfruleGrammar;
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

			internal JSGFRuleGrammar this_0;
		}
	}
}

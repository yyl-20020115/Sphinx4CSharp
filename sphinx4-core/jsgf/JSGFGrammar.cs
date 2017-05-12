using edu.cmu.sphinx.jsgf.parser;
using edu.cmu.sphinx.jsgf.rule;
using edu.cmu.sphinx.linguist.language.grammar;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.jsgf
{
	public class JSGFGrammar : Grammar
	{		
		public JSGFGrammar(URL baseURL, string grammarName, bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, linguist.dictionary.Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
			this.loadGrammar = true;
			this.logger = Logger.getLogger(Object.instancehelper_getClass(this).getName());
			this.logMath = LogMath.getLogMath();
			this.baseURL = baseURL;
			this.grammarName = grammarName;
			this.loadGrammar = true;
		}
		
		public virtual void commitChanges()
		{
			try
			{
				if (this.loadGrammar)
				{
					if (this.manager == null)
					{
						this.getGrammarManager();
					}
					this.ruleGrammar = this.loadNamedGrammar(this.grammarName);
					this.loadImports(this.ruleGrammar);
					this.loadGrammar = false;
				}
				this.manager.linkGrammars();
				this.__ruleStack = new JSGFGrammar.RuleStack(this);
				this.newGrammar();
				this.firstNode = this.createGrammarNode("<sil>");
				GrammarNode grammarNode = this.createGrammarNode("<sil>");
				grammarNode.setFinalNode(true);
				Iterator iterator = this.ruleGrammar.getRuleNames().iterator();
				while (iterator.hasNext())
				{
					string text = (string)iterator.next();
					if (this.ruleGrammar.isRulePublic(text))
					{
						string fullRuleName = this.getFullRuleName(text);
						JSGFGrammar.GrammarGraph grammarGraph = new JSGFGrammar.GrammarGraph(this);
						this.__ruleStack.push(fullRuleName, grammarGraph);
						JSGFRule rule = this.ruleGrammar.getRule(text);
						JSGFGrammar.GrammarGraph grammarGraph2 = this.processRule(rule);
						this.__ruleStack.pop();
						this.firstNode.add(grammarGraph.getStartNode(), 0f);
						grammarGraph.getEndNode().add(grammarNode, 0f);
						grammarGraph.getStartNode().add(grammarGraph2.getStartNode(), 0f);
						grammarGraph2.getEndNode().add(grammarGraph.getEndNode(), 0f);
					}
				}
				this.postProcessGrammar();
				if (this.logger.isLoggable(Level.FINEST))
				{
					this.dumpGrammar();
				}
			}
			catch (MalformedURLException ex)
			{
				throw new IOException(new StringBuilder().append("bad base grammar URL ").append(this.baseURL).append(' ').append(ex).toString(),ex);
			}			
		}
		
		private JSGFGrammar.GrammarGraph processRuleAlternatives(JSGFRuleAlternatives jsgfruleAlternatives)
		{
			this.logger.fine(new StringBuilder().append("parseRuleAlternatives: ").append(jsgfruleAlternatives).toString());
			JSGFGrammar.GrammarGraph grammarGraph = new JSGFGrammar.GrammarGraph(this);
			List rules = jsgfruleAlternatives.getRules();
			List normalizedWeights = this.getNormalizedWeights(jsgfruleAlternatives.getWeights());
			for (int i = 0; i < rules.size(); i++)
			{
				JSGFRule jsgfrule = (JSGFRule)rules.get(i);
				float logProbability = 0f;
				if (normalizedWeights != null)
				{
					logProbability = ((Float)normalizedWeights.get(i)).floatValue();
				}
				this.logger.fine(new StringBuilder().append("Alternative: ").append(jsgfrule).toString());
				JSGFGrammar.GrammarGraph grammarGraph2 = this.processRule(jsgfrule);
				grammarGraph.getStartNode().add(grammarGraph2.getStartNode(), logProbability);
				grammarGraph2.getEndNode().add(grammarGraph.getEndNode(), 0f);
			}
			return grammarGraph;
		}
		
		private JSGFGrammar.GrammarGraph processRuleCount(JSGFRuleCount jsgfruleCount)
		{
			this.logger.fine(new StringBuilder().append("parseRuleCount: ").append(jsgfruleCount).toString());
			JSGFGrammar.GrammarGraph grammarGraph = new JSGFGrammar.GrammarGraph(this);
			int count = jsgfruleCount.getCount();
			JSGFGrammar.GrammarGraph grammarGraph2 = this.processRule(jsgfruleCount.getRule());
			grammarGraph.getStartNode().add(grammarGraph2.getStartNode(), 0f);
			grammarGraph2.getEndNode().add(grammarGraph.getEndNode(), 0f);
			if (count == 4 || count == 2)
			{
				grammarGraph.getStartNode().add(grammarGraph.getEndNode(), 0f);
			}
			if (count == 3 || count == 4)
			{
				grammarGraph2.getEndNode().add(grammarGraph2.getStartNode(), 0f);
			}
			return grammarGraph;
		}
		
		private JSGFGrammar.GrammarGraph processRuleName(JSGFRuleName jsgfruleName)
		{
			this.logger.fine(new StringBuilder().append("parseRuleName: ").append(jsgfruleName).toString());
			JSGFGrammar.GrammarGraph grammarGraph = this.__ruleStack.contains(jsgfruleName.getRuleName());
			if (grammarGraph != null)
			{
				return grammarGraph;
			}
			grammarGraph = new JSGFGrammar.GrammarGraph(this);
			this.__ruleStack.push(jsgfruleName.getRuleName(), grammarGraph);
			JSGFRuleName jsgfruleName2 = this.ruleGrammar.resolve(jsgfruleName);
			if (jsgfruleName2 == JSGFRuleName.__NULL)
			{
				grammarGraph.getStartNode().add(grammarGraph.getEndNode(), 0f);
			}
			else if (jsgfruleName2 != JSGFRuleName.__VOID)
			{
				if (jsgfruleName2 == null)
				{
					string message = new StringBuilder().append("Can't resolve ").append(jsgfruleName).append(" g ").append(jsgfruleName.getFullGrammarName()).toString();
					
					throw new JSGFGrammarException(message);
				}
				JSGFRuleGrammar jsgfruleGrammar = this.manager.retrieveGrammar(jsgfruleName2.getFullGrammarName());
				if (jsgfruleGrammar == null)
				{
					string message2 = new StringBuilder().append("Can't resolve grammar name ").append(jsgfruleName2.getFullGrammarName()).toString();
					
					throw new JSGFGrammarException(message2);
				}
				JSGFRule rule = jsgfruleGrammar.getRule(jsgfruleName2.getSimpleRuleName());
				if (rule == null)
				{
					string message3 = new StringBuilder().append("Can't resolve rule: ").append(jsgfruleName2.getRuleName()).toString();
					
					throw new JSGFGrammarException(message3);
				}
				JSGFGrammar.GrammarGraph grammarGraph2 = this.processRule(rule);
				if (grammarGraph != grammarGraph2)
				{
					grammarGraph.getStartNode().add(grammarGraph2.getStartNode(), 0f);
					grammarGraph2.getEndNode().add(grammarGraph.getEndNode(), 0f);
				}
			}
			this.__ruleStack.pop();
			return grammarGraph;
		}
		
		private JSGFGrammar.GrammarGraph processRuleSequence(JSGFRuleSequence jsgfruleSequence)
		{
			GrammarNode grammarNode = null;
			GrammarNode grammarNode2 = null;
			this.logger.fine(new StringBuilder().append("parseRuleSequence: ").append(jsgfruleSequence).toString());
			List rules = jsgfruleSequence.getRules();
			GrammarNode grammarNode3 = null;
			for (int i = 0; i < rules.size(); i++)
			{
				JSGFRule rule = (JSGFRule)rules.get(i);
				JSGFGrammar.GrammarGraph grammarGraph = this.processRule(rule);
				if (i == 0)
				{
					grammarNode = grammarGraph.getStartNode();
				}
				if (i == rules.size() - 1)
				{
					grammarNode2 = grammarGraph.getEndNode();
				}
				if (i > 0)
				{
					grammarNode3.add(grammarGraph.getStartNode(), 0f);
				}
				grammarNode3 = grammarGraph.getEndNode();
			}
			return new JSGFGrammar.GrammarGraph(this, grammarNode, grammarNode2);
		}
		
		private JSGFGrammar.GrammarGraph processRuleTag(JSGFRuleTag jsgfruleTag)
		{
			this.logger.fine(new StringBuilder().append("parseRuleTag: ").append(jsgfruleTag).toString());
			JSGFRule rule = jsgfruleTag.getRule();
			return this.processRule(rule);
		}

		private JSGFGrammar.GrammarGraph processRuleToken(JSGFRuleToken jsgfruleToken)
		{
			GrammarNode grammarNode = this.createGrammarNode(jsgfruleToken.getText());
			return new JSGFGrammar.GrammarGraph(this, grammarNode, grammarNode);
		}
		
		protected internal virtual JSGFGrammar.GrammarGraph processRule(JSGFRule rule)
		{
			if (rule != null)
			{
				this.logger.fine(new StringBuilder().append("parseRule: ").append(rule).toString());
			}
			JSGFGrammar.GrammarGraph result;
			if (rule is JSGFRuleAlternatives)
			{
				result = this.processRuleAlternatives((JSGFRuleAlternatives)rule);
			}
			else if (rule is JSGFRuleCount)
			{
				result = this.processRuleCount((JSGFRuleCount)rule);
			}
			else if (rule is JSGFRuleName)
			{
				result = this.processRuleName((JSGFRuleName)rule);
			}
			else if (rule is JSGFRuleSequence)
			{
				result = this.processRuleSequence((JSGFRuleSequence)rule);
			}
			else if (rule is JSGFRuleTag)
			{
				result = this.processRuleTag((JSGFRuleTag)rule);
			}
			else
			{
				if (!(rule is JSGFRuleToken))
				{
					string text = new StringBuilder().append("Unsupported Rule type: ").append(rule).toString();
					
					throw new IllegalArgumentException(text);
				}
				result = this.processRuleToken((JSGFRuleToken)rule);
			}
			return result;
		}
		
		private List getNormalizedWeights(List list)
		{
			if (list == null)
			{
				return null;
			}
			double num = (double)0f;
			Iterator iterator = list.iterator();
			while (iterator.hasNext())
			{
				float num2 = ((Float)iterator.next()).floatValue();
				if (num2 < 0f)
				{
					string text = new StringBuilder().append("Negative weight ").append(num2).toString();
					
					throw new IllegalArgumentException(text);
				}
				num += (double)num2;
			}
			LinkedList linkedList = new LinkedList(list);
			for (int i = 0; i < list.size(); i++)
			{
				if (num == (double)0f)
				{
					linkedList.set(i, Float.valueOf(float.MinValue));
				}
				else
				{
					linkedList.set(i, Float.valueOf(this.logMath.linearToLog((double)((Float)list.get(i)).floatValue() / num)));
				}
			}
			return linkedList;
		}
		
		public virtual JSGFRuleGrammarManager getGrammarManager()
		{
			if (this.manager == null)
			{
				this.manager = new JSGFRuleGrammarManager();
			}
			return this.manager;
		}
		
		private JSGFRuleGrammar loadNamedGrammar(string text)
		{
			URL url = JSGFGrammar.grammarNameToURL(this.baseURL, text);
			URL url2 = url;
			JSGFRuleGrammar jsgfruleGrammar = JSGFParser.newGrammarFromJSGF(url2, new JSGFRuleGrammarFactory(this.manager));
			jsgfruleGrammar.setEnabled(true);
			return jsgfruleGrammar;
		}
		
		private void loadImports(JSGFRuleGrammar jsgfruleGrammar)
		{
			Iterator iterator = jsgfruleGrammar.__imports.iterator();
			while (iterator.hasNext())
			{
				JSGFRuleName jsgfruleName = (JSGFRuleName)iterator.next();
				string fullGrammarName = jsgfruleName.getFullGrammarName();
				JSGFRuleGrammar jsgfruleGrammar2 = this.getNamedRuleGrammar(fullGrammarName);
				if (jsgfruleGrammar2 == null)
				{
					jsgfruleGrammar2 = this.loadNamedGrammar(jsgfruleName.getFullGrammarName());
				}
				if (jsgfruleGrammar2 != null)
				{
					this.loadImports(jsgfruleGrammar2);
				}
			}
			this.loadFullQualifiedRules(jsgfruleGrammar);
		}
		
		private string getFullRuleName(string name)
		{
			JSGFRuleName jsgfruleName = this.ruleGrammar.resolve(new JSGFRuleName(name));
			return jsgfruleName.getRuleName();
		}
		
		protected internal virtual void dumpGrammar()
		{
			java.lang.System.@out.println("Imported rules { ");
			Iterator iterator = this.ruleGrammar.getImports().iterator();
			while (iterator.hasNext())
			{
				JSGFRuleName jsgfruleName = (JSGFRuleName)iterator.next();
				java.lang.System.@out.println(new StringBuilder().append("  Import ").append(jsgfruleName.getRuleName()).toString());
			}
			java.lang.System.@out.println("}");
			java.lang.System.@out.println("Rulenames { ");
			iterator = this.ruleGrammar.getRuleNames().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				java.lang.System.@out.println(new StringBuilder().append("  Name ").append(text).toString());
			}
			java.lang.System.@out.println("}");
		}

		private JSGFRuleGrammar getNamedRuleGrammar(string name)
		{
			return this.manager.retrieveGrammar(name);
		}
		
		private void loadFullQualifiedRules(JSGFRuleGrammar jsgfruleGrammar)
		{
			Iterator iterator = jsgfruleGrammar.getRuleNames().iterator();
			while (iterator.hasNext())
			{
				string ruleName = (string)iterator.next();
				string text = jsgfruleGrammar.getRule(ruleName).toString();
				int i = 0;
				while (i < String.instancehelper_length(text))
				{
					i = String.instancehelper_indexOf(text, 60, i);
					if (i < 0)
					{
						break;
					}
					JSGFRuleName jsgfruleName = new JSGFRuleName(String.instancehelper_trim(String.instancehelper_substring(text, i + 1, String.instancehelper_indexOf(text, 62, i + 1))));
					i = String.instancehelper_indexOf(text, 62, i) + 1;
					if (jsgfruleName.getFullGrammarName() != null)
					{
						string fullGrammarName = jsgfruleName.getFullGrammarName();
						JSGFRuleGrammar jsgfruleGrammar2 = this.getNamedRuleGrammar(fullGrammarName);
						if (jsgfruleGrammar2 == null)
						{
							jsgfruleGrammar2 = this.loadNamedGrammar(fullGrammarName);
						}
						if (jsgfruleGrammar2 != null)
						{
							this.loadImports(jsgfruleGrammar2);
						}
					}
				}
			}
		}
		
		private static URL grammarNameToURL(URL url, string text)
		{
			text = String.instancehelper_replace(text, '.', '/');
			StringBuilder stringBuilder = new StringBuilder();
			if (url != null)
			{
				stringBuilder.append(url);
				if (stringBuilder.charAt(stringBuilder.length() - 1) != '/')
				{
					stringBuilder.append('/');
				}
			}
			stringBuilder.append(text).append(".gram");
			string text2 = stringBuilder.toString();
			URL url2;
			try
			{
				url2 = new URL(text2);
			}
			catch (MalformedURLException)
			{
				goto IL_62;
			}
			return url2;
			IL_62:
			url2 = ClassLoader.getSystemResource(text2);
			if (url2 == null)
			{
				string text3 = text2;
				
				throw new MalformedURLException(text3);
			}
			return url2;
		}
		
		public JSGFGrammar(string location, string grammarName, bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, linguist.dictionary.Dictionary dictionary) : this(ConfigurationManagerUtils.resourceToURL(location), grammarName, showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
		}
		
		public JSGFGrammar()
		{
			this.loadGrammar = true;
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logger = ps.getLogger();
			this.logMath = LogMath.getLogMath();
			this.baseURL = ConfigurationManagerUtils.getResource("grammarLocation", ps);
			this.grammarName = ps.getString("grammarName");
			this.loadGrammar = true;
		}

		public virtual JSGFRuleGrammar getRuleGrammar()
		{
			return this.ruleGrammar;
		}

		public virtual void setBaseURL(URL url)
		{
			this.baseURL = url;
		}

		public virtual string getGrammarName()
		{
			return this.grammarName;
		}
		
		public virtual void loadJSGF(string grammarName)
		{
			this.grammarName = grammarName;
			this.loadGrammar = true;
			this.commitChanges();
		}
		
		protected internal override GrammarNode createGrammar()
		{
			try
			{
				try
				{
					this.commitChanges();
				}
				catch (JSGFGrammarException ex)
				{
					throw new IOException(ex);
				}
			}
			catch (JSGFGrammarParseException ex3)
			{
				throw new IOException(ex3);
			}
			return this.firstNode;
		}

		public override GrammarNode getInitialNode()
		{
			return this.firstNode;
		}
		
		internal static GrammarNode access_000(JSGFGrammar jsgfgrammar, bool isFinal)
		{
			return jsgfgrammar.createGrammarNode(isFinal);
		}

		internal static GrammarNode access_100(JSGFGrammar jsgfgrammar, bool isFinal)
		{
			return jsgfgrammar.createGrammarNode(isFinal);
		}
	
		protected internal object ruleStack
		{
			
			get
			{
				return this.__ruleStack;
			}
			
			set
			{
				this.__ruleStack = (JSGFGrammar.RuleStack)value;
			}
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;"
		})]
		public const string PROP_BASE_GRAMMAR_URL = "grammarLocation";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"default.gram"
		})]
		public const string PROP_GRAMMAR_NAME = "grammarName";

		private JSGFRuleGrammar ruleGrammar;

		protected internal JSGFRuleGrammarManager manager;

		internal JSGFGrammar.RuleStack __ruleStack;

		private string grammarName;

		protected internal URL baseURL;

		private LogMath logMath;

		protected internal bool loadGrammar;

		protected internal GrammarNode firstNode;

		protected internal new Logger logger;
		
		public sealed class GrammarGraph : Object
		{			
			internal GrammarGraph(JSGFGrammar jsgfgrammar)
			{
				this.this_0 = jsgfgrammar;
				this.startNode = JSGFGrammar.access_000(jsgfgrammar, false);
				this.endNode = JSGFGrammar.access_100(jsgfgrammar, false);
			}

			internal GrammarNode getStartNode()
			{
				return this.startNode;
			}

			internal GrammarNode getEndNode()
			{
				return this.endNode;
			}
			
			internal GrammarGraph(JSGFGrammar jsgfgrammar, GrammarNode grammarNode, GrammarNode grammarNode2)
			{
				this.startNode = grammarNode;
				this.endNode = grammarNode2;
			}

			private GrammarNode startNode;

			private GrammarNode endNode;

			internal JSGFGrammar this_0;
		}

		internal sealed class RuleStack : Object
		{			
			public JSGFGrammar.GrammarGraph contains(string text)
			{
				if (this.stack.contains(text))
				{
					return (JSGFGrammar.GrammarGraph)this.map.get(text);
				}
				return null;
			}
			
			public void push(string text, JSGFGrammar.GrammarGraph grammarGraph)
			{
				this.stack.add(0, text);
				this.map.put(text, grammarGraph);
			}
			
			public void pop()
			{
				this.map.remove(this.stack.remove(0));
			}
			
			public RuleStack(JSGFGrammar jsgfgrammar)
			{
				this.this_0 = jsgfgrammar;
				this.clear();
			}
			
			public void clear()
			{
				this.stack = new LinkedList();
				this.map = new HashMap();
			}
			
			private List stack;

			private HashMap map;

			internal JSGFGrammar this_0;
		}
	}
}

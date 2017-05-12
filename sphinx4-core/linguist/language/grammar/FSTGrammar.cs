using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class FSTGrammar : Grammar
	{
		private int createNodes(string text)
		{
			ExtendedStreamTokenizer extendedStreamTokenizer = new ExtendedStreamTokenizer(text, true);
			int num = 0;
			while (!extendedStreamTokenizer.isEOF())
			{
				extendedStreamTokenizer.skipwhite();
				string @string = extendedStreamTokenizer.getString();
				if (@string == null)
				{
					break;
				}
				if (String.instancehelper_equals(@string, "T"))
				{
					extendedStreamTokenizer.getInt("src id");
					int @int = extendedStreamTokenizer.getInt("dest id");
					if (@int > num)
					{
						num = @int;
					}
					if (extendedStreamTokenizer.getString() != null)
					{
						string string2 = extendedStreamTokenizer.getString();
						extendedStreamTokenizer.getString();
						string text2 = new StringBuilder().append("G").append(@int).toString();
						GrammarNode grammarNode = (GrammarNode)this.nodes.get(text2);
						if (grammarNode == null)
						{
							if (String.instancehelper_equals(string2, ","))
							{
								grammarNode = this.createGrammarNode(@int, false);
							}
							else
							{
								grammarNode = this.createGrammarNode(@int, string2);
							}
							this.nodes.put(text2, grammarNode);
						}
						else if (!String.instancehelper_equals(string2, ",") && !FSTGrammar.assertionsDisabled && !String.instancehelper_equals(string2, this.getWord(grammarNode)))
						{
							
							throw new AssertionError();
						}
					}
				}
			}
			extendedStreamTokenizer.close();
			return num;
		}
		
		private int expandWordNodes(int num)
		{
			Collection collection = this.nodes.values();
			string[][] alts = new string[][]
			{
				new string[]
				{
					"<sil>"
				}
			};
			Iterator iterator = collection.iterator();
			while (iterator.hasNext())
			{
				GrammarNode grammarNode = (GrammarNode)iterator.next();
				if (grammarNode.getNumAlternatives() > 0)
				{
					num++;
					GrammarNode node = this.createGrammarNode(num, false);
					grammarNode.add(node, 0f);
					if (this.addOptionalSilence)
					{
						num++;
						GrammarNode grammarNode2 = this.createGrammarNode(num, alts);
						grammarNode.add(grammarNode2, 0f);
						grammarNode2.add(node, 0f);
					}
					this.expandedNodes.add(grammarNode);
				}
			}
			return num;
		}
		
		private GrammarNode get(int num)
		{
			string text = new StringBuilder().append("G").append(num).toString();
			GrammarNode grammarNode = (GrammarNode)this.nodes.get(text);
			if (grammarNode == null)
			{
				grammarNode = this.createGrammarNode(num, false);
				this.nodes.put(text, grammarNode);
			}
			return grammarNode;
		}
		
		private bool hasEndNode(GrammarNode grammarNode)
		{
			return this.expandedNodes.contains(grammarNode);
		}
		
		private GrammarNode getEndNode(GrammarNode grammarNode)
		{
			GrammarArc[] successors = grammarNode.getSuccessors();
			if (!FSTGrammar.assertionsDisabled && (successors == null || successors.Length <= 0))
			{
				
				throw new AssertionError();
			}
			return successors[0].getGrammarNode();
		}
		
		private bool hasWord(GrammarNode grammarNode)
		{
			return grammarNode.getNumAlternatives() > 0;
		}
		
		private float convertProbability(float num)
		{
			return this.logMath.lnToLog(-num);
		}
		
		private string getWord(GrammarNode grammarNode)
		{
			string result = null;
			if (grammarNode.getNumAlternatives() > 0)
			{
				Word[][] alternatives = grammarNode.getAlternatives();
				result = alternatives[0][0].getSpelling();
			}
			return result;
		}		
		
		protected internal override GrammarNode createGrammar(string bogusText)
		{
			string text = "Does not create grammar with reference text";
			
			throw new NoSuchMethodException(text);
		}
		
		public FSTGrammar(string path, bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, dictionary.Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
			this.ignoreUnknownTransitions = true;
			this.nodes = new HashMap();
			this.expandedNodes = new HashSet();
			this.logMath = LogMath.getLogMath();
			this.path = path;
		}
		
		public FSTGrammar()
		{
			this.ignoreUnknownTransitions = true;
			this.nodes = new HashMap();
			this.expandedNodes = new HashSet();
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logMath = LogMath.getLogMath();
			this.path = ps.getString("path");
		}
		
		protected internal override GrammarNode createGrammar()
		{
			GrammarNode grammarNode = null;
			int num = this.createNodes(this.path);
			num++;
			GrammarNode grammarNode2 = this.createGrammarNode(num, "<sil>");
			grammarNode2.setFinalNode(true);
			num = this.expandWordNodes(num);
			ExtendedStreamTokenizer extendedStreamTokenizer = new ExtendedStreamTokenizer(this.path, true);
			while (!extendedStreamTokenizer.isEOF())
			{
				extendedStreamTokenizer.skipwhite();
				string @string = extendedStreamTokenizer.getString();
				if (@string == null)
				{
					break;
				}
				if (String.instancehelper_equals(@string, "I"))
				{
					if (!FSTGrammar.assertionsDisabled && grammarNode != null)
					{
						
						throw new AssertionError();
					}
					int @int = extendedStreamTokenizer.getInt("initial ID");
					string text = new StringBuilder().append("G").append(@int).toString();
					grammarNode = this.createGrammarNode(@int, "<sil>");
					this.nodes.put(text, grammarNode);
					if (this.addInitialSilenceNode)
					{
						num++;
						GrammarNode grammarNode3 = this.createGrammarNode(num, "<sil>");
						grammarNode.add(grammarNode3, 0f);
						grammarNode3.add(grammarNode, 0f);
					}
				}
				else if (String.instancehelper_equals(@string, "T"))
				{
					int @int = extendedStreamTokenizer.getInt("this id");
					int int2 = extendedStreamTokenizer.getInt("next id");
					GrammarNode grammarNode3 = this.get(@int);
					GrammarNode grammarNode4 = this.get(int2);
					if (this.hasEndNode(grammarNode3))
					{
						grammarNode3 = this.getEndNode(grammarNode3);
					}
					float num2 = 0f;
					string string2 = extendedStreamTokenizer.getString();
					if (string2 == null || String.instancehelper_equals(string2, ","))
					{
						if (string2 != null && String.instancehelper_equals(string2, ","))
						{
							extendedStreamTokenizer.getString();
							num2 = extendedStreamTokenizer.getFloat("probability");
						}
						if (this.hasEndNode(grammarNode4))
						{
							grammarNode4 = this.getEndNode(grammarNode4);
						}
					}
					else
					{
						string string3 = extendedStreamTokenizer.getString();
						num2 = extendedStreamTokenizer.getFloat("probability");
						if (String.instancehelper_equals(string3, "<unknown>"))
						{
							continue;
						}
						if (!FSTGrammar.assertionsDisabled && !this.hasWord(grammarNode4))
						{
							
							throw new AssertionError();
						}
					}
					grammarNode3.add(grammarNode4, this.convertProbability(num2));
				}
				else if (String.instancehelper_equals(@string, "F"))
				{
					int @int = extendedStreamTokenizer.getInt("this id");
					float @float = extendedStreamTokenizer.getFloat("probability");
					GrammarNode grammarNode3 = this.get(@int);
					GrammarNode grammarNode4 = grammarNode2;
					if (this.hasEndNode(grammarNode3))
					{
						grammarNode3 = this.getEndNode(grammarNode3);
					}
					grammarNode3.add(grammarNode4, this.convertProbability(@float));
				}
			}
			extendedStreamTokenizer.close();
			if (!FSTGrammar.assertionsDisabled && grammarNode == null)
			{
				
				throw new AssertionError();
			}
			return grammarNode;
		}
		
		static FSTGrammar()
		{
			FSTGrammar.assertionsDisabled = !ClassLiteral<FSTGrammar>.Value.desiredAssertionStatus();
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"default.arpa_gram"
		})]
		public const string PROP_PATH = "path";

		private bool addInitialSilenceNode;

		private bool addOptionalSilence;

		private bool ignoreUnknownTransitions;

		private string path;

		private LogMath logMath;
		
		private Map nodes;
				
		private Set expandedNodes;
		
		internal static bool assertionsDisabled;
	}
}

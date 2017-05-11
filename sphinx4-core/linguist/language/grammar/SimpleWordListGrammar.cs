using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class SimpleWordListGrammar : Grammar, Configurable
	{
		public SimpleWordListGrammar(string path, bool isLooping, bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, dictionary.Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
			this.path = path;
			this.isLooping = isLooping;
			this.logMath = LogMath.getLogMath();
		}
		
		public SimpleWordListGrammar()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.path = ps.getString("path");
			this.isLooping = ps.getBoolean("isLooping").booleanValue();
			this.logMath = LogMath.getLogMath();
		}		
		
		protected internal override GrammarNode createGrammar(string bogusText)
		{
			string text = "Does not create grammar with reference text";
			
			throw new NoSuchMethodException(text);
		}
		
		protected internal override GrammarNode createGrammar()
		{
			ExtendedStreamTokenizer extendedStreamTokenizer = new ExtendedStreamTokenizer(this.path, true);
			GrammarNode grammarNode = this.createGrammarNode("<sil>");
			GrammarNode grammarNode2 = this.createGrammarNode(false);
			GrammarNode grammarNode3 = this.createGrammarNode("<sil>");
			grammarNode3.setFinalNode(true);
			LinkedList linkedList = new LinkedList();
			while (!extendedStreamTokenizer.isEOF())
			{
				string @string;
				while ((@string = extendedStreamTokenizer.getString()) != null)
				{
					GrammarNode grammarNode4 = this.createGrammarNode(@string);
					linkedList.add(grammarNode4);
				}
			}
			grammarNode.add(grammarNode2, 0f);
			float logProbability = this.logMath.linearToLog((double)1f / (double)linkedList.size());
			Iterator iterator = linkedList.iterator();
			while (iterator.hasNext())
			{
				GrammarNode grammarNode5 = (GrammarNode)iterator.next();
				grammarNode2.add(grammarNode5, logProbability);
				grammarNode5.add(grammarNode3, 0f);
				if (this.isLooping)
				{
					grammarNode5.add(grammarNode2, 0f);
				}
			}
			return grammarNode;
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"spelling.gram"
		})]
		public const string PROP_PATH = "path";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_LOOP = "isLooping";

		private string path;

		private bool isLooping;

		private LogMath logMath;
	}
}

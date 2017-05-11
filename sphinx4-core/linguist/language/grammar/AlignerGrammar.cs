using edu.cmu.sphinx.linguist.dictionary;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class AlignerGrammar : Grammar
	{		
		public virtual void setWords(Iterable words)
		{
			this.tokens.clear();
			Iterator iterator = words.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				if (!java.lang.String.instancehelper_isEmpty(text))
				{
					this.tokens.add(text);
				}
			}
			this.createGrammar();
			this.postProcessGrammar();
		}
		
		protected internal override GrammarNode createGrammar()
		{
			this.logger.info("Making Grammar");
			this.initialNode = this.createGrammarNode("<sil>");
			this.finalNode = this.createGrammarNode(true);
			GrammarNode grammarNode = this.initialNode;
			Iterator iterator = this.tokens.iterator();
			while (iterator.hasNext())
			{
				string word = (string)iterator.next();
				GrammarNode grammarNode2 = this.createGrammarNode(word);
				GrammarNode grammarNode3 = this.createGrammarNode(false);
				GrammarNode grammarNode4 = this.createGrammarNode(false);
				grammarNode.add(grammarNode2, 0f);
				grammarNode.add(grammarNode3, 0f);
				grammarNode2.add(grammarNode4, 0f);
				grammarNode3.add(grammarNode4, 0f);
				grammarNode = grammarNode4;
			}
			grammarNode.add(this.finalNode, 0f);
			this.logger.info("Done making Grammar");
			return this.initialNode;
		}
		
		public AlignerGrammar(bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
			this.tokens = new ArrayList();
		}
		
		public AlignerGrammar()
		{
			this.tokens = new ArrayList();
		}
		
		public virtual void setText(string text)
		{
			this.setWords(Arrays.asList(java.lang.String.instancehelper_split(text, " ")));
		}
		
		protected internal GrammarNode finalNode;
		
		private List tokens;
	}
}

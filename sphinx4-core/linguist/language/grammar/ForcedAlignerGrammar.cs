using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class ForcedAlignerGrammar : Grammar
	{
		protected internal virtual GrammarNode createForcedAlignerGrammar(GrammarNode iNode, GrammarNode fNode, string transcript)
		{
			StringTokenizer stringTokenizer = new StringTokenizer(transcript);
			GrammarNode grammarNode = null;
			GrammarNode grammarNode2 = null;
			while (stringTokenizer.hasMoreTokens())
			{
				string word = stringTokenizer.nextToken();
				GrammarNode grammarNode3 = grammarNode2;
				grammarNode2 = this.createGrammarNode(word);
				if (grammarNode == null)
				{
					grammarNode = grammarNode2;
				}
				if (grammarNode3 != null)
				{
					grammarNode3.add(grammarNode2, 0f);
				}
			}
			iNode.add(grammarNode, 0f);
			grammarNode2.add(fNode, 0f);
			return grammarNode;
		}
		
		public ForcedAlignerGrammar(bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, dictionary.Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
		}
		
		public ForcedAlignerGrammar()
		{
		}
		
		protected internal override GrammarNode createGrammar()
		{
			string text = "Not implemented";
			
			throw new Error(text);
		}
		
		protected internal override GrammarNode createGrammar(string referenceText)
		{
			this.initialNode = this.createGrammarNode(false);
			this.finalNode = this.createGrammarNode(true);
			this.createForcedAlignerGrammar(this.initialNode, this.finalNode, referenceText);
			return this.initialNode;
		}

		protected internal GrammarNode finalNode;
	}
}

using System;

using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class ForcedAlignerGrammar : Grammar
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			19,
			134,
			135,
			98,
			130,
			168,
			135,
			99,
			104,
			133,
			100,
			141,
			130,
			108,
			140
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			135,
			169,
			108
		})]
		
		public ForcedAlignerGrammar(bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			176,
			134
		})]
		
		public ForcedAlignerGrammar()
		{
		}

		
		
		protected internal override GrammarNode createGrammar()
		{
			string text = "Not implemented";
			
			throw new Error(text);
		}

		[Throws(new string[]
		{
			"java.lang.NoSuchMethodException"
		})]
		[LineNumberTable(new byte[]
		{
			1,
			109,
			109,
			148
		})]
		
		protected internal override GrammarNode createGrammar(string referenceText)
		{
			this.initialNode = this.createGrammarNode(false);
			this.finalNode = this.createGrammarNode(true);
			this.createForcedAlignerGrammar(this.initialNode, this.finalNode, referenceText);
			return this.initialNode;
		}

		
		static ForcedAlignerGrammar()
		{
			Grammar.__<clinit>();
		}

		protected internal GrammarNode finalNode;
	}
}

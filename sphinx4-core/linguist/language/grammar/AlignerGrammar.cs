using System;

using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class AlignerGrammar : Grammar
	{
		
		public new static void __<clinit>()
		{
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			185,
			107,
			123,
			104,
			141,
			98,
			103,
			102
		})]
		
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

		[LineNumberTable(new byte[]
		{
			6,
			144,
			113,
			141,
			103,
			127,
			4,
			104,
			105,
			105,
			108,
			109,
			109,
			110,
			99,
			101,
			145,
			112
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			135,
			105,
			239,
			60,
			235,
			69
		})]
		
		public AlignerGrammar(bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
			this.tokens = new ArrayList();
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			232,
			57,
			235,
			72
		})]
		
		public AlignerGrammar()
		{
			this.tokens = new ArrayList();
		}

		[LineNumberTable(new byte[]
		{
			159,
			181,
			118
		})]
		
		public virtual void setText(string text)
		{
			this.setWords(Arrays.asList(java.lang.String.instancehelper_split(text, " ")));
		}

		
		static AlignerGrammar()
		{
			Grammar.__<clinit>();
		}

		protected internal GrammarNode finalNode;

		
		
		private List tokens;
	}
}

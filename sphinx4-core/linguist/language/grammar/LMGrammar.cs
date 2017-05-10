using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.ngram;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class LMGrammar : Grammar
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			131,
			74,
			111,
			103
		})]
		
		public LMGrammar(LanguageModel languageModel, bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
			this.languageModel = languageModel;
		}

		[LineNumberTable(new byte[]
		{
			159,
			190,
			134
		})]
		
		public LMGrammar()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			9,
			103,
			118
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.languageModel = (LanguageModel)ps.getComponent("languageModel");
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			22,
			107,
			112,
			98,
			110,
			143,
			102,
			140,
			127,
			0,
			106,
			109,
			104,
			42,
			135,
			101,
			104,
			42,
			135,
			136,
			137,
			101,
			99,
			144,
			159,
			0,
			105,
			130,
			127,
			3,
			110,
			110,
			121,
			111,
			109,
			103,
			107,
			101,
			101,
			113,
			107
		})]
		
		protected internal override GrammarNode createGrammar()
		{
			this.languageModel.allocate();
			TimerPool.getTimer(this, "LMGrammar.create").start();
			GrammarNode grammarNode = null;
			if (this.languageModel.getMaxDepth() > 2)
			{
				java.lang.System.@out.println("Warning: LMGrammar  limited to bigrams");
			}
			ArrayList arrayList = new ArrayList();
			Set vocabulary = this.languageModel.getVocabulary();
			Iterator iterator = vocabulary.iterator();
			while (iterator.hasNext())
			{
				string word = (string)iterator.next();
				GrammarNode grammarNode2 = this.createGrammarNode(word);
				if (grammarNode2 != null && !grammarNode2.isEmpty())
				{
					if (grammarNode2.getWord().equals(this.getDictionary().getSentenceStartWord()))
					{
						grammarNode = grammarNode2;
					}
					else if (grammarNode2.getWord().equals(this.getDictionary().getSentenceEndWord()))
					{
						grammarNode2.setFinalNode(true);
					}
					arrayList.add(grammarNode2);
				}
			}
			if (grammarNode == null)
			{
				string text = "No sentence start found in language model";
				
				throw new Error(text);
			}
			iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				GrammarNode grammarNode3 = (GrammarNode)iterator.next();
				if (!grammarNode3.isFinalNode())
				{
					Iterator iterator2 = arrayList.iterator();
					while (iterator2.hasNext())
					{
						GrammarNode grammarNode4 = (GrammarNode)iterator2.next();
						string spelling = grammarNode3.getWord().getSpelling();
						string spelling2 = grammarNode4.getWord().getSpelling();
						Word[] words = new Word[]
						{
							this.getDictionary().getWord(spelling),
							this.getDictionary().getWord(spelling2)
						};
						float probability = this.languageModel.getProbability(new WordSequence(words));
						grammarNode3.add(grammarNode4, probability);
					}
				}
			}
			TimerPool.getTimer(this, "LMGrammar.create").stop();
			this.languageModel.deallocate();
			return grammarNode;
		}

		
		static LMGrammar()
		{
			Grammar.__<clinit>();
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/language/ngram/LanguageModel, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_LANGUAGE_MODEL = "languageModel";

		private LanguageModel languageModel;
	}
}

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	public class KeywordOptimizerModel : LanguageModelBase
	{		
		public KeywordOptimizerModel(LanguageModel parent)
		{
			this.parent = parent;
		}
		
		public KeywordOptimizerModel()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			this.parent = (LanguageModel)ps.getComponent("parent");
		}
		
		public override void allocate()
		{
			this.parent.allocate();
		}
		
		public override void deallocate()
		{
			this.parent.deallocate();
		}
		
		public override float getProbability(WordSequence wordSequence)
		{
			float num = this.parent.getProbability(wordSequence);
			if (this.keywordProbs == null)
			{
				return num;
			}
			Word[] words = wordSequence.getWords();
			int num2 = words.Length;
			for (int i = 0; i < num2; i++)
			{
				Word word = words[i];
				string text = word.toString();
				if (this.keywordProbs.containsKey(text))
				{
					num *= ((Float)this.keywordProbs.get(text)).floatValue();
				}
			}
			return num;
		}
		
		public override float getSmear(WordSequence wordSequence)
		{
			return this.parent.getSmear(wordSequence);
		}
				
		public override int getMaxDepth()
		{
			return this.parent.getMaxDepth();
		}		
		
		public override Set getVocabulary()
		{
			return this.parent.getVocabulary();
		}

		public override void onUtteranceEnd()
		{
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
		public const string PROP_PARENT = "parent";
		
		public HashMap keywordProbs;

		private LanguageModel parent;
	}
}

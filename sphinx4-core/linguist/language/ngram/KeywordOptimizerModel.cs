using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.language.ngram.LanguageModel"
	})]
	public class KeywordOptimizerModel : java.lang.Object, LanguageModel, Configurable
	{
		[LineNumberTable(new byte[]
		{
			159,
			182,
			104,
			103
		})]
		
		public KeywordOptimizerModel(LanguageModel parent)
		{
			this.parent = parent;
		}

		[LineNumberTable(new byte[]
		{
			159,
			186,
			134
		})]
		
		public KeywordOptimizerModel()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			5,
			118
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.parent = (LanguageModel)ps.getComponent("parent");
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			13,
			107
		})]
		
		public virtual void allocate()
		{
			this.parent.allocate();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			21,
			107
		})]
		
		public virtual void deallocate()
		{
			this.parent.deallocate();
		}

		[LineNumberTable(new byte[]
		{
			33,
			141,
			104,
			130,
			117,
			105,
			111,
			251,
			61,
			230,
			71
		})]
		
		public virtual float getProbability(WordSequence wordSequence)
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

		
		
		public virtual float getSmear(WordSequence wordSequence)
		{
			return this.parent.getSmear(wordSequence);
		}

		
		
		public virtual int getMaxDepth()
		{
			return this.parent.getMaxDepth();
		}

		
		
		
		public virtual Set getVocabulary()
		{
			return this.parent.getVocabulary();
		}

		public virtual void onUtteranceEnd()
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

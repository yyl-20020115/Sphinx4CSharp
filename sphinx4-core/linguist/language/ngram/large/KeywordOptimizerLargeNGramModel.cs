﻿using edu.cmu.sphinx.linguist.dictionary;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	public class KeywordOptimizerLargeNGramModel : LargeNGramModel
	{
		public KeywordOptimizerLargeNGramModel()
		{
		}
		
		public override float getProbability(WordSequence wordSequence)
		{
			float num = base.getProbability(wordSequence);
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
		
		public HashMap keywordProbs;
	}
}

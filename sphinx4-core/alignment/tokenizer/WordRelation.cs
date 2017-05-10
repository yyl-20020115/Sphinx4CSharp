using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class WordRelation : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			180,
			108
		})]
		
		public static WordRelation createWordRelation(Utterance utterance, USEnglishTokenizer tokenToWords)
		{
			Relation relation = utterance.createRelation("Word");
			return new WordRelation(relation, tokenToWords);
		}

		[LineNumberTable(new byte[]
		{
			9,
			108,
			103,
			103,
			108,
			109
		})]
		
		public virtual void addWord(string word)
		{
			Item tokenItem = this.tokenToWords.getTokenItem();
			Item item = tokenItem.createDaughter();
			FeatureSet features = item.getFeatures();
			features.setString("name", word);
			this.relation.appendItem(item);
		}

		[LineNumberTable(new byte[]
		{
			159,
			188,
			108,
			99,
			103,
			144
		})]
		
		public virtual void addBreak()
		{
			Item tail = this.relation.getTail();
			if (tail != null)
			{
				FeatureSet features = tail.getFeatures();
				features.setString("break", "1");
			}
		}

		
		
		public virtual Item getTail()
		{
			return this.relation.getTail();
		}

		[LineNumberTable(new byte[]
		{
			22,
			108,
			103,
			108
		})]
		
		public virtual void setLastWord(string word)
		{
			Item tail = this.relation.getTail();
			FeatureSet features = tail.getFeatures();
			features.setString("name", word);
		}

		[LineNumberTable(new byte[]
		{
			159,
			165,
			104,
			103,
			103
		})]
		
		private WordRelation(Relation relation, USEnglishTokenizer usenglishTokenizer)
		{
			this.relation = relation;
			this.tokenToWords = usenglishTokenizer;
		}

		private Relation relation;

		private USEnglishTokenizer tokenToWords;
	}
}

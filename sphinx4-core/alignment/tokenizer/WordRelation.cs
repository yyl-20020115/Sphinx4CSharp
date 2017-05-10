namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class WordRelation : java.lang.Object
	{	
		public static WordRelation createWordRelation(Utterance utterance, USEnglishTokenizer tokenToWords)
		{
			Relation relation = utterance.createRelation("Word");
			return new WordRelation(relation, tokenToWords);
		}
		
		public virtual void addWord(string word)
		{
			Item tokenItem = this.tokenToWords.getTokenItem();
			Item item = tokenItem.createDaughter();
			FeatureSet features = item.getFeatures();
			features.setString("name", word);
			this.relation.appendItem(item);
		}
		
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
		
		public virtual void setLastWord(string word)
		{
			Item tail = this.relation.getTail();
			FeatureSet features = tail.getFeatures();
			features.setString("name", word);
		}
	
		private WordRelation(Relation relation, USEnglishTokenizer usenglishTokenizer)
		{
			this.relation = relation;
			this.tokenToWords = usenglishTokenizer;
		}

		private Relation relation;

		private USEnglishTokenizer tokenToWords;
	}
}

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	internal sealed class NGramProbability : java.lang.Object
	{
		public int getProbabilityID()
		{
			return this.probabilityID;
		}

		public int getBackoffID()
		{
			return this.backoffID;
		}

		public int getWhichFollower()
		{
			return this.which;
		}

		public int getFirstNPlus1GramEntry()
		{
			return this.firstNPlus1GramEntry;
		}
		
		public NGramProbability(int num, int num2, int num3, int num4, int num5)
		{
			this.which = num;
			this.wordID = num2;
			this.probabilityID = num3;
			this.backoffID = num4;
			this.firstNPlus1GramEntry = num5;
		}

		public int getWordID()
		{
			return this.wordID;
		}
		
		private int which;
		
		private int wordID;
		
		private int probabilityID;
		
		private int backoffID;
		
		private int firstNPlus1GramEntry;
	}
}

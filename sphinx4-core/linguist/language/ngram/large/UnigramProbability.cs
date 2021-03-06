﻿using java.lang;

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	public sealed class UnigramProbability : Object
	{
		public void setLogProbability(float num)
		{
			this.logProbability = num;
		}

		public void setLogBackoff(float num)
		{
			this.logBackoff = num;
		}

		public float getLogProbability()
		{
			return this.logProbability;
		}

		public float getLogBackoff()
		{
			return this.logBackoff;
		}
		
		public UnigramProbability(int num, float num2, float num3, int num4)
		{
			this.wordID = num;
			this.logProbability = num2;
			this.logBackoff = num3;
			this.firstBigramEntry = num4;
		}

		public int getFirstBigramEntry()
		{
			return this.firstBigramEntry;
		}

		public int getWordID()
		{
			return this.wordID;
		}
		
		public override string toString()
		{
			return new StringBuilder().append("Prob: ").append(this.logProbability).append(' ').append(this.logBackoff).toString();
		}
		
		private int wordID;

		private float logProbability;

		private float logBackoff;

		private int firstBigramEntry;
	}
}

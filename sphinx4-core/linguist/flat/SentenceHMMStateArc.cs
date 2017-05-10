using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.SearchStateArc"
	})]
	public class SentenceHMMStateArc : java.lang.Object, SearchStateArc
	{
		public virtual SearchState getState()
		{
			return this.nextState;
		}

		[LineNumberTable(new byte[]
		{
			159,
			183,
			104,
			103,
			104,
			136,
			110,
			107,
			140
		})]
		
		public SentenceHMMStateArc(SentenceHMMState nextState, float logLanguageProbability, float logInsertionProbability)
		{
			this.nextState = nextState;
			this.logLanguageProbability = logLanguageProbability;
			this.logInsertionProbability = logInsertionProbability;
			this.hashCode = 111 + Object.instancehelper_hashCode(nextState) + 17 * Float.floatToIntBits(logLanguageProbability) + 23 * Float.floatToIntBits(logInsertionProbability);
		}

		[LineNumberTable(new byte[]
		{
			11,
			100,
			98,
			104,
			103,
			255,
			16,
			69
		})]
		public override bool equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (o is SentenceHMMStateArc)
			{
				SentenceHMMStateArc sentenceHMMStateArc = (SentenceHMMStateArc)o;
				return this.nextState == sentenceHMMStateArc.nextState && this.logLanguageProbability == sentenceHMMStateArc.logLanguageProbability && this.logInsertionProbability == sentenceHMMStateArc.logInsertionProbability;
			}
			return false;
		}

		public override int hashCode()
		{
			return this.hashCode;
		}

		
		
		public virtual SentenceHMMState getNextState()
		{
			return (SentenceHMMState)this.getState();
		}

		public virtual float getLanguageProbability()
		{
			return this.logLanguageProbability;
		}

		public virtual float getInsertionProbability()
		{
			return this.logInsertionProbability;
		}

		public virtual float getProbability()
		{
			return this.logLanguageProbability + this.logInsertionProbability;
		}

		
		private SentenceHMMState nextState;

		
		private float logLanguageProbability;

		
		private float logInsertionProbability;

		
		private int hashCode;
	}
}

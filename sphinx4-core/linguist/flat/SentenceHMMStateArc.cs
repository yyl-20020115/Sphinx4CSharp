using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	public class SentenceHMMStateArc : java.lang.Object, SearchStateArc
	{
		public virtual SearchState getState()
		{
			return this.nextState;
		}
		
		public SentenceHMMStateArc(SentenceHMMState nextState, float logLanguageProbability, float logInsertionProbability)
		{
			this.nextState = nextState;
			this.logLanguageProbability = logLanguageProbability;
			this.logInsertionProbability = logInsertionProbability;
			this._hashCode = 111 + java.lang.Object.instancehelper_hashCode(nextState) + 17 * Float.floatToIntBits(logLanguageProbability) + 23 * Float.floatToIntBits(logInsertionProbability);
		}

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
			return this._hashCode;
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
		
		private int _hashCode;
	}
}

using edu.cmu.sphinx.linguist.acoustic;
using java.lang;

namespace edu.cmu.sphinx.linguist.allphone
{
	public class PhoneNonEmittingSearchState : java.lang.Object, SearchState, SearchStateArc
	{		
		public PhoneNonEmittingSearchState(Unit unit, AllphoneLinguist linguist, float insertionProb, float languageProb)
		{
			this.unit = unit;
			this.linguist = linguist;
			this.insertionProb = insertionProb;
			this.languageProb = languageProb;
		}

		public virtual float getLanguageProbability()
		{
			return this.languageProb;
		}

		public virtual float getInsertionProbability()
		{
			return this.insertionProb;
		}
		
		public virtual SearchStateArc[] getSuccessors()
		{
			return new SearchStateArc[]
			{
				new PhoneWordSearchState(this.unit, this.linguist, this.insertionProb, this.languageProb)
			};
		}

		public virtual bool isEmitting()
		{
			return false;
		}

		public virtual bool isFinal()
		{
			return false;
		}
		
		public virtual string toPrettyString()
		{
			return new StringBuilder().append("Unit ").append(this.unit.toString()).toString();
		}

		public virtual string getSignature()
		{
			return null;
		}

		public virtual WordSequence getWordHistory()
		{
			return null;
		}

		public virtual int getOrder()
		{
			return 0;
		}

		public virtual SearchState getState()
		{
			return this;
		}
		
		public virtual float getProbability()
		{
			return this.getLanguageProbability() + this.getInsertionProbability();
		}

		public virtual object getLexState()
		{
			return null;
		}
		
		public override bool equals(object obj)
		{
			if (!(obj is PhoneNonEmittingSearchState))
			{
				return false;
			}
			int num = (((PhoneNonEmittingSearchState)obj).unit.getBaseID() == this.unit.getBaseID()) ? 1 : 0;
			int num2 = ((PhoneNonEmittingSearchState)obj).unit.getContext().equals(this.unit.getContext()) ? 1 : 0;
			return num != 0 && num2 != 0;
		}
		
		public override int hashCode()
		{
			return this.unit.getContext().hashCode() * 91 + this.unit.getBaseID();
		}

		protected internal Unit unit;

		protected internal AllphoneLinguist linguist;

		private float insertionProb;

		private float languageProb;
	}
}

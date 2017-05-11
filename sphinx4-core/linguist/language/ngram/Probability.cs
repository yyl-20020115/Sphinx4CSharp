using java.lang;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	internal sealed class Probability : java.lang.Object
	{		
		internal Probability(float num, float num2)
		{
			this.logProbability = num;
			this.logBackoff = num2;
		}
		
		public override string toString()
		{
			return new StringBuilder().append("Prob: ").append(this.logProbability).append(' ').append(this.logBackoff).toString();
		}

		internal float logProbability;
		
		internal float logBackoff;
	}
}

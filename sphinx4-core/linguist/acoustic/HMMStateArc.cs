using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic
{
	public class HMMStateArc : Object
	{		
		public HMMStateArc(HMMState hmmState, float probability)
		{
			this.hmmState = hmmState;
			this.probability = probability;
		}

		public virtual HMMState getHMMState()
		{
			return this.hmmState;
		}

		public virtual float getLogProbability()
		{
			return this.probability;
		}
		
		public override string toString()
		{
			return new StringBuilder().append("HSA ").append(this.hmmState).append(" prob ").append(this.probability).toString();
		}
		
		private HMMState hmmState;
		
		private float probability;
	}
}

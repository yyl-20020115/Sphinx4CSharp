using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic
{
	public class HMMStateArc : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			175,
			104,
			103,
			104
		})]
		
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

using System;
using edu.cmu.sphinx.linguist.acoustic;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.lextree
{
	internal sealed class HMMNode : UnitNode
	{
		internal HMMNode(HMM hmm, float num) : base(num)
		{
			this.hmm = hmm;
			Unit baseUnit = this.getBaseUnit();
			int type = 1;
			if (baseUnit.isSilence())
			{
				type = 3;
			}
			else if (baseUnit.isFiller())
			{
				type = 4;
			}
			else if (hmm.getPosition().isWordBeginning())
			{
				type = 2;
			}
			this.setType(type);
		}

		internal override Unit getBaseUnit()
		{
			return this.hmm.getBaseUnit();
		}

		internal HMM getHMM()
		{
			return this.hmm;
		}

		private Set getRCSet()
		{
			if (this.rcSet == null)
			{
				this.rcSet = new HashSet();
			}
			if (!HMMNode.assertionsDisabled && !(this.rcSet is HashSet))
			{
				
				throw new AssertionError();
			}
			return (Set)this.rcSet;
		}

		internal override void freeze()
		{
			base.freeze();
			if (this.rcSet is Set)
			{
				Set set = (Set)this.rcSet;
				this.rcSet = set.toArray(new Unit[set.size()]);
			}
		}

		internal HMM getKeyHMM()
		{
			return this.getHMM();
		}	
		
		internal override HMMPosition getPosition()
		{
			return this.hmm.getPosition();
		}
		
		public override string toString()
		{
			return new StringBuilder().append("HMMNode ").append(this.hmm).append(" p ").append(this.getUnigramProbability()).toString();
		}

		internal void addRC(Unit unit)
		{
			this.getRCSet().add(unit);
		}
	
		internal Unit[] getRC()
		{
			if (this.rcSet is HashSet)
			{
				this.freeze();
			}
			return (Unit[])((Unit[])this.rcSet);
		}

		internal override object getKey()
		{
			return this.getKeyHMM();
		}

		static HMMNode()
		{
			HMMNode.assertionsDisabled = !ClassLiteral<HMMNode>.Value.desiredAssertionStatus();
		}

		private HMM hmm;

		private object rcSet;
		
		internal new static bool assertionsDisabled;
	}
}

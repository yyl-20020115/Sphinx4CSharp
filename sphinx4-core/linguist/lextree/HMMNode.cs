using System;

using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.lextree
{
	[SourceFile("HMMTree.java")]
	
	internal sealed class HMMNode : UnitNode
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			99,
			106,
			135,
			135,
			98,
			104,
			100,
			104,
			100,
			109,
			130,
			103
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			161,
			191,
			104,
			171,
			127,
			0
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			176,
			102,
			109,
			108,
			151
		})]
		
		internal override void freeze()
		{
			base.freeze();
			if (this.rcSet is Set)
			{
				Set set = (Set)this.rcSet;
				this.rcSet = set.toArray(new Unit[set.size()]);
			}
		}

		
		
		internal new HMM getKey()
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

		[LineNumberTable(new byte[]
		{
			161,
			168,
			109
		})]
		
		internal void addRC(Unit unit)
		{
			this.getRCSet().add(unit);
		}

		[LineNumberTable(new byte[]
		{
			161,
			206,
			109,
			134
		})]
		
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
			return this.getKey();
		}

		
		static HMMNode()
		{
			UnitNode.__<clinit>();
			HMMNode.assertionsDisabled = !ClassLiteral<HMMNode>.Value.desiredAssertionStatus();
		}

		
		private HMM hmm;

		private object rcSet;

		
		internal new static bool assertionsDisabled;
	}
}

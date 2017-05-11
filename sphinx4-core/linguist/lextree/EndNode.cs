using System;

using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.lextree
{
	.
	
	internal sealed class EndNode : UnitNode
	{
		
		public new static void __<clinit>()
		{
		}

		internal override Unit getBaseUnit()
		{
			return this.baseUnit;
		}

		internal new Integer getKey()
		{
			return this.key;
		}

		[LineNumberTable(new byte[]
		{
			161,
			229,
			106,
			103,
			103,
			127,
			1
		})]
		
		internal EndNode(Unit unit, Unit unit2, float num) : base(num)
		{
			this.baseUnit = unit;
			this.leftContext = unit2;
			this.key = Integer.valueOf(unit.getBaseID() * 121 + this.leftContext.getBaseID());
		}

		internal Unit getLeftContext()
		{
			return this.leftContext;
		}

		
		
		internal override HMMPosition getPosition()
		{
			return HMMPosition.__END;
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append("EndNode base:").append(this.baseUnit).append(" lc ").append(this.leftContext).append(' ').append(this.key).toString();
		}

		[LineNumberTable(new byte[]
		{
			162,
			27,
			102
		})]
		
		internal override void freeze()
		{
			base.freeze();
		}

		
		
		
		internal override object getKey()
		{
			return this.getKey();
		}

		
		static EndNode()
		{
			UnitNode.__<clinit>();
		}

		
		internal Unit baseUnit;

		
		internal Unit leftContext;

		
		internal Integer key;
	}
}

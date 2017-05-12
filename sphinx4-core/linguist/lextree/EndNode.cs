using edu.cmu.sphinx.linguist.acoustic;
using java.lang;

namespace edu.cmu.sphinx.linguist.lextree
{
	internal sealed class EndNode : UnitNode
	{
		internal override Unit getBaseUnit()
		{
			return this.baseUnit;
		}

		internal Integer getKeyInteger()
		{
			return this.key;
		}
		
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
		
		internal override void freeze()
		{
			base.freeze();
		}

		internal override object getKey()
		{
			return this.getKeyInteger();
		}

		internal Unit baseUnit;
		
		internal Unit leftContext;
		
		internal Integer key;
	}
}

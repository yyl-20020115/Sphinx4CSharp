using edu.cmu.sphinx.util;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	internal sealed class ContextPair : Object
	{
		private ContextPair(UnitContext unitContext, UnitContext unitContext2)
		{
			this.left = unitContext;
			this.right = unitContext2;
			this._hashCode = 99 + unitContext.hashCode() * 113 + unitContext2.hashCode();
		}
		
		internal static ContextPair get(UnitContext unitContext, UnitContext unitContext2)
		{
			ContextPair contextPair = new ContextPair(unitContext, unitContext2);
			ContextPair contextPair2 = (ContextPair)ContextPair.contextPairCache.cache(contextPair);
			return (contextPair2 != null) ? contextPair2 : contextPair;
		}
		
		public override bool equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj is ContextPair)
			{
				ContextPair contextPair = (ContextPair)obj;
				return this.left.equals(contextPair.left) && this.right.equals(contextPair.right);
			}
			return false;
		}

		public override int hashCode()
		{
			return this._hashCode;
		}
		
		public override string toString()
		{
			return new StringBuilder().append("CP left: ").append(this.left).append(" right: ").append(this.right).toString();
		}

		public UnitContext getLeftContext()
		{
			return this.left;
		}

		public UnitContext getRightContext()
		{
			return this.right;
		}				
		
		internal static Cache contextPairCache = new Cache();
	
		private UnitContext left;
		
		private UnitContext right;
		
		private int _hashCode;
	}
}

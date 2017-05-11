using System;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	.
	
	internal sealed class ContextPair : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			165,
			165,
			104,
			103,
			103,
			121
		})]
		
		private ContextPair(UnitContext unitContext, UnitContext unitContext2)
		{
			this.left = unitContext;
			this.right = unitContext2;
			this.hashCode = 99 + unitContext.hashCode() * 113 + unitContext2.hashCode();
		}

		[LineNumberTable(new byte[]
		{
			165,
			181,
			104,
			113
		})]
		
		internal static ContextPair get(UnitContext unitContext, UnitContext unitContext2)
		{
			ContextPair contextPair = new ContextPair(unitContext, unitContext2);
			ContextPair contextPair2 = (ContextPair)ContextPair.contextPairCache.cache(contextPair);
			return (contextPair2 != null) ? contextPair2 : contextPair;
		}

		[LineNumberTable(new byte[]
		{
			165,
			195,
			100,
			98,
			104,
			103,
			159,
			12
		})]
		
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
			return this.hashCode;
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

		
		static ContextPair()
		{
		}

		
		
		internal static Cache contextPairCache = new Cache();

		
		private UnitContext left;

		
		private UnitContext right;

		
		private int hashCode;
	}
}

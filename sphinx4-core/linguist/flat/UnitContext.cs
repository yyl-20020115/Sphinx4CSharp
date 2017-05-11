using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	.
	
	internal sealed class UnitContext : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		public override int hashCode()
		{
			return this.hashCode;
		}

		[LineNumberTable(new byte[]
		{
			165,
			95,
			100,
			98,
			104,
			103,
			112,
			130,
			108,
			114,
			2,
			230,
			69,
			162
		})]
		public override bool equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (!(obj is UnitContext))
			{
				return false;
			}
			UnitContext unitContext = (UnitContext)obj;
			if (this.context.Length != unitContext.context.Length)
			{
				return false;
			}
			for (int i = 0; i < this.context.Length; i++)
			{
				if (this.context[i] != unitContext.context[i])
				{
					return false;
				}
			}
			return true;
		}

		[LineNumberTable(new byte[]
		{
			165,
			71,
			103,
			113
		})]
		
		internal static UnitContext get(Unit[] array)
		{
			UnitContext unitContext = new UnitContext(array);
			UnitContext unitContext2 = (UnitContext)UnitContext.unitContextCache.cache(unitContext);
			return (unitContext2 != null) ? unitContext2 : unitContext;
		}

		
		
		public override string toString()
		{
			return LeftRightContext.getContextName(this.context);
		}

		public Unit[] getUnits()
		{
			return this.context;
		}

		[LineNumberTable(new byte[]
		{
			165,
			55,
			232,
			48,
			232,
			81,
			103,
			104,
			103,
			63,
			2,
			166
		})]
		
		private UnitContext(Unit[] array)
		{
			this.hashCode = 12;
			this.context = array;
			this.hashCode = 12;
			for (int i = 0; i < array.Length; i++)
			{
				this.hashCode += java.lang.String.instancehelper_hashCode(array[i].getName()) * ((i + 1) * 34);
			}
		}

		[LineNumberTable(new byte[]
		{
			165,
			130,
			121,
			63,
			9,
			133
		})]
		
		public static void dumpInfo()
		{
			java.lang.System.@out.println(new StringBuilder().append("Total number of UnitContexts : ").append(UnitContext.unitContextCache.getMisses()).append(" folded: ").append(UnitContext.unitContextCache.getHits()).toString());
		}

		[LineNumberTable(new byte[]
		{
			165,
			37,
			170,
			111,
			184,
			112,
			112
		})]
		static UnitContext()
		{
			UnitContext.unitContextCache.cache(UnitContext.EMPTY);
			UnitContext.unitContextCache.cache(UnitContext.SILENCE);
		}

		
		
		private static Cache unitContextCache = new Cache();

		
		private Unit[] context;

		private int hashCode;

		
		public static UnitContext EMPTY = new UnitContext(Unit.__EMPTY_ARRAY);

		
		public static UnitContext SILENCE = new UnitContext(new Unit[]
		{
			UnitManager.__SILENCE
		});
	}
}

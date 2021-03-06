﻿using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.util;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	internal sealed class UnitContext : Object
	{
		public override int hashCode()
		{
			return this._hashCode;
		}

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
		
		private UnitContext(Unit[] array)
		{
			this.context = array;
			this._hashCode = 12;
			for (int i = 0; i < array.Length; i++)
			{
				this._hashCode += String.instancehelper_hashCode(array[i].getName()) * ((i + 1) * 34);
			}
		}
		
		public static void dumpInfo()
		{
			java.lang.System.@out.println(new StringBuilder().append("Total number of UnitContexts : ").append(UnitContext.unitContextCache.getMisses()).append(" folded: ").append(UnitContext.unitContextCache.getHits()).toString());
		}

		static UnitContext()
		{
			UnitContext.unitContextCache.cache(UnitContext.EMPTY);
			UnitContext.unitContextCache.cache(UnitContext.SILENCE);
		}
		
		private static Cache unitContextCache = new Cache();

		private Unit[] context;

		private int _hashCode;
	
		public static UnitContext EMPTY = new UnitContext(Unit.__EMPTY_ARRAY);

		public static UnitContext SILENCE = new UnitContext(new Unit[]
		{
			UnitManager.__SILENCE
		});
	}
}

using System;

using System.Threading;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic
{
	public class Unit : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		public virtual string getName()
		{
			return this.name;
		}

		public virtual int getBaseID()
		{
			return this.baseID;
		}

		public virtual bool isFiller()
		{
			return this.filler;
		}

		
		
		public virtual bool isContextDependent()
		{
			return this.getContext() != Context.__EMPTY_CONTEXT;
		}

		public virtual Context getContext()
		{
			return this.context;
		}

		[LineNumberTable(new byte[]
		{
			120,
			109,
			109,
			159,
			30,
			191,
			53
		})]
		
		public override string toString()
		{
			if (this.key == null)
			{
				if (this.context == Context.__EMPTY_CONTEXT)
				{
					this.key = new StringBuilder().append((!this.filler) ? "" : "*").append(this.name).toString();
					Thread.MemoryBarrier();
				}
				else
				{
					this.key = new StringBuilder().append((!this.filler) ? "" : "*").append(this.name).append('[').append(this.context).append(']').toString();
					Thread.MemoryBarrier();
				}
			}
			return this.key;
		}

		[LineNumberTable(new byte[]
		{
			160,
			99,
			102,
			101,
			102,
			130,
			103,
			119,
			2,
			230,
			69
		})]
		
		public static bool isContextMatch(Unit[] a, Unit[] b)
		{
			if (a == null || b == null)
			{
				return a == b;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (!java.lang.String.instancehelper_equals(a[i].getName(), b[i].getName()))
				{
					return false;
				}
			}
			return true;
		}

		
		
		private string getKey()
		{
			return this.toString();
		}

		[LineNumberTable(new byte[]
		{
			159,
			133,
			162,
			104,
			103,
			103,
			113,
			103,
			103,
			107
		})]
		
		internal Unit(string text, bool flag, int num)
		{
			this.name = text;
			this.filler = flag;
			this.silence = java.lang.String.instancehelper_equals(text, "SIL");
			this.baseID = num;
			this.baseUnit = this;
			this.context = Context.__EMPTY_CONTEXT;
		}

		[LineNumberTable(new byte[]
		{
			159,
			128,
			66,
			104,
			108,
			103,
			118,
			108,
			103,
			103
		})]
		
		internal Unit(Unit unit, bool flag, Context context)
		{
			this.name = unit.getName();
			this.filler = flag;
			this.silence = java.lang.String.instancehelper_equals(this.name, "SIL");
			this.baseID = unit.getBaseID();
			this.baseUnit = unit;
			this.context = context;
		}

		public virtual bool isSilence()
		{
			return this.silence;
		}

		public virtual Unit getBaseUnit()
		{
			return this.baseUnit;
		}

		[LineNumberTable(new byte[]
		{
			93,
			100,
			98,
			104,
			103,
			146
		})]
		
		public override bool equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (o is Unit)
			{
				Unit unit = (Unit)o;
				return java.lang.String.instancehelper_equals(this.getKey(), unit.getKey());
			}
			return false;
		}

		
		
		public override int hashCode()
		{
			return java.lang.String.instancehelper_hashCode(this.getKey());
		}

		
		
		public virtual bool isPartialMatch(string name, Context context)
		{
			return java.lang.String.instancehelper_equals(this.getName(), name) && context.isPartialMatch(this.context);
		}

		[LineNumberTable(new byte[]
		{
			160,
			86,
			103,
			107
		})]
		
		public static Unit[] getEmptyContext(int size)
		{
			Unit[] array = new Unit[size];
			Arrays.fill(array, UnitManager.__SILENCE);
			return array;
		}

		
		static Unit()
		{
		}

		
		public static Unit[] EMPTY_ARRAY
		{
			
			get
			{
				return Unit.__EMPTY_ARRAY;
			}
		}

		internal static Unit[] __EMPTY_ARRAY = new Unit[0];

		
		private string name;

		
		private bool filler;

		
		private bool silence;

		
		private int baseID;

		
		private Unit baseUnit;

		
		private Context context;

		private volatile string key;
	}
}

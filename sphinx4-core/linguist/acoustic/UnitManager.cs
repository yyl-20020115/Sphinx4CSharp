using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public class UnitManager : java.lang.Object, Configurable
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			128,
			130,
			114,
			107,
			102,
			127,
			1,
			110,
			122,
			223,
			8,
			137
		})]
		
		public virtual Unit getUnit(string name, bool filler, Context context)
		{
			Unit unit = (Unit)this.ciMap.get(name);
			if (context == Context.__EMPTY_CONTEXT)
			{
				if (unit == null)
				{
					Unit.__<clinit>();
					int num = this.nextID;
					int num2 = num;
					this.nextID = num + 1;
					unit = new Unit(name, filler, num2);
					this.ciMap.put(name, unit);
					if (this.logger != null && this.logger.isLoggable(Level.INFO))
					{
						this.logger.info(new StringBuilder().append("CI Unit: ").append(unit).toString());
					}
				}
			}
			else
			{
				unit = new Unit(unit, filler, context);
			}
			return unit;
		}

		[LineNumberTable(new byte[]
		{
			159,
			183,
			232,
			56,
			139,
			182,
			199,
			118
		})]
		
		public UnitManager()
		{
			this.ciMap = new HashMap();
			this.ciMap.put("SIL", UnitManager.__SILENCE);
			this.nextID = 2;
			this.logger = Logger.getLogger(Object.instancehelper_getClass(this).getName());
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			188,
			108
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
		}

		
		
		public virtual Unit getUnit(string name, bool filler)
		{
			return this.getUnit(name, filler, Context.__EMPTY_CONTEXT);
		}

		
		
		public virtual Unit getUnit(string name)
		{
			return this.getUnit(name, false, Context.__EMPTY_CONTEXT);
		}

		
		static UnitManager()
		{
		}

		
		public static Unit SILENCE
		{
			
			get
			{
				return UnitManager.__SILENCE;
			}
		}

		public const string SILENCE_NAME = "SIL";

		private const int SILENCE_ID = 1;

		internal static Unit __SILENCE = new Unit("SIL", true, 1);

		
		
		private Map ciMap;

		private int nextID;

		private Logger logger;
	}
}

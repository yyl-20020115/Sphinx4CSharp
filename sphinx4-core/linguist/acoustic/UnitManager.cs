using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic
{
	public class UnitManager : java.lang.Object, Configurable
	{		
		public virtual Unit getUnit(string name, bool filler, Context context)
		{
			Unit unit = (Unit)this.ciMap.get(name);
			if (context == Context.__EMPTY_CONTEXT)
			{
				if (unit == null)
				{
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
		
		public UnitManager()
		{
			this.ciMap = new HashMap();
			this.ciMap.put("SIL", UnitManager.__SILENCE);
			this.nextID = 2;
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
		}
		
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

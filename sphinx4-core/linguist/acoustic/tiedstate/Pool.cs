using ikvm.@internal;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{

	public class Pool : Object
	{		
		public virtual int size()
		{
			return this.pool.size();
		}
		
		public virtual object get(int id)
		{
			return this.pool.get(id);
		}
		
		public Pool(string name)
		{
			this.features = new EnumMap(ClassLiteral<Pool.Feature>.Value);
			this.name = name;
			this.pool = new ArrayList();
		}
		
		public virtual void setFeature(Pool.Feature feature, int value)
		{
			this.features.put(feature, Integer.valueOf(value));
		}
		
		public virtual void put(int id, object o)
		{
			if (id == this.pool.size())
			{
				this.pool.add(o);
			}
			else
			{
				this.pool.set(id, o);
			}
		}
		
		public virtual void logInfo(Logger logger)
		{
			logger.info(new StringBuilder().append("Pool ").append(this.name).append(" Entries: ").append(this.size()).toString());
		}

		public virtual string getName()
		{
			return this.name;
		}
		
		public virtual int indexOf(object @object)
		{
			return this.pool.indexOf(@object);
		}
		
		public virtual int getFeature(Pool.Feature feature, int defaultValue)
		{
			Integer integer = (Integer)this.features.get(feature);
			return (integer != null) ? integer.intValue() : defaultValue;
		}

		private string name;
		
		private List pool;
		
		private Map features;
				
		[System.Serializable]
		public sealed class Feature : Enum
		{
			private Feature(string text, int num) : base(text, num)
			{
				System.GC.KeepAlive(this);
			}
			
			public static Pool.Feature[] values()
			{
				return (Pool.Feature[])Pool.Feature._VALUES_.Clone();
			}
			
			public static Pool.Feature valueOf(string name)
			{
				return (Pool.Feature)Enum.valueOf(ClassLiteral<Pool.Feature>.Value, name);
			}
			
			public static Pool.Feature NUM_SENONES
			{
				
				get
				{
					return Pool.Feature.__NUM_SENONES;
				}
			}

			public static Pool.Feature NUM_GAUSSIANS_PER_STATE
			{
				
				get
				{
					return Pool.Feature.__NUM_GAUSSIANS_PER_STATE;
				}
			}

			public static Pool.Feature NUM_STREAMS
			{
				
				get
				{
					return Pool.Feature.__NUM_STREAMS;
				}
			}
			
			internal static Pool.Feature __NUM_SENONES = new Pool.Feature("NUM_SENONES", 0);

			internal static Pool.Feature __NUM_GAUSSIANS_PER_STATE = new Pool.Feature("NUM_GAUSSIANS_PER_STATE", 1);
			
			internal static Pool.Feature __NUM_STREAMS = new Pool.Feature("NUM_STREAMS", 2);

			private static Pool.Feature[] _VALUES_ = new Pool.Feature[]
			{
				Pool.Feature.__NUM_SENONES,
				Pool.Feature.__NUM_GAUSSIANS_PER_STATE,
				Pool.Feature.__NUM_STREAMS
			};

			
			[System.Serializable]
			public enum __Enum
			{
				NUM_SENONES,
				NUM_GAUSSIANS_PER_STATE,
				NUM_STREAMS
			}
		}
	}
}

using System;

using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	
	public class Pool : java.lang.Object
	{
		
		
		public virtual int size()
		{
			return this.pool.size();
		}

		
		
		
		public virtual object get(int id)
		{
			return this.pool.get(id);
		}

		[LineNumberTable(new byte[]
		{
			159,
			175,
			232,
			57,
			245,
			72,
			103,
			107
		})]
		
		public Pool(string name)
		{
			EnumMap.__<clinit>();
			this.features = new EnumMap(ClassLiteral<Pool.Feature>.Value);
			this.name = name;
			this.pool = new ArrayList();
		}

		[LineNumberTable(new byte[]
		{
			57,
			115
		})]
		
		public virtual void setFeature(Pool.Feature feature, int value)
		{
			this.features.put(feature, Integer.valueOf(value));
		}

		
		[LineNumberTable(new byte[]
		{
			25,
			110,
			143,
			142
		})]
		
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

		[LineNumberTable(new byte[]
		{
			47,
			127,
			27
		})]
		
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

		[LineNumberTable(new byte[]
		{
			68,
			114
		})]
		
		public virtual int getFeature(Pool.Feature feature, int defaultValue)
		{
			Integer integer = (Integer)this.features.get(feature);
			return (integer != null) ? integer.intValue() : defaultValue;
		}

		
		private string name;

		
		
		private List pool;

		
		
		private Map features;

		
		
		[SourceFile("Pool.java")]
		
		[Serializable]
		public sealed class Feature : Enum
		{
			
			public static void __<clinit>()
			{
			}

			
			
			
			private Feature(string text, int num) : base(text, num)
			{
				GC.KeepAlive(this);
			}

			
			
			public static Pool.Feature[] values()
			{
				return (Pool.Feature[])Pool.Feature._VALUES_.Clone();
			}

			
			
			public static Pool.Feature valueOf(string name)
			{
				return (Pool.Feature)Enum.valueOf(ClassLiteral<Pool.Feature>.Value, name);
			}

			
			static Feature()
			{
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

			
			[Serializable]
			public enum __Enum
			{
				NUM_SENONES,
				NUM_GAUSSIANS_PER_STATE,
				NUM_STREAMS
			}
		}
	}
}

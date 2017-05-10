using System;
using System.Collections;

using IKVM.Attributes;
using ikvm.@internal;
using ikvm.lang;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[Implements(new string[]
	{
		"java.lang.Iterable"
	})]
	
	public class HMMManager : java.lang.Object, Iterable, IEnumerable
	{
		[LineNumberTable(new byte[]
		{
			26,
			130,
			127,
			6,
			99,
			137,
			98
		})]
		
		private int getNumHMMs()
		{
			int num = 0;
			Iterator iterator = this.hmmsPerPosition.values().iterator();
			while (iterator.hasNext())
			{
				Map map = (Map)iterator.next();
				if (map != null)
				{
					num += map.size();
				}
			}
			return num;
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			232,
			61,
			107,
			181,
			116,
			51,
			134
		})]
		
		public HMMManager()
		{
			this.allHMMs = new ArrayList();
			EnumMap.__<clinit>();
			this.hmmsPerPosition = new EnumMap(ClassLiteral<HMMPosition>.Value);
			HMMPosition[] array = HMMPosition.values();
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				HMMPosition hmmposition = array[i];
				this.hmmsPerPosition.put(hmmposition, new HashMap());
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			185,
			127,
			4,
			109
		})]
		
		public virtual void put(HMM hmm)
		{
			((Map)this.hmmsPerPosition.get(hmm.getPosition())).put(hmm.getUnit(), hmm);
			this.allHMMs.add(hmm);
		}

		
		
		public virtual HMM get(HMMPosition position, Unit unit)
		{
			return (HMM)((Map)this.hmmsPerPosition.get(position)).get(unit);
		}

		
		
		
		public virtual Iterator iterator()
		{
			return this.allHMMs.iterator();
		}

		[LineNumberTable(new byte[]
		{
			43,
			127,
			16
		})]
		
		public virtual void logInfo(Logger logger)
		{
			logger.info(new StringBuilder().append("HMM Manager: ").append(this.getNumHMMs()).append(" hmms").toString());
		}

		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new IterableEnumerator(this);
		}

		
		
		private List allHMMs;

		
		
		private Map hmmsPerPosition;
	}
}

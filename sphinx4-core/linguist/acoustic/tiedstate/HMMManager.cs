using ikvm.@internal;
using ikvm.lang;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public class HMMManager : java.lang.Object, Iterable, System.Collections.IEnumerable
	{		
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
		
		public HMMManager()
		{
			this.allHMMs = new java.util.ArrayList();
			this.hmmsPerPosition = new EnumMap(ClassLiteral<HMMPosition>.Value);
			HMMPosition[] array = HMMPosition.values();
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				HMMPosition hmmposition = array[i];
				this.hmmsPerPosition.put(hmmposition, new HashMap());
			}
		}
		
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
		
		public virtual void logInfo(Logger logger)
		{
			logger.info(new StringBuilder().append("HMM Manager: ").append(this.getNumHMMs()).append(" hmms").toString());
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new IterableEnumerator(this);
		}
		
		private List allHMMs;
		
		private Map hmmsPerPosition;
	}
}

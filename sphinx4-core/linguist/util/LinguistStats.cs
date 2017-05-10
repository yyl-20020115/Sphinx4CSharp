using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.util
{
	public class LinguistStats : LinguistProcessor
	{
		[LineNumberTable(new byte[]
		{
			21,
			119,
			99,
			135,
			110,
			115
		})]
		
		private void incrementStateTypeCount(SearchState searchState)
		{
			Integer integer = (Integer)this.stateCountByType.get(java.lang.Object.instancehelper_getClass(searchState));
			if (integer == null)
			{
				integer = Integer.valueOf(0);
			}
			integer = Integer.valueOf(integer.intValue() + 1);
			this.stateCountByType.put(java.lang.Object.instancehelper_getClass(searchState), integer);
		}

		[LineNumberTable(new byte[]
		{
			32,
			127,
			6,
			127,
			33
		})]
		
		private void dumpStateTypeCounts()
		{
			Iterator iterator = this.stateCountByType.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				java.lang.System.@out.println(new StringBuilder().append("# ").append(entry.getKey()).append(": ").append(entry.getValue()).toString());
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			166,
			233,
			61,
			203
		})]
		
		public LinguistStats(Linguist linguist) : base(linguist)
		{
			this.stateCountByType = new HashMap();
		}

		[LineNumberTable(new byte[]
		{
			159,
			169,
			232,
			58,
			235,
			72
		})]
		
		public LinguistStats()
		{
			this.stateCountByType = new HashMap();
		}

		[LineNumberTable(new byte[]
		{
			159,
			176,
			103,
			102,
			102,
			98,
			114,
			107,
			110,
			106,
			100,
			104,
			105,
			105,
			108,
			108,
			9,
			232,
			76,
			101,
			111,
			127,
			5,
			102
		})]
		
		public override void run()
		{
			Linguist linguist = this.getLinguist();
			LinkedList linkedList = new LinkedList();
			HashSet hashSet = new HashSet();
			int num = 0;
			linkedList.add(linguist.getSearchGraph().getInitialState());
			while (!linkedList.isEmpty())
			{
				SearchState searchState = (SearchState)linkedList.remove(0);
				if (!hashSet.contains(searchState))
				{
					num++;
					this.incrementStateTypeCount(searchState);
					hashSet.add(searchState);
					SearchStateArc[] successors = searchState.getSuccessors();
					for (int i = successors.Length - 1; i >= 0; i += -1)
					{
						SearchState state = successors[i].getState();
						linkedList.add(state);
					}
				}
			}
			java.lang.System.@out.println("# ----------- linguist stats ------------ ");
			java.lang.System.@out.println(new StringBuilder().append("# Total states: ").append(num).toString());
			this.dumpStateTypeCounts();
		}

		
		
		private Map stateCountByType;
	}
}

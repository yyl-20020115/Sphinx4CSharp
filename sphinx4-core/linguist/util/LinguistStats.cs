using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.util
{
	public class LinguistStats : LinguistProcessor
	{		
		private void incrementStateTypeCount(SearchState searchState)
		{
			Integer integer = (Integer)this.stateCountByType.get(Object.instancehelper_getClass(searchState));
			if (integer == null)
			{
				integer = Integer.valueOf(0);
			}
			integer = Integer.valueOf(integer.intValue() + 1);
			this.stateCountByType.put(Object.instancehelper_getClass(searchState), integer);
		}
		
		private void dumpStateTypeCounts()
		{
			Iterator iterator = this.stateCountByType.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				java.lang.System.@out.println(new StringBuilder().append("# ").append(entry.getKey()).append(": ").append(entry.getValue()).toString());
			}
		}
		
		public LinguistStats(Linguist linguist) : base(linguist)
		{
			this.stateCountByType = new HashMap();
		}
		
		public LinguistStats()
		{
			this.stateCountByType = new HashMap();
		}
		
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
					for (int i = successors.Length - 1; i >= 0; i --)
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

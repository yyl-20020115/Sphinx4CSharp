using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	public class Connect : java.lang.Object
	{
		private static void calcCoAccessible(Fst fst, State state, ArrayList arrayList, HashSet hashSet)
		{
			ArrayList arrayList2 = new ArrayList();
			Iterator iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				ArrayList arrayList3 = (ArrayList)iterator.next();
				int num = arrayList3.lastIndexOf(state);
				if (num != -1 && (state.getFinalWeight() != fst.getSemiring().zero() || hashSet.contains(state)))
				{
					for (int i = num; i > -1; i += -1)
					{
						if (!hashSet.contains(arrayList3.get(i)))
						{
							arrayList2.add(arrayList3.get(i));
							hashSet.add(arrayList3.get(i));
						}
					}
				}
			}
			iterator = arrayList2.iterator();
			while (iterator.hasNext())
			{
				State state2 = (State)iterator.next();
				Connect.calcCoAccessible(fst, state2, arrayList, hashSet);
			}
		}
		
		private static void duplicatePath(int num, State state, State state2, ArrayList arrayList)
		{
			ArrayList arrayList2 = (ArrayList)arrayList.get(num);
			int num2 = arrayList2.indexOf(state);
			int num3 = arrayList2.indexOf(state2);
			if (num3 == -1)
			{
				num3 = arrayList2.size() - 1;
			}
			ArrayList arrayList3 = new ArrayList(arrayList2.subList(num2, num3));
			arrayList.add(arrayList3);
		}
		
		private static void addExploredArc(int num, Arc arc, ArrayList[] array)
		{
			if (array[num] == null)
			{
				array[num] = new ArrayList();
			}
			array[num].add(arc);
		}
		
		private static State depthFirstSearchNext(Fst fst, State state, ArrayList arrayList, ArrayList[] array, HashSet hashSet)
		{
			int num = arrayList.size() - 1;
			ArrayList arrayList2 = array[state.getId()];
			((ArrayList)arrayList.get(num)).add(state);
			if (state.getNumArcs() != 0)
			{
				int num2 = 0;
				int numArcs = state.getNumArcs();
				for (int i = 0; i < numArcs; i++)
				{
					Arc arc = state.getArc(i);
					if (arrayList2 == null || !arrayList2.contains(arc))
					{
						num = arrayList.size() - 1;
						int num3 = num2;
						num2++;
						if (num3 > 0)
						{
							Connect.duplicatePath(num, fst.getStart(), state, arrayList);
							num = arrayList.size() - 1;
							((ArrayList)arrayList.get(num)).add(state);
						}
						State nextState = arc.getNextState();
						Connect.addExploredArc(state.getId(), arc, array);
						if (nextState.getId() != state.getId())
						{
							Connect.depthFirstSearchNext(fst, nextState, arrayList, array, hashSet);
						}
					}
				}
			}
			int num4 = arrayList.size() - 1;
			hashSet.add(state);
			return state;
		}
		
		private static void depthFirstSearch(Fst fst, HashSet hashSet, ArrayList arrayList, ArrayList[] array, HashSet hashSet2)
		{
			State start = fst.getStart();
			State state = start;
			do
			{
				if (!hashSet.contains(start))
				{
					state = Connect.depthFirstSearchNext(fst, start, arrayList, array, hashSet);
				}
			}
			while (start.getId() != state.getId());
			int numStates = fst.getNumStates();
			for (int i = 0; i < numStates; i++)
			{
				State state2 = fst.getState(i);
				if (state2.getFinalWeight() != fst.getSemiring().zero())
				{
					Connect.calcCoAccessible(fst, state2, arrayList, hashSet2);
				}
			}
		}
		
		public Connect()
		{
		}
		
		public static void apply(Fst fst)
		{
			if (fst.getSemiring() == null)
			{
				java.lang.System.@out.println("Fst has no semiring.");
				return;
			}
			HashSet hashSet = new HashSet();
			HashSet hashSet2 = new HashSet();
			ArrayList[] array = new ArrayList[fst.getNumStates()];
			ArrayList arrayList = new ArrayList();
			arrayList.add(new ArrayList());
			Connect.depthFirstSearch(fst, hashSet, arrayList, array, hashSet2);
			HashSet hashSet3 = new HashSet();
			for (int i = 0; i < fst.getNumStates(); i++)
			{
				State state = fst.getState(i);
				if (!hashSet.contains(state) && !hashSet2.contains(state))
				{
					hashSet3.add(state);
				}
			}
			fst.deleteStates(hashSet3);
		}
	}
}

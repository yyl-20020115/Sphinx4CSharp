using edu.cmu.sphinx.fst.semiring;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	public class RmEpsilon : Object
	{	
		private static Float getPathWeight(State state, State state2, HashMap[] array)
		{
			if (array[state.getId()] != null)
			{
				return (Float)array[state.getId()].get(state2);
			}
			return null;
		}
		
		private static void put(State state, State state2, float num, HashMap[] array)
		{
			HashMap hashMap = array[state.getId()];
			if (hashMap == null)
			{
				hashMap = new HashMap();
				array[state.getId()] = hashMap;
			}
			hashMap.put(state2, Float.valueOf(num));
		}
		
		private static void calcClosure(Fst fst, State state, HashMap[] array, Semiring semiring)
		{
			int numArcs = state.getNumArcs();
			for (int i = 0; i < numArcs; i++)
			{
				Arc arc = state.getArc(i);
				if (arc.getIlabel() == 0 && arc.getOlabel() == 0)
				{
					if (array[arc.getNextState().getId()] == null)
					{
						RmEpsilon.calcClosure(fst, arc.getNextState(), array, semiring);
					}
					if (array[arc.getNextState().getId()] != null)
					{
						Iterator iterator = array[arc.getNextState().getId()].keySet().iterator();
						while (iterator.hasNext())
						{
							State state2 = (State)iterator.next();
							float num = semiring.times(RmEpsilon.getPathWeight(arc.getNextState(), state2, array).floatValue(), arc.getWeight());
							RmEpsilon.add(state, state2, num, array, semiring);
						}
					}
					RmEpsilon.add(state, arc.getNextState(), arc.getWeight(), array, semiring);
				}
			}
		}
		
		private static void add(State state, State state2, float num, HashMap[] array, Semiring semiring)
		{
			Float pathWeight = RmEpsilon.getPathWeight(state, state2, array);
			if (pathWeight == null)
			{
				RmEpsilon.put(state, state2, num, array);
			}
			else
			{
				RmEpsilon.put(state, state2, semiring.plus(num, pathWeight.floatValue()), array);
			}
		}
		
		private RmEpsilon()
		{
		}
		
		public static Fst get(Fst fst)
		{
			if (fst == null)
			{
				return null;
			}
			if (fst.getSemiring() == null)
			{
				return null;
			}
			Semiring semiring = fst.getSemiring();
			Fst fst2 = new Fst(semiring);
			HashMap[] array = new HashMap[fst.getNumStates()];
			State[] array2 = new State[fst.getNumStates()];
			State[] array3 = new State[fst.getNumStates()];
			int numStates = fst.getNumStates();
			for (int i = 0; i < numStates; i++)
			{
				State state = fst.getState(i);
				State state2 = new State(state.getFinalWeight());
				fst2.addState(state2);
				array2[state.getId()] = state2;
				array3[state2.getId()] = state;
				if (state2.getId() == fst.getStart().getId())
				{
					fst2.setStart(state2);
				}
			}
			for (int i = 0; i < numStates; i++)
			{
				State state = fst.getState(i);
				State state2 = array2[state.getId()];
				int numArcs = state.getNumArcs();
				for (int j = 0; j < numArcs; j++)
				{
					Arc arc = state.getArc(j);
					if (arc.getIlabel() != 0 || arc.getOlabel() != 0)
					{
						state2.addArc(new Arc(arc.getIlabel(), arc.getOlabel(), arc.getWeight(), array2[arc.getNextState().getId()]));
					}
				}
				if (array[state.getId()] == null)
				{
					RmEpsilon.calcClosure(fst, state, array, semiring);
				}
			}
			numStates = fst2.getNumStates();
			for (int i = 0; i < numStates; i++)
			{
				State state = fst2.getState(i);
				State state2 = array3[state.getId()];
				if (array[state2.getId()] != null)
				{
					Iterator iterator = array[state2.getId()].keySet().iterator();
					while (iterator.hasNext())
					{
						State state3 = (State)iterator.next();
						State state4 = state3;
						if (state4.getFinalWeight() != semiring.zero())
						{
							state.setFinalWeight(semiring.plus(state.getFinalWeight(), semiring.times(RmEpsilon.getPathWeight(state2, state4, array).floatValue(), state4.getFinalWeight())));
						}
						int numArcs2 = state4.getNumArcs();
						for (int k = 0; k < numArcs2; k++)
						{
							Arc arc2 = state4.getArc(k);
							if (arc2.getIlabel() != 0 || arc2.getOlabel() != 0)
							{
								Arc arc3 = new Arc(arc2.getIlabel(), arc2.getOlabel(), semiring.times(arc2.getWeight(), RmEpsilon.getPathWeight(state2, state4, array).floatValue()), array2[arc2.getNextState().getId()]);
								state.addArc(arc3);
							}
						}
					}
				}
			}
			fst2.setIsyms(fst.getIsyms());
			fst2.setOsyms(fst.getOsyms());
			Connect.apply(fst2);
			return fst2;
		}
	}
}

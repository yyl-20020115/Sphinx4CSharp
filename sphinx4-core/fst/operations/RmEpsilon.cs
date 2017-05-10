using System;

using edu.cmu.sphinx.fst.semiring;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	public class RmEpsilon : java.lang.Object
	{
		
		[LineNumberTable(new byte[]
		{
			47,
			106,
			180
		})]
		
		private static Float getPathWeight(State state, State state2, HashMap[] array)
		{
			if (array[state.getId()] != null)
			{
				return (Float)array[state.getId()].get(state2);
			}
			return null;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			183,
			105,
			99,
			102,
			137,
			111
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			18,
			162,
			103,
			105,
			104,
			118,
			111,
			142,
			114,
			109,
			37,
			158,
			98,
			115,
			5,
			167,
			108,
			130,
			244,
			49,
			233,
			82
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			4,
			105,
			99,
			140,
			183
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			175,
			102
		})]
		
		private RmEpsilon()
		{
		}

		[LineNumberTable(new byte[]
		{
			63,
			99,
			162,
			104,
			162,
			135,
			167,
			108,
			108,
			141,
			104,
			108,
			138,
			110,
			104,
			107,
			108,
			116,
			232,
			56,
			235,
			76,
			108,
			138,
			107,
			105,
			105,
			107,
			114,
			114,
			109,
			11,
			229,
			61,
			235,
			74,
			107,
			234,
			48,
			235,
			85,
			104,
			108,
			106,
			108,
			110,
			127,
			16,
			100,
			111,
			112,
			108,
			37,
			37,
			202,
			105,
			108,
			107,
			114,
			113,
			106,
			42,
			136,
			114,
			233,
			57,
			235,
			74,
			229,
			42,
			235,
			90,
			108,
			140,
			134
		})]
		
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

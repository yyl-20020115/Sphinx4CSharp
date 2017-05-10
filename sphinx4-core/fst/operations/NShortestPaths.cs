using System;

using edu.cmu.sphinx.fst.semiring;
using edu.cmu.sphinx.fst.utils;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	public class NShortestPaths : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			5,
			135,
			108,
			140,
			135,
			108,
			140,
			135,
			142,
			115,
			147,
			108,
			115,
			138,
			107,
			143,
			113,
			107,
			105,
			112,
			104,
			42,
			135,
			102,
			112,
			113,
			112,
			37,
			37,
			166,
			107,
			234,
			52,
			235,
			80,
			101
		})]
		
		public static float[] shortestDistance(Fst fst)
		{
			Fst fst2 = Reverse.get(fst);
			float[] array = new float[fst2.getNumStates()];
			float[] array2 = new float[fst2.getNumStates()];
			Semiring semiring = fst2.getSemiring();
			Arrays.fill(array, semiring.zero());
			Arrays.fill(array2, semiring.zero());
			LinkedHashSet linkedHashSet = new LinkedHashSet();
			linkedHashSet.add(fst2.getStart());
			array[fst2.getStart().getId()] = semiring.one();
			array2[fst2.getStart().getId()] = semiring.one();
			while (!linkedHashSet.isEmpty())
			{
				State state = (State)linkedHashSet.iterator().next();
				linkedHashSet.remove(state);
				float f = array2[state.getId()];
				array2[state.getId()] = semiring.zero();
				for (int i = 0; i < state.getNumArcs(); i++)
				{
					Arc arc = state.getArc(i);
					State nextState = arc.getNextState();
					float num = array[arc.getNextState().getId()];
					float num2 = semiring.plus(num, semiring.times(f, arc.getWeight()));
					if (num != num2)
					{
						array[arc.getNextState().getId()] = num2;
						array2[arc.getNextState().getId()] = semiring.plus(array2[arc.getNextState().getId()], semiring.times(f, arc.getWeight()));
						if (!linkedHashSet.contains(nextState))
						{
							linkedHashSet.add(nextState);
						}
					}
				}
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			159,
			185,
			102
		})]
		
		private NShortestPaths()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			114,
			66,
			99,
			162,
			104,
			130,
			98,
			99,
			135,
			103,
			103,
			108,
			140,
			136,
			134,
			141,
			246,
			88,
			97,
			108,
			97,
			140,
			104,
			116,
			106,
			139,
			108,
			110,
			110,
			142,
			110,
			104,
			108,
			139,
			173,
			119,
			122,
			110,
			107,
			112,
			107,
			51,
			229,
			61,
			232,
			73,
			110,
			153,
			124,
			165,
			112,
			110,
			107,
			118,
			98,
			115,
			108,
			234,
			58,
			235,
			73,
			133
		})]
		
		public static Fst get(Fst fst, int n, bool determinize)
		{
			if (fst == null)
			{
				return null;
			}
			if (fst.getSemiring() == null)
			{
				return null;
			}
			Fst fst2 = fst;
			if (determinize)
			{
				fst2 = Determinize.get(fst);
			}
			Semiring semiring = fst2.getSemiring();
			Fst fst3 = new Fst(semiring);
			fst3.setIsyms(fst2.getIsyms());
			fst3.setOsyms(fst2.getOsyms());
			float[] array = NShortestPaths.shortestDistance(fst2);
			ExtendFinal.apply(fst2);
			int[] array2 = new int[fst2.getNumStates()];
			PriorityQueue.__<clinit>();
			PriorityQueue priorityQueue = new PriorityQueue(10, new NShortestPaths_1(array, semiring));
			HashMap hashMap = new HashMap(fst.getNumStates());
			HashMap hashMap2 = new HashMap(fst.getNumStates());
			State start = fst2.getStart();
			Pair pair = new Pair(start, Float.valueOf(semiring.one()));
			priorityQueue.add(pair);
			hashMap.put(pair, null);
			while (!priorityQueue.isEmpty())
			{
				Pair pair2 = (Pair)priorityQueue.remove();
				State state = (State)pair2.getLeft();
				Float @float = (Float)pair2.getRight();
				State state2 = new State(state.getFinalWeight());
				fst3.addState(state2);
				hashMap2.put(pair2, state2);
				if (hashMap.get(pair2) == null)
				{
					fst3.setStart(state2);
				}
				else
				{
					State state3 = (State)hashMap2.get(hashMap.get(pair2));
					State state4 = (State)((Pair)hashMap.get(pair2)).getLeft();
					for (int i = 0; i < state4.getNumArcs(); i++)
					{
						Arc arc = state4.getArc(i);
						if (arc.getNextState().equals(state))
						{
							state3.addArc(new Arc(arc.getIlabel(), arc.getOlabel(), arc.getWeight(), state2));
						}
					}
				}
				Integer integer = Integer.valueOf(state.getId());
				int[] array3 = array2;
				int num = integer.intValue();
				int[] array4 = array3;
				array4[num]++;
				if (array2[integer.intValue()] == n && state.getFinalWeight() != semiring.zero())
				{
					break;
				}
				if (array2[integer.intValue()] <= n)
				{
					for (int j = 0; j < state.getNumArcs(); j++)
					{
						Arc arc2 = state.getArc(j);
						float num2 = semiring.times(@float.floatValue(), arc2.getWeight());
						Pair pair3 = new Pair(arc2.getNextState(), Float.valueOf(num2));
						hashMap.put(pair3, pair2);
						priorityQueue.add(pair3);
					}
				}
			}
			return fst3;
		}
	}
}

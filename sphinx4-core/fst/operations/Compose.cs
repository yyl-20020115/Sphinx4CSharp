using System;

using edu.cmu.sphinx.fst.semiring;
using edu.cmu.sphinx.fst.utils;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	public class Compose : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			120,
			135,
			99,
			133,
			103,
			167,
			106,
			108,
			105,
			109,
			105,
			109,
			103,
			116,
			117,
			117,
			105,
			54,
			168,
			167,
			104,
			118,
			105,
			55,
			232,
			69,
			104,
			118,
			105,
			55,
			200
		})]
		
		public static Fst getFilter(string[] syms, Semiring semiring)
		{
			Fst fst = new Fst(semiring);
			int num = syms.Length;
			int num2 = syms.Length + 1;
			fst.setIsyms(syms);
			fst.setOsyms(syms);
			State state = new State(syms.Length + 3);
			state.setFinalWeight(semiring.one());
			State state2 = new State(syms.Length);
			state2.setFinalWeight(semiring.one());
			State state3 = new State(syms.Length);
			state3.setFinalWeight(semiring.one());
			fst.addState(state);
			state.addArc(new Arc(num2, num, semiring.one(), state));
			state.addArc(new Arc(num, num, semiring.one(), state2));
			state.addArc(new Arc(num2, num2, semiring.one(), state3));
			for (int i = 1; i < syms.Length; i++)
			{
				state.addArc(new Arc(i, i, semiring.one(), state));
			}
			fst.setStart(state);
			fst.addState(state2);
			state2.addArc(new Arc(num, num, semiring.one(), state2));
			for (int i = 1; i < syms.Length; i++)
			{
				state2.addArc(new Arc(i, i, semiring.one(), state));
			}
			fst.addState(state3);
			state3.addArc(new Arc(num2, num2, semiring.one(), state3));
			for (int i = 1; i < syms.Length; i++)
			{
				state3.addArc(new Arc(i, i, semiring.one(), state));
			}
			return fst;
		}

		[LineNumberTable(new byte[]
		{
			160,
			116,
			103,
			135,
			99,
			133,
			100,
			134,
			104,
			108,
			138,
			117,
			103,
			105,
			107,
			109,
			107,
			108,
			232,
			59,
			232,
			72,
			99,
			104,
			186,
			152,
			100,
			104,
			187,
			247,
			39,
			235,
			93
		})]
		
		public static void augment(int label, Fst fst, Semiring semiring)
		{
			string[] isyms = fst.getIsyms();
			string[] osyms = fst.getOsyms();
			int ilabel = isyms.Length;
			int iLabel = isyms.Length + 1;
			int oLabel = osyms.Length;
			int olabel = osyms.Length + 1;
			int numStates = fst.getNumStates();
			for (int i = 0; i < numStates; i++)
			{
				State state = fst.getState(i);
				int num = (!(fst is ImmutableFst)) ? state.getNumArcs() : (state.getNumArcs() - 1);
				for (int j = 0; j < num; j++)
				{
					Arc arc = state.getArc(j);
					if (label == 1 && arc.getOlabel() == 0)
					{
						arc.setOlabel(olabel);
					}
					else if (label == 0 && arc.getIlabel() == 0)
					{
						arc.setIlabel(ilabel);
					}
				}
				if (label == 0)
				{
					if (fst is ImmutableFst)
					{
						state.setArc(num, new Arc(iLabel, 0, semiring.one(), state));
					}
					else
					{
						state.addArc(new Arc(iLabel, 0, semiring.one(), state));
					}
				}
				else if (label == 1)
				{
					if (fst is ImmutableFst)
					{
						state.setArc(num, new Arc(0, oLabel, semiring.one(), state));
					}
					else
					{
						state.addArc(new Arc(0, oLabel, semiring.one(), state));
					}
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			128,
			98,
			147,
			162,
			135,
			102,
			134,
			104,
			136,
			104,
			111,
			162,
			107,
			106,
			37,
			172,
			104,
			104,
			107,
			137,
			107,
			109,
			110,
			110,
			111,
			105,
			105,
			108,
			107,
			108,
			107,
			115,
			101,
			115,
			105,
			105,
			139,
			111,
			100,
			99,
			103,
			5,
			172,
			104,
			107,
			137,
			113,
			154,
			233,
			43,
			11,
			235,
			91,
			133,
			108,
			140
		})]
		
		public static Fst compose(Fst fst1, Fst fst2, Semiring semiring, bool sorted)
		{
			if (!Arrays.equals(fst1.getOsyms(), fst2.getIsyms()))
			{
				return null;
			}
			Fst fst3 = new Fst(semiring);
			HashMap hashMap = new HashMap();
			LinkedList linkedList = new LinkedList();
			State state = fst1.getStart();
			State state2 = fst2.getStart();
			if (state == null || state2 == null)
			{
				java.lang.System.err.println("Cannot find initial state.");
				return null;
			}
			Pair pair = new Pair(state, state2);
			State state3 = new State(semiring.times(state.getFinalWeight(), state2.getFinalWeight()));
			fst3.addState(state3);
			fst3.setStart(state3);
			hashMap.put(pair, state3);
			linkedList.add(pair);
			while (!linkedList.isEmpty())
			{
				pair = (Pair)linkedList.remove();
				state = (State)pair.getLeft();
				state2 = (State)pair.getRight();
				state3 = (State)hashMap.get(pair);
				int numArcs = state.getNumArcs();
				int numArcs2 = state2.getNumArcs();
				for (int i = 0; i < numArcs; i++)
				{
					Arc arc = state.getArc(i);
					for (int j = 0; j < numArcs2; j++)
					{
						Arc arc2 = state2.getArc(j);
						if (sorted && arc.getOlabel() < arc2.getIlabel())
						{
							break;
						}
						if (arc.getOlabel() == arc2.getIlabel())
						{
							State nextState = arc.getNextState();
							State nextState2 = arc2.getNextState();
							Pair pair2 = new Pair(nextState, nextState2);
							State state4 = (State)hashMap.get(pair2);
							if (state4 == null)
							{
								state4 = new State(semiring.times(nextState.getFinalWeight(), nextState2.getFinalWeight()));
								fst3.addState(state4);
								hashMap.put(pair2, state4);
								linkedList.add(pair2);
							}
							Arc arc3 = new Arc(arc.getIlabel(), arc2.getOlabel(), semiring.times(arc.getWeight(), arc2.getWeight()), state4);
							state3.addArc(arc3);
						}
					}
				}
			}
			fst3.setIsyms(fst1.getIsyms());
			fst3.setOsyms(fst2.getOsyms());
			return fst3;
		}

		[LineNumberTable(new byte[]
		{
			159,
			183,
			102
		})]
		
		private Compose()
		{
		}

		[LineNumberTable(new byte[]
		{
			86,
			102,
			162,
			147,
			162,
			109,
			104,
			136,
			138,
			202
		})]
		
		public static Fst get(Fst fst1, Fst fst2, Semiring semiring)
		{
			if (fst1 == null || fst2 == null)
			{
				return null;
			}
			if (!Arrays.equals(fst1.getOsyms(), fst2.getIsyms()))
			{
				return null;
			}
			Fst filter = Compose.getFilter(fst1.getOsyms(), semiring);
			Compose.augment(1, fst1, semiring);
			Compose.augment(0, fst2, semiring);
			Fst fst3 = Compose.compose(fst1, filter, semiring, false);
			return Compose.compose(fst3, fst2, semiring, false);
		}
	}
}

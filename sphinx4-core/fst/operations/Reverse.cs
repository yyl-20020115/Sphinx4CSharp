using System;

using edu.cmu.sphinx.fst.semiring;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.fst.operations
{
	public class Reverse : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			183,
			104,
			162,
			134,
			135,
			108,
			135,
			108,
			140,
			108,
			103,
			104,
			106,
			109,
			104,
			107,
			111,
			232,
			58,
			232,
			74,
			152,
			107,
			106,
			107,
			105,
			105,
			107,
			112,
			113,
			115,
			233,
			59,
			232,
			60,
			235,
			77,
			102
		})]
		
		public static Fst get(Fst fst)
		{
			if (fst.getSemiring() == null)
			{
				return null;
			}
			ExtendFinal.apply(fst);
			Semiring semiring = fst.getSemiring();
			Fst fst2 = new Fst(fst.getNumStates());
			fst2.setSemiring(semiring);
			fst2.setIsyms(fst.getOsyms());
			fst2.setOsyms(fst.getIsyms());
			State[] array = new State[fst.getNumStates()];
			int numStates = fst.getNumStates();
			for (int i = 0; i < numStates; i++)
			{
				State state = fst.getState(i);
				State state2 = new State(semiring.zero());
				fst2.addState(state2);
				array[state.getId()] = state2;
				if (state.getFinalWeight() != semiring.zero())
				{
					fst2.setStart(state2);
				}
			}
			array[fst.getStart().getId()].setFinalWeight(semiring.one());
			for (int i = 0; i < numStates; i++)
			{
				State state = fst.getState(i);
				State state2 = array[state.getId()];
				int numArcs = state.getNumArcs();
				for (int j = 0; j < numArcs; j++)
				{
					Arc arc = state.getArc(j);
					State state3 = array[arc.getNextState().getId()];
					Arc arc2 = new Arc(arc.getIlabel(), arc.getOlabel(), semiring.reverse(arc.getWeight()), state2);
					state3.addArc(arc2);
				}
			}
			ExtendFinal.undo(fst);
			return fst2;
		}

		[LineNumberTable(new byte[]
		{
			159,
			173,
			102
		})]
		
		private Reverse()
		{
		}
	}
}

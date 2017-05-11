using edu.cmu.sphinx.fst.semiring;

namespace edu.cmu.sphinx.fst.operations
{
	public class Reverse : java.lang.Object
	{		
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
		
		private Reverse()
		{
		}
	}
}

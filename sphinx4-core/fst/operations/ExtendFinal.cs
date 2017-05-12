using edu.cmu.sphinx.fst.semiring;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.fst.operations
{
	public class ExtendFinal : Object
	{		
		private ExtendFinal()
		{
		}
		
		public static void apply(Fst fst)
		{
			Semiring semiring = fst.getSemiring();
			ArrayList arrayList = new ArrayList();
			int numStates = fst.getNumStates();
			for (int i = 0; i < numStates; i++)
			{
				State state = fst.getState(i);
				if (state.getFinalWeight() != semiring.zero())
				{
					arrayList.add(state);
				}
			}
			State state2 = new State(semiring.one());
			fst.addState(state2);
			Iterator iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				State state3 = (State)iterator.next();
				state3.addArc(new Arc(0, 0, state3.getFinalWeight(), state2));
				state3.setFinalWeight(semiring.zero());
			}
		}
		
		public static void undo(Fst fst)
		{
			State state = null;
			int numStates = fst.getNumStates();
			for (int i = 0; i < numStates; i++)
			{
				State state2 = fst.getState(i);
				if (state2.getFinalWeight() != fst.getSemiring().zero())
				{
					state = state2;
					break;
				}
			}
			if (state == null)
			{
				java.lang.System.err.println("Final state not found.");
				return;
			}
			for (int i = 0; i < numStates; i++)
			{
				State state2 = fst.getState(i);
				for (int j = 0; j < state2.getNumArcs(); j++)
				{
					Arc arc = state2.getArc(j);
					if (arc.getIlabel() == 0 && arc.getOlabel() == 0 && arc.getNextState().getId() == state.getId())
					{
						state2.setFinalWeight(arc.getWeight());
					}
				}
			}
			fst.deleteState(state);
		}
	}
}

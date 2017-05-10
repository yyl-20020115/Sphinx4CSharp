using System;

using edu.cmu.sphinx.fst.semiring;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	public class ExtendFinal : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			175,
			102
		})]
		
		private ExtendFinal()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			188,
			103,
			134,
			103,
			102,
			105,
			111,
			233,
			61,
			230,
			72,
			109,
			104,
			159,
			0,
			151,
			109,
			98
		})]
		
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

		[LineNumberTable(new byte[]
		{
			23,
			98,
			103,
			102,
			104,
			115,
			98,
			226,
			60,
			230,
			72,
			99,
			111,
			129,
			105,
			104,
			109,
			106,
			116,
			114,
			237,
			60,
			8,
			233,
			74,
			103
		})]
		
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

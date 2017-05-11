using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	public class ArcSort : java.lang.Object
	{
		private ArcSort()
		{
		}

		public static void apply(Fst fst, Comparator cmp)
		{
			int numStates = fst.getNumStates();
			for (int i = 0; i < numStates; i++)
			{
				State state = fst.getState(i);
				state.arcSort(cmp);
			}
		}
	}
}

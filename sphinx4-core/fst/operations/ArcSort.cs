using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	public class ArcSort : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			173,
			102
		})]
		
		private ArcSort()
		{
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			187,
			103,
			102,
			104,
			7,
			198
		})]
		
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

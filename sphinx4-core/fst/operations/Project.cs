using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.fst.operations
{
	public class Project : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			163,
			102
		})]
		
		private Project()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			104,
			110,
			104,
			172,
			103,
			105,
			136,
			115,
			102,
			104,
			106,
			104,
			112,
			104,
			238,
			59,
			232,
			59,
			233,
			78
		})]
		
		public static void apply(Fst fst, ProjectType pType)
		{
			if (pType == ProjectType.__INPUT)
			{
				fst.setOsyms(fst.getIsyms());
			}
			else if (pType == ProjectType.__OUTPUT)
			{
				fst.setIsyms(fst.getOsyms());
			}
			int numStates = fst.getNumStates();
			for (int i = 0; i < numStates; i++)
			{
				State state = fst.getState(i);
				int num = (!(fst is ImmutableFst)) ? state.getNumArcs() : (state.getNumArcs() - 1);
				for (int j = 0; j < num; j++)
				{
					Arc arc = state.getArc(j);
					if (pType == ProjectType.__INPUT)
					{
						arc.setOlabel(arc.getIlabel());
					}
					else if (pType == ProjectType.__OUTPUT)
					{
						arc.setIlabel(arc.getOlabel());
					}
				}
			}
		}
	}
}

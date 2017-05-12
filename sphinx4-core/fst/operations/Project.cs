using java.lang;

namespace edu.cmu.sphinx.fst.operations
{
	public class Project : Object
	{		
		private Project()
		{
		}
		
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

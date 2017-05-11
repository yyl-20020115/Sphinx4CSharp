using java.util;

namespace edu.cmu.sphinx.fst.sequitur
{

	internal sealed class SequiturImport_FSA_1 : java.lang.Object, Comparator
	{
		public int compare(SequiturImport.State state, SequiturImport.State state2)
		{
			return state.id - state2.id;
		}
		
		internal SequiturImport_FSA_1(SequiturImport.FSA fsa)
		{
			this.this_0 = fsa;
		}

		public int compare(object obj, object obj2)
		{
			return this.compare((SequiturImport.State)obj, (SequiturImport.State)obj2);
		}
		
		bool Comparator.equals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}
		
		internal SequiturImport.FSA this_0;
	}
}

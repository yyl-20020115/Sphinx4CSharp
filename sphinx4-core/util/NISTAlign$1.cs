using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class NISTAlign_1 : Object, NISTAlign.Comparator
	{	
		internal NISTAlign_1(NISTAlign nistalign)
		{
			this_0 = nistalign;
		}
		
		public bool isSimilar(object obj, object obj2)
		{
			return obj is string && obj2 is string && String.instancehelper_equals((string)obj, obj2);
		}

		internal NISTAlign this_0;
	}
}

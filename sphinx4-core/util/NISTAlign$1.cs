namespace edu.cmu.sphinx.util
{
	internal sealed class NISTAlign_1 : java.lang.Object, NISTAlign.Comparator
	{	
		internal NISTAlign_1(NISTAlign nistalign)
		{
			this_0 = nistalign;
		}
		
		public bool isSimilar(object obj, object obj2)
		{
			return obj is string && obj2 is string && java.lang.String.instancehelper_equals((string)obj, obj2);
		}

		internal NISTAlign this_0;
	}
}

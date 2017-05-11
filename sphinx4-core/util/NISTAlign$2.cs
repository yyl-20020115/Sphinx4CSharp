namespace edu.cmu.sphinx.util
{
	internal sealed class NISTAlign_2 : java.lang.Object, NISTAlign.StringRenderer
	{
		internal NISTAlign_2(NISTAlign nistalign)
		{
			this_0 = nistalign;
		}

		public string getRef(object obj, object obj2)
		{
			return (string)obj;
		}
		
		public string getHyp(object obj, object obj2)
		{
			return (string)obj2;
		}

		internal NISTAlign this_0;
	}
}

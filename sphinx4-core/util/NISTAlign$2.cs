using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.NISTAlign_StringRenderer"
	})]
	[EnclosingMethod("edu.cmu.sphinx.util.NISTAlign", "align", "(Ljava.lang.java.lang.String;Ljava.lang.java.lang.String;)Z")]
	[SourceFile("NISTAlign.java")]
	
	internal sealed class NISTAlign_2 : java.lang.Object, NISTAlign.StringRenderer
	{
		
		
		internal NISTAlign_2(NISTAlign nistalign)
		{
		}

		
		public string getRef(object obj, object obj2)
		{
			return (string)obj;
		}

		
		public string getHyp(object obj, object obj2)
		{
			return (string)obj2;
		}

		
		internal NISTAlign this_0 = nistalign;
	}
}

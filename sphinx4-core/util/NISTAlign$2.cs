using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.NISTAlign$StringRenderer"
	})]
	[EnclosingMethod("edu.cmu.sphinx.util.NISTAlign", "align", "(Ljava.lang.java.lang.String;Ljava.lang.java.lang.String;)Z")]
	[SourceFile("NISTAlign.java")]
	
	internal sealed class NISTAlign$2 : java.lang.Object, NISTAlign.StringRenderer
	{
		
		
		internal NISTAlign$2(NISTAlign nistalign)
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

		
		internal NISTAlign this$0 = nistalign;
	}
}

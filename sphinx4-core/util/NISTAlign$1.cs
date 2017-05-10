using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.NISTAlign_Comparator"
	})]
	[EnclosingMethod("edu.cmu.sphinx.util.NISTAlign", "align", "(Ljava.lang.java.lang.String;Ljava.lang.java.lang.String;)Z")]
	[SourceFile("NISTAlign.java")]
	
	internal sealed class NISTAlign_1 : java.lang.Object, NISTAlign.Comparator
	{
		
		
		internal NISTAlign_1(NISTAlign nistalign)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			101,
			112,
			141
		})]
		
		public bool isSimilar(object obj, object obj2)
		{
			return obj is string && obj2 is string && java.lang.String.instancehelper_equals((string)obj, obj2);
		}

		
		internal NISTAlign this_0 = nistalign;
	}
}

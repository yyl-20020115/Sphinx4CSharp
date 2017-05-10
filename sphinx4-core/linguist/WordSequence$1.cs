using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist
{
	
	[Implements(new string[]
	{
		"java.util.Comparator"
	})]
	
	[EnclosingMethod("edu.cmu.sphinx.linguist.WordSequence", null, null)]
	[SourceFile("WordSequence.java")]
	internal sealed class WordSequence$1 : java.lang.Object, Comparator
	{
		
		
		public int compare(WordSequence wordSequence, WordSequence wordSequence2)
		{
			return wordSequence.getOldest().compareTo(wordSequence2.getOldest());
		}

		
		
		internal WordSequence$1()
		{
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((WordSequence)obj, (WordSequence)obj2);
		}

		
		bool Comparator.Object;)Zequals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}
	}
}

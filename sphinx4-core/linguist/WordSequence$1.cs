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
	
	
	.
	internal sealed class WordSequence_1 : java.lang.Object, Comparator
	{
		
		
		public int compare(WordSequence wordSequence, WordSequence wordSequence2)
		{
			return wordSequence.getOldest().compareTo(wordSequence2.getOldest());
		}

		
		
		internal WordSequence_1()
		{
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((WordSequence)obj, (WordSequence)obj2);
		}

		
		bool Comparator.equals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}
	}
}

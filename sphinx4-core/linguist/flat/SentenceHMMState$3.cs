using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.flat
{
	
	[Implements(new string[]
	{
		"java.util.Comparator"
	})]
	
	[EnclosingMethod("edu.cmu.sphinx.linguist.flat.SentenceHMMState", "visitStates", "(Ledu.cmu.sphinx.linguist.flat.SentenceHMMStateVisitor;Ledu.cmu.sphinx.linguist.flat.SentenceHMMState;Z)Z")]
	[SourceFile("SentenceHMMState.java")]
	internal sealed class SentenceHMMState$3 : java.lang.Object, Comparator
	{
		
		
		internal SentenceHMMState$3()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			184,
			98,
			98
		})]
		
		public int compare(SentenceHMMState sentenceHMMState, SentenceHMMState sentenceHMMState2)
		{
			return SentenceHMMState.access$100(sentenceHMMState) - SentenceHMMState.access$100(sentenceHMMState2);
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((SentenceHMMState)obj, (SentenceHMMState)obj2);
		}

		
		bool Comparator.Object;)Zequals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}
	}
}

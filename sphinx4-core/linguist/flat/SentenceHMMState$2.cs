using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.flat.SentenceHMMStateVisitor"
	})]
	
	.
	
	internal sealed class SentenceHMMState_2 : java.lang.Object, SentenceHMMStateVisitor
	{
		
		
		internal SentenceHMMState_2(SentenceHMMState sentenceHMMState)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			239,
			102
		})]
		
		public bool visit(SentenceHMMState sentenceHMMState)
		{
			SentenceHMMState.access_000(sentenceHMMState);
			return false;
		}

		
		internal SentenceHMMState this_0 = sentenceHMMState;
	}
}

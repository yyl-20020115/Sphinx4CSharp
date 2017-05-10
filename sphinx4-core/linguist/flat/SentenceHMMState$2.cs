using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.flat.SentenceHMMStateVisitor"
	})]
	[EnclosingMethod("edu.cmu.sphinx.linguist.flat.SentenceHMMState", "dumpAll", "()V")]
	[SourceFile("SentenceHMMState.java")]
	
	internal sealed class SentenceHMMState$2 : java.lang.Object, SentenceHMMStateVisitor
	{
		
		
		internal SentenceHMMState$2(SentenceHMMState sentenceHMMState)
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

		
		internal SentenceHMMState this$0 = sentenceHMMState;
	}
}

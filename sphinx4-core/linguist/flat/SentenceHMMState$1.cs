using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.flat.SentenceHMMStateVisitor"
	})]
	[EnclosingMethod("edu.cmu.sphinx.linguist.flat.SentenceHMMState", "resetAllProcessed", "()V")]
	[SourceFile("SentenceHMMState.java")]
	
	internal sealed class SentenceHMMState$1 : java.lang.Object, SentenceHMMStateVisitor
	{
		
		
		internal SentenceHMMState$1(SentenceHMMState sentenceHMMState)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			102,
			103
		})]
		
		public bool visit(SentenceHMMState sentenceHMMState)
		{
			sentenceHMMState.setProcessed(false);
			return false;
		}

		
		internal SentenceHMMState this$0 = sentenceHMMState;
	}
}

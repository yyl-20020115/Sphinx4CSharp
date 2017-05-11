﻿using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.flat.SentenceHMMStateVisitor"
	})]
	
	.
	
	internal sealed class SentenceHMMState_1 : java.lang.Object, SentenceHMMStateVisitor
	{
		
		
		internal SentenceHMMState_1(SentenceHMMState sentenceHMMState)
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

		
		internal SentenceHMMState this_0 = sentenceHMMState;
	}
}

﻿using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.flat
{
	
	[Implements(new string[]
	{
		"java.util.Comparator"
	})]
	
	
	.
	internal sealed class SentenceHMMState_3 : java.lang.Object, Comparator
	{
		
		
		internal SentenceHMMState_3()
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
			return SentenceHMMState.access_100(sentenceHMMState) - SentenceHMMState.access_100(sentenceHMMState2);
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((SentenceHMMState)obj, (SentenceHMMState)obj2);
		}

		
		bool Comparator.equals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}
	}
}

﻿using java.util;

namespace edu.cmu.sphinx.linguist.flat
{
	internal sealed class SentenceHMMState_3 : java.lang.Object, Comparator
	{
		internal SentenceHMMState_3()
		{
		}
		
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

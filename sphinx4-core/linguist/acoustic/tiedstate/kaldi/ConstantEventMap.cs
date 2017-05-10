using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi.EventMap"
	})]
	public class ConstantEventMap : java.lang.Object, EventMap
	{
		[LineNumberTable(new byte[]
		{
			159,
			160,
			104,
			103
		})]
		
		public ConstantEventMap(int value)
		{
			this.value = value;
		}

		public virtual int map(int pdfClass, int[] context)
		{
			return this.value;
		}

		
		private int value;
	}
}

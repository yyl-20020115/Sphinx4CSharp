using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi.EventMap"
	})]
	public abstract class EventMapWithKey : java.lang.Object, EventMap
	{
		[LineNumberTable(new byte[]
		{
			159,
			158,
			104,
			103
		})]
		
		protected internal EventMapWithKey(int key)
		{
			this.__key = key;
		}

		
		protected internal virtual int getKeyValue(int pdfClass, int[] context)
		{
			return (-1 != this.__key) ? context[this.__key] : pdfClass;
		}

		[HideFromReflection]
		public abstract int map(int, int[]);

		
		protected internal int key
		{
			
			get
			{
				return this.__key;
			}
			
			private set
			{
				this.__key = value;
			}
		}

		internal int __key;
	}
}

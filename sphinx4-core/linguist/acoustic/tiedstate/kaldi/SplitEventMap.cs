using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	public class SplitEventMap : EventMapWithKey
	{
		
		[LineNumberTable(new byte[]
		{
			159,
			176,
			105,
			108,
			103,
			104
		})]
		
		public SplitEventMap(int key, Collection values, EventMap yesMap, EventMap noMap) : base(key)
		{
			this.values = new HashSet(values);
			this.yesMap = yesMap;
			this.noMap = noMap;
		}

		[LineNumberTable(new byte[]
		{
			159,
			190,
			127,
			3,
			52
		})]
		
		public override int map(int pdfClass, int[] context)
		{
			return (!this.values.contains(Integer.valueOf(this.getKeyValue(pdfClass, context)))) ? this.noMap.map(pdfClass, context) : this.yesMap.map(pdfClass, context);
		}

		
		
		private Set values;

		
		private EventMap yesMap;

		
		private EventMap noMap;
	}
}

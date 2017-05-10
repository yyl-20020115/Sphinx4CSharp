using System;

using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	public class TableEventMap : EventMapWithKey
	{
		
		[LineNumberTable(new byte[]
		{
			159,
			160,
			105,
			108
		})]
		
		public TableEventMap(int key, List table) : base(key)
		{
			this.table = new ArrayList(table);
		}

		[LineNumberTable(new byte[]
		{
			159,
			168,
			121
		})]
		
		public override int map(int pdfClass, int[] context)
		{
			EventMap eventMap = (EventMap)this.table.get(this.getKeyValue(pdfClass, context));
			return eventMap.map(pdfClass, context);
		}

		
		
		private List table;
	}
}

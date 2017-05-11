using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	public class TableEventMap : EventMapWithKey
	{		
		public TableEventMap(int key, List table) : base(key)
		{
			this.table = new ArrayList(table);
		}
		
		public override int map(int pdfClass, int[] context)
		{
			EventMap eventMap = (EventMap)this.table.get(this.getKeyValue(pdfClass, context));
			return eventMap.map(pdfClass, context);
		}
		
		private List table;
	}
}

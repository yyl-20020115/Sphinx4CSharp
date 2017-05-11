namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	public abstract class EventMapWithKey : java.lang.Object, EventMap
	{		
		protected internal EventMapWithKey(int key)
		{
			this.__key = key;
		}
		
		protected internal virtual int getKeyValue(int pdfClass, int[] context)
		{
			return (-1 != this.__key) ? context[this.__key] : pdfClass;
		}

		public abstract int map(int key, int[] ints);
		
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

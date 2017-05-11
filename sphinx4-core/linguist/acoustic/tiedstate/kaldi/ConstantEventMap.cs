namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	public class ConstantEventMap : java.lang.Object, EventMap
	{		
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

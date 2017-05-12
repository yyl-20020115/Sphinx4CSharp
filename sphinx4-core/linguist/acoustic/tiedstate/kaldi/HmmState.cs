using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	internal sealed class HmmState : Object
	{		
		public HmmState(int num, int num2, Collection collection)
		{
			this.id = num;
			this.pdfClass = num2;
			this.transitions = new ArrayList(collection);
		}

		public int getId()
		{
			return this.id;
		}

		public int getPdfClass()
		{
			return this.pdfClass;
		}

		public List getTransitions()
		{
			return this.transitions;
		}
		
		public int size()
		{
			return this.transitions.size();
		}
		
		public override string toString()
		{
			return String.format("HmmSate {%d, %d, %s}", new object[]
			{
				Integer.valueOf(this.id),
				Integer.valueOf(this.pdfClass),
				this.transitions
			});
		}
		
		private int id;

		private int pdfClass;		
		
		private List transitions;
	}
}

using java.lang;

namespace edu.cmu.sphinx.util
{
	public class Range : Object
	{
		public virtual bool contains(int shift)
		{
			return shift >= this.start && shift < this.end;
		}
		
		public Range(int start, int end)
		{
			this.start = start;
			this.end = end;
		}

		public virtual int upperEndpoint()
		{
			return this.end;
		}

		public virtual int lowerEndpoint()
		{
			return this.start;
		}
		
		public override string toString()
		{
			return String.format("[%d, %d]", new object[]
			{
				Integer.valueOf(this.start),
				Integer.valueOf(this.end)
			});
		}

		private int start;

		private int end;
	}
}

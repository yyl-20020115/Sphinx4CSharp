using java.lang;

namespace edu.cmu.sphinx.frontend
{
	public class DataStartSignal : Signal
	{
		public virtual int getSampleRate()
		{
			return this.sampleRate;
		}
		
		public DataStartSignal(int sampleRate, long time) : base(time)
		{
			this.sampleRate = sampleRate;
		}
		
		public DataStartSignal(int sampleRate) : this(sampleRate, java.lang.System.currentTimeMillis())
		{
		}
		
		public override string toString()
		{
			return new StringBuilder().append("DataStartSignal: creation time: ").append(this.getTime()).toString();
		}
		
		private int sampleRate;
	}
}

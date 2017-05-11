using java.lang;

namespace edu.cmu.sphinx.frontend
{
	public class DataEndSignal : Signal
	{		
		public DataEndSignal(long duration, long time) : base(time)
		{
			this.duration = duration;
		}

		public virtual long getDuration()
		{
			return this.duration;
		}
		
		public DataEndSignal(long duration) : this(duration, java.lang.System.currentTimeMillis())
		{
		}
		
		public override string toString()
		{
			return new StringBuilder().append("DataEndSignal: creation time: ").append(this.getTime()).append(", duration: ").append(this.getDuration()).append("ms").toString();
		}
		
		private long duration;
	}
}

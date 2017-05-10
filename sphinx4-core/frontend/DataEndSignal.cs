using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend
{
	public class DataEndSignal : Signal
	{
		[LineNumberTable(new byte[]
		{
			159,
			187,
			105,
			103
		})]
		
		public DataEndSignal(long duration, long time) : base(time)
		{
			this.duration = duration;
		}

		public virtual long getDuration()
		{
			return this.duration;
		}

		[LineNumberTable(new byte[]
		{
			159,
			176,
			108
		})]
		
		public DataEndSignal(long duration) : this(duration, java.lang.System.currentTimeMillis())
		{
		}

		[LineNumberTable(new byte[]
		{
			17,
			127,
			6,
			57
		})]
		
		public override string toString()
		{
			return new StringBuilder().append("DataEndSignal: creation time: ").append(this.getTime()).append(", duration: ").append(this.getDuration()).append("ms").toString();
		}

		
		private long duration;
	}
}

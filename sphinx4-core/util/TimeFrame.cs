using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util
{
	public class TimeFrame : java.lang.Object
	{
		public virtual long getEnd()
		{
			return this.end;
		}

		public virtual long getStart()
		{
			return this.start;
		}

		public TimeFrame(long start, long end)
		{
			this.start = start;
			this.end = end;
		}

		public virtual long length()
		{
			return this.end - this.start;
		}
		
		public TimeFrame(long duration) : this(0L, duration)
		{
		}

		public override string toString()
		{
			return java.lang.String.format("%d:%d", new object[]
			{
				Long.valueOf(this.start),
				Long.valueOf(this.end)
			});
		}

		static TimeFrame()
		{
		}

		
		public static TimeFrame NULL
		{
			get
			{
				return TimeFrame.__NULL;
			}
		}

		
		public static TimeFrame INFINITE
		{
			get
			{
				return TimeFrame.__INFINITE;
			}
		}

		internal static TimeFrame __NULL = new TimeFrame(0L);

		internal static TimeFrame __INFINITE = new TimeFrame(long.MaxValue);

		private long start;

		private long end;
	}
}

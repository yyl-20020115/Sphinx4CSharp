using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend
{
	public class DataStartSignal : Signal
	{
		public virtual int getSampleRate()
		{
			return this.sampleRate;
		}

		[LineNumberTable(new byte[]
		{
			159,
			176,
			105,
			103
		})]
		
		public DataStartSignal(int sampleRate, long time) : base(time)
		{
			this.sampleRate = sampleRate;
		}

		[LineNumberTable(new byte[]
		{
			159,
			186,
			108
		})]
		
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

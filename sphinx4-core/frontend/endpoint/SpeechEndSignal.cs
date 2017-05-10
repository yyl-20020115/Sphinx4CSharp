using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend.endpoint
{
	public class SpeechEndSignal : Signal
	{
		[LineNumberTable(new byte[]
		{
			159,
			175,
			103
		})]
		
		public SpeechEndSignal(long time) : base(time)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			165,
			107
		})]
		
		public SpeechEndSignal() : this(System.currentTimeMillis())
		{
		}

		public override string toString()
		{
			return "SpeechEndSignal";
		}
	}
}

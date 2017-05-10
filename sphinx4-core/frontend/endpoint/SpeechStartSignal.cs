using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend.endpoint
{
	public class SpeechStartSignal : Signal
	{
		[LineNumberTable(new byte[]
		{
			159,
			175,
			103
		})]
		
		public SpeechStartSignal(long time) : base(time)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			165,
			107
		})]
		
		public SpeechStartSignal() : this(System.currentTimeMillis())
		{
		}

		public override string toString()
		{
			return "SpeechStartSignal";
		}
	}
}

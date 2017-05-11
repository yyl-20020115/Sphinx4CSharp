namespace edu.cmu.sphinx.frontend.endpoint
{
	public class SpeechStartSignal : Signal
	{
		public SpeechStartSignal(long time) : base(time)
		{
		}
		public SpeechStartSignal() : this(java.lang.System.currentTimeMillis())
		{
		}

		public override string toString()
		{
			return "SpeechStartSignal";
		}
	}
}

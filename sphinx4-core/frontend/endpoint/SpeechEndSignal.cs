namespace edu.cmu.sphinx.frontend.endpoint
{
	public class SpeechEndSignal : Signal
	{
		public SpeechEndSignal(long time) : base(time)
		{
		}
		
		public SpeechEndSignal() : this(java.lang.System.currentTimeMillis())
		{
		}

		public override string toString()
		{
			return "SpeechEndSignal";
		}
	}
}

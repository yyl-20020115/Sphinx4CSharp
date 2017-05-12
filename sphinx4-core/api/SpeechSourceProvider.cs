using java.lang;

namespace edu.cmu.sphinx.api
{
	public class SpeechSourceProvider : Object
	{
		public SpeechSourceProvider()
		{
		}

		internal virtual Microphone getMicrophone()
		{
			return new Microphone(16000f, 16, true, false);
		}
	}
}

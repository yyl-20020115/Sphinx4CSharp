using java.lang;
using java.util;
using javax.sound.sampled;

namespace edu.cmu.sphinx.frontend.util
{
	internal sealed class Microphone_1 : java.lang.Object, LineListener, EventListener
	{
		internal Microphone_1(Microphone microphone)
		{
		}

		public void update(LineEvent lineEvent)
		{
			Microphone.access_000(this.this_0).info(new StringBuilder().append("line listener ").append(lineEvent).toString());
		}

		
		internal Microphone this_0 = microphone;
	}
}

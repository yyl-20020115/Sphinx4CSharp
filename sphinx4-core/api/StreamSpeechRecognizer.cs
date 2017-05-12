using edu.cmu.sphinx.util;
using java.io;

namespace edu.cmu.sphinx.api
{
	public class StreamSpeechRecognizer : AbstractSpeechRecognizer
	{
		public virtual void startRecognition(InputStream stream, TimeFrame timeFrame)
		{
			this.__recognizer.allocate();
			this.__context.setSpeechSource(stream, timeFrame);
		}

		public StreamSpeechRecognizer(Configuration configuration) : base(configuration)
		{
		}

		public virtual void startRecognition(InputStream stream)
		{
			this.startRecognition(stream, TimeFrame.__INFINITE);
		}

		public virtual void stopRecognition()
		{
			this.__recognizer.deallocate();
		}
	}
}

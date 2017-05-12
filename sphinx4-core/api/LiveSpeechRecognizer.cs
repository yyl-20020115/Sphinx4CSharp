using edu.cmu.sphinx.frontend.util;
using ikvm.@internal;

namespace edu.cmu.sphinx.api
{
	public class LiveSpeechRecognizer : AbstractSpeechRecognizer
	{
		public LiveSpeechRecognizer(Configuration configuration) : base(configuration)
		{
			this.microphone = this.__speechSourceProvider.getMicrophone();
			((StreamDataSource)this.__context.getInstance(ClassLiteral<StreamDataSource>.Value)).setInputStream(this.microphone.getStream());
		}

		public virtual void startRecognition(bool clear)
		{
			this.__recognizer.allocate();
			this.microphone.startRecording();
		}

		public virtual void stopRecognition()
		{
			this.microphone.stopRecording();
			this.__recognizer.deallocate();
		}

		private Microphone microphone;
	}
}

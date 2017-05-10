using System;

using edu.cmu.sphinx.frontend.util;
using IKVM.Attributes;
using ikvm.@internal;

namespace edu.cmu.sphinx.api
{
	public class LiveSpeechRecognizer : AbstractSpeechRecognizer
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			176,
			105,
			113,
			123,
			106
		})]
		
		public LiveSpeechRecognizer(Configuration configuration) : base(configuration)
		{
			this.microphone = this.__speechSourceProvider.getMicrophone();
			((StreamDataSource)this.__context.getInstance(ClassLiteral<StreamDataSource>.Value)).setInputStream(this.microphone.getStream());
		}

		[LineNumberTable(new byte[]
		{
			159,
			189,
			107,
			107
		})]
		
		public virtual void startRecognition(bool clear)
		{
			this.__recognizer.allocate();
			this.microphone.startRecording();
		}

		[LineNumberTable(new byte[]
		{
			9,
			107,
			107
		})]
		
		public virtual void stopRecognition()
		{
			this.microphone.stopRecording();
			this.__recognizer.deallocate();
		}

		
		private Microphone microphone;
	}
}

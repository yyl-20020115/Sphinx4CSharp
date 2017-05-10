using System;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.io;

namespace edu.cmu.sphinx.api
{
	public class StreamSpeechRecognizer : AbstractSpeechRecognizer
	{
		[LineNumberTable(new byte[]
		{
			2,
			107,
			109
		})]
		
		public virtual void startRecognition(InputStream stream, TimeFrame timeFrame)
		{
			this.__recognizer.allocate();
			this.__context.setSpeechSource(stream, timeFrame);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			177,
			103
		})]
		
		public StreamSpeechRecognizer(Configuration configuration) : base(configuration)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			181,
			108
		})]
		
		public virtual void startRecognition(InputStream stream)
		{
			this.startRecognition(stream, TimeFrame.__INFINITE);
		}

		[LineNumberTable(new byte[]
		{
			14,
			107
		})]
		
		public virtual void stopRecognition()
		{
			this.__recognizer.deallocate();
		}
	}
}

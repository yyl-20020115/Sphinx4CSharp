using System;

using edu.cmu.sphinx.api;
using IKVM.Attributes;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.tools.transcriber
{
	public class Transcriber : java.lang.Object
	{
		
		
		public Transcriber()
		{
		}

		
		public static void main(string[] args)
		{
			Configuration configuration = new Configuration();
			configuration.setAcousticModelPath("resource:/edu/cmu/sphinx/models/en-us/en-us");
			configuration.setDictionaryPath("resource:/edu/cmu/sphinx/models/en-us/cmudict-en-us.dict");
			configuration.setLanguageModelPath("resource:/edu/cmu/sphinx/models/en-us/en-us.lm.bin");
			StreamSpeechRecognizer streamSpeechRecognizer = new StreamSpeechRecognizer(configuration);
			FileInputStream fileInputStream = new FileInputStream(new File(args[0]));
			fileInputStream.skip((long)((ulong)44));
			streamSpeechRecognizer.startRecognition(fileInputStream);
			SpeechResult result;
			while ((result = streamSpeechRecognizer.getResult()) != null)
			{
				java.lang.System.@out.println(result.getHypothesis());
			}
			streamSpeechRecognizer.stopRecognition();
		}
	}
}

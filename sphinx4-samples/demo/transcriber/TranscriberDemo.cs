using edu.cmu.sphinx.api;
using edu.cmu.sphinx.decoder.adaptation;
using edu.cmu.sphinx.result;
using ikvm.@internal;
using java.io;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.demo.transcriber
{
	public class TranscriberDemo : Object
	{
		public TranscriberDemo()
		{
		}

		public static void main(string[] args)
		{
			java.lang.System.@out.println("Loading models...");
			Configuration configuration = new Configuration();
			configuration.setAcousticModelPath("resource:/edu/cmu/sphinx/models/en-us/en-us");
			configuration.setDictionaryPath("resource:/edu/cmu/sphinx/models/en-us/cmudict-en-us.dict");
			configuration.setLanguageModelPath("resource:/edu/cmu/sphinx/models/en-us/en-us.lm.bin");
			StreamSpeechRecognizer streamSpeechRecognizer = new StreamSpeechRecognizer(configuration);
			InputStream resourceAsStream = ClassLiteral<TranscriberDemo>.Value.getResourceAsStream("/edu/cmu/sphinx/demo/aligner/10001-90210-01803.wav");
			resourceAsStream.skip((long)((ulong)44));
			streamSpeechRecognizer.startRecognition(resourceAsStream);
			SpeechResult result;
			while ((result = streamSpeechRecognizer.getResult()) != null)
			{
				java.lang.System.@out.format("Hypothesis: %s\n", new object[]
				{
					result.getHypothesis()
				});
				java.lang.System.@out.println("List of recognized words and their times:");
				Iterator iterator = result.getWords().iterator();
				while (iterator.hasNext())
				{
					WordResult wordResult = (WordResult)iterator.next();
					java.lang.System.@out.println(wordResult);
				}
				java.lang.System.@out.println("Best 3 hypothesis:");
				iterator = result.getNbest(3).iterator();
				while (iterator.hasNext())
				{
					string text = (string)iterator.next();
					java.lang.System.@out.println(text);
				}
			}
			streamSpeechRecognizer.stopRecognition();
			resourceAsStream = ClassLiteral<TranscriberDemo>.Value.getResourceAsStream("/edu/cmu/sphinx/demo/aligner/10001-90210-01803.wav");
			resourceAsStream.skip((long)((ulong)44));
			Stats stats = streamSpeechRecognizer.createStats(1);
			streamSpeechRecognizer.startRecognition(resourceAsStream);
			while ((result = streamSpeechRecognizer.getResult()) != null)
			{
				stats.collect(result);
			}
			streamSpeechRecognizer.stopRecognition();
			Transform transform = stats.createTransform();
			streamSpeechRecognizer.setTransform(transform);
			resourceAsStream = ClassLiteral<TranscriberDemo>.Value.getResourceAsStream("/edu/cmu/sphinx/demo/aligner/10001-90210-01803.wav");
			resourceAsStream.skip((long)((ulong)44));
			streamSpeechRecognizer.startRecognition(resourceAsStream);
			while ((result = streamSpeechRecognizer.getResult()) != null)
			{
				java.lang.System.@out.format("Hypothesis: %s\n", new object[]
				{
					result.getHypothesis()
				});
			}
			streamSpeechRecognizer.stopRecognition();
		}
	}
}

using System;
using System.Runtime.CompilerServices;
using edu.cmu.sphinx.api;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.demo.allphone
{
	public class AllphoneDemo : java.lang.Object
	{
		[LineNumberTable(26)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AllphoneDemo()
		{
		}

		[Throws(new string[]
		{
			"java.lang.Exception"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			171,
			143,
			166,
			102,
			229,
			69,
			102,
			101,
			103,
			112,
			113,
			106,
			102,
			170,
			102,
			140,
			110,
			105,
			159,
			1,
			111,
			127,
			6,
			108,
			98,
			101,
			134
		})]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void main(string[] args)
		{
			java.lang.System.@out.println("Loading models...");
			Configuration configuration = new Configuration();
			configuration.setAcousticModelPath("resource:/edu/cmu/sphinx/models/en-us/en-us");
			configuration.setDictionaryPath("resource:/edu/cmu/sphinx/models/en-us/cmudict-en-us.dict");
			Context context = new Context(configuration);
			context.setLocalProperty("decoder->searchManager", "allphoneSearchManager");
			Recognizer recognizer = (Recognizer)context.getInstance(ClassLiteral<Recognizer>.Value);
			InputStream resourceAsStream = ClassLiteral<AllphoneDemo>.Value.getResourceAsStream("/edu/cmu/sphinx/demo/aligner/10001-90210-01803.wav");
			resourceAsStream.skip((long)((ulong)44));
			recognizer.allocate();
			context.setSpeechSource(resourceAsStream, TimeFrame.__<>INFINITE);
			Result result;
			while ((result = recognizer.recognize()) != null)
			{
				SpeechResult speechResult = new SpeechResult(result);
				java.lang.System.@out.format("Hypothesis: %s\n", new object[]
				{
					speechResult.getHypothesis()
				});
				java.lang.System.@out.println("List of recognized words and their times:");
				Iterator iterator = speechResult.getWords().iterator();
				while (iterator.hasNext())
				{
					WordResult wordResult = (WordResult)iterator.next();
					java.lang.System.@out.println(wordResult);
				}
			}
			recognizer.deallocate();
		}
	}
}

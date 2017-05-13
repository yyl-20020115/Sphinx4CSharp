using edu.cmu.sphinx.alignment;
using edu.cmu.sphinx.api;
using edu.cmu.sphinx.result;
using ikvm.@internal;
using java.io;
using java.net;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.demo.aligner   
{
	public class AlignerDemo : Object
	{
		public AlignerDemo()
		{
		}

		public static void main(string[] args)
		{
			URL audioUrl;
			string text;
			if (args.Length > 1)
			{
				audioUrl = new File(args[0]).toURI().toURL();
				Scanner scanner = new Scanner(new File(args[1]));
				scanner.useDelimiter("\\Z");
				text = scanner.next();
				scanner.close();
			}
			else
			{
				audioUrl = ClassLiteral<AlignerDemo>.Value.getResource("10001-90210-01803.wav");
				text = "one zero zero zero one nine oh two one oh zero one eight zero three";
			}
			string amPath = (args.Length <= 2) ? "resource:/edu/cmu/sphinx/models/en-us/en-us" : args[2];
			string dictPath = (args.Length <= 3) ? "resource:/edu/cmu/sphinx/models/en-us/cmudict-en-us.dict" : args[3];
			string g2pPath = (args.Length <= 4) ? null : args[4];
			SpeechAligner speechAligner = new SpeechAligner(amPath, dictPath, g2pPath);
			List list = speechAligner.align(audioUrl, text);
			ArrayList arrayList = new ArrayList();
			Iterator iterator = list.iterator();
			while (iterator.hasNext())
			{
				WordResult wordResult = (WordResult)iterator.next();
				arrayList.add(wordResult.getWord().getSpelling());
			}
			LongTextAligner longTextAligner = new LongTextAligner(arrayList, 2);
			List sentenceTranscript = speechAligner.getTokenizer().expand(text);
			List list2 = speechAligner.sentenceToWords(sentenceTranscript);
			int[] array = longTextAligner.align(list2);
			int num = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == -1)
				{
					java.lang.System.@out.format("- %s\n", new object[]
					{
						list2.get(i)
					});
				}
				else
				{
					if (array[i] - num > 1)
					{
						Iterator iterator2 = list.subList(num + 1, array[i]).iterator();
						while (iterator2.hasNext())
						{
							WordResult wordResult2 = (WordResult)iterator2.next();
							java.lang.System.@out.format("+ %-25s [%s]\n", new object[]
							{
								wordResult2.getWord().getSpelling(),
								wordResult2.getTimeFrame()
							});
						}
					}
					java.lang.System.@out.format("  %-25s [%s]\n", new object[]
					{
						((WordResult)list.get(array[i])).getWord().getSpelling(),
						((WordResult)list.get(array[i])).getTimeFrame()
					});
					num = array[i];
				}
			}
			if (num >= 0 && list.size() - num > 1)
			{
				Iterator iterator3 = list.subList(num + 1, list.size()).iterator();
				while (iterator3.hasNext())
				{
					WordResult wordResult3 = (WordResult)iterator3.next();
					java.lang.System.@out.format("+ %-25s [%s]\n", new object[]
					{
						wordResult3.getWord().getSpelling(),
						wordResult3.getTimeFrame()
					});
				}
			}
		}

		private const string ACOUSTIC_MODEL_PATH = "resource:/edu/cmu/sphinx/models/en-us/en-us";

		private const string DICTIONARY_PATH = "resource:/edu/cmu/sphinx/models/en-us/cmudict-en-us.dict";

		private const string TEXT = "one zero zero zero one nine oh two one oh zero one eight zero three";
	}
}

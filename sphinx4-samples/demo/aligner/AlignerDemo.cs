using System;
using System.Runtime.CompilerServices;
using edu.cmu.sphinx.alignment;
using edu.cmu.sphinx.api;
using edu.cmu.sphinx.result;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.demo.aligner
{
	public class AlignerDemo : java.lang.Object
	{
		[LineNumberTable(40)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AlignerDemo()
		{
		}

		[Throws(new string[]
		{
			"java.lang.Exception"
		})]
		[LineNumberTable(new byte[]
		{
			1,
			101,
			120,
			120,
			108,
			103,
			102,
			98,
			112,
			134,
			144,
			113,
			109,
			172,
			107,
			103,
			127,
			1,
			116,
			130,
			138,
			111,
			139,
			139,
			99,
			109,
			104,
			159,
			8,
			107,
			159,
			15,
			121,
			48,
			134,
			130,
			127,
			4,
			126,
			6,
			166,
			231,
			50,
			235,
			82,
			117,
			104,
			37,
			159,
			4,
			121,
			48,
			134,
			130
		})]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void main(string[] args)
		{
			URL audioUrl;
			string text;
			if (args.Length > 1)
			{
				File.__<clinit>();
				audioUrl = new File(args[0]).toURI().toURL();
				Scanner.__<clinit>();
				File.__<clinit>();
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

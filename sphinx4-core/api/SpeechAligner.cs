using edu.cmu.sphinx.alignment;
using edu.cmu.sphinx.linguist.language.grammar;
using edu.cmu.sphinx.linguist.language.ngram;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using ikvm.@internal;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.api
{
	public class SpeechAligner : java.lang.Object
	{
		public virtual void setTokenizer(TextTokenizer wordExpander)
		{
			this.tokenizer = wordExpander;
		}

		public virtual TextTokenizer getTokenizer()
		{
			return this.tokenizer;
		}
	
		public virtual List align(URL audioUrl, List sentenceTranscript)
		{
			List list = this.sentenceToWords(sentenceTranscript);
			LongTextAligner longTextAligner = new LongTextAligner(list, 3);
			TreeMap treeMap = new TreeMap();
			LinkedList linkedList = new LinkedList();
			ArrayDeque arrayDeque = new ArrayDeque();
			ArrayDeque arrayDeque2 = new ArrayDeque();
			linkedList.offer(new Range(0, list.size()));
			arrayDeque.offer(list);
			TimeFrame _INFINITE = TimeFrame.__INFINITE;
			arrayDeque2.offer(_INFINITE);
			long end = TimeFrame.__INFINITE.getEnd();
			this.languageModel.setText(sentenceTranscript);
			for (int i = 0; i < 4; i++)
			{
				if (i == 1)
				{
					this.context.setLocalProperty("decoder->searchManager", "alignerSearchManager");
				}
				while (!arrayDeque.isEmpty())
				{
					if (!SpeechAligner.assertionsDisabled && arrayDeque.size() != linkedList.size())
					{
						
						throw new AssertionError();
					}
					if (!SpeechAligner.assertionsDisabled && arrayDeque.size() != arrayDeque2.size())
					{
						
						throw new AssertionError();
					}
					List list2 = (List)arrayDeque.poll();
					TimeFrame timeFrame = (TimeFrame)arrayDeque2.poll();
					Range range = (Range)linkedList.poll();
					this.logger.info(new StringBuilder().append("Aligning frame ").append(timeFrame).append(" to text ").append(list2).append(" range ").append(range).toString());
					this.recognizer.allocate();
					if (i >= 1)
					{
						this.grammar.setWords(list2);
					}
					InputStream inputStream = audioUrl.openStream();
					this.context.setSpeechSource(inputStream, timeFrame);
					ArrayList arrayList = new ArrayList();
					Result result;
					while (null != (result = this.recognizer.recognize()))
					{
						this.logger.info(new StringBuilder().append("Utterance result ").append(result.getTimedBestResult(true)).toString());
						arrayList.addAll(result.getTimedBestResult(false));
					}
					if (i == 0 && arrayList.size() > 0)
					{
						end = ((WordResult)arrayList.get(arrayList.size() - 1)).getTimeFrame().getEnd();
					}
					ArrayList arrayList2 = new ArrayList();
					Iterator iterator = arrayList.iterator();
					while (iterator.hasNext())
					{
						WordResult wordResult = (WordResult)iterator.next();
						arrayList2.add(wordResult.getWord().getSpelling());
					}
					int[] array = longTextAligner.align(arrayList2, range);
					ArrayList arrayList3 = arrayList;
					this.logger.info(new StringBuilder().append("Decoding result is ").append(arrayList3).toString());
					this.dumpAlignmentStats(list, array, arrayList3);
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j] != -1)
						{
							treeMap.put(Integer.valueOf(array[j]), arrayList.get(j));
						}
					}
					inputStream.close();
					this.recognizer.deallocate();
				}
				this.scheduleNextAlignment(list, treeMap, linkedList, arrayDeque, arrayDeque2, end);
			}
			return new ArrayList(treeMap.values());
		}
		
		public virtual List sentenceToWords(List sentenceTranscript)
		{
			ArrayList arrayList = new ArrayList();
			Iterator iterator = sentenceTranscript.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				string[] array = java.lang.String.instancehelper_split(text, "\\s+");
				string[] array2 = array;
				int num = array2.Length;
				for (int i = 0; i < num; i++)
				{
					string text2 = array2[i];
					if (java.lang.String.instancehelper_length(text2) > 0)
					{
						arrayList.add(text2);
					}
				}
			}
			return arrayList;
		}
	
		private void dumpAlignmentStats(List list, int[] array, List list2)
		{
			int num = 0;
			int num2 = 0;
			int num3 = list.size();
			int num4 = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == -1)
				{
					num++;
				}
				else
				{
					if (array[i] - num4 > 1)
					{
						num2 += array[i] - num4;
					}
					num4 = array[i];
				}
			}
			if (num4 >= 0 && list.size() - num4 > 1)
			{
				num2 += list.size() - num4;
			}
			this.logger.info(java.lang.String.format("Size %d deletions %d insertions %d error rate %.2f", new object[]
			{
				Integer.valueOf(num3),
				Integer.valueOf(num),
				Integer.valueOf(num2),
				Float.valueOf((float)(num + num2) / (float)num3 * 100f)
			}));
		}
		
		private void scheduleNextAlignment(List list, Map map, Queue queue, Queue queue2, Queue queue3, long num)
		{
			int num2 = 0;
			long num3 = 0L;
			Iterator iterator = map.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				if (((Integer)entry.getKey()).intValue() - num2 > 1)
				{
					this.checkedOffer(list, queue2, queue3, queue, num2, ((Integer)entry.getKey()).intValue() + 1, num3, ((WordResult)entry.getValue()).getTimeFrame().getEnd());
				}
				num2 = ((Integer)entry.getKey()).intValue();
				num3 = ((WordResult)entry.getValue()).getTimeFrame().getStart();
			}
			if (list.size() - num2 > 1)
			{
				this.checkedOffer(list, queue2, queue3, queue, num2, list.size(), num3, num);
			}
		}
	
		private void checkedOffer(List list, Queue queue, Queue queue2, Queue queue3, int num, int num2, long num3, long num4)
		{
			double num5 = (double)(num4 - num3) / (double)(num2 - num);
			if (num5 < 10.0 && num2 - num > 3)
			{
				this.logger.info(new StringBuilder().append("Skipping text range due to a high density ").append(java.lang.Object.instancehelper_toString(list.subList(num, num2))).toString());
				return;
			}
			queue.offer(list.subList(num, num2));
			queue2.offer(new TimeFrame(num3, num4));
			queue3.offer(new Range(num, num2 - 1));
		}
	
		public SpeechAligner(string amPath, string dictPath, string g2pPath)
		{
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getSimpleName());
			Configuration configuration = new Configuration();
			configuration.setAcousticModelPath(amPath);
			configuration.setDictionaryPath(dictPath);
			this.context = new Context(configuration);
			if (g2pPath != null)
			{
				this.context.setLocalProperty("dictionary->g2pModelPath", g2pPath);
				this.context.setLocalProperty("dictionary->g2pMaxPron", "2");
			}
			this.context.setLocalProperty("lexTreeLinguist->languageModel", "dynamicTrigramModel");
			this.recognizer = (Recognizer)this.context.getInstance(ClassLiteral<Recognizer>.Value);
			this.grammar = (AlignerGrammar)this.context.getInstance(ClassLiteral<AlignerGrammar>.Value);
			this.languageModel = (DynamicTrigramModel)this.context.getInstance(ClassLiteral<DynamicTrigramModel>.Value);
			this.setTokenizer(new SimpleTokenizer());
		}		
		
		public virtual List align(URL audioUrl, string transcript)
		{
			return this.align(audioUrl, this.getTokenizer().expand(transcript));
		}
		
		public virtual void dumpAlignment(List transcript, int[] alignment, List results)
		{
			this.logger.info("Alignment");
			int num = -1;
			for (int i = 0; i < alignment.Length; i++)
			{
				if (alignment[i] == -1)
				{
					this.logger.info(java.lang.String.format("+ %s", new object[]
					{
						results.get(i)
					}));
				}
				else
				{
					if (alignment[i] - num > 1)
					{
						Iterator iterator = transcript.subList(num + 1, alignment[i]).iterator();
						while (iterator.hasNext())
						{
							string text = (string)iterator.next();
							this.logger.info(java.lang.String.format("- %-25s", new object[]
							{
								text
							}));
						}
					}
					else
					{
						this.logger.info(java.lang.String.format("  %-25s", new object[]
						{
							transcript.get(alignment[i])
						}));
					}
					num = alignment[i];
				}
			}
			if (num >= 0 && transcript.size() - num > 1)
			{
				Iterator iterator2 = transcript.subList(num + 1, transcript.size()).iterator();
				while (iterator2.hasNext())
				{
					string text2 = (string)iterator2.next();
					this.logger.info(java.lang.String.format("- %-25s", new object[]
					{
						text2
					}));
				}
			}
		}		
		
		private Logger logger;

		private const int TUPLE_SIZE = 3;

		private Context context;

		private Recognizer recognizer;
		
		private AlignerGrammar grammar;

		private DynamicTrigramModel languageModel;

		private TextTokenizer tokenizer;
		
		internal static bool assertionsDisabled = !ClassLiteral<SpeechAligner>.Value.desiredAssertionStatus();
	}
}

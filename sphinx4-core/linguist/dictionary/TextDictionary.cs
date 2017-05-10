using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.g2p;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.dictionary
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.dictionary.Dictionary"
	})]
	public class TextDictionary : java.lang.Object, Dictionary, Configurable
	{
		
		[LineNumberTable(new byte[]
		{
			53,
			232,
			41,
			231,
			88,
			150,
			103,
			103,
			103,
			104,
			104
		})]
		
		public TextDictionary(URL wordDictionaryFile, URL fillerDictionaryFile, List addendaUrlList, string wordReplacement, UnitManager unitManager)
		{
			this.g2pMaxPron = 0;
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
			this.wordDictionaryFile = wordDictionaryFile;
			this.fillerDictionaryFile = fillerDictionaryFile;
			this.addendaUrlList = addendaUrlList;
			this.wordReplacement = wordReplacement;
			this.unitManager = unitManager;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			87,
			130,
			103,
			167,
			109,
			103,
			104,
			130,
			105,
			101,
			159,
			6,
			171,
			111,
			163,
			127,
			8,
			111,
			164,
			99,
			127,
			9,
			144,
			143,
			133,
			102,
			102,
			102
		})]
		
		protected internal virtual void loadDictionary(InputStream inputStream, bool isFillerDict)
		{
			InputStreamReader inputStreamReader = new InputStreamReader(inputStream);
			BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				text = java.lang.String.instancehelper_trim(text);
				if (!java.lang.String.instancehelper_isEmpty(text))
				{
					int spaceIndex = this.getSpaceIndex(text);
					if (spaceIndex < 0)
					{
						string text2 = new StringBuilder().append("Error loading word: ").append(text).toString();
						
						throw new Error(text2);
					}
					string text3 = java.lang.String.instancehelper_substring(text, 0, spaceIndex);
					if (this.dictionary.containsKey(text3))
					{
						int num = 2;
						string text5;
						do
						{
							string text4 = "%s(%d)";
							object[] array = new object[2];
							array[0] = text3;
							int num2 = 1;
							int num3 = num;
							num++;
							array[num2] = Integer.valueOf(num3);
							text5 = java.lang.String.format(text4, array);
						}
						while (this.dictionary.containsKey(text5));
						text3 = text5;
					}
					if (isFillerDict)
					{
						this.dictionary.put(text3, new StringBuilder().append("-F-").append(text).toString());
						this.fillerWords.add(text3);
					}
					else
					{
						this.dictionary.put(text3, text);
					}
				}
			}
			bufferedReader.close();
			inputStreamReader.close();
			inputStream.close();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			161,
			109,
			99,
			123,
			127,
			6,
			109,
			130
		})]
		
		private void loadCustomDictionaries(List list)
		{
			if (list != null)
			{
				Iterator iterator = list.iterator();
				while (iterator.hasNext())
				{
					URL url = (URL)iterator.next();
					this.logger.info(new StringBuilder().append("Loading addendum dictionary from: ").append(url).toString());
					this.loadDictionary(url.openStream(), false);
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			147,
			107,
			118,
			2,
			198
		})]
		
		private int getSpaceIndex(string text)
		{
			for (int i = 0; i < java.lang.String.instancehelper_length(text); i++)
			{
				if (java.lang.String.instancehelper_charAt(text, i) == ' ' || java.lang.String.instancehelper_charAt(text, i) == '\t')
				{
					return i;
				}
			}
			return -1;
		}

		[LineNumberTable(new byte[]
		{
			160,
			206,
			146,
			99,
			162,
			114,
			102,
			127,
			16,
			104,
			114,
			127,
			0,
			127,
			16,
			104,
			176,
			168
		})]
		
		public virtual Word getWord(string text)
		{
			Word word = (Word)this.wordDictionary.get(text);
			if (word != null)
			{
				return word;
			}
			if ((string)this.dictionary.get(text) == null)
			{
				this.logger.info(new StringBuilder().append("The dictionary is missing a phonetic transcription for the word '").append(text).append("'").toString());
				if (this.wordReplacement != null)
				{
					word = this.getWord(this.wordReplacement);
				}
				else if (this.g2pModelFile != null && !java.lang.String.instancehelper_equals(this.g2pModelFile.getPath(), ""))
				{
					this.logger.info(new StringBuilder().append("Generating phonetic transcription(s) for the word '").append(text).append("' using g2p model").toString());
					word = this.extractPronunciation(text);
					this.wordDictionary.put(text, word);
				}
			}
			else
			{
				word = this.processEntry(text);
			}
			return word;
		}

		[LineNumberTable(new byte[]
		{
			160,
			231,
			115,
			102,
			126,
			109,
			105,
			127,
			5,
			113,
			98,
			105,
			141,
			110,
			101,
			120,
			108,
			121,
			41,
			168
		})]
		
		private Word extractPronunciation(string text)
		{
			ArrayList arrayList = this.g2pDecoder.phoneticize(text, this.g2pMaxPron);
			LinkedList linkedList = new LinkedList();
			Iterator iterator = arrayList.iterator();
			int num;
			while (iterator.hasNext())
			{
				Path path = (Path)iterator.next();
				num = path.getPath().size();
				ArrayList arrayList2 = new ArrayList(num);
				Iterator iterator2 = path.getPath().iterator();
				while (iterator2.hasNext())
				{
					string name = (string)iterator2.next();
					arrayList2.add(this.getCIUnit(name, false));
				}
				if (arrayList2.size() == 0)
				{
					arrayList2.add(UnitManager.__SILENCE);
				}
				linkedList.add(new Pronunciation(arrayList2));
			}
			Pronunciation[] array = (Pronunciation[])linkedList.toArray(new Pronunciation[linkedList.size()]);
			Word word = this.createWord(text, array, false);
			Pronunciation[] array2 = array;
			num = array2.Length;
			for (int i = 0; i < num; i++)
			{
				Pronunciation pronunciation = array2[i];
				pronunciation.setWord(word);
			}
			return word;
		}

		[LineNumberTable(new byte[]
		{
			161,
			21,
			134,
			98,
			162,
			100,
			98,
			100,
			159,
			6,
			115,
			103,
			137,
			105,
			109,
			137,
			105,
			105,
			105,
			17,
			200,
			142,
			135,
			120,
			140,
			121,
			41,
			168,
			143
		})]
		
		private Word processEntry(string text)
		{
			LinkedList linkedList = new LinkedList();
			int num = 0;
			int num2 = 0;
			string text3;
			int num3;
			do
			{
				num++;
				string text2 = text;
				if (num > 1)
				{
					text2 = new StringBuilder().append(text2).append('(').append(num).append(')').toString();
				}
				text3 = (string)this.dictionary.get(text2);
				if (text3 != null)
				{
					StringTokenizer stringTokenizer = new StringTokenizer(text3);
					string text4 = stringTokenizer.nextToken();
					num2 = (java.lang.String.instancehelper_startsWith(text4, "-F-") ? 1 : 0);
					num3 = stringTokenizer.countTokens();
					ArrayList arrayList = new ArrayList(num3);
					for (int i = 0; i < num3; i++)
					{
						string name = stringTokenizer.nextToken();
						arrayList.add(this.getCIUnit(name, num2 != 0));
					}
					linkedList.add(new Pronunciation(arrayList));
				}
			}
			while (text3 != null);
			Pronunciation[] array = (Pronunciation[])linkedList.toArray(new Pronunciation[linkedList.size()]);
			Word word = this.createWord(text, array, num2 != 0);
			Pronunciation[] array2 = array;
			num3 = array2.Length;
			for (int j = 0; j < num3; j++)
			{
				Pronunciation pronunciation = array2[j];
				pronunciation.setWord(word);
			}
			this.wordDictionary.put(text, word);
			return word;
		}

		
		
		protected internal virtual Unit getCIUnit(string name, bool isFiller)
		{
			return this.unitManager.getUnit(name, isFiller, Context.__EMPTY_CONTEXT);
		}

		[LineNumberTable(new byte[]
		{
			159,
			48,
			162,
			105,
			115
		})]
		
		private Word createWord(string text, Pronunciation[] pronunciations, bool isFiller)
		{
			Word word = new Word(text, pronunciations, isFiller);
			this.dictionary.put(text, word.toString());
			return word;
		}

		[LineNumberTable(new byte[]
		{
			161,
			68,
			113,
			134,
			127,
			1,
			114,
			127,
			4,
			130
		})]
		
		public override string toString()
		{
			TreeMap.__<clinit>();
			TreeMap treeMap = new TreeMap(this.dictionary);
			StringBuilder stringBuilder = new StringBuilder();
			Iterator iterator = treeMap.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				stringBuilder.append((string)entry.getKey());
				stringBuilder.append("   ").append((string)entry.getValue()).append('\n');
			}
			return stringBuilder.toString();
		}

		[Throws(new string[]
		{
			"java.net.MalformedURLException",
			"java.lang.ClassNotFoundException"
		})]
		
		[LineNumberTable(new byte[]
		{
			48,
			104,
			42,
			133
		})]
		
		public TextDictionary(string wordDictionaryFile, string fillerDictionaryFile, List addendaUrlList, bool addSilEndingPronunciation, string wordReplacement, UnitManager unitManager) : this(ConfigurationManagerUtils.resourceToURL(wordDictionaryFile), ConfigurationManagerUtils.resourceToURL(fillerDictionaryFile), addendaUrlList, wordReplacement, unitManager)
		{
		}

		
		[LineNumberTable(new byte[]
		{
			65,
			111,
			104,
			104
		})]
		
		public TextDictionary(URL wordDictionaryFile, URL fillerDictionaryFile, List addendaUrlList, bool addSilEndingPronunciation, string wordReplacement, UnitManager unitManager, URL g2pModelFile, int g2pMaxPron) : this(wordDictionaryFile, fillerDictionaryFile, addendaUrlList, wordReplacement, unitManager)
		{
			this.g2pModelFile = g2pModelFile;
			this.g2pMaxPron = g2pMaxPron;
		}

		[LineNumberTable(new byte[]
		{
			70,
			232,
			24,
			231,
			106
		})]
		
		public TextDictionary()
		{
			this.g2pMaxPron = 0;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			83,
			140,
			113,
			113,
			113,
			113,
			118,
			113,
			113
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.wordDictionaryFile = ConfigurationManagerUtils.getResource("dictionaryPath", ps);
			this.fillerDictionaryFile = ConfigurationManagerUtils.getResource("fillerPath", ps);
			this.addendaUrlList = ps.getResourceList("addenda");
			this.wordReplacement = ps.getString("wordReplacement");
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
			this.g2pModelFile = ConfigurationManagerUtils.getResource("g2pModelPath", ps);
			this.g2pMaxPron = ps.getInt("g2pMaxPron");
		}

		public virtual URL getWordDictionaryFile()
		{
			return this.wordDictionaryFile;
		}

		public virtual URL getFillerDictionaryFile()
		{
			return this.fillerDictionaryFile;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			119,
			107,
			107,
			139,
			108,
			139,
			134,
			159,
			11,
			146,
			140,
			159,
			11,
			146,
			127,
			0,
			145,
			167
		})]
		
		public virtual void allocate()
		{
			if (!this.allocated)
			{
				this.dictionary = new HashMap();
				this.wordDictionary = new HashMap();
				Timer timer = TimerPool.getTimer(this, "Load Dictionary");
				this.fillerWords = new HashSet();
				timer.start();
				this.logger.info(new StringBuilder().append("Loading dictionary from: ").append(this.wordDictionaryFile).toString());
				this.loadDictionary(this.wordDictionaryFile.openStream(), false);
				this.loadCustomDictionaries(this.addendaUrlList);
				this.logger.info(new StringBuilder().append("Loading filler dictionary from: ").append(this.fillerDictionaryFile).toString());
				this.loadDictionary(this.fillerDictionaryFile.openStream(), true);
				if (this.g2pModelFile != null && !java.lang.String.instancehelper_equals(this.g2pModelFile.getPath(), ""))
				{
					this.g2pDecoder = new G2PConverter(this.g2pModelFile);
				}
				timer.stop();
			}
		}

		public virtual void deallocate()
		{
			if (this.allocated)
			{
				this.dictionary = null;
				this.g2pDecoder = null;
				this.allocated = false;
			}
		}

		
		
		public virtual Word getSentenceStartWord()
		{
			return this.getWord("<s>");
		}

		
		
		public virtual Word getSentenceEndWord()
		{
			return this.getWord("</s>");
		}

		
		
		public virtual Word getSilenceWord()
		{
			return this.getWord("<sil>");
		}

		[LineNumberTable(new byte[]
		{
			161,
			85,
			113,
			98,
			127,
			1,
			110,
			98
		})]
		
		public virtual Word[] getFillerWords()
		{
			Word[] array = new Word[this.fillerWords.size()];
			int num = 0;
			Iterator iterator = this.fillerWords.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				Word[] array2 = array;
				int num2 = num;
				num++;
				array2[num2] = this.getWord(text);
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			161,
			97,
			112
		})]
		
		public virtual void dump()
		{
			java.lang.System.@out.print(this.toString());
		}

		protected internal Logger logger;

		protected internal URL wordDictionaryFile;

		protected internal URL fillerDictionaryFile;

		
		protected internal List addendaUrlList;

		private string wordReplacement;

		protected internal URL g2pModelFile;

		protected internal int g2pMaxPron;

		protected internal UnitManager unitManager;

		
		protected internal Map dictionary;

		
		protected internal Map wordDictionary;

		protected internal G2PConverter g2pDecoder;

		protected internal const string FILLER_TAG = "-F-";

		
		protected internal Set fillerWords;

		protected internal bool allocated;
	}
}

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.util;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.language.ngram.trie
{
	public class NgramTrieModel : LanguageModelBase
	{		
		private void buildUnigramIDMap()
		{
			int num = 0;
			if (this.unigramIDMap == null)
			{
				this.unigramIDMap = new HashMap();
			}
			for (int i = 0; i < this.words.Length; i++)
			{
				Word word = this.dictionary.getWord(this.words[i]);
				if (word == null)
				{
					this.logger.warning(new StringBuilder().append("The dictionary is missing a phonetic transcription for the word '").append(this.words[i]).append("'").toString());
					num++;
				}
				this.unigramIDMap.put(word, Integer.valueOf(i));
				if (this.logger.isLoggable(Level.FINE))
				{
					this.logger.fine(new StringBuilder().append("Word: ").append(word).toString());
				}
			}
			if (num > 0)
			{
				this.logger.warning(new StringBuilder().append("Dictionary is missing ").append(num).append(" words that are contained in the language model.").toString());
			}
		}
		
		private float getAvailableProb(WordSequence wordSequence, NgramTrieModel.TrieRange trieRange, float num)
		{
			if (!trieRange.isSearchable())
			{
				return num;
			}
			for (int i = wordSequence.size() - 2; i >= 0; i --)
			{
				int num2 = wordSequence.size() - 2 - i;
				if (num2 + 1 == this.maxDepth)
				{
					break;
				}
				int wordId = ((Integer)this.unigramIDMap.get(wordSequence.getWord(i))).intValue();
				float num3 = this.trie.readNgramProb(wordId, num2, trieRange, this.quant);
				if (!trieRange.getFound())
				{
					break;
				}
				num = num3;
				this.curDepth++;
				if (!trieRange.isSearchable())
				{
					break;
				}
			}
			return num;
		}
		
		private float getAvailableBackoff(WordSequence wordSequence)
		{
			float num = 0f;
			int num2 = wordSequence.size();
			int num3 = ((Integer)this.unigramIDMap.get(wordSequence.getWord(num2 - 2))).intValue();
			NgramTrieModel.TrieRange trieRange = new NgramTrieModel.TrieRange(this.unigrams[num3].next, this.unigrams[num3 + 1].next);
			if (this.curDepth == 1)
			{
				num += this.unigrams[num3].backoff;
			}
			int i = num2 - 3;
			int num4 = 0;
			while (i >= 0)
			{
				int wordId = ((Integer)this.unigramIDMap.get(wordSequence.getWord(i))).intValue();
				float num5 = this.trie.readNgramBackoff(wordId, num4, trieRange, this.quant);
				if (!trieRange.getFound())
				{
					break;
				}
				num += num5;
				if (!trieRange.isSearchable())
				{
					break;
				}
				i --;
				num4++;
			}
			return num;
		}
		
		private float getProbabilityRaw(WordSequence wordSequence)
		{
			int num = wordSequence.size();
			int num2 = ((Integer)this.unigramIDMap.get(wordSequence.getWord(num - 1))).intValue();
			NgramTrieModel.TrieRange trieRange = new NgramTrieModel.TrieRange(this.unigrams[num2].next, this.unigrams[num2 + 1].next);
			float num3 = this.unigrams[num2].prob;
			this.curDepth = 1;
			if (num == 1)
			{
				return num3;
			}
			num3 = this.getAvailableProb(wordSequence, trieRange, num3);
			if (this.curDepth < num)
			{
				num3 += this.getAvailableBackoff(wordSequence);
			}
			return num3;
		}

		private float applyWeights(float num)
		{
			if (this.applyLanguageWeightAndWip)
			{
				return num * this.languageWeight + this.logWip;
			}
			return num;
		}
		
		private void clearCache()
		{
			this.logger.info(new StringBuilder().append("LM Cache Size: ").append(this.ngramProbCache.size()).append(" Hits: ").append(this.ngramHits).append(" Misses: ").append(this.ngramMisses).toString());
			if (this.clearCacheAfterUtterance)
			{
				this.ngramProbCache = new LRUCache(this.ngramCacheSize);
			}
		}
		
		public NgramTrieModel(string format, URL location, string ngramLogFile, int maxNGramCacheSize, bool clearCacheAfterUtterance, int maxDepth, dictionary.Dictionary dictionary, bool applyLanguageWeightAndWip, float languageWeight, double wip, float unigramWeight)
		{
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
			this.format = format;
			this.location = location;
			this.ngramLogFile = ngramLogFile;
			this.ngramCacheSize = maxNGramCacheSize;
			this.clearCacheAfterUtterance = clearCacheAfterUtterance;
			this.maxDepth = maxDepth;
			this.logMath = LogMath.getLogMath();
			this.dictionary = dictionary;
			this.applyLanguageWeightAndWip = applyLanguageWeightAndWip;
			this.languageWeight = languageWeight;
			this.logWip = this.logMath.linearToLog(wip);
			this.unigramWeight = unigramWeight;
		}
		
		public NgramTrieModel()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.logMath = LogMath.getLogMath();
			this.location = ConfigurationManagerUtils.getResource("location", ps);
			this.ngramLogFile = ps.getString("queryLogFile");
			this.maxDepth = ps.getInt("maxDepth");
			this.ngramCacheSize = ps.getInt("ngramCacheSize");
			this.clearCacheAfterUtterance = ps.getBoolean("clearCachesAfterUtterance").booleanValue();
			this.dictionary = (dictionary.Dictionary)ps.getComponent("dictionary");
			this.applyLanguageWeightAndWip = ps.getBoolean("applyLanguageWeightAndWip").booleanValue();
			this.languageWeight = ps.getFloat("languageWeight");
			this.logWip = this.logMath.linearToLog(ps.getDouble("wordInsertionProbability"));
			this.unigramWeight = ps.getFloat("unigramWeight");
		}
		
		public override void allocate()
		{
			TimerPool.getTimer(this, "Load LM").start();
			this.logger.info(new StringBuilder().append("Loading n-gram language model from: ").append(this.location).toString());
			if (this.ngramLogFile != null)
			{
				this.logFile = new PrintWriter(new FileOutputStream(this.ngramLogFile));
			}
			BinaryLoader binaryLoader;
			if (this.location.getProtocol() != null)
			{
				if (!java.lang.String.instancehelper_equals(this.location.getProtocol(), "file"))
				{
					binaryLoader = new BinaryLoader(this.location);
					goto IL_EC;
				}
			}
			try
			{
				binaryLoader = new BinaryLoader(new File(this.location.toURI()));
			}
			catch (System.Exception)
			{
				binaryLoader = new BinaryLoader(new File(this.location.getPath()));
			}
			IL_EC:
			binaryLoader.verifyHeader();
			this.counts = binaryLoader.readCounts();
			if (this.maxDepth <= 0 || this.maxDepth > this.counts.Length)
			{
				this.maxDepth = this.counts.Length;
			}
			if (this.maxDepth > 1)
			{
				this.quant = binaryLoader.readQuant(this.maxDepth);
			}
			this.unigrams = binaryLoader.readUnigrams(this.counts[0]);
			if (this.maxDepth > 1)
			{
				this.trie = new NgramTrie(this.counts, this.quant.getProbBoSize(), this.quant.getProbSize());
				binaryLoader.readTrieByteArr(this.trie.getMem());
			}
			this.words = binaryLoader.readWords(this.counts[0]);
			this.buildUnigramIDMap();
			this.ngramProbCache = new LRUCache(this.ngramCacheSize);
			binaryLoader.close();
			TimerPool.getTimer(this, "Load LM").stop();
		}
		
		public override void deallocate()
		{
			if (this.logFile != null)
			{
				this.logFile.flush();
			}
		}
		
		public override float getProbability(WordSequence wordSequence)
		{
			int num = wordSequence.size();
			if (num > this.maxDepth)
			{
				string text = new StringBuilder().append("Unsupported NGram: ").append(wordSequence.size()).toString();
				
				throw new Error(text);
			}
			if (num == this.maxDepth)
			{
				Float @float = (Float)this.ngramProbCache.get(wordSequence);
				if (@float != null)
				{
					this.ngramHits++;
					return @float.floatValue();
				}
				this.ngramMisses++;
			}
			float num2 = this.applyWeights(this.getProbabilityRaw(wordSequence));
			if (num == this.maxDepth)
			{
				this.ngramProbCache.put(wordSequence, Float.valueOf(num2));
			}
			if (this.logFile != null)
			{
				PrintWriter printWriter = this.logFile;
				StringBuilder stringBuilder = new StringBuilder();
				string text2 = wordSequence.toString();
				object obj = "][";
				object obj2 = " ";
				object _ref = obj;
				CharSequence charSequence = CharSequence.Cast(_ref);
				CharSequence charSequence2 = charSequence;
				_ref = obj2;
				charSequence = CharSequence.Cast(_ref);
				printWriter.println(stringBuilder.append(java.lang.String.instancehelper_replace(text2, charSequence2, charSequence)).append(" : ").append(Float.toString(num2)).toString());
			}
			return num2;
		}

		public override float getSmear(WordSequence wordSequence)
		{
			return 0f;
		}

		public override Set getVocabulary()
		{
			HashSet hashSet = new HashSet(Arrays.asList(this.words));
			return Collections.unmodifiableSet(hashSet);
		}

		public virtual int getNGramMisses()
		{
			return this.ngramMisses;
		}

		public virtual int getNGramHits()
		{
			return this.ngramHits;
		}

		public override int getMaxDepth()
		{
			return this.maxDepth;
		}
		
		public override void onUtteranceEnd()
		{
			this.clearCache();
			if (this.logFile != null)
			{
				this.logFile.println("<END_UTT>");
				this.logFile.flush();
			}
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"mandatory",
			false
		})]
		public const string PROP_QUERY_LOG_FILE = "queryLogFile";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			100000
		})]
		public const string PROP_NGRAM_CACHE_SIZE = "ngramCacheSize";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_CLEAR_CACHES_AFTER_UTTERANCE = "clearCachesAfterUtterance";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_LANGUAGE_WEIGHT = "languageWeight";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_APPLY_LANGUAGE_WEIGHT_AND_WIP = "applyLanguageWeightAndWip";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_WORD_INSERTION_PROBABILITY = "wordInsertionProbability";

		internal URL location;

		protected internal Logger logger;

		protected internal LogMath logMath;

		protected internal int maxDepth;

		protected internal int curDepth;

		protected internal int[] counts;

		protected internal int ngramCacheSize;

		protected internal bool clearCacheAfterUtterance;

		protected internal dictionary.Dictionary dictionary;

		protected internal string format;

		protected internal bool applyLanguageWeightAndWip;

		protected internal float languageWeight;

		protected internal float unigramWeight;

		protected internal float logWip;

		protected internal string ngramLogFile;

		private int ngramMisses;

		private int ngramHits;

		private PrintWriter logFile;

		protected internal NgramTrieModel.TrieUnigram[] unigrams;

		protected internal string[] words;

		protected internal NgramTrieQuant quant;

		protected internal NgramTrie trie;

		protected internal Map unigramIDMap;
		
		private LRUCache ngramProbCache;

		public class TrieRange : java.lang.Object
		{
			internal virtual void setFound(bool flag)
			{
				this.found = flag;
			}
			
			internal TrieRange(int num, int num2)
			{
				this.begin = num;
				this.end = num2;
				this.found = true;
			}

			internal virtual int getWidth()
			{
				return this.end - this.begin;
			}

			internal virtual bool getFound()
			{
				return this.found;
			}
			
			internal virtual bool isSearchable()
			{
				return this.getWidth() > 0;
			}

			internal int begin;

			internal int end;

			internal bool found;
		}

		public class TrieUnigram : java.lang.Object
		{
			public TrieUnigram()
			{
			}

			public float prob;

			public float backoff;

			public int next;
		}
	}
}

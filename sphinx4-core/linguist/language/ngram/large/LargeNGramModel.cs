using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.util;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	public class LargeNGramModel : LanguageModelBase
	{		
		private void buildUnigramIDMap(dictionary.Dictionary dictionary)
		{
			int num = 0;
			string[] words = this.loader.getWords();
			for (int i = 0; i < words.Length; i++)
			{
				Word word = dictionary.getWord(words[i]);
				if (word == null)
				{
					this.logger.warning(new StringBuilder().append("The dictionary is missing a phonetic transcription for the word '").append(words[i]).append("'").toString());
					num++;
				}
				this.unigramIDMap.put(word, this.unigrams[i]);
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
		
		private void buildUnigramIDMap()
		{
			string[] words = this.loader.getWords();
			for (int i = 0; i < words.Length; i++)
			{
				Word word = new Word(words[i], null, false);
				this.unigramIDMap.put(word, this.unigrams[i]);
			}
		}
		
		private void readSmearInfo(string text)
		{
			DataInputStream dataInputStream = new DataInputStream(new FileInputStream(text));
			if (dataInputStream.readInt() != -1060454374)
			{
				dataInputStream.close();
				string text2 = new StringBuilder().append("Bad smear format for ").append(text).toString();
				
				throw new IOException(text2);
			}
			if (dataInputStream.readInt() != this.unigrams.Length)
			{
				dataInputStream.close();
				string text3 = new StringBuilder().append("Bad unigram length in ").append(text).toString();
				
				throw new IOException(text3);
			}
			this.bigramSmearMap = new HashMap();
			this.unigramSmearTerm = new float[this.unigrams.Length];
			java.lang.System.@out.println(new StringBuilder().append("Reading ").append(this.unigrams.Length).toString());
			for (int i = 0; i < this.unigrams.Length; i++)
			{
				this.unigramSmearTerm[i] = dataInputStream.readFloat();
			}
			for (int i = 0; i < this.unigrams.Length; i++)
			{
				java.lang.System.@out.println(new StringBuilder().append("Processed ").append(i).append(" of ").append(this.loadedBigramBuffers.Length).toString());
				int num = dataInputStream.readInt();
				NGramBuffer bigramBuffer = this.getBigramBuffer(i);
				if (bigramBuffer.getNumberNGrams() != num)
				{
					dataInputStream.close();
					string text4 = new StringBuilder().append("Bad ngrams for unigram ").append(i).append(" Found ").append(num).append(" expected ").append(bigramBuffer.getNumberNGrams()).toString();
					
					throw new IOException(text4);
				}
				for (int j = 0; j < num; j++)
				{
					int wordID = bigramBuffer.getWordID(j);
					this.putSmearTerm(i, wordID, dataInputStream.readFloat());
				}
			}
			dataInputStream.close();
		}
		
		private void buildSmearInfo()
		{
			double num = (double)0f;
			double num2 = (double)0f;
			this.bigramSmearMap = new HashMap();
			double[] array = new double[this.unigrams.Length];
			double[] array2 = new double[this.unigrams.Length];
			double[] array3 = new double[this.unigrams.Length];
			this.unigramSmearTerm = new float[this.unigrams.Length];
			UnigramProbability[] array4 = this.unigrams;
			int num3 = array4.Length;
			for (int i = 0; i < num3; i++)
			{
				UnigramProbability unigramProbability = array4[i];
				float logProbability = unigramProbability.getLogProbability();
				double num4 = this.logMath.logToLinear(logProbability);
				num += num4 * (double)logProbability;
				num2 += num4 * (double)logProbability * (double)logProbability;
			}
			java.lang.System.@out.println(new StringBuilder().append("R0 S0 ").append(num2).append(' ').append(num).toString());
			for (int j = 0; j < this.loadedBigramBuffers.Length; j++)
			{
				NGramBuffer bigramBuffer = this.getBigramBuffer(j);
				if (bigramBuffer == null)
				{
					this.unigramSmearTerm[j] = 0f;
				}
				else
				{
					array[j] = (double)0f;
					array2[j] = (double)0f;
					array3[j] = (double)0f;
					float logBackoff = this.unigrams[j].getLogBackoff();
					double num5 = this.logMath.logToLinear(logBackoff);
					int num11;
					double[] array6;
					for (int k = 0; k < bigramBuffer.getNumberNGrams(); k++)
					{
						int wordID = bigramBuffer.getWordID(k);
						NGramProbability ngramProbability = bigramBuffer.getNGramProbability(k);
						float logProbability2 = this.unigrams[wordID].getLogProbability();
						float num6 = this.ngramProbTable[1][ngramProbability.getProbabilityID()];
						double num7 = this.logMath.logToLinear(logProbability2);
						double num8 = this.logMath.logToLinear(num6);
						double num9 = num5 * num7;
						double num10 = (double)this.logMath.linearToLog(num9);
						double[] array5 = array;
						num11 = j;
						array6 = array5;
						array6[num11] += (num8 * (double)num6 - num9 * num10) * (double)logProbability2;
						double[] array7 = array2;
						num11 = j;
						array6 = array7;
						array6[num11] += (num8 - num9) * (double)logProbability2;
					}
					double[] array8 = array;
					num11 = j;
					array6 = array8;
					array6[num11] += num5 * ((double)logBackoff * num + num2);
					array3[j] = array2[j] + num5 * num;
					double[] array9 = array2;
					num11 = j;
					array6 = array9;
					array6[num11] += num5 * num2;
					this.unigramSmearTerm[j] = (float)(array[j] / array2[j]);
				}
			}
			for (int j = 0; j < this.loadedBigramBuffers.Length; j++)
			{
				java.lang.System.@out.println(new StringBuilder().append("Processed ").append(j).append(" of ").append(this.loadedBigramBuffers.Length).toString());
				NGramBuffer bigramBuffer = this.getBigramBuffer(j);
				if (bigramBuffer != null)
				{
					for (int i = 0; i < bigramBuffer.getNumberNGrams(); i++)
					{
						NGramProbability ngramProbability2 = bigramBuffer.getNGramProbability(i);
						float num12 = this.ngramBackoffTable[2][ngramProbability2.getBackoffID()];
						double num13 = this.logMath.logToLinear(num12);
						int wordID2 = bigramBuffer.getWordID(i);
						NGramBuffer ngramBuffer = this.loadTrigramBuffer(j, wordID2);
						float num14;
						if (ngramBuffer == null)
						{
							num14 = this.unigramSmearTerm[wordID2];
						}
						else
						{
							double num7 = (double)0f;
							double num8 = (double)0f;
							for (int l = 0; l < ngramBuffer.getNumberNGrams(); l++)
							{
								int wordID3 = ngramBuffer.getWordID(l);
								float num15 = this.ngramProbTable[2][ngramBuffer.getProbabilityID(l)];
								double num16 = this.logMath.logToLinear(num15);
								float bigramProb = this.getBigramProb(wordID2, wordID3);
								double num17 = this.logMath.logToLinear(bigramProb);
								float logProbability3 = this.unigrams[wordID3].getLogProbability();
								double num18 = num13 * num17;
								double num19 = (double)this.logMath.linearToLog(num18);
								num7 += (num16 * (double)num15 - num18 * num19) * (double)logProbability3;
								num8 += (num16 - num18) * (double)logProbability3 * (double)logProbability3;
							}
							num7 += num13 * ((double)num12 * array3[wordID2] - array[wordID2]);
							num8 += num13 * array2[wordID2];
							num14 = (float)(num7 / num8);
							this.smearTermCount++;
						}
						this.putSmearTerm(j, wordID2, num14);
					}
				}
			}
			java.lang.System.@out.println(new StringBuilder().append("Smear count is ").append(this.smearTermCount).toString());
		}
		
		private void clearCache()
		{
			for (int i = 0; i < this.loadedBigramBuffers.Length; i++)
			{
				NGramBuffer ngramBuffer = this.loadedBigramBuffers[i];
				if (ngramBuffer != null)
				{
					if (!ngramBuffer.getUsed())
					{
						this.loadedBigramBuffers[i] = null;
					}
					else
					{
						ngramBuffer.setUsed(false);
					}
				}
			}
			this.loadedBigramBuffers = new NGramBuffer[this.unigrams.Length];
			for (int i = 2; i <= this.loader.getMaxDepth(); i++)
			{
				this.loadedNGramBuffers[i - 1] = new HashMap();
			}
			this.logger.info(new StringBuilder().append("LM Cache Size: ").append(this.ngramProbCache.size()).append(" Hits: ").append(this.ngramHits).append(" Misses: ").append(this.ngramMisses).toString());
			if (this.clearCacheAfterUtterance)
			{
				this.ngramProbCache = new LRUCache(this.ngramCacheSize);
			}
		}
		
		private Float getNGramProbability(WordSequence wordSequence)
		{
			int num = wordSequence.size();
			Word word = wordSequence.getWord(0);
			if (this.loader.getNumberNGrams(num) == 0 || !this.hasUnigram(word))
			{
				return this.getNGramProbability(wordSequence.getNewest());
			}
			if (num < 2)
			{
				return Float.valueOf(this.getUnigramProbability(wordSequence));
			}
			NGramProbability ngramProbability = this.findNGram(wordSequence);
			if (ngramProbability != null)
			{
				return Float.valueOf(this.ngramProbTable[num - 1][ngramProbability.getProbabilityID()]);
			}
			if (num == 2)
			{
				UnigramProbability unigram = this.getUnigram(word);
				UnigramProbability unigram2 = this.getUnigram(wordSequence.getWord(1));
				return Float.valueOf(unigram.getLogBackoff() + unigram2.getLogProbability());
			}
			NGramProbability ngramProbability2 = this.findNGram(wordSequence.getOldest());
			if (ngramProbability2 != null)
			{
				return Float.valueOf(this.ngramBackoffTable[num - 1][ngramProbability2.getBackoffID()] + this.getProbability(wordSequence.getNewest()));
			}
			return Float.valueOf(this.getProbability(wordSequence.getNewest()));
		}
		
		private bool hasUnigram(Word word)
		{
			return this.unigramIDMap.get(word) != null;
		}

		private float getUnigramProbability(WordSequence wordSequence)
		{
			Word word = wordSequence.getWord(0);
			UnigramProbability unigram = this.getUnigram(word);
			if (unigram == null)
			{
				string text = new StringBuilder().append("Unigram not in LM: ").append(word).toString();
				
				throw new Error(text);
			}
			return unigram.getLogProbability();
		}
		
		private NGramProbability findNGram(WordSequence wordSequence)
		{
			int num = wordSequence.size();
			NGramProbability result = null;
			WordSequence oldest = wordSequence.getOldest();
			NGramBuffer ngramBuffer = (NGramBuffer)this.loadedNGramBuffers[num - 1].get(oldest);
			if (ngramBuffer == null)
			{
				ngramBuffer = this.getNGramBuffer(oldest);
				if (ngramBuffer != null)
				{
					this.loadedNGramBuffers[num - 1].put(oldest, ngramBuffer);
				}
			}
			if (ngramBuffer != null)
			{
				int wordID = this.getWordID(wordSequence.getWord(num - 1));
				result = ngramBuffer.findNGram(wordID);
			}
			return result;
		}
		
		private UnigramProbability getUnigram(Word word)
		{
			return (UnigramProbability)this.unigramIDMap.get(word);
		}
		
		public override float getProbability(WordSequence wordSequence)
		{
			int num = wordSequence.size();
			if (num > this.maxDepth)
			{
				string text = new StringBuilder().append("Unsupported NGram: ").append(wordSequence.size()).toString();
				
				throw new Error(text);
			}
			Float @float;
			if (num == this.maxDepth)
			{
				@float = (Float)this.ngramProbCache.get(wordSequence);
				if (@float != null)
				{
					this.ngramHits++;
					return @float.floatValue();
				}
				this.ngramMisses++;
			}
			@float = this.getNGramProbability(wordSequence);
			if (num == this.maxDepth)
			{
				this.ngramProbCache.put(wordSequence, @float);
			}
			if (this.logFile != null && @float != null)
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
				printWriter.println(stringBuilder.append(java.lang.String.instancehelper_replace(text2, charSequence2, charSequence)).append(" : ").append(Float.toString(@float.floatValue())).toString());
			}
			return @float.floatValue();
		}
		
		private NGramBuffer getNGramBuffer(WordSequence wordSequence)
		{
			NGramBuffer ngramBuffer = null;
			int num = wordSequence.size();
			if (num > 1)
			{
				ngramBuffer = (NGramBuffer)this.loadedNGramBuffers[num - 1].get(wordSequence);
			}
			if (ngramBuffer == null)
			{
				ngramBuffer = this.loadNGramBuffer(wordSequence);
				if (ngramBuffer != null)
				{
					this.loadedNGramBuffers[num - 1].put(wordSequence, ngramBuffer);
				}
			}
			return ngramBuffer;
		}
		
		public int getWordID(Word word)
		{
			UnigramProbability unigram = this.getUnigram(word);
			if (unigram == null)
			{
				string text = new StringBuilder().append("No word ID: ").append(word).toString();
				
				throw new IllegalArgumentException(text);
			}
			return unigram.getWordID();
		}
		
		private int getNumberBigramFollowers(int num)
		{
			if (num == this.unigrams.Length - 1)
			{
				return 0;
			}
			return this.unigrams[num + 1].getFirstBigramEntry() - this.unigrams[num].getFirstBigramEntry();
		}
		
		private int getFirstNGramEntry(NGramProbability ngramProbability, int num, int num2)
		{
			return this.ngramSegmentTable[num2 - 1][num + ngramProbability.getWhichFollower() >> this.loader.getLogNGramSegmentSize()] + ngramProbability.getFirstNPlus1GramEntry();
		}
		
		private bool is32bits()
		{
			return this.loader.getBytesPerField() == 4;
		}

		private NGramBuffer loadNGramBuffer(WordSequence wordSequence)
		{
			int wordID = this.getWordID(wordSequence.getWord(0));
			int num = wordSequence.size() + 1;
			int num2 = this.unigrams[wordID].getFirstBigramEntry();
			int num3 = this.getNumberBigramFollowers(wordID) + 1;
			if (num3 == 1)
			{
				return null;
			}
			int size;
			long position;
			if (num == 2)
			{
				size = num3 * ((this.loader.getMaxDepth() != num) ? 4 : 2) * this.loader.getBytesPerField();
				position = this.loader.getNGramOffset(num) + (long)(num2 * ((this.loader.getMaxDepth() != num) ? 4 : 2) * this.loader.getBytesPerField());
			}
			else
			{
				int wordID2 = this.getWordID(wordSequence.getWord(wordSequence.size() - 1));
				NGramBuffer ngramBuffer = this.getNGramBuffer(wordSequence.getOldest());
				int num4 = ngramBuffer.findNGramIndex(wordID2);
				if (num4 == -1)
				{
					return null;
				}
				int firstNGramEntry = ngramBuffer.getFirstNGramEntry();
				num2 = this.getFirstNGramEntry(ngramBuffer.getNGramProbability(num4), firstNGramEntry, num);
				int firstNGramEntry2 = this.getFirstNGramEntry(ngramBuffer.getNGramProbability(num4 + 1), firstNGramEntry, num);
				num3 = firstNGramEntry2 - num2;
				if (num3 == 0)
				{
					return null;
				}
				if (this.loader.getMaxDepth() != num)
				{
					num3++;
				}
				size = num3 * ((this.loader.getMaxDepth() != num) ? 4 : 2) * this.loader.getBytesPerField();
				position = this.loader.getNGramOffset(num) + (long)num2 * ((this.loader.getMaxDepth() != num) ? 4L : 2L) * (long)this.loader.getBytesPerField();
			}
			NGramBuffer result;
			try
			{
				byte[] array = this.loader.loadBuffer(position, size);
				if (this.loader.getMaxDepth() == num)
				{
					result = new NMaxGramBuffer(array, num3, this.loader.getBigEndian(), this.is32bits(), num, num2);
				}
				else
				{
					result = new NGramBuffer(array, num3, this.loader.getBigEndian(), this.is32bits(), num, num2);
				}
			}
			catch (IOException ex)
			{
				throw new Error(new StringBuilder().append("Error loading ").append(num).append("-Grams.").toString(),ex);
			}
			return result;
		}
		
		private Float getSmearTerm(int num, int num2)
		{
			long num3 = (long)num << 32 | (long)num2;
			return (Float)this.bigramSmearMap.get(Long.valueOf(num3));
		}
		
		private NGramBuffer getBigramBuffer(int num)
		{
			WordSequence wordSequence = new WordSequence(new Word[]
			{
				this.dictionary.getWord(this.loader.getWords()[num])
			});
			return this.loadNGramBuffer(wordSequence);
		}
		
		private NGramBuffer loadTrigramBuffer(int num, int num2)
		{
			WordSequence wordSequence = new WordSequence(new Word[]
			{
				this.dictionary.getWord(this.loader.getWords()[num]),
				this.dictionary.getWord(this.loader.getWords()[num2])
			});
			return this.loadNGramBuffer(wordSequence);
		}
		
		private float getBigramProb(int num, int num2)
		{
			NGramBuffer bigramBuffer = this.getBigramBuffer(num);
			NGramProbability ngramProbability = bigramBuffer.findNGram(num2);
			return this.ngramProbTable[1][ngramProbability.getProbabilityID()];
		}
		
		private void putSmearTerm(int num, int num2, float num3)
		{
			long num4 = (long)num << 32 | (long)num2;
			this.bigramSmearMap.put(Long.valueOf(num4), Float.valueOf(num3));
		}
		
		public LargeNGramModel(string format, URL location, string ngramLogFile, int maxNGramCacheSize, bool clearCacheAfterUtterance, int maxDepth, dictionary.Dictionary dictionary, bool applyLanguageWeightAndWip, float languageWeight, double wip, float unigramWeight, bool fullSmear)
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
			this.wip = wip;
			this.unigramWeight = unigramWeight;
			this.fullSmear = fullSmear;
		}
		
		public LargeNGramModel()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.location = ConfigurationManagerUtils.getResource("location", ps);
			this.ngramLogFile = ps.getString("queryLogFile");
			this.ngramCacheSize = ps.getInt("ngramCacheSize");
			this.clearCacheAfterUtterance = ps.getBoolean("clearCachesAfterUtterance").booleanValue();
			this.maxDepth = ps.getInt("maxDepth");
			this.dictionary = (dictionary.Dictionary)ps.getComponent("dictionary");
			this.applyLanguageWeightAndWip = ps.getBoolean("applyLanguageWeightAndWip").booleanValue();
			this.languageWeight = ps.getFloat("languageWeight");
			this.wip = ps.getDouble("wordInsertionProbability");
			this.unigramWeight = ps.getFloat("unigramWeight");
			this.fullSmear = ps.getBoolean("fullSmear").booleanValue();
		}
		
		public override void allocate()
		{
			TimerPool.getTimer(this, "Load LM").start();
			this.logger.info(new StringBuilder().append("Loading n-gram language model from: ").append(this.location).toString());
			if (this.ngramLogFile != null)
			{
				this.logFile = new PrintWriter(new FileOutputStream(this.ngramLogFile));
			}
			if (this.location.getProtocol() != null)
			{
				if (!java.lang.String.instancehelper_equals(this.location.getProtocol(), "file"))
				{
					this.loader = new BinaryStreamLoader(this.location, this.format, this.applyLanguageWeightAndWip, this.languageWeight, this.wip, this.unigramWeight);
					goto IL_15B;
				}
			}
			try
			{
				this.loader = new BinaryLoader(new File(this.location.toURI()), this.format, this.applyLanguageWeightAndWip, this.languageWeight, this.wip, this.unigramWeight);
			}
			catch (System.Exception)
			{
				this.loader = new BinaryLoader(new File(this.location.getPath()), this.format, this.applyLanguageWeightAndWip, this.languageWeight, this.wip, this.unigramWeight);
			}
			IL_15B:
			this.unigramIDMap = new HashMap();
			this.unigrams = this.loader.getUnigrams();
			this.loadedNGramBuffers = new Map[this.loader.getMaxDepth()];
			this.ngramProbTable = new float[this.loader.getMaxDepth()][];
			this.ngramBackoffTable = new float[this.loader.getMaxDepth()][];
			this.ngramSegmentTable = new int[this.loader.getMaxDepth()][];
			for (int i = 1; i <= this.loader.getMaxDepth(); i++)
			{
				this.loadedNGramBuffers[i - 1] = new HashMap();
				if (i >= 2)
				{
					this.ngramProbTable[i - 1] = this.loader.getNGramProbabilities(i);
				}
				if (i > 2)
				{
					this.ngramBackoffTable[i - 1] = this.loader.getNGramBackoffWeights(i);
					this.ngramSegmentTable[i - 1] = this.loader.getNGramSegments(i);
				}
			}
			this.ngramProbCache = new LRUCache(this.ngramCacheSize);
			if (this.dictionary != null)
			{
				this.buildUnigramIDMap(this.dictionary);
			}
			else
			{
				this.buildUnigramIDMap();
			}
			this.loadedBigramBuffers = new NGramBuffer[this.unigrams.Length];
			if (this.maxDepth <= 0 || this.maxDepth > this.loader.getMaxDepth())
			{
				this.maxDepth = this.loader.getMaxDepth();
			}
			for (int i = 1; i <= this.loader.getMaxDepth(); i++)
			{
				this.logger.info(new StringBuilder().append(Integer.toString(i)).append("-grams: ").append(this.loader.getNumberNGrams(i)).toString());
			}
			if (this.fullSmear)
			{
				java.lang.System.@out.println("Full Smear");
				try
				{
					java.lang.System.@out.println("... Reading ...");
					this.readSmearInfo("smear.dat");
					java.lang.System.@out.println("... Done ");
				}
				catch (IOException ex2)
				{
					java.lang.System.@out.println(new StringBuilder().append("... ").append(ex2).toString());
					java.lang.System.@out.println("... Calculating");
					this.buildSmearInfo();
					java.lang.System.@out.println("... Writing");
					java.lang.System.@out.println("... Done");
				}
			}
			TimerPool.getTimer(this, "Load LM").stop();
		}
		
		public override void deallocate()
		{
			this.loader.deallocate();
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

		public virtual bool hasWord(Word w)
		{
			Map map = this.unigramIDMap;
			return map.get(new Word(w.toString(), null, false)) != null;
		}
		
		public virtual float getSmearOld(WordSequence wordSequence)
		{
			float num = 0f;
			if (this.fullSmear)
			{
				int num2 = wordSequence.size();
				if (num2 > 0)
				{
					int wordID = this.getWordID(wordSequence.getWord(num2 - 1));
					num = this.unigramSmearTerm[wordID];
				}
			}
			if (this.fullSmear && this.logger.isLoggable(Level.FINE))
			{
				this.logger.fine(new StringBuilder().append("SmearTerm: ").append(num).toString());
			}
			return num;
		}
		
		public override float getSmear(WordSequence wordSequence)
		{
			float num = 0f;
			if (this.fullSmear)
			{
				this.smearCount++;
				int num2 = wordSequence.size();
				if (num2 == 1)
				{
					int num3 = this.getWordID(wordSequence.getWord(0));
					num = this.unigramSmearTerm[num3];
				}
				else if (num2 >= 2)
				{
					int num3 = wordSequence.size();
					int wordID = this.getWordID(wordSequence.getWord(num3 - 2));
					int wordID2 = this.getWordID(wordSequence.getWord(num3 - 1));
					Float smearTerm = this.getSmearTerm(wordID, wordID2);
					if (smearTerm == null)
					{
						num = this.unigramSmearTerm[wordID2];
					}
					else
					{
						num = smearTerm.floatValue();
						this.smearBigramHit++;
					}
				}
				bool flag = this.smearCount != 0;
				int num4 = 100000;
				if (num4 == -1 || (flag ? 1 : 0) % num4 == 0)
				{
					java.lang.System.@out.println(new StringBuilder().append("Smear hit: ").append(this.smearBigramHit).append(" tot: ").append(this.smearCount).toString());
				}
			}
			if (this.fullSmear && this.logger.isLoggable(Level.FINE))
			{
				this.logger.fine(new StringBuilder().append("SmearTerm: ").append(num).toString());
			}
			return num;
		}

		public override int getMaxDepth()
		{
			return this.maxDepth;
		}
		
		public override Set getVocabulary()
		{
			HashSet hashSet = new HashSet(Arrays.asList(this.loader.getWords()));
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
		
		private void dumpProbs(double[] array, double[] array2, int num, int num2, float num3, float num4, double num5, double num6, double num7, double num8)
		{
			java.lang.System.@out.println(new StringBuilder().append("ubo ").append(num5).append(' ').append(num6).append(' ').append(num7).toString());
			java.lang.System.@out.println(new StringBuilder().append("logubo ").append(num3).append(' ').append(num4).append(' ').append(num8).toString());
			java.lang.System.@out.println(new StringBuilder().append("n/d ").append(num2).append(' ').append(array[num]).append(' ').append(array2[num]).toString());
			java.lang.System.@out.print(new StringBuilder().append(num5).append(" ").append(num6).append(' ').append(num7).toString());
			java.lang.System.@out.print(new StringBuilder().append(" ").append(num3).append(' ').append(num4).append(' ').append(num8).toString());
			java.lang.System.@out.println(new StringBuilder().append("  ").append(array[num]).append(' ').append(array2[num]).toString());
		}
		
		private void writeSmearInfo(string text)
		{
			DataOutputStream dataOutputStream = new DataOutputStream(new FileOutputStream(text));
			dataOutputStream.writeInt(-1060454374);
			java.lang.System.@out.println(new StringBuilder().append("writing ").append(this.unigrams.Length).toString());
			dataOutputStream.writeInt(this.unigrams.Length);
			for (int i = 0; i < this.unigrams.Length; i++)
			{
				dataOutputStream.writeFloat(this.unigramSmearTerm[i]);
			}
			for (int i = 0; i < this.unigrams.Length; i++)
			{
				java.lang.System.@out.println(new StringBuilder().append("Writing ").append(i).append(" of ").append(this.unigrams.Length).toString());
				NGramBuffer bigramBuffer = this.getBigramBuffer(i);
				if (bigramBuffer == null)
				{
					dataOutputStream.writeInt(0);
				}
				else
				{
					dataOutputStream.writeInt(bigramBuffer.getNumberNGrams());
					for (int j = 0; j < bigramBuffer.getNumberNGrams(); j++)
					{
						int wordID = bigramBuffer.getWordID(j);
						Float smearTerm = this.getSmearTerm(i, wordID);
						dataOutputStream.writeInt(wordID);
						dataOutputStream.writeFloat(smearTerm.floatValue());
					}
				}
			}
			dataOutputStream.close();
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

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_FULL_SMEAR = "fullSmear";

		public const int BYTES_PER_NGRAM = 4;

		public const int BYTES_PER_NMAXGRAM = 2;

		private const int SMEAR_MAGIC = -1060454374;

		internal URL location;

		protected internal Logger logger;

		protected internal LogMath logMath;

		protected internal int maxDepth;

		protected internal int ngramCacheSize;

		protected internal bool clearCacheAfterUtterance;

		protected internal bool fullSmear;

		protected internal dictionary.Dictionary dictionary;

		protected internal string format;

		protected internal bool applyLanguageWeightAndWip;

		protected internal float languageWeight;

		protected internal float unigramWeight;

		protected internal double wip;

		private int ngramMisses;

		private int ngramHits;

		private int smearTermCount;

		protected internal string ngramLogFile;

		private BinaryLoader loader;

		private PrintWriter logFile;
		
		private Map unigramIDMap;
		
		private Map[] loadedNGramBuffers;
		
		private LRUCache ngramProbCache;
		
		private Map bigramSmearMap;

		private NGramBuffer[] loadedBigramBuffers;

		private UnigramProbability[] unigrams;

		private int[][] ngramSegmentTable;

		private float[][] ngramProbTable;

		private float[][] ngramBackoffTable;

		private float[] unigramSmearTerm;

		internal int smearCount;

		internal int smearBigramHit;
	}
}

using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.util;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.language.ngram.LanguageModel"
	})]
	public class LargeNGramModel : java.lang.Object, LanguageModel, Configurable
	{
		[LineNumberTable(new byte[]
		{
			160,
			170,
			98,
			108,
			106,
			138,
			99,
			159,
			18,
			164,
			149,
			114,
			255,
			6,
			52,
			233,
			79,
			100,
			159,
			16
		})]
		
		private void buildUnigramIDMap(Dictionary dictionary)
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

		[LineNumberTable(new byte[]
		{
			160,
			193,
			108,
			103,
			144,
			245,
			61,
			230,
			69
		})]
		
		private void buildUnigramIDMap()
		{
			string[] words = this.loader.getWords();
			for (int i = 0; i < words.Length; i++)
			{
				Word.__<clinit>();
				Word word = new Word(words[i], null, false);
				this.unigramIDMap.put(word, this.unigrams[i]);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			163,
			134,
			140,
			109,
			102,
			191,
			6,
			111,
			102,
			191,
			6,
			107,
			114,
			159,
			11,
			108,
			46,
			166,
			111,
			159,
			27,
			103,
			136,
			105,
			102,
			127,
			17,
			186,
			104,
			106,
			15,
			232,
			52,
			233,
			82,
			102
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			195,
			102,
			134,
			139,
			109,
			109,
			142,
			146,
			125,
			105,
			112,
			106,
			238,
			60,
			232,
			71,
			159,
			18,
			113,
			138,
			100,
			110,
			165,
			105,
			105,
			138,
			112,
			144,
			113,
			107,
			139,
			112,
			146,
			112,
			144,
			104,
			144,
			223,
			4,
			252,
			47,
			235,
			87,
			125,
			111,
			247,
			69,
			243,
			19,
			235,
			115,
			113,
			159,
			28,
			138,
			100,
			133,
			145,
			107,
			114,
			112,
			107,
			140,
			100,
			144,
			103,
			103,
			113,
			107,
			108,
			104,
			112,
			108,
			112,
			112,
			104,
			104,
			136,
			214,
			243,
			48,
			235,
			84,
			151,
			141,
			104,
			174,
			236,
			23,
			235,
			56,
			235,
			117,
			127,
			10
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			213,
			108,
			137,
			99,
			104,
			139,
			231,
			57,
			230,
			75,
			114,
			112,
			47,
			166,
			159,
			58,
			104,
			145
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			19,
			103,
			136,
			119,
			141,
			100,
			173,
			136,
			99,
			107,
			38,
			198,
			100,
			104,
			99,
			37,
			135,
			104,
			39,
			198,
			142,
			100,
			108,
			52,
			198
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			238,
			104,
			136,
			99,
			159,
			6
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			62,
			103,
			130,
			103,
			107,
			107,
			99,
			104,
			99,
			178,
			99,
			113,
			169
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			245,
			167,
			105,
			191,
			11,
			105,
			146,
			99,
			110,
			135,
			174,
			136,
			105,
			142,
			113,
			127,
			51,
			52,
			165
		})]
		
		public virtual float getProbability(WordSequence wordSequence)
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
				object _<ref> = obj;
				CharSequence charSequence;
				charSequence.__<ref> = _<ref>;
				CharSequence charSequence2 = charSequence;
				_<ref> = obj2;
				charSequence.__<ref> = _<ref>;
				printWriter.println(stringBuilder.append(java.lang.String.instancehelper_replace(text2, charSequence2, charSequence)).append(" : ").append(Float.toString(@float.floatValue())).toString());
			}
			return @float.floatValue();
		}

		[LineNumberTable(new byte[]
		{
			161,
			185,
			98,
			135,
			100,
			246,
			69,
			99,
			136,
			99,
			242,
			72
		})]
		
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

		[LineNumberTable(new byte[]
		{
			162,
			23,
			136,
			99,
			159,
			6
		})]
		
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

		[LineNumberTable(new byte[]
		{
			162,
			112,
			108,
			130,
			119,
			38
		})]
		
		private int getNumberBigramFollowers(int num)
		{
			if (num == this.unigrams.Length - 1)
			{
				return 0;
			}
			return this.unigrams[num + 1].getFirstBigramEntry() - this.unigrams[num].getFirstBigramEntry();
		}

		[LineNumberTable(new byte[]
		{
			161,
			223,
			108,
			119,
			135
		})]
		
		private int getFirstNGramEntry(NGramProbability ngramProbability, int num, int num2)
		{
			return this.ngramSegmentTable[num2 - 1][num + ngramProbability.getWhichFollower() >> this.loader.getLogNGramSegmentSize()] + ngramProbability.getFirstNPlus1GramEntry();
		}

		[LineNumberTable(new byte[]
		{
			161,
			88,
			110,
			98
		})]
		
		private bool is32bits()
		{
			return this.loader.getBytesPerField() == 4;
		}

		[LineNumberTable(new byte[]
		{
			161,
			102,
			110,
			98,
			98,
			98,
			99,
			105,
			98,
			130,
			110,
			138,
			132,
			130,
			100,
			103,
			115,
			104,
			115,
			115,
			143,
			118,
			110,
			139,
			101,
			130,
			105,
			101,
			40,
			166,
			103,
			40,
			167,
			133,
			99,
			130,
			110,
			132,
			103,
			115,
			104,
			148,
			116,
			202,
			145,
			110,
			105,
			182,
			105,
			255,
			9,
			70,
			226,
			61,
			98,
			103,
			191,
			16
		})]
		
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
			IOException ex2;
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
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_1E0;
			}
			return result;
			IL_1E0:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
			string text = new StringBuilder().append("Error loading ").append(num).append("-Grams.").toString();
			
			throw new Error(text);
		}

		[LineNumberTable(new byte[]
		{
			163,
			199,
			105
		})]
		
		private Float getSmearTerm(int num, int num2)
		{
			long num3 = (long)num << 32 | (long)num2;
			return (Float)this.bigramSmearMap.get(Long.valueOf(num3));
		}

		[LineNumberTable(new byte[]
		{
			162,
			167,
			103,
			123,
			135
		})]
		
		private NGramBuffer getBigramBuffer(int num)
		{
			WordSequence wordSequence = new WordSequence(new Word[]
			{
				this.dictionary.getWord(this.loader.getWords()[num])
			});
			return this.loadNGramBuffer(wordSequence);
		}

		[LineNumberTable(new byte[]
		{
			162,
			186,
			103,
			123,
			123,
			135
		})]
		
		private NGramBuffer loadTrigramBuffer(int num, int num2)
		{
			WordSequence wordSequence = new WordSequence(new Word[]
			{
				this.dictionary.getWord(this.loader.getWords()[num]),
				this.dictionary.getWord(this.loader.getWords()[num2])
			});
			return this.loadNGramBuffer(wordSequence);
		}

		[LineNumberTable(new byte[]
		{
			163,
			213,
			104,
			104
		})]
		
		private float getBigramProb(int num, int num2)
		{
			NGramBuffer bigramBuffer = this.getBigramBuffer(num);
			NGramProbability ngramProbability = bigramBuffer.findNGram(num2);
			return this.ngramProbTable[1][ngramProbability.getProbabilityID()];
		}

		[LineNumberTable(new byte[]
		{
			163,
			185,
			105,
			121
		})]
		
		private void putSmearTerm(int num, int num2, float num3)
		{
			long num4 = (long)num << 32 | (long)num2;
			this.bigramSmearMap.put(Long.valueOf(num4), Float.valueOf(num3));
		}

		[LineNumberTable(new byte[]
		{
			159,
			107,
			73,
			104,
			118,
			103,
			103,
			103,
			104,
			103,
			104,
			107,
			104,
			103,
			105,
			106,
			105,
			103
		})]
		
		public LargeNGramModel(string format, URL location, string ngramLogFile, int maxNGramCacheSize, bool clearCacheAfterUtterance, int maxDepth, Dictionary dictionary, bool applyLanguageWeightAndWip, float languageWeight, double wip, float unigramWeight, bool fullSmear)
		{
			this.logger = Logger.getLogger(Object.instancehelper_getClass(this).getName());
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

		[LineNumberTable(new byte[]
		{
			107,
			134
		})]
		
		public LargeNGramModel()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			118,
			108,
			113,
			113,
			113,
			103,
			37,
			138,
			113,
			118,
			103,
			37,
			138,
			113,
			114,
			113,
			118
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.location = ConfigurationManagerUtils.getResource("location", ps);
			this.ngramLogFile = ps.getString("queryLogFile");
			this.ngramCacheSize = ps.getInt("ngramCacheSize");
			this.clearCacheAfterUtterance = ps.getBoolean("clearCachesAfterUtterance").booleanValue();
			this.maxDepth = ps.getInt("maxDepth");
			this.dictionary = (Dictionary)ps.getComponent("dictionary");
			this.applyLanguageWeightAndWip = ps.getBoolean("applyLanguageWeightAndWip").booleanValue();
			this.languageWeight = ps.getFloat("languageWeight");
			this.wip = ps.getDouble("wordInsertionProbability");
			this.unigramWeight = ps.getFloat("unigramWeight");
			this.fullSmear = ps.getBoolean("fullSmear").booleanValue();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			77,
			144,
			191,
			11,
			104,
			155,
			115,
			148,
			255,
			57,
			71,
			229,
			60,
			97,
			191,
			36,
			130,
			255,
			21,
			69,
			107,
			113,
			118,
			118,
			118,
			150,
			115,
			143,
			100,
			150,
			100,
			118,
			246,
			56,
			233,
			76,
			113,
			104,
			142,
			102,
			146,
			124,
			145,
			112,
			127,
			8,
			47,
			37,
			198,
			107,
			143,
			111,
			107,
			255,
			2,
			72,
			226,
			57,
			97,
			127,
			5,
			111,
			102,
			143,
			207,
			113
		})]
		
		public virtual void allocate()
		{
			TimerPool.getTimer(this, "Load LM").start();
			this.logger.info(new StringBuilder().append("Loading n-gram language model from: ").append(this.location).toString());
			if (this.ngramLogFile != null)
			{
				FileOutputStream.__<clinit>();
				this.logFile = new PrintWriter(new FileOutputStream(this.ngramLogFile));
			}
			if (this.location.getProtocol() != null)
			{
				if (!java.lang.String.instancehelper_equals(this.location.getProtocol(), "file"))
				{
					BinaryStreamLoader.__<clinit>();
					this.loader = new BinaryStreamLoader(this.location, this.format, this.applyLanguageWeightAndWip, this.languageWeight, this.wip, this.unigramWeight);
					goto IL_15B;
				}
			}
			try
			{
				BinaryLoader.__<clinit>();
				File.__<clinit>();
				this.loader = new BinaryLoader(new File(this.location.toURI()), this.format, this.applyLanguageWeightAndWip, this.languageWeight, this.wip, this.unigramWeight);
			}
			catch (Exception ex)
			{
				if (ByteCodeHelper.MapException<Exception>(ex, 2) == null)
				{
					throw;
				}
				goto IL_D9;
			}
			goto IL_15B;
			IL_D9:
			BinaryLoader.__<clinit>();
			File.__<clinit>();
			this.loader = new BinaryLoader(new File(this.location.getPath()), this.format, this.applyLanguageWeightAndWip, this.languageWeight, this.wip, this.unigramWeight);
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
				IOException ex3;
				try
				{
					java.lang.System.@out.println("... Reading ...");
					this.readSmearInfo("smear.dat");
					java.lang.System.@out.println("... Done ");
				}
				catch (IOException ex2)
				{
					ex3 = ByteCodeHelper.MapException<IOException>(ex2, 1);
					goto IL_353;
				}
				goto IL_3B0;
				IL_353:
				IOException ex4 = ex3;
				java.lang.System.@out.println(new StringBuilder().append("... ").append(ex4).toString());
				java.lang.System.@out.println("... Calculating");
				this.buildSmearInfo();
				java.lang.System.@out.println("... Writing");
				java.lang.System.@out.println("... Done");
			}
			IL_3B0:
			TimerPool.getTimer(this, "Load LM").stop();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			160,
			107
		})]
		
		public virtual void deallocate()
		{
			this.loader.deallocate();
		}

		[LineNumberTable(new byte[]
		{
			160,
			203,
			134,
			104,
			112,
			139
		})]
		
		public virtual void onUtteranceEnd()
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
			Word.__<clinit>();
			return map.get(new Word(w.toString(), null, false)) != null;
		}

		[LineNumberTable(new byte[]
		{
			162,
			49,
			134,
			104,
			135,
			100,
			112,
			201,
			122,
			159,
			6
		})]
		
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

		[LineNumberTable(new byte[]
		{
			162,
			70,
			134,
			107,
			110,
			135,
			100,
			110,
			105,
			108,
			103,
			112,
			113,
			139,
			100,
			140,
			104,
			206,
			119,
			223,
			31,
			122,
			159,
			6
		})]
		
		public virtual float getSmear(WordSequence wordSequence)
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

		public virtual int getMaxDepth()
		{
			return this.maxDepth;
		}

		
		[LineNumberTable(new byte[]
		{
			162,
			134,
			107,
			37,
			139
		})]
		
		public virtual Set getVocabulary()
		{
			HashSet.__<clinit>();
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

		[LineNumberTable(new byte[]
		{
			163,
			71,
			127,
			32,
			101,
			159,
			37,
			191,
			36,
			127,
			30,
			159,
			37,
			127,
			22
		})]
		
		private void dumpProbs(double[] array, double[] array2, int num, int num2, float num3, float num4, double num5, double num6, double num7, double num8)
		{
			java.lang.System.@out.println(new StringBuilder().append("ubo ").append(num5).append(' ').append(num6).append(' ').append(num7).toString());
			java.lang.System.@out.println(new StringBuilder().append("logubo ").append(num3).append(' ').append(num4).append(' ').append(num8).toString());
			java.lang.System.@out.println(new StringBuilder().append("n/d ").append(num2).append(' ').append(array[num]).append(' ').append(array2[num]).toString());
			java.lang.System.@out.print(new StringBuilder().append(num5).append(" ").append(num6).append(' ').append(num7).toString());
			java.lang.System.@out.print(new StringBuilder().append(" ").append(num3).append(' ').append(num4).append(' ').append(num8).toString());
			java.lang.System.@out.println(new StringBuilder().append("  ").append(array[num]).append(' ').append(array2[num]).toString());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			163,
			94,
			140,
			107,
			127,
			11,
			141,
			108,
			46,
			166,
			111,
			127,
			27,
			136,
			99,
			103,
			162,
			140,
			107,
			105,
			107,
			104,
			237,
			60,
			230,
			53,
			233,
			83,
			102
		})]
		
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

		protected internal Dictionary dictionary;

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

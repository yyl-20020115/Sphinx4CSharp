﻿using edu.cmu.sphinx.util;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util.regex;

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	public class BinaryLoader : Object
	{		
		public BinaryLoader(string format, bool applyLanguageWeightAndWip, float languageWeight, double wip, float unigramWeight)
		{
			this.bigEndian = true;
			this.startWordID = -1;
			this.endWordID = -1;
			this.applyLanguageWeightAndWip = applyLanguageWeightAndWip;
			this.logMath = LogMath.getLogMath();
			this.languageWeight = languageWeight;
			this.wip = wip;
			this.unigramWeight = unigramWeight;
		}
		
		protected internal virtual void loadModelLayout(InputStream inputStream)
		{
			DataInputStream dataInputStream = new DataInputStream(new BufferedInputStream(inputStream));
			this.readHeader(dataInputStream);
			this.unigrams = this.readUnigrams(dataInputStream, this.numberNGrams[0] + 1, this.bigEndian);
			this.skipNGrams(dataInputStream);
			int i;
			for (i = 1; i < this.maxNGram; i++)
			{
				if (this.numberNGrams[i] > 0)
				{
					if (i == 1)
					{
						this.NGramProbTable[i] = this.readFloatTable(dataInputStream, this.bigEndian);
					}
					else
					{
						this.NGramBackoffTable[i] = this.readFloatTable(dataInputStream, this.bigEndian);
						this.NGramProbTable[i] = this.readFloatTable(dataInputStream, this.bigEndian);
						int j = 1 << this.logNGramSegmentSize;
						int num = this.numberNGrams[i - 1] + 1;
						int num2 = j;
						int num3 = ((num2 != -1) ? (num / num2) : (-num)) + 1;
						this.NGramSegmentTable[i] = this.readIntTable(dataInputStream, this.bigEndian, num3);
					}
				}
			}
			i = this.readInt(dataInputStream, this.bigEndian);
			if (i <= 0)
			{
				string text = new StringBuilder().append("Bad word string size: ").append(i).toString();
				
				throw new Error(text);
			}
			this.words = this.readWords(dataInputStream, i, this.numberNGrams[0]);
			if (this.startWordID > -1)
			{
				UnigramProbability unigramProbability = this.unigrams[this.startWordID];
				unigramProbability.setLogProbability(-99f);
			}
			if (this.endWordID > -1)
			{
				UnigramProbability unigramProbability = this.unigrams[this.endWordID];
				unigramProbability.setLogBackoff(-99f);
			}
			this.applyUnigramWeight();
			if (this.applyLanguageWeightAndWip)
			{
				for (int j = 0; j <= this.maxNGram; j++)
				{
					this.applyLanguageWeight(this.NGramProbTable[j], this.languageWeight);
					this.applyWip(this.NGramProbTable[j], this.wip);
					if (j > 1)
					{
						this.applyLanguageWeight(this.NGramBackoffTable[j], this.languageWeight);
					}
				}
			}
			dataInputStream.close();
		}
		
		public virtual int getNumberNGrams(int n)
		{
			if (!BinaryLoader.assertionsDisabled && !(n <= this.maxNGram & n > 0))
			{
				
				throw new AssertionError();
			}
			return this.numberNGrams[n - 1];
		}
		
		public virtual float[] getNGramProbabilities(int n)
		{
			if (!BinaryLoader.assertionsDisabled && (n > this.maxNGram || n <= 1))
			{
				
				throw new AssertionError();
			}
			return this.NGramProbTable[n - 1];
		}
		
		public virtual float[] getNGramBackoffWeights(int n)
		{
			if (!BinaryLoader.assertionsDisabled && !(n <= this.maxNGram & n > 2))
			{
				
				throw new AssertionError();
			}
			return this.NGramBackoffTable[n - 1];
		}
		
		public virtual int[] getNGramSegments(int n)
		{
			if (!BinaryLoader.assertionsDisabled && !(n <= this.maxNGram & n > 2))
			{
				
				throw new AssertionError();
			}
			return this.NGramSegmentTable[n - 1];
		}
		
		public virtual long getNGramOffset(int n)
		{
			if (!BinaryLoader.assertionsDisabled && !(n <= this.maxNGram & n > 1))
			{
				
				throw new AssertionError();
			}
			return this.NGramOffset[n - 1];
		}
		
		private void readHeader(DataInputStream dataInputStream)
		{
			int num = this.readInt(dataInputStream, this.bigEndian);
			if (num != String.instancehelper_length("Darpa Trigram LM") + 1 && num != String.instancehelper_length("Darpa Quadrigram LM") + 1 && num != String.instancehelper_length("Darpa \\d-gram LM") - 1)
			{
				num = Utilities.swapInteger(num);
				if (num != String.instancehelper_length("Darpa Trigram LM") + 1 && num != String.instancehelper_length("Darpa Quadrigram LM") + 1 && num != String.instancehelper_length("Darpa \\d-gram LM") - 1)
				{
					string text = new StringBuilder().append("Bad binary LM file magic number: ").append(num).append(", not an LM dumpfile?").toString();
					
					throw new Error(text);
				}
				this.bigEndian = false;
			}
			string text2 = this.readString(dataInputStream, num - 1);
			sbyte b = (sbyte)dataInputStream.readByte();
			this.bytesRead += 1L;
			CharSequence charSequence;
			if (!String.instancehelper_equals(text2, "Darpa Trigram LM") && !String.instancehelper_equals(text2, "Darpa Quadrigram LM"))
			{
				string text3 = "Darpa \\d-gram LM";
				object _ref = text2;
				charSequence = CharSequence.Cast(_ref);
				if (!Pattern.matches(text3, charSequence))
				{
					string text4 = new StringBuilder().append("Bad binary LM file header: ").append(text2).toString();
					
					throw new Error(text4);
				}
			}
			if (String.instancehelper_equals(text2, "Darpa Trigram LM"))
			{
				this.maxNGram = 3;
			}
			else if (String.instancehelper_equals(text2, "Darpa Quadrigram LM"))
			{
				this.maxNGram = 4;
			}
			else
			{
				Pattern pattern = Pattern.compile("\\d");
				Pattern pattern2 = pattern;
				object _ref = text2;
				charSequence = CharSequence.Cast(_ref);
				Matcher matcher = pattern2.matcher(charSequence);
				this.maxNGram = Integer.parseInt(matcher.group());
			}
			int num2 = this.readInt(dataInputStream, this.bigEndian);
			this.skipStreamBytes(dataInputStream, (long)num2);
			this.numberNGrams = new int[this.maxNGram];
			this.NGramOffset = new long[this.maxNGram];
			this.NGramProbTable = new float[this.maxNGram][];
			this.NGramBackoffTable = new float[this.maxNGram][];
			this.NGramSegmentTable = new int[this.maxNGram][];
			this.numberNGrams[0] = 0;
			this.logNGramSegmentSize = 9;
			int num3 = this.readInt(dataInputStream, this.bigEndian);
			this.bytesPerField = 2;
			if (num3 <= 0)
			{
				this.readInt(dataInputStream, this.bigEndian);
				if (num3 <= -3)
				{
					this.bytesPerField = 4;
				}
				int i;
				while ((i = this.readInt(dataInputStream, this.bigEndian)) != 0)
				{
					this.bytesRead += (long)dataInputStream.skipBytes(i);
				}
				if (num3 == -2)
				{
					this.logNGramSegmentSize = this.readInt(dataInputStream, this.bigEndian);
					if (this.logNGramSegmentSize < 1 || this.logNGramSegmentSize > 15)
					{
						string text5 = "log2(bg_seg_sz) outside range 1..15";
						
						throw new Error(text5);
					}
				}
				this.numberNGrams[0] = this.readInt(dataInputStream, this.bigEndian);
			}
			else
			{
				this.numberNGrams[0] = num3;
			}
			if (this.numberNGrams[0] <= 0)
			{
				string text6 = new StringBuilder().append("Bad number of unigrams: ").append(this.numberNGrams[0]).append(", must be > 0.").toString();
				
				throw new Error(text6);
			}
			for (int i = 1; i < this.maxNGram; i++)
			{
				int[] array = this.numberNGrams;
				int num4 = i;
				int num5 = this.readInt(dataInputStream, this.bigEndian);
				int num6 = num4;
				int[] array2 = array;
				int num7 = num5;
				array2[num6] = num5;
				if (num7 < 0)
				{
					string text7 = new StringBuilder().append("Bad number of ").append(String.valueOf(i)).append("-grams: ").append(this.numberNGrams[i]).toString();
					
					throw new Error(text7);
				}
			}
		}
		
		private UnigramProbability[] readUnigrams(DataInputStream dataInputStream, int num, bool flag)
		{
			UnigramProbability[] array = new UnigramProbability[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = this.readInt(dataInputStream, flag);
				if (num2 < 1)
				{
					num2 = i;
				}
				if (i != num - 1 && !BinaryLoader.assertionsDisabled && num2 != i)
				{
					
					throw new AssertionError();
				}
				float logSource = this.readFloat(dataInputStream, flag);
				float logSource2 = this.readFloat(dataInputStream, flag);
				int num3 = this.readInt(dataInputStream, flag);
				float num4 = this.logMath.log10ToLog(logSource);
				float num5 = this.logMath.log10ToLog(logSource2);
				array[i] = new UnigramProbability(num2, num4, num5, num3);
			}
			return array;
		}
		
		private void skipNGrams(DataInputStream dataInputStream)
		{
			this.NGramOffset[1] = this.bytesRead;
			long num = (long)((this.numberNGrams[1] + 1) * 4 * this.getBytesPerField());
			this.skipStreamBytes(dataInputStream, num);
			for (int i = 2; i < this.maxNGram; i++)
			{
				if (this.numberNGrams[i] > 0 && i < this.maxNGram - 1)
				{
					this.NGramOffset[i] = this.bytesRead;
					num = (long)(this.numberNGrams[i] + 1) * 4L * (long)this.getBytesPerField();
					this.skipStreamBytes(dataInputStream, num);
				}
				else if (this.numberNGrams[i] > 0 && i == this.maxNGram - 1)
				{
					this.NGramOffset[i] = this.bytesRead;
					num = (long)this.numberNGrams[i] * 2L * (long)this.getBytesPerField();
					this.skipStreamBytes(dataInputStream, num);
				}
			}
		}
		
		private float[] readFloatTable(DataInputStream dataInputStream, bool flag)
		{
			int num = this.readInt(dataInputStream, flag);
			if (num <= 0 || num > 2147483647)
			{
				string text = new StringBuilder().append("Bad probabilities table size: ").append(num).toString();
				
				throw new Error(text);
			}
			float[] array = new float[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = this.logMath.log10ToLog(this.readFloat(dataInputStream, flag));
			}
			return array;
		}
		
		private int[] readIntTable(DataInputStream dataInputStream, bool flag, int num)
		{
			int num2 = this.readInt(dataInputStream, flag);
			if (num2 != num)
			{
				string text = new StringBuilder().append("Bad NGram seg table size: ").append(num2).toString();
				
				throw new Error(text);
			}
			int[] array = new int[num2];
			for (int i = 0; i < num2; i++)
			{
				array[i] = this.readInt(dataInputStream, flag);
			}
			return array;
		}
		
		private int readInt(DataInputStream dataInputStream, bool flag)
		{
			this.bytesRead += 4L;
			if (flag)
			{
				return dataInputStream.readInt();
			}
			return Utilities.readLittleEndianInt(dataInputStream);
		}
		
		private string[] readWords(DataInputStream dataInputStream, int num, int num2)
		{
			string[] array = new string[num2];
			byte[] array2 = new byte[num];
			this.bytesRead += (long)dataInputStream.read(array2);
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < num; i++)
			{
				int num5 = (int)((ushort)array2[i]);
				this.bytesRead += 1L;
				if (num5 == 0)
				{
					array[num3] = String.newhelper(array2, num4, i - num4);
					num4 = i + 1;
					if (String.instancehelper_equals(array[num3], "<s>"))
					{
						this.startWordID = num3;
					}
					else if (String.instancehelper_equals(array[num3], "</s>"))
					{
						this.endWordID = num3;
					}
					num3++;
				}
			}
			if (!BinaryLoader.assertionsDisabled && num3 != num2)
			{
				
				throw new AssertionError();
			}
			return array;
		}
		
		private void applyUnigramWeight()
		{
			float num = this.logMath.linearToLog((double)this.unigramWeight);
			float num2 = this.logMath.linearToLog((double)(1f - this.unigramWeight));
			float num3 = this.logMath.linearToLog((double)(1f / (float)this.numberNGrams[0]));
			float num4 = this.logMath.linearToLog(this.wip);
			float logVal = num3 + num2;
			for (int i = 0; i < this.numberNGrams[0]; i++)
			{
				UnigramProbability unigramProbability = this.unigrams[i];
				float num5 = unigramProbability.getLogProbability();
				if (i != this.startWordID)
				{
					num5 += num;
					num5 = this.logMath.addAsLinear(num5, logVal);
				}
				if (this.applyLanguageWeightAndWip)
				{
					num5 = num5 * this.languageWeight + num4;
					unigramProbability.setLogBackoff(unigramProbability.getLogBackoff() * this.languageWeight);
				}
				unigramProbability.setLogProbability(num5);
			}
		}

		private void applyLanguageWeight(float[] array, float num)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] *= num;
			}
		}
		
		private void applyWip(float[] array, double num)
		{
			float num2 = this.logMath.linearToLog(num);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] += num2;
			}
		}
		
		private string readString(DataInputStream dataInputStream, int num)
		{
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = new byte[num];
			this.bytesRead += (long)dataInputStream.read(array);
			for (int i = 0; i < num; i++)
			{
				stringBuilder.append((char)array[i]);
			}
			return stringBuilder.toString();
		}
		
		private void skipStreamBytes(DataInputStream dataInputStream, long num)
		{
			while (num > 0L)
			{
				long num2 = dataInputStream.skip(num);
				this.bytesRead += num2;
				num -= num2;
			}
		}
		public virtual int getBytesPerField()
		{
			return this.bytesPerField;
		}
		
		private float readFloat(DataInputStream dataInputStream, bool flag)
		{
			this.bytesRead += 4L;
			if (flag)
			{
				return dataInputStream.readFloat();
			}
			return Utilities.readLittleEndianFloat(dataInputStream);
		}
		
		public BinaryLoader(File location, string format, bool applyLanguageWeightAndWip, float languageWeight, double wip, float unigramWeight) : this(format, applyLanguageWeightAndWip, languageWeight, wip, unigramWeight)
		{
			this.loadModelLayout(new FileInputStream(location));
			this.file = new RandomAccessFile(location, "r");
		}
		
		public virtual void deallocate()
		{
			if (null != this.file)
			{
				this.file.close();
			}
		}
		public virtual int getNumberUnigrams()
		{
			return this.getNumberNGrams(1);
		}

		public virtual int getNumberBigrams()
		{
			return this.getNumberNGrams(2);
		}

		public virtual int getNumberTrigrams()
		{
			return this.getNumberNGrams(3);
		}

		public virtual UnigramProbability[] getUnigrams()
		{
			return this.unigrams;
		}
		
		public virtual float[] getBigramProbabilities()
		{
			return this.getNGramProbabilities(2);
		}
		
		public virtual float[] getTrigramProbabilities()
		{
			return this.getNGramProbabilities(3);
		}
		
		public virtual float[] getTrigramBackoffWeights()
		{
			return this.getNGramBackoffWeights(3);
		}		
		
		public virtual int[] getTrigramSegments()
		{
			return this.getNGramSegments(3);
		}

		public virtual int getLogBigramSegmentSize()
		{
			return this.logNGramSegmentSize;
		}

		public virtual int getLogNGramSegmentSize()
		{
			return this.logNGramSegmentSize;
		}

		public virtual string[] getWords()
		{
			return this.words;
		}
		
		public virtual long getBigramOffset()
		{
			return this.getNGramOffset(2);
		}
		
		public virtual long getTrigramOffset()
		{
			return this.getNGramOffset(3);
		}

		public virtual int getMaxDepth()
		{
			return this.maxNGram;
		}

		public virtual bool getBigEndian()
		{
			return this.bigEndian;
		}
		
		public virtual byte[] loadBuffer(long position, int size)
		{
			this.file.seek(position);
			byte[] array = new byte[size];
			if (this.file.read(array) != size)
			{
				string text = new StringBuilder().append("Incorrect number of bytes read. Size = ").append(size).append(". Position =").append(position).append(".").toString();
				
				throw new IOException(text);
			}
			return array;
		}

		private const string DARPA_TG_HEADER = "Darpa Trigram LM";

		private const string DARPA_QG_HEADER = "Darpa Quadrigram LM";

		private const string DARPA_NG_HEADER = "Darpa \\d-gram LM";

		private const int LOG2_NGRAM_SEGMENT_SIZE = 9;

		private const float MIN_PROBABILITY = -99f;

		private const int MAX_PROB_TABLE_SIZE = 2147483647;

		private LogMath logMath;

		private int maxNGram;

		private float unigramWeight;

		private float languageWeight;

		private double wip;

		private bool bigEndian;

		private bool applyLanguageWeightAndWip;

		private long bytesRead;

		private UnigramProbability[] unigrams;

		private string[] words;

		private long[] NGramOffset;

		private int[] numberNGrams;

		private int logNGramSegmentSize;

		private int startWordID;

		private int endWordID;

		private int[][] NGramSegmentTable;

		private float[][] NGramProbTable;

		private float[][] NGramBackoffTable;

		private RandomAccessFile file;

		private int bytesPerField;

		internal static bool assertionsDisabled = !ClassLiteral<BinaryLoader>.Value.desiredAssertionStatus();
	}
}

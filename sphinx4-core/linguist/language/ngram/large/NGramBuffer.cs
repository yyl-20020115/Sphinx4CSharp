using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	internal class NGramBuffer : java.lang.Object
	{
		public virtual bool getUsed()
		{
			return this.used;
		}

		public virtual void setUsed(bool flag)
		{
			this.used = flag;
		}

		[LineNumberTable(new byte[]
		{
			160,
			154,
			107,
			130,
			102,
			102,
			105,
			101,
			102,
			101,
			132,
			104,
			130,
			130
		})]
		
		public virtual NGramProbability findNGram(int num)
		{
			int num2 = 0;
			int num3 = this.getNumberNGrams() - 1;
			NGramProbability result = null;
			while (num3 - num2 > 0)
			{
				int num4 = (num2 + num3) / 2;
				int wordID = this.getWordID(num4);
				if (wordID < num)
				{
					num2 = num4 + 1;
				}
				else
				{
					if (wordID <= num)
					{
						result = this.getNGramProbability(num4);
						break;
					}
					num3 = num4;
				}
			}
			return result;
		}

		[LineNumberTable(new byte[]
		{
			160,
			181,
			141,
			102,
			102,
			104,
			100,
			102,
			100,
			194,
			130
		})]
		
		public virtual int findNGramIndex(int num)
		{
			int num2 = -1;
			int num3 = 0;
			int num4 = this.getNumberNGrams() - 1;
			while (num4 - num3 > 0)
			{
				num2 = (num3 + num4) / 2;
				int wordID = this.getWordID(num2);
				if (wordID < num)
				{
					num3 = num2 + 1;
				}
				else
				{
					if (wordID <= num)
					{
						break;
					}
					num4 = num2;
				}
			}
			return num2;
		}

		public virtual int getFirstNGramEntry()
		{
			return this.firstNGramEntry;
		}

		[LineNumberTable(new byte[]
		{
			160,
			206,
			138,
			145,
			135,
			103,
			103,
			103,
			136
		})]
		
		public virtual NGramProbability getNGramProbability(int num)
		{
			int num2 = num * 4 * ((!this.is32bits) ? 2 : 4);
			this.setPosition(num2);
			int num3 = this.readBytesAsInt();
			int num4 = this.readBytesAsInt();
			int num5 = this.readBytesAsInt();
			int num6 = this.readBytesAsInt();
			return new NGramProbability(num, num3, num4, num5, num6);
		}

		[LineNumberTable(new byte[]
		{
			159,
			131,
			101,
			104,
			103,
			103,
			103,
			103,
			103,
			104,
			104
		})]
		
		public NGramBuffer(byte[] array, int num, bool flag, bool flag2, int num2, int num3)
		{
			this.buffer = array;
			this.numberNGrams = num;
			this.bigEndian = flag;
			this.is32bits = flag2;
			this.position = 0;
			this.n = num2;
			this.firstNGramEntry = num3;
		}

		public virtual int getNumberNGrams()
		{
			return this.numberNGrams;
		}

		[LineNumberTable(new byte[]
		{
			77,
			121,
			103
		})]
		
		public int getWordID(int num)
		{
			int num2 = this.buffer.Length;
			int num3 = this.numberNGrams;
			int num4 = num * ((num3 != -1) ? (num2 / num3) : (-num2));
			this.setPosition(num4);
			return this.readBytesAsInt();
		}

		[LineNumberTable(new byte[]
		{
			160,
			137,
			130,
			113,
			148
		})]
		
		public virtual int getProbabilityID(int num)
		{
			int num2 = num * 4 * ((!this.is32bits) ? 2 : 4);
			this.setPosition(num2 + ((!this.is32bits) ? 2 : 4));
			return this.readBytesAsInt();
		}

		protected internal virtual void setPosition(int num)
		{
			this.position = num;
		}

		[LineNumberTable(new byte[]
		{
			108,
			107,
			107,
			127,
			2,
			100,
			127,
			4,
			100,
			127,
			4,
			100,
			127,
			4,
			130,
			118,
			100,
			120,
			100,
			120,
			100,
			118,
			110,
			194,
			104,
			127,
			2,
			100,
			127,
			4,
			130,
			118,
			100,
			118,
			110
		})]
		public int readBytesAsInt()
		{
			if (this.is32bits)
			{
				int num4;
				if (this.bigEndian)
				{
					int num = 255;
					byte[] array = this.buffer;
					int num2 = this.position;
					int num3 = num2;
					this.position = num2 + 1;
					num4 = (num & array[num3]);
					num4 <<= 8;
					int num5 = num4;
					int num6 = 255;
					byte[] array2 = this.buffer;
					num2 = this.position;
					int num7 = num2;
					this.position = num2 + 1;
					num4 = (num5 | (num6 & array2[num7]));
					num4 <<= 8;
					int num8 = num4;
					int num9 = 255;
					byte[] array3 = this.buffer;
					num2 = this.position;
					int num10 = num2;
					this.position = num2 + 1;
					num4 = (num8 | (num9 & array3[num10]));
					num4 <<= 8;
					int num11 = num4;
					int num12 = 255;
					byte[] array4 = this.buffer;
					num2 = this.position;
					int num13 = num2;
					this.position = num2 + 1;
					return num11 | (num12 & array4[num13]);
				}
				num4 = (int)(byte.MaxValue & this.buffer[this.position + 3]);
				num4 <<= 8;
				num4 |= (int)(byte.MaxValue & this.buffer[this.position + 2]);
				num4 <<= 8;
				num4 |= (int)(byte.MaxValue & this.buffer[this.position + 1]);
				num4 <<= 8;
				num4 |= (int)(byte.MaxValue & this.buffer[this.position]);
				this.position += 4;
				return num4;
			}
			else
			{
				int num4;
				if (this.bigEndian)
				{
					int num14 = 255;
					byte[] array5 = this.buffer;
					int num2 = this.position;
					int num15 = num2;
					this.position = num2 + 1;
					num4 = (num14 & array5[num15]);
					num4 <<= 8;
					int num16 = num4;
					int num17 = 255;
					byte[] array6 = this.buffer;
					num2 = this.position;
					int num18 = num2;
					this.position = num2 + 1;
					return num16 | (num17 & array6[num18]);
				}
				num4 = (int)(byte.MaxValue & this.buffer[this.position + 1]);
				num4 <<= 8;
				num4 |= (int)(byte.MaxValue & this.buffer[this.position]);
				this.position += 2;
				return num4;
			}
		}

		public virtual byte[] getBuffer()
		{
			return this.buffer;
		}

		
		public virtual int getSize()
		{
			return this.buffer.Length;
		}

		protected internal virtual int getPosition()
		{
			return this.position;
		}

		protected internal virtual int getN()
		{
			return this.n;
		}

		public bool isBigEndian()
		{
			return this.bigEndian;
		}

		public bool is32bits()
		{
			return this.is32bits;
		}

		[LineNumberTable(new byte[]
		{
			160,
			110,
			137,
			130,
			102,
			102,
			105,
			101,
			102,
			101,
			132,
			104,
			130,
			98
		})]
		
		public virtual int findProbabilityID(int num)
		{
			int num2 = 0;
			int num3 = this.getNumberNGrams();
			int result = -1;
			while (num3 - num2 > 0)
			{
				int num4 = (num2 + num3) / 2;
				int wordID = this.getWordID(num4);
				if (wordID < num)
				{
					num2 = num4 + 1;
				}
				else
				{
					if (wordID <= num)
					{
						result = this.getProbabilityID(num4);
						break;
					}
					num3 = num4;
				}
			}
			return result;
		}

		
		private byte[] buffer;

		
		private int numberNGrams;

		private int position;

		
		private bool bigEndian;

		
		private bool is32bits;

		
		private int n;

		private bool used;

		private int firstNGramEntry;
	}
}

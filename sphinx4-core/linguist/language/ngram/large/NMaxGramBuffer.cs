using System;

using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	
	internal sealed class NMaxGramBuffer : NGramBuffer
	{
		[LineNumberTable(new byte[]
		{
			159,
			133,
			133,
			110
		})]
		
		public NMaxGramBuffer(byte[] array, int num, bool flag, bool flag2, int num2, int num3) : base(array, num, flag, flag2, num2, num3)
		{
		}

		[LineNumberTable(new byte[]
		{
			46,
			138,
			145,
			135,
			103,
			136
		})]
		
		public override NGramProbability getNGramProbability(int num)
		{
			int num2 = 0;
			int num3 = 0;
			int position = num * 2 * ((!base.is32bits()) ? 2 : 4);
			this.setPosition(position);
			int num4 = base.readBytesAsInt();
			int num5 = base.readBytesAsInt();
			return new NGramProbability(num, num4, num5, num2, num3);
		}

		[LineNumberTable(new byte[]
		{
			0,
			130,
			113,
			148
		})]
		
		public override int getProbabilityID(int num)
		{
			int num2 = num * 2 * ((!base.is32bits()) ? 2 : 4);
			this.setPosition(num2 + ((!base.is32bits()) ? 2 : 4));
			return base.readBytesAsInt();
		}

		[LineNumberTable(new byte[]
		{
			18,
			105,
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
		
		public override NGramProbability findNGram(int num)
		{
			int num2 = 0;
			int num3 = this.getNumberNGrams();
			NGramProbability result = null;
			while (num3 - num2 > 0)
			{
				int num4 = (num2 + num3) / 2;
				int wordID = base.getWordID(num4);
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
	}
}

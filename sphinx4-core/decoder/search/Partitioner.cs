using System;

using edu.cmu.sphinx.decoder.scorer;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	public class Partitioner : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			165,
			168
		})]
		
		public Partitioner()
		{
			this.MAX_DEPTH = 50;
		}

		[LineNumberTable(new byte[]
		{
			45,
			101,
			142
		})]
		
		public virtual int partition(Token[] tokens, int size, int n)
		{
			if (tokens.Length > n)
			{
				return this.midPointSelect(tokens, 0, size - 1, n, 0);
			}
			return this.findBest(tokens, size);
		}

		[LineNumberTable(new byte[]
		{
			85,
			100
		})]
		private void setToken(Token[] array, int num, Token token)
		{
			array[num] = token;
		}

		[LineNumberTable(new byte[]
		{
			159,
			182,
			100,
			135,
			98,
			196,
			111,
			102,
			111,
			134,
			100,
			130,
			101,
			107,
			106,
			133,
			107,
			105
		})]
		
		private int endPointPartition(Token[] array, int num, int num2)
		{
			Token token = array[num2];
			float score = token.getScore();
			int num3 = num;
			int num4 = num2 - 1;
			for (;;)
			{
				if (num3 < num2 && array[num3].getScore() >= score)
				{
					num3++;
				}
				else
				{
					while (num4 > num3 && array[num4].getScore() < score)
					{
						num4 += -1;
					}
					if (num4 <= num3)
					{
						break;
					}
					Token token2 = array[num4];
					this.setToken(array, num4, array[num3]);
					this.setToken(array, num3, token2);
				}
			}
			this.setToken(array, num2, array[num3]);
			this.setToken(array, num3, token);
			return num3;
		}

		[LineNumberTable(new byte[]
		{
			99,
			102,
			140,
			100,
			130,
			106,
			102,
			101,
			98,
			101,
			146
		})]
		
		private int midPointSelect(Token[] array, int num, int num2, int num3, int num4)
		{
			if (num4 > 50)
			{
				return this.simplePointSelect(array, num, num2, num3);
			}
			if (num == num2)
			{
				return num;
			}
			int num5 = this.midPointPartition(array, num, num2);
			int num6 = num5 - num + 1;
			if (num3 == num6)
			{
				return num5;
			}
			if (num3 < num6)
			{
				return this.midPointSelect(array, num, num5 - 1, num3, num4 + 1);
			}
			return this.midPointSelect(array, num5 + 1, num2, num3 - num6, num4 + 1);
		}

		[LineNumberTable(new byte[]
		{
			60,
			98,
			102,
			103,
			105,
			100,
			98,
			226,
			60,
			230,
			74,
			100,
			100,
			101,
			107,
			202
		})]
		
		private int findBest(Token[] array, int num)
		{
			int num2 = -1;
			float num3 = float.MaxValue;
			int i;
			for (i = 0; i < array.Length; i++)
			{
				float score = array[i].getScore();
				if (score <= num3)
				{
					num3 = score;
					num2 = i;
				}
			}
			i = num - 1;
			if (i >= 0)
			{
				Token token = array[i];
				this.setToken(array, i, array[num2]);
				this.setToken(array, num2, token);
			}
			return i;
		}

		[LineNumberTable(new byte[]
		{
			126,
			111
		})]
		
		private int simplePointSelect(Token[] array, int num, int num2, int num3)
		{
			Arrays.sort(array, num, num2 + 1, Scoreable.COMPARATOR);
			return num + num3 - 1;
		}

		[LineNumberTable(new byte[]
		{
			27,
			102,
			100,
			107,
			105
		})]
		
		private int midPointPartition(Token[] array, int num, int num2)
		{
			int num3 = (int)((uint)(num + num2) >> 1);
			Token token = array[num2];
			this.setToken(array, num2, array[num3]);
			this.setToken(array, num3, token);
			return this.endPointPartition(array, num, num2);
		}

		
		private int MAX_DEPTH;
	}
}

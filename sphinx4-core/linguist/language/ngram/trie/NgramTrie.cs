using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.ngram.trie
{
	public class NgramTrie : java.lang.Object
	{
		
		
		
		internal static int access_000(NgramTrie ngramTrie, int num)
		{
			return ngramTrie.requiredBits(num);
		}

		
		
		internal static NgramTrieBitarr access$100(NgramTrie ngramTrie)
		{
			return ngramTrie.bitArr;
		}

		
		
		internal static int access$300(NgramTrie ngramTrie)
		{
			return ngramTrie.quantProbLen;
		}

		private int requiredBits(int num)
		{
			if (num == 0)
			{
				return 0;
			}
			int num2 = 1;
			while ((num >>= 1) != 0)
			{
				num2++;
			}
			return num2;
		}

		[LineNumberTable(new byte[]
		{
			85,
			109,
			108,
			127,
			7,
			104,
			100,
			103,
			105,
			100,
			103,
			137,
			130,
			101
		})]
		
		private int uniformFind(NgramTrie.NgramSet ngramSet, NgramTrieModel.TrieRange trieRange, int num)
		{
			NgramTrieModel.TrieRange trieRange2 = new NgramTrieModel.TrieRange(0, ngramSet.maxVocab);
			while (trieRange.getWidth() > 1)
			{
				int num2 = trieRange.begin + 1 + this.calculatePivot(num - trieRange2.begin, trieRange2.getWidth(), trieRange.getWidth() - 1);
				int num3 = ngramSet.readNgramWord(num2);
				if (num3 < num)
				{
					trieRange.begin = num2;
					trieRange2.begin = num3;
				}
				else
				{
					if (num3 <= num)
					{
						return num2;
					}
					trieRange.end = num2;
					trieRange2.end = num3;
				}
			}
			return -1;
		}

		[LineNumberTable(new byte[]
		{
			106,
			107,
			103
		})]
		private NgramTrie.NgramSet getNgram(int num)
		{
			if (num == this.ordersNum - 1)
			{
				return this.longest;
			}
			return this.middles[num];
		}

		[LineNumberTable(new byte[]
		{
			27,
			110,
			110,
			103,
			162,
			104,
			109
		})]
		
		private int findNgram(NgramTrie.NgramSet ngramSet, int num, NgramTrieModel.TrieRange trieRange)
		{
			trieRange.begin--;
			int num2;
			if ((num2 = this.uniformFind(ngramSet, trieRange, num)) < 0)
			{
				trieRange.setFound(false);
				return -1;
			}
			if (ngramSet is NgramTrie.MiddleNgramSet)
			{
				((NgramTrie.MiddleNgramSet)ngramSet).readNextRange(num2, trieRange);
			}
			return num2;
		}

		
		private int calculatePivot(int num, int num2, int num3)
		{
			long num4 = (long)num * (long)num3;
			long num5 = (long)(num2 + 1);
			return (int)((num5 != -1L) ? (num4 / num5) : (-(int)num4));
		}

		[LineNumberTable(new byte[]
		{
			159,
			161,
			104,
			98,
			106,
			108,
			107,
			135,
			168,
			112,
			230,
			70,
			112,
			103,
			229,
			48,
			233,
			82,
			108,
			103,
			103,
			111,
			107,
			99,
			107,
			103,
			9,
			232,
			69,
			107,
			63,
			7,
			168,
			114,
			111
		})]
		
		public NgramTrie(int[] counts, int quantProbBoLen, int quantProbLen)
		{
			int num = 0;
			int[] array = new int[counts.Length - 1];
			int num2;
			for (int i = 1; i <= counts.Length - 1; i++)
			{
				num2 = this.requiredBits(counts[0]);
				if (i == counts.Length - 1)
				{
					num2 += quantProbLen;
				}
				else
				{
					num2 += this.requiredBits(counts[i + 1]);
					num2 += quantProbBoLen;
				}
				int j = ((1 + counts[i]) * num2 + 7) / 8 + 8;
				array[i - 1] = j;
				num += j;
			}
			this.bitArr = new NgramTrieBitarr(num);
			this.quantProbLen = quantProbLen;
			this.quantProbBoLen = quantProbBoLen;
			this.middles = new NgramTrie.MiddleNgramSet[counts.Length - 2];
			int[] array2 = new int[counts.Length - 2];
			num2 = 0;
			for (int j = 0; j < counts.Length - 2; j++)
			{
				array2[j] = num2;
				num2 += array[j];
			}
			for (int j = counts.Length - 1; j >= 2; j += -1)
			{
				this.middles[j - 2] = new NgramTrie.MiddleNgramSet(this, array2[j - 2], quantProbBoLen, counts[j - 1], counts[0], counts[j]);
			}
			this.longest = new NgramTrie.LongestNgramSet(this, num2, quantProbLen, counts[0]);
			this.ordersNum = this.middles.Length + 1;
		}

		
		
		public virtual byte[] getMem()
		{
			return this.bitArr.getArr();
		}

		[LineNumberTable(new byte[]
		{
			50,
			104,
			110,
			102
		})]
		
		public virtual float readNgramBackoff(int wordId, int orderMinusTwo, NgramTrieModel.TrieRange range, NgramTrieQuant quant)
		{
			NgramTrie.NgramSet ngram = this.getNgram(orderMinusTwo);
			int num;
			if ((num = this.findNgram(ngram, wordId, range)) < 0)
			{
				return 0f;
			}
			return quant.readBackoff(this.bitArr, ngram.memPtr, ngram.getNgramWeightsOffset(num), orderMinusTwo);
		}

		[LineNumberTable(new byte[]
		{
			68,
			104,
			110,
			102
		})]
		
		public virtual float readNgramProb(int wordId, int orderMinusTwo, NgramTrieModel.TrieRange range, NgramTrieQuant quant)
		{
			NgramTrie.NgramSet ngram = this.getNgram(orderMinusTwo);
			int num;
			if ((num = this.findNgram(ngram, wordId, range)) < 0)
			{
				return 0f;
			}
			return quant.readProb(this.bitArr, ngram.memPtr, ngram.getNgramWeightsOffset(num), orderMinusTwo);
		}

		
		
		internal static int access$200(NgramTrie ngramTrie)
		{
			return ngramTrie.quantProbBoLen;
		}

		private NgramTrie.MiddleNgramSet[] middles;

		private NgramTrie.LongestNgramSet longest;

		private NgramTrieBitarr bitArr;

		private int ordersNum;

		private int quantProbBoLen;

		private int quantProbLen;

		
		[SourceFile("NgramTrie.java")]
		
		internal sealed class LongestNgramSet : NgramTrie.NgramSet
		{
			[LineNumberTable(new byte[]
			{
				160,
				123,
				103,
				107
			})]
			
			internal LongestNgramSet(NgramTrie ngramTrie, int num, int num2, int num3) : base(ngramTrie, num, num3, num2)
			{
			}

			
			
			internal override int getQuantBits()
			{
				return NgramTrie.access$300(this.this$0);
			}

			
			internal new NgramTrie this$0 = ngramTrie;
		}

		
		[SourceFile("NgramTrie.java")]
		
		internal sealed class MiddleNgramSet : NgramTrie.NgramSet
		{
			[LineNumberTable(new byte[]
			{
				160,
				97,
				103,
				118,
				117,
				116,
				112
			})]
			
			internal MiddleNgramSet(NgramTrie ngramTrie, int num, int num2, int num3, int num4, int num5) : base(ngramTrie, num, num4, num2 + NgramTrie.access_000(ngramTrie, num5))
			{
				this.nextMask = (1 << NgramTrie.access_000(ngramTrie, num5)) - 1;
				if (num3 + 1 >= 33554432 || num5 >= 33554432)
				{
					string text = "Sorry, current implementation doesn't support more than 33554432 n-grams of particular order";
					
					throw new Error(text);
				}
			}

			[LineNumberTable(new byte[]
			{
				160,
				105,
				105,
				105,
				105,
				127,
				4,
				105,
				127,
				4
			})]
			
			internal void readNextRange(int num, NgramTrieModel.TrieRange trieRange)
			{
				int num2 = num * this.totalBits;
				num2 += this.wordBits;
				num2 += this.getQuantBits();
				trieRange.begin = NgramTrie.access$100(this.this$0).readInt(this.memPtr, num2, this.nextMask);
				num2 += this.totalBits;
				trieRange.end = NgramTrie.access$100(this.this$0).readInt(this.memPtr, num2, this.nextMask);
			}

			
			
			internal override int getQuantBits()
			{
				return NgramTrie.access$200(this.this$0);
			}

			internal int nextMask;

			internal int nextOrderMemPtr;

			
			internal new NgramTrie this$0 = ngramTrie;
		}

		
		[SourceFile("NgramTrie.java")]
		internal abstract class NgramSet : java.lang.Object
		{
			[LineNumberTable(new byte[]
			{
				160,
				67,
				111,
				103,
				103,
				109,
				106,
				112,
				111,
				115,
				103
			})]
			
			internal NgramSet(NgramTrie ngramTrie, int num, int num2, int num3)
			{
				this.maxVocab = num2;
				this.memPtr = num;
				this.wordBits = NgramTrie.access_000(ngramTrie, num2);
				if (this.wordBits > 25)
				{
					string text = "Sorry, word indices more than33554432 are not implemented";
					
					throw new Error(text);
				}
				this.totalBits = this.wordBits + num3;
				this.wordMask = (1 << this.wordBits) - 1;
				this.insertIdx = 0;
			}

			[LineNumberTable(new byte[]
			{
				160,
				79,
				105
			})]
			
			internal virtual int readNgramWord(int num)
			{
				int bitOffset = num * this.totalBits;
				return NgramTrie.access$100(this.this$0).readInt(this.memPtr, bitOffset, this.wordMask);
			}

			internal virtual int getNgramWeightsOffset(int num)
			{
				return num * this.totalBits + this.wordBits;
			}

			internal abstract int getQuantBits();

			internal int memPtr;

			internal int wordBits;

			internal int wordMask;

			internal int totalBits;

			internal int insertIdx;

			internal int maxVocab;

			
			internal NgramTrie this$0 = ngramTrie;
		}
	}
}

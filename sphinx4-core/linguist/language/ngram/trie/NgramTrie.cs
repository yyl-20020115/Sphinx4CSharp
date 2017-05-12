using java.lang;

namespace edu.cmu.sphinx.linguist.language.ngram.trie
{
	public class NgramTrie : Object
	{		
		internal static int access_000(NgramTrie ngramTrie, int num)
		{
			return ngramTrie.requiredBits(num);
		}
		
		internal static NgramTrieBitarr access_100(NgramTrie ngramTrie)
		{
			return ngramTrie.bitArr;
		}
		
		internal static int access_300(NgramTrie ngramTrie)
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

		private NgramTrie.NgramSet getNgram(int num)
		{
			if (num == this.ordersNum - 1)
			{
				return this.longest;
			}
			return this.middles[num];
		}
		
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
			for (int j = counts.Length - 1; j >= 2; j --)
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

		internal static int access_200(NgramTrie ngramTrie)
		{
			return ngramTrie.quantProbBoLen;
		}

		private NgramTrie.MiddleNgramSet[] middles;

		private NgramTrie.LongestNgramSet longest;

		private NgramTrieBitarr bitArr;

		private int ordersNum;

		private int quantProbBoLen;

		private int quantProbLen;
		
		internal sealed class LongestNgramSet : NgramTrie.NgramSet
		{			
			internal LongestNgramSet(NgramTrie ngramTrie, int num, int num2, int num3) : base(ngramTrie, num, num3, num2)
			{
			}
			
			internal override int getQuantBits()
			{
				return NgramTrie.access_300(this.this_0);
			}
		}
		
		internal sealed class MiddleNgramSet : NgramTrie.NgramSet
		{			
			internal MiddleNgramSet(NgramTrie ngramTrie, int num, int num2, int num3, int num4, int num5) : base(ngramTrie, num, num4, num2 + NgramTrie.access_000(ngramTrie, num5))
			{
				this.nextMask = (1 << NgramTrie.access_000(ngramTrie, num5)) - 1;
				if (num3 + 1 >= 33554432 || num5 >= 33554432)
				{
					string text = "Sorry, current implementation doesn't support more than 33554432 n-grams of particular order";
					
					throw new Error(text);
				}
			}
			
			internal void readNextRange(int num, NgramTrieModel.TrieRange trieRange)
			{
				int num2 = num * this.totalBits;
				num2 += this.wordBits;
				num2 += this.getQuantBits();
				trieRange.begin = NgramTrie.access_100(this.this_0).readInt(this.memPtr, num2, this.nextMask);
				num2 += this.totalBits;
				trieRange.end = NgramTrie.access_100(this.this_0).readInt(this.memPtr, num2, this.nextMask);
			}
			
			internal override int getQuantBits()
			{
				return NgramTrie.access_200(this.this_0);
			}

			internal int nextMask;

			internal int nextOrderMemPtr;
		}

		internal abstract class NgramSet : Object
		{
			internal NgramSet(NgramTrie ngramTrie, int num, int num2, int num3)
			{
				this.this_0 = ngramTrie;
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

			internal virtual int readNgramWord(int num)
			{
				int bitOffset = num * this.totalBits;
				return NgramTrie.access_100(this.this_0).readInt(this.memPtr, bitOffset, this.wordMask);
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
			
			internal NgramTrie this_0;
		}
	}
}

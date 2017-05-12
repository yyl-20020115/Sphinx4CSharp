using java.lang;

namespace edu.cmu.sphinx.linguist.language.ngram.trie
{
	public class NgramTrieBitarr : Object
	{
		public virtual int readInt(int memPtr, int bitOffset, int mask)
		{
			int num = memPtr + (bitOffset >> 3);
			byte[] array = this.mem;
			int num2 = num;
			num++;
			int num3 = array[num2];
			int num4 = num3;
			byte[] array2 = this.mem;
			int num5 = num;
			num++;
			num3 = (num4 | (array2[num5] << 8 & 65535));
			int num6 = num3;
			byte[] array3 = this.mem;
			int num7 = num;
			num++;
			num3 = (num6 | (array3[num7] << 16 & 16777215));
			int num8 = num3;
			byte[] array4 = this.mem;
			int num9 = num;
			num++;
			num3 = (num8 | (array4[num9] << 24 & -1));
			num3 >>= (bitOffset & 7);
			return num3 & mask;
		}
		
		public NgramTrieBitarr(int memLen)
		{
			this.mem = new byte[memLen];
		}

		public virtual byte[] getArr()
		{
			return this.mem;
		}

		public virtual float readNegativeFloat(int memPtr, int bitOffset)
		{
			return 0f;
		}

		public virtual float readFloat(int memPtr, int bitOffset)
		{
			return 0f;
		}

		private byte[] mem;
	}
}

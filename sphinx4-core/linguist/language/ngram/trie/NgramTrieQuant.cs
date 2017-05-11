using System;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.ngram.trie
{
	public class NgramTrieQuant : java.lang.Object
	{		
		public NgramTrieQuant(int order, NgramTrieQuant.QuantType quantType)
		{
			int num = NgramTrieQuant_1._SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType[quantType.ordinal()];
			if (num == 1)
			{
				return;
			}
			if (num == 2)
			{
				this.probBits = 16;
				this.backoffBits = 16;
				this.probMask = (1 << this.probBits) - 1;
				this.backoffMask = (1 << this.backoffBits) - 1;
				this.tables = new float[(order - 1) * 2 - 1][];
				this.quantType = quantType;
				return;
			}
			string text = new StringBuilder().append("Unsupported quantization type: ").append(quantType).toString();
			
			throw new Error(text);
		}
		public virtual int getProbTableLen()
		{
			return 1 << this.probBits;
		}

		public virtual void setTable(float[] table, int order, bool isProb)
		{
			int num = (order - 2) * 2;
			if (!isProb)
			{
				num++;
			}
			this.tables[num] = table;
		}

		public virtual int getBackoffTableLen()
		{
			return 1 << this.backoffBits;
		}
		
		public virtual float readBackoff(NgramTrieBitarr bitArr, int memPtr, int bitOffset, int orderMinusTwo)
		{
			int num = NgramTrieQuant_1._SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType[this.quantType.ordinal()];
			if (num == 1)
			{
				bitOffset += 31;
				return bitArr.readFloat(memPtr, bitOffset);
			}
			if (num == 2)
			{
				int num2 = orderMinusTwo * 2 + 1;
				return this.binsDecode(num2, bitArr.readInt(memPtr, bitOffset, this.probMask));
			}
			string text = new StringBuilder().append("Unsupported quantization type: ").append(this.quantType).toString();
			
			throw new Error(text);
		}
		
		public virtual float readProb(NgramTrieBitarr bitArr, int memPtr, int bitOffset, int orderMinusTwo)
		{
			int num = NgramTrieQuant_1._SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType[this.quantType.ordinal()];
			if (num == 1)
			{
				return bitArr.readNegativeFloat(memPtr, bitOffset);
			}
			if (num == 2)
			{
				int num2 = orderMinusTwo * 2;
				if (num2 < this.tables.Length - 1)
				{
					bitOffset += this.backoffBits;
				}
				return this.binsDecode(num2, bitArr.readInt(memPtr, bitOffset, this.backoffMask));
			}
			string text = new StringBuilder().append("Unsupported quantization type: ").append(this.quantType).toString();
			
			throw new Error(text);
		}
		
		public virtual int getProbBoSize()
		{
			int num = NgramTrieQuant_1._SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType[this.quantType.ordinal()];
			if (num == 1)
			{
				return 63;
			}
			if (num == 2)
			{
				return 32;
			}
			string text = new StringBuilder().append("Unsupported quantization type: ").append(this.quantType).toString();
			
			throw new Error(text);
		}
		
		public virtual int getProbSize()
		{
			int num = NgramTrieQuant_1._SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType[this.quantType.ordinal()];
			if (num == 1)
			{
				return 31;
			}
			if (num == 2)
			{
				return 16;
			}
			string text = new StringBuilder().append("Unsupported quantization type: ").append(this.quantType).toString();
			
			throw new Error(text);
		}
		
		private float binsDecode(int num, int num2)
		{
			return this.tables[num][num2];
		}

		private int probBits;

		private int backoffBits;

		private int probMask;

		private int backoffMask;

		private float[][] tables;

		private NgramTrieQuant.QuantType quantType;
		
		[Serializable]
		public sealed class QuantType : java.lang.Enum
		{			
			public static NgramTrieQuant.QuantType[] values()
			{
				return (NgramTrieQuant.QuantType[])NgramTrieQuant.QuantType._VALUES_.Clone();
			}
			
			private QuantType(string text, int num) : base(text, num)
			{
				GC.KeepAlive(this);
			}
			
			public static NgramTrieQuant.QuantType valueOf(string name)
			{
				return (NgramTrieQuant.QuantType)java.lang.Enum.valueOf(ClassLiteral<NgramTrieQuant.QuantType>.Value, name);
			}
			
			public static NgramTrieQuant.QuantType NO_QUANT
			{
				
				get
				{
					return NgramTrieQuant.QuantType.__NO_QUANT;
				}
			}
			
			public static NgramTrieQuant.QuantType QUANT_16
			{
				
				get
				{
					return NgramTrieQuant.QuantType.__QUANT_16;
				}
			}
			
			internal static NgramTrieQuant.QuantType __NO_QUANT = new NgramTrieQuant.QuantType("NO_QUANT", 0);
			
			internal static NgramTrieQuant.QuantType __QUANT_16 = new NgramTrieQuant.QuantType("QUANT_16", 1);
			
			private static NgramTrieQuant.QuantType[] _VALUES_ = new NgramTrieQuant.QuantType[]
			{
				NgramTrieQuant.QuantType.__NO_QUANT,
				NgramTrieQuant.QuantType.__QUANT_16
			};

			[Serializable]
			public enum __Enum
			{
				NO_QUANT,
				QUANT_16
			}
		}
	}
}

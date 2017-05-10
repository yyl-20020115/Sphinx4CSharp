using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.ngram.trie
{
	
	[EnclosingMethod("edu.cmu.sphinx.linguist.language.ngram.trie.NgramTrieQuant", null, null)]
	[SourceFile("NgramTrieQuant.java")]
	
	internal sealed class NgramTrieQuant$1 : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		
		static NgramTrieQuant$1()
		{
			try
			{
				NgramTrieQuant$1.$SwitchMap$edu$cmu$sphinx$linguist$language$ngram$trie$NgramTrieQuant$QuantType[NgramTrieQuant.QuantType.__NO_QUANT.ordinal()] = 1;
			}
			catch (Exception ex)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex, 2) == null)
				{
					throw;
				}
			}
			try
			{
				NgramTrieQuant$1.$SwitchMap$edu$cmu$sphinx$linguist$language$ngram$trie$NgramTrieQuant$QuantType[NgramTrieQuant.QuantType.__QUANT_16.ordinal()] = 2;
			}
			catch (Exception ex2)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex2, 2) == null)
				{
					throw;
				}
			}
		}

		NgramTrieQuant$1()
		{
			throw null;
		}

		
		internal static int[] $SwitchMap$edu$cmu$sphinx$linguist$language$ngram$trie$NgramTrieQuant$QuantType = new int[NgramTrieQuant.QuantType.values().Length];
	}
}

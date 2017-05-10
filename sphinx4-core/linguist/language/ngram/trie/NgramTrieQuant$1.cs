using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.ngram.trie
{
	
	[EnclosingMethod("edu.cmu.sphinx.linguist.language.ngram.trie.NgramTrieQuant", null, null)]
	[SourceFile("NgramTrieQuant.java")]
	
	internal sealed class NgramTrieQuant_1 : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		
		static NgramTrieQuant_1()
		{
			try
			{
				NgramTrieQuant_1._SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType[NgramTrieQuant.QuantType.__NO_QUANT.ordinal()] = 1;
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
				NgramTrieQuant_1._SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType[NgramTrieQuant.QuantType.__QUANT_16.ordinal()] = 2;
			}
			catch (Exception ex2)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex2, 2) == null)
				{
					throw;
				}
			}
		}

		NgramTrieQuant_1()
		{
			throw null;
		}

		
		internal static int[] _SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType = new int[NgramTrieQuant.QuantType.values().Length];
	}
}

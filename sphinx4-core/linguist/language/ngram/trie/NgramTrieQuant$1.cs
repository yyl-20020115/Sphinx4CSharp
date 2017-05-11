namespace edu.cmu.sphinx.linguist.language.ngram.trie
{
	internal sealed class NgramTrieQuant_1 : java.lang.Object
	{	
		static NgramTrieQuant_1()
		{
			try
			{
				NgramTrieQuant_1._SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType[NgramTrieQuant.QuantType.__NO_QUANT.ordinal()] = 1;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			try
			{
				NgramTrieQuant_1._SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType[NgramTrieQuant.QuantType.__QUANT_16.ordinal()] = 2;
			}
			catch (System.Exception ex2)
			{
				throw ex2;
			}
		}

		NgramTrieQuant_1()
		{
			throw null;
		}

		internal static int[] _SwitchMap_edu_cmu_sphinx_linguist_language_ngram_trie_NgramTrieQuant_QuantType = new int[NgramTrieQuant.QuantType.values().Length];
	}
}

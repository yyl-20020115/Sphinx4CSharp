using System;
using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.decoder.search.stats
{
	internal sealed class TokenTypeTracker_1 : java.lang.Object
	{
		static TokenTypeTracker_1()
		{
			try
			{
				TokenTypeTracker_1._SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition[HMMPosition.__BEGIN.ordinal()] = 1;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			try
			{
				TokenTypeTracker_1._SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition[HMMPosition.__END.ordinal()] = 2;
			}
			catch (System.Exception ex2)
			{
				throw ex2;
			}
			try
			{
				TokenTypeTracker_1._SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition[HMMPosition.__SINGLE.ordinal()] = 3;
			}
			catch (System.Exception ex3)
			{
				throw ex3;
			}
			try
			{
				TokenTypeTracker_1._SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition[HMMPosition.__INTERNAL.ordinal()] = 4;
			}
			catch (System.Exception ex4)
			{
				throw ex4;
			}
		}

		TokenTypeTracker_1()
		{
			throw null;
		}
		
		internal static int[] _SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition = new int[HMMPosition.values().Length];
	}
}

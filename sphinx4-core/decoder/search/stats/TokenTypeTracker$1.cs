using System;

using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.decoder.search.stats
{
	
	[EnclosingMethod("edu.cmu.sphinx.decoder.search.stats.TokenTypeTracker", null, null)]
	[SourceFile("TokenTypeTracker.java")]
	
	internal sealed class TokenTypeTracker_1 : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		
		static TokenTypeTracker_1()
		{
			try
			{
				TokenTypeTracker_1._SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition[HMMPosition.__BEGIN.ordinal()] = 1;
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
				TokenTypeTracker_1._SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition[HMMPosition.__END.ordinal()] = 2;
			}
			catch (Exception ex2)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex2, 2) == null)
				{
					throw;
				}
			}
			try
			{
				TokenTypeTracker_1._SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition[HMMPosition.__SINGLE.ordinal()] = 3;
			}
			catch (Exception ex3)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex3, 2) == null)
				{
					throw;
				}
			}
			try
			{
				TokenTypeTracker_1._SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition[HMMPosition.__INTERNAL.ordinal()] = 4;
			}
			catch (Exception ex4)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex4, 2) == null)
				{
					throw;
				}
			}
		}

		TokenTypeTracker_1()
		{
			throw null;
		}

		
		internal static int[] _SwitchMap_edu_cmu_sphinx_linguist_acoustic_HMMPosition = new int[HMMPosition.values().Length];
	}
}

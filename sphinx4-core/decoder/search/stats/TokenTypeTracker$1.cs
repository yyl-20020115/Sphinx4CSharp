using System;

using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.decoder.search.stats
{
	
	[EnclosingMethod("edu.cmu.sphinx.decoder.search.stats.TokenTypeTracker", null, null)]
	[SourceFile("TokenTypeTracker.java")]
	
	internal sealed class TokenTypeTracker$1 : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		
		static TokenTypeTracker$1()
		{
			try
			{
				TokenTypeTracker$1.$SwitchMap$edu$cmu$sphinx$linguist$acoustic$HMMPosition[HMMPosition.__BEGIN.ordinal()] = 1;
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
				TokenTypeTracker$1.$SwitchMap$edu$cmu$sphinx$linguist$acoustic$HMMPosition[HMMPosition.__END.ordinal()] = 2;
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
				TokenTypeTracker$1.$SwitchMap$edu$cmu$sphinx$linguist$acoustic$HMMPosition[HMMPosition.__SINGLE.ordinal()] = 3;
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
				TokenTypeTracker$1.$SwitchMap$edu$cmu$sphinx$linguist$acoustic$HMMPosition[HMMPosition.__INTERNAL.ordinal()] = 4;
			}
			catch (Exception ex4)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex4, 2) == null)
				{
					throw;
				}
			}
		}

		TokenTypeTracker$1()
		{
			throw null;
		}

		
		internal static int[] $SwitchMap$edu$cmu$sphinx$linguist$acoustic$HMMPosition = new int[HMMPosition.values().Length];
	}
}

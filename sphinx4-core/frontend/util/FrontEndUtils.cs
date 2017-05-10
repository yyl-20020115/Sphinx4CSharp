using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class FrontEndUtils : java.lang.Object
	{
		
		
		public FrontEndUtils()
		{
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			165,
			105,
			104,
			143,
			136,
			99,
			194
		})]
		
		public static DataProcessor getFrontEndProcessor(DataProcessor dp, Class predecClass)
		{
			while (!predecClass.isInstance(dp))
			{
				if (dp is FrontEnd)
				{
					dp = ((FrontEnd)dp).getLastDataProcessor();
				}
				else
				{
					dp = dp.getPredecessor();
				}
				if (dp == null)
				{
					return null;
				}
			}
			return (DataProcessor)predecClass.cast(dp);
		}
	}
}

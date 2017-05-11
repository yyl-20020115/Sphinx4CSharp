using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class FrontEndUtils : java.lang.Object
	{		
		public FrontEndUtils()
		{
		}
		
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

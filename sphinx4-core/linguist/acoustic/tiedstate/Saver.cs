using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public interface Saver : Configurable
	{
		Pool getMeansPool();
		
		Pool getMeansTransformationMatrixPool();
		
		Pool getMeansTransformationVectorPool();
		
		Pool getVariancePool();
		
		Pool getVarianceTransformationMatrixPool();
		
		Pool getVarianceTransformationVectorPool();
		
		Pool getSenonePool();

		HMMManager getHMMManager();
		
		Map getContextIndependentUnits();

		void logInfo();

		int getLeftContextSize();

		int getRightContextSize();

		void save(string str, bool b);

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			9.9999997473787516E-05
		})]
		public const string PROP_VARIANCE_FLOOR = "varianceFloor";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.0
		})]
		public const string PROP_MC_FLOOR = "MixtureComponentScoreFloor";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0000000116860974E-07
		})]
		public const string PROP_MW_FLOOR = "mixtureWeightFloor";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_SPARSE_FORM = "sparseForm";

		
		public static class __Fields
		{
			public const string PROP_VARIANCE_FLOOR = "varianceFloor";

			public const string PROP_MC_FLOOR = "MixtureComponentScoreFloor";

			public const string PROP_MW_FLOOR = "mixtureWeightFloor";

			public const string PROP_SPARSE_FORM = "sparseForm";
		}
	}
}

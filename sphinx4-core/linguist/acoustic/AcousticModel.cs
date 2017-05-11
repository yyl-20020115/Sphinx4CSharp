using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic
{
	public interface AcousticModel : Configurable
	{
		void allocate();

		void deallocate();

		string getName();

		HMM lookupNearestHMM(Unit u, HMMPosition hmmp, bool b);

		Iterator getHMMIterator();

		Iterator getContextIndependentUnitIterator();

		int getLeftContextSize();

		int getRightContextSize();

		Properties getProperties();

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"."
		})]
		public const string PROP_LOCATION = "location";

		
		public static class __Fields
		{
			public const string PROP_LOCATION = "location";
		}
	}
}

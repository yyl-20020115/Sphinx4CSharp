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

	}

	public abstract class AcousticModelBase : java.lang.Object, AcousticModel
	{
		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"."
		})]
		public const string PROP_LOCATION = "location";

		public abstract void allocate();
		public abstract void deallocate();
		public abstract Iterator getContextIndependentUnitIterator();
		public abstract Iterator getHMMIterator();
		public abstract int getLeftContextSize();
		public abstract string getName();
		public abstract Properties getProperties();
		public abstract int getRightContextSize();
		public abstract HMM lookupNearestHMM(Unit u, HMMPosition hmmp, bool b);
		public abstract void newProperties(PropertySheet ps);

		public static class __Fields
		{
			public const string PROP_LOCATION = "location";
		}
	}
}

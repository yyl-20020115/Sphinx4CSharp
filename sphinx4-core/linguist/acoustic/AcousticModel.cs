using System;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public interface AcousticModel : Configurable
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
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

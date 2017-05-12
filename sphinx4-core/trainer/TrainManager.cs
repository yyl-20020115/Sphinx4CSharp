using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.trainer
{
	public interface TrainManager : Configurable
	{
		void train();

		void saveModels(string str);

		void copyModels(string str);

		void initializeModels(string str);

		void trainContextIndependentModels(string str);

	}

	public abstract class TrainManagerBase : java.lang.Object, TrainManager
	{

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.20000000298023224
		})]
		public const string PROP_MINIMUM_IMPROVEMENT = "minimumImprovement";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			15
		})]
		public const string PROP_MAXIMUM_ITERATION = "maximumIteration";

		public abstract void copyModels(string str);
		public abstract void initializeModels(string str);
		public abstract void newProperties(PropertySheet ps);
		public abstract void saveModels(string str);
		public abstract void train();
		public abstract void trainContextIndependentModels(string str);

		public static class __Fields
		{
			public const string PROP_MINIMUM_IMPROVEMENT = "minimumImprovement";

			public const string PROP_MAXIMUM_ITERATION = "maximumIteration";
		}
	}
}

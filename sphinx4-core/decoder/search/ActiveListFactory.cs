using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.decoder.search
{
	public abstract class ActiveListFactory : java.lang.Object, Configurable
	{
		public ActiveListFactory(int absoluteBeamWidth, double relativeBeamWidth)
		{
			this.logMath = LogMath.getLogMath();
			this.absoluteBeamWidth = absoluteBeamWidth;
			this.logRelativeBeamWidth = this.logMath.linearToLog(relativeBeamWidth);
		}
		public ActiveListFactory()
		{
		}

		public virtual void newProperties(PropertySheet ps)
		{
			this.logMath = LogMath.getLogMath();
			this.absoluteBeamWidth = ps.getInt("absoluteBeamWidth");
			double linearValue = ps.getDouble("relativeBeamWidth");
			this.logRelativeBeamWidth = this.logMath.linearToLog(linearValue);
		}

		public abstract ActiveList newInstance();

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			-1
		})]
		public const string PROP_ABSOLUTE_BEAM_WIDTH = "absoluteBeamWidth";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1E-80
		})]
		public const string PROP_RELATIVE_BEAM_WIDTH = "relativeBeamWidth";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_STRICT_PRUNING = "strictPruning";

		protected internal LogMath logMath;

		protected internal int absoluteBeamWidth;

		protected internal float logRelativeBeamWidth;
	}
}

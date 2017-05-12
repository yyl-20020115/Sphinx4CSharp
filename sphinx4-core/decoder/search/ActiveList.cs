using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	public interface ActiveList : Iterable
	{
		ActiveList purge();

		Token getBestToken();

		void add(Token t);

		int size();

		List getTokens();

		float getBeamThreshold();

		float getBestScore();

		void setBestToken(Token t);

		ActiveList newInstance();


	}

	public abstract class ActiveListBase : ActiveList
	{
		[S4Integer(new object[]
{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			2000
})]
		public const string PROP_ABSOLUTE_BEAM_WIDTH = "absoluteBeamWidth";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.0
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

		public abstract void add(Token t);
		public abstract float getBeamThreshold();
		public abstract float getBestScore();
		public abstract Token getBestToken();
		public abstract List getTokens();
		public abstract Iterator iterator();
		public abstract ActiveList newInstance();
		public abstract ActiveList purge();
		public abstract void setBestToken(Token t);
		public abstract int size();

		public static class __Fields
		{
			public const string PROP_ABSOLUTE_BEAM_WIDTH = "absoluteBeamWidth";

			public const string PROP_RELATIVE_BEAM_WIDTH = "relativeBeamWidth";

			public const string PROP_STRICT_PRUNING = "strictPruning";
		}
	}
}

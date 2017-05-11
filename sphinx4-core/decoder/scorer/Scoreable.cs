using edu.cmu.sphinx.frontend;
using java.util;

namespace edu.cmu.sphinx.decoder.scorer
{
	public abstract class Scoreable : Data
	{
		public abstract float normalizeScore(float f);

		public abstract float getScore();

		public abstract float calculateScore(Data d);


		public static readonly Comparator COMPARATOR = new Scoreable_1();

		
		public static class __Fields
		{
			static __Fields()
			{
			}

			public static readonly Comparator COMPARATOR = Scoreable.COMPARATOR;
		}
	}
}

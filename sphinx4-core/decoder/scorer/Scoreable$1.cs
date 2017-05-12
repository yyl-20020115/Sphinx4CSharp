using java.util;
using java.lang;

namespace edu.cmu.sphinx.decoder.scorer
{
	internal sealed class Scoreable_1 : Object, Comparator
	{	
		public int compare(Scoreable scoreable, Scoreable scoreable2)
		{
			if (scoreable.getScore() > scoreable2.getScore())
			{
				return -1;
			}
			if (scoreable.getScore() == scoreable2.getScore())
			{
				return 0;
			}
			return 1;
		}
				
		internal Scoreable_1()
		{
		}

		public int compare(object obj, object obj2)
		{
			return this.compare((Scoreable)obj, (Scoreable)obj2);
		}

		bool Comparator.equals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}
	}
}

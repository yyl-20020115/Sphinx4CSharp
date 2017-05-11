using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.decoder.scorer
{
	public class MaxScoreNormalizer : java.lang.Object, ScoreNormalizer, Configurable
	{
		public virtual void newProperties(PropertySheet ps)
		{
		}
		
		public MaxScoreNormalizer()
		{
		}
	
		public virtual Scoreable normalize(List scoreableList, Scoreable bestToken)
		{
			Iterator iterator = scoreableList.iterator();
			while (iterator.hasNext())
			{
				Scoreable scoreable = (Scoreable)iterator.next();
				scoreable.normalizeScore(bestToken.getScore());
			}
			return bestToken;
		}
	}
}

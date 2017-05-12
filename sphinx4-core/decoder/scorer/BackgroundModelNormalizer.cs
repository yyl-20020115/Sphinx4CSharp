using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.util.props;
using java.util;
using java.util.logging;
using java.lang;

namespace edu.cmu.sphinx.decoder.scorer
{
	public class BackgroundModelNormalizer : Object, ScoreNormalizer, Configurable
	{
		public BackgroundModelNormalizer()
		{
		}

		public virtual void newProperties(PropertySheet ps)
		{
			this.activeListProvider = (SimpleBreadthFirstSearchManager)ps.getComponent("activeListProvider");
			this.logger = ps.getLogger();
			this.logger.warning("no active list set.");
		}
	
		public BackgroundModelNormalizer(SimpleBreadthFirstSearchManager activeListProvider)
		{
			this.activeListProvider = activeListProvider;
			this.logger = Logger.getLogger(Object.instancehelper_getClass(this).getName());
			this.logger.warning("no active list set.");
		}
	
		public virtual Scoreable normalize(List scoreableList, Scoreable bestToken)
		{
			if (this.activeListProvider == null)
			{
				return bestToken;
			}
			Token bestToken2 = this.activeListProvider.getActiveList().getBestToken();
			float score = bestToken2.getScore();
			Iterator iterator = scoreableList.iterator();
			while (iterator.hasNext())
			{
				Scoreable scoreable = (Scoreable)iterator.next();
				if (scoreable is Token)
				{
					scoreable.normalizeScore(score);
				}
			}
			return bestToken;
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/decoder/search/SimpleBreadthFirstSearchManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			},
			"mandatory",
			false
		})]
		public const string ACTIVE_LIST_PROVIDER = "activeListProvider";

		private SimpleBreadthFirstSearchManager activeListProvider;

		private Logger logger;
	}
}

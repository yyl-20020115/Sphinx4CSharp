using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.scorer
{
	public class SimpleAcousticScorer : ConfigurableAdapter, AcousticScorer, Configurable
	{
		protected internal virtual Data getNextData()
		{
			return this.frontEnd.getData();
		}
	
		protected internal virtual Data calculateScoresForData(List scoreableList, Data data)
		{
			object obj = data;
			if (((Data)obj) is SpeechEndSignal || ((Data)obj) is DataEndSignal)
			{
				return (Data)obj;
			}
			if (scoreableList.isEmpty())
			{
				return null;
			}
			if (((Data)obj) is DoubleData)
			{
				obj = DataUtil.DoubleData2FloatData((DoubleData)((Data)obj));
			}
			object obj2 = obj;
			Data data2;
			if (obj2 != null)
			{
				if ((data2 = (obj2 as Data)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				data2 = null;
			}
			Scoreable scoreable = this.doScoring(scoreableList, data2);
			if (this.scoreNormalizer != null && scoreable is Token)
			{
				scoreable = this.scoreNormalizer.normalize(scoreableList, scoreable);
			}
			return scoreable;
		}
	
		protected internal virtual Scoreable doScoring(List scoreableList, Data data)
		{
			Scoreable result = null;
			float num = float.MinValue;
			Iterator iterator = scoreableList.iterator();
			while (iterator.hasNext())
			{
				Scoreable scoreable = (Scoreable)iterator.next();
				scoreable.calculateScore(data);
				if (scoreable.getScore() > num)
				{
					num = scoreable.getScore();
					result = scoreable;
				}
			}
			return result;
		}

		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.frontEnd = (BaseDataProcessor)ps.getComponent("frontend");
			this.scoreNormalizer = (ScoreNormalizer)ps.getComponent("scoreNormalizer");
			this.storedData = new LinkedList();
		}
	
		public SimpleAcousticScorer(BaseDataProcessor frontEnd, ScoreNormalizer scoreNormalizer)
		{
			this.seenEnd = false;
			this.initLogger();
			this.frontEnd = frontEnd;
			this.scoreNormalizer = scoreNormalizer;
			this.storedData = new LinkedList();
		}
	
		public SimpleAcousticScorer()
		{
			this.seenEnd = false;
		}

		public virtual Data calculateScores(List scoreableList)
		{
			Data data;
			if (this.storedData.isEmpty())
			{
				while ((data = this.getNextData()) is Signal)
				{
					if (data is SpeechEndSignal)
					{
						this.seenEnd = true;
						break;
					}
					if (data is DataEndSignal)
					{
						if (this.seenEnd)
						{
							return null;
						}
						break;
					}
				}
				if (data == null)
				{
					return null;
				}
			}
			else
			{
				data = (Data)this.storedData.poll();
			}
			return this.calculateScoresForData(scoreableList, data);
		}
	
		public virtual Data calculateScoresAndStoreData(List scoreableList)
		{
			Data nextData;
			while ((nextData = this.getNextData()) is Signal)
			{
				if (nextData is SpeechEndSignal)
				{
					this.seenEnd = true;
					break;
				}
				if (nextData is DataEndSignal)
				{
					if (this.seenEnd)
					{
						return null;
					}
					break;
				}
			}
			if (nextData == null)
			{
				return null;
			}
			this.storedData.add(nextData);
			return this.calculateScoresForData(scoreableList, nextData);
		}
		
		public virtual void startRecognition()
		{
			this.storedData.clear();
		}

		public virtual void stopRecognition()
		{
		}

		public virtual void allocate()
		{
		}

		public virtual void deallocate()
		{
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/frontend/BaseDataProcessor, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string FEATURE_FRONTEND = "frontend";

		protected internal BaseDataProcessor frontEnd;

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/decoder/scorer/ScoreNormalizer, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			},
			"mandatory",
			false
		})]
		public const string SCORE_NORMALIZER = "scoreNormalizer";

		protected internal ScoreNormalizer scoreNormalizer;
		
		private LinkedList storedData;

		private bool seenEnd;
	}
}

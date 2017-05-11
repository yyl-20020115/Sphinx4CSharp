using edu.cmu.sphinx.frontend;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer
{
	public class TrainerScore : java.lang.Object
	{
		public virtual float getScalingFactor()
		{
			return this.logScalingFactor;
		}

		public virtual HMMState getState()
		{
			return this.hmmState;
		}

		public virtual int getSenoneID()
		{
			return this.senoneID;
		}

		public virtual Data getData()
		{
			return this.feature;
		}

		public virtual float[] getComponentGamma()
		{
			return this.logComponentGamma;
		}

		public virtual float getAlpha()
		{
			return this.logAlpha;
		}

		public virtual float getBeta()
		{
			return this.logBeta;
		}

		public virtual float getScore()
		{
			return this.logOutputProbability;
		}
		
		public TrainerScore(Data feature, float probability, HMMState state, float logAlpha, float logBeta, float[] logComponentProb)
		{
			this.feature = feature;
			this.hmmState = state;
			this.logScalingFactor = 0f;
			if (state != null && state.isEmitting())
			{
				int i = state.getState();
				SenoneHMM senoneHMM = (SenoneHMM)state.getHMM();
				SenoneSequence senoneSequence = senoneHMM.getSenoneSequence();
				this.senone = senoneSequence.getSenones()[i];
			}
			this.logOutputProbability = probability;
			this.logAlpha = logAlpha;
			this.logBeta = logBeta;
			this.logGamma = logAlpha + logBeta;
			if (logComponentProb != null)
			{
				this.logComponentProb = new float[logComponentProb.Length];
				this.logComponentGamma = new float[logComponentProb.Length];
				for (int i = 0; i < logComponentProb.Length; i++)
				{
					this.logComponentProb[i] = logComponentProb[i];
					this.logComponentGamma[i] = logComponentProb[i] + this.logGamma;
				}
			}
			else
			{
				this.logComponentProb = null;
				this.logComponentGamma = null;
			}
		}
		
		public TrainerScore(Data feature, float probability, int senone)
		{
			this.feature = feature;
			this.logOutputProbability = probability;
			this.senoneID = senone;
			this.logScalingFactor = 0f;
			this.logAlpha = 0f;
			this.logBeta = 0f;
			this.logGamma = 0f;
			this.logComponentProb = new float[1];
			this.logComponentProb[0] = 0f;
			this.logComponentGamma = new float[1];
			this.logComponentGamma[0] = 0f;
		}
		
		public TrainerScore(Data feature, float probability, HMMState state, float logAlpha, float[] logComponentProb) : this(feature, probability, state, logAlpha, 0f, logComponentProb)
		{
		}
		
		public TrainerScore(Data feature, float probability, HMMState state, float[] logComponentProb) : this(feature, probability, state, 0f, 0f, logComponentProb)
		{
		}

		public TrainerScore(Data feature, float probability, HMMState state) : this(feature, probability, state, 0f, 0f, null)
		{
		}

		public static float getLogLikelihood()
		{
			return TrainerScore.logLikelihood;
		}

		public virtual float getGamma()
		{
			return this.logGamma;
		}

		public static void setLogLikelihood(float likelihood)
		{
			TrainerScore.logLikelihood = likelihood;
		}

		public virtual void setAlpha(float logAlpha)
		{
			this.logAlpha = logAlpha;
		}

		public virtual void setBeta(float logBeta)
		{
			this.logBeta = logBeta;
		}

		public virtual void setGamma()
		{
			this.logGamma = this.logAlpha + this.logBeta + this.logScalingFactor;
			if (this.logComponentGamma != null)
			{
				for (int i = 0; i < this.logComponentGamma.Length; i++)
				{
					this.logComponentGamma[i] = this.logComponentProb[i] + this.logGamma;
					if (this.logComponentGamma[i] < -3.40282347E+38f)
					{
						this.logComponentGamma[i] = float.MinValue;
					}
				}
			}
		}

		public virtual void setScalingFactor(float logScalingFactor)
		{
			this.logScalingFactor = logScalingFactor;
		}

		public virtual Senone getSenone()
		{
			return this.senone;
		}

		private Data feature;

		private float logOutputProbability;

		private float logScalingFactor;

		private int senoneID;

		private HMMState hmmState;

		private Senone senone;

		private float logAlpha;

		private float logBeta;

		private float logGamma;

		private float[] logComponentGamma;

		private float[] logComponentProb;

		private static float logLikelihood;
	}
}

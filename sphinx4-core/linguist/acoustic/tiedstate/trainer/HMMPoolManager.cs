using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.util;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer
{
	internal class HMMPoolManager : Object
	{
		protected internal void createBuffers()
		{
			this.meansBufferPool = this.create1DPoolBuffer(this.meansPool, false);
			this.varianceBufferPool = this.create1DPoolBuffer(this.variancePool, false);
			this.matrixBufferPool = this.create2DPoolBuffer(this.matrixPool, true);
			this.mixtureWeightsBufferPool = this.createWeightsPoolBuffer(this.mixtureWeights);
		}
		
		private Pool create1DPoolBuffer(Pool pool, bool flag)
		{
			Pool pool2 = new Pool(pool.getName());
			for (int i = 0; i < pool.size(); i++)
			{
				float[] array = (float[])pool.get(i);
				this.indexMap.put(array, Integer.valueOf(i));
				Buffer o = new Buffer(array.Length, flag, i);
				pool2.put(i, o);
			}
			return pool2;
		}

		private Pool create2DPoolBuffer(Pool pool, bool flag)
		{
			Pool pool2 = new Pool(pool.getName());
			for (int i = 0; i < pool.size(); i++)
			{
				float[][] array = (float[][])pool.get(i);
				this.indexMap.put(array, Integer.valueOf(i));
				int num = array.Length;
				Buffer[] array2 = new Buffer[num];
				for (int j = 0; j < num; j++)
				{
					Buffer[] array3 = array2;
					int num2 = j;
					array3[num2] = new Buffer(array[j].Length, flag, j);
				}
				pool2.put(i, array2);
			}
			return pool2;
		}

		private Pool createWeightsPoolBuffer(GaussianWeights gaussianWeights)
		{
			Pool pool = new Pool(gaussianWeights.getName());
			int statesNum = gaussianWeights.getStatesNum();
			int streamsNum = gaussianWeights.getStreamsNum();
			int gauPerState = gaussianWeights.getGauPerState();
			for (int i = 0; i < streamsNum; i++)
			{
				for (int j = 0; j < statesNum; j++)
				{
					int num = i * statesNum + j;
					Buffer o = new Buffer(gauPerState, true, num);
					pool.put(num, o);
				}
			}
			return pool;
		}
		
		protected internal void accumulate(int num, TrainerScore[] array, TrainerScore[] array2)
		{
			TrainerScore trainerScore = array[num];
			this.currentLogLikelihood = 0f;
			this.logLikelihood -= array[0].getScalingFactor();
			SenoneHMMState senoneHMMState = (SenoneHMMState)trainerScore.getState();
			if (senoneHMMState == null)
			{
				int num2 = trainerScore.getSenoneID();
				if (num2 == -1)
				{
					this.accumulateMean(num2, array[num]);
					this.accumulateVariance(num2, array[num]);
					this.accumulateMixture(num2, array[num]);
					this.accumulateTransition(num2, num, array, array2);
				}
			}
			else if (senoneHMMState.isEmitting())
			{
				int num2 = this.senonePool.indexOf(senoneHMMState.getSenone());
				this.accumulateMixture(num2, array[num]);
				this.accumulateTransition(num2, num, array, array2);
			}
		}

		private void accumulateMean(int num, TrainerScore trainerScore)
		{
			if (num == -1)
			{
				for (int i = 0; i < this.senonePool.size(); i++)
				{
					this.accumulateMean(i, trainerScore);
				}
			}
			else
			{
				GaussianMixture gaussianMixture = (GaussianMixture)this.senonePool.get(num);
				MixtureComponent[] mixtureComponents = gaussianMixture.getMixtureComponents();
				for (int j = 0; j < mixtureComponents.Length; j++)
				{
					float[] mean = mixtureComponents[j].getMean();
					int num2 = ((Integer)this.indexMap.get(mean)).intValue();
					if (!HMMPoolManager.assertionsDisabled && num2 < 0)
					{
						
						throw new AssertionError();
					}
					if (!HMMPoolManager.assertionsDisabled && num2 != num)
					{
						
						throw new AssertionError();
					}
					Buffer buffer = (Buffer)this.meansBufferPool.get(num2);
					float[] values = ((FloatData)trainerScore.getData()).getValues();
					double[] array = new double[values.Length];
					float num3 = trainerScore.getComponentGamma()[j];
					num3 -= this.currentLogLikelihood;
					double num4 = this.logMath.logToLinear(num3);
					for (int k = 0; k < array.Length; k++)
					{
						array[k] = (double)values[k] * num4;
					}
					buffer.accumulate(array, num4);
				}
			}
		}
		
		private void accumulateVariance(int num, TrainerScore trainerScore)
		{
			if (num == -1)
			{
				for (int i = 0; i < this.senonePool.size(); i++)
				{
					this.accumulateVariance(i, trainerScore);
				}
			}
			else
			{
				GaussianMixture gaussianMixture = (GaussianMixture)this.senonePool.get(num);
				MixtureComponent[] mixtureComponents = gaussianMixture.getMixtureComponents();
				for (int j = 0; j < mixtureComponents.Length; j++)
				{
					float[] mean = mixtureComponents[j].getMean();
					float[] variance = mixtureComponents[j].getVariance();
					int id = ((Integer)this.indexMap.get(variance)).intValue();
					Buffer buffer = (Buffer)this.varianceBufferPool.get(id);
					float[] values = ((FloatData)trainerScore.getData()).getValues();
					double[] array = new double[values.Length];
					float num2 = trainerScore.getComponentGamma()[j];
					num2 -= this.currentLogLikelihood;
					double num3 = this.logMath.logToLinear(num2);
					for (int k = 0; k < array.Length; k++)
					{
						array[k] = (double)(values[k] - mean[k]);
						double[] array2 = array;
						int num4 = k;
						double[] array3 = array2;
						array3[num4] *= array[k] * num3;
					}
					buffer.accumulate(array, num3);
				}
			}
		}

		private void accumulateMixture(int num, TrainerScore trainerScore)
		{
			if (num == -1)
			{
				for (int i = 0; i < this.senonePool.size(); i++)
				{
					this.accumulateMixture(i, trainerScore);
				}
			}
			else
			{
				Buffer buffer = (Buffer)this.mixtureWeightsBufferPool.get(num);
				for (int j = 0; j < this.mixtureWeights.getGauPerState(); j++)
				{
					float num2 = trainerScore.getComponentGamma()[j];
					num2 -= this.currentLogLikelihood;
					buffer.logAccumulate(num2, j, this.logMath);
				}
			}
		}

		private void accumulateTransition(int num, int num2, TrainerScore[] array, TrainerScore[] array2)
		{
			if (num == -1)
			{
				Iterator iterator = this.hmmManager.iterator();
				while (iterator.hasNext())
				{
					HMM hmm = (HMM)iterator.next();
					for (int i = 0; i < hmm.getOrder(); i++)
					{
						this.accumulateStateTransition(i, (SenoneHMM)hmm, array[num2].getScore());
					}
				}
			}
			else if (array2 != null)
			{
				this.accumulateStateTransition(num2, array, array2);
			}
		}
		
		private void accumulateStateTransition(int num, SenoneHMM senoneHMM, float num2)
		{
			float[][] transitionMatrix = senoneHMM.getTransitionMatrix();
			float[] array = transitionMatrix[num];
			int id = ((Integer)this.indexMap.get(transitionMatrix)).intValue();
			Buffer[] array2 = (Buffer[])this.matrixBufferPool.get(id);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != -3.40282347E+38f)
				{
					array2[num].logAccumulate(num2, i, this.logMath);
				}
			}
		}
		
		private void accumulateStateTransition(int num, TrainerScore[] array, TrainerScore[] array2)
		{
			HMMState state = array[num].getState();
			if (state == null)
			{
				return;
			}
			int state2 = state.getState();
			SenoneHMM senoneHMM = (SenoneHMM)state.getHMM();
			float[][] transitionMatrix = senoneHMM.getTransitionMatrix();
			int id = ((Integer)this.indexMap.get(transitionMatrix)).intValue();
			Buffer[] array3 = (Buffer[])this.matrixBufferPool.get(id);
			float[] array4 = transitionMatrix[state2];
			for (int i = 0; i < array4.Length; i++)
			{
				if (array4[i] != -3.40282347E+38f)
				{
					int num2 = i - state2;
					int num3 = num + num2;
					if (!HMMPoolManager.assertionsDisabled && array2[num3].getState() != null && array2[num3].getState().getHMM() != senoneHMM)
					{
						
						throw new AssertionError();
					}
					float alpha = array[num].getAlpha();
					float beta = array2[num3].getBeta();
					float num4 = array4[i];
					float score = array2[num3].getScore();
					float num5 = alpha + beta + num4 + score;
					num5 -= this.currentLogLikelihood;
					array3[state2].logAccumulate(num5, i, this.logMath);
				}
			}
		}
		
		private void normalizePool(Pool pool)
		{
			if (!HMMPoolManager.assertionsDisabled && pool == null)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < pool.size(); i++)
			{
				Buffer buffer = (Buffer)pool.get(i);
				if (buffer.wasUsed())
				{
					buffer.normalize();
				}
			}
		}

		private void logNormalizePool(Pool pool)
		{
			if (!HMMPoolManager.assertionsDisabled && pool == null)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < pool.size(); i++)
			{
				Buffer buffer = (Buffer)pool.get(i);
				if (buffer.wasUsed())
				{
					buffer.logNormalize();
				}
			}
		}
		
		private void logNormalize2DPool(Pool pool, Pool pool2)
		{
			if (!HMMPoolManager.assertionsDisabled && pool == null)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < pool.size(); i++)
			{
				Buffer[] array = (Buffer[])pool.get(i);
				float[][] array2 = (float[][])pool2.get(i);
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j].wasUsed())
					{
						array[j].logNormalizeNonZero(array2[j]);
					}
				}
			}
		}
		
		private void updateMeans()
		{
			if (!HMMPoolManager.assertionsDisabled && this.meansPool.size() != this.meansBufferPool.size())
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < this.meansPool.size(); i++)
			{
				float[] array = (float[])this.meansPool.get(i);
				Buffer buffer = (Buffer)this.meansBufferPool.get(i);
				if (buffer.wasUsed())
				{
					float[] values = buffer.getValues();
					this.copyVector(values, array);
				}
				else
				{
					HMMPoolManager.logger.info(new StringBuilder().append("Senone ").append(i).append(" not used.").toString());
				}
			}
		}
		
		private void updateVariances()
		{
			if (!HMMPoolManager.assertionsDisabled && this.variancePool.size() != this.varianceBufferPool.size())
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < this.variancePool.size(); i++)
			{
				float[] array = (float[])this.meansPool.get(i);
				float[] array2 = (float[])this.variancePool.get(i);
				Buffer buffer = (Buffer)this.varianceBufferPool.get(i);
				if (buffer.wasUsed())
				{
					float[] values = buffer.getValues();
					if (!HMMPoolManager.assertionsDisabled && array.Length != values.Length)
					{
						
						throw new AssertionError();
					}
					for (int j = 0; j < array.Length; j++)
					{
						float[] array3 = values;
						int num = j;
						float[] array4 = array3;
						array4[num] -= array[j] * array[j];
						if (values[j] < this.varianceFloor)
						{
							values[j] = this.varianceFloor;
						}
					}
					this.copyVector(values, array2);
				}
			}
		}
		
		private void recomputeMixtureComponents()
		{
			for (int i = 0; i < this.senonePool.size(); i++)
			{
				GaussianMixture gaussianMixture = (GaussianMixture)this.senonePool.get(i);
				MixtureComponent[] mixtureComponents = gaussianMixture.getMixtureComponents();
				MixtureComponent[] array = mixtureComponents;
				int num = array.Length;
				for (int j = 0; j < num; j++)
				{
					MixtureComponent mixtureComponent = array[j];
					mixtureComponent.precomputeDistance();
				}
			}
		}
		
		private void updateMixtureWeights()
		{
			int statesNum = this.mixtureWeights.getStatesNum();
			int streamsNum = this.mixtureWeights.getStreamsNum();
			if (!HMMPoolManager.assertionsDisabled && statesNum * streamsNum != this.mixtureWeightsBufferPool.size())
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < streamsNum; i++)
			{
				for (int j = 0; j < statesNum; j++)
				{
					int id = i * statesNum + j;
					Buffer buffer = (Buffer)this.mixtureWeightsBufferPool.get(id);
					if (buffer.wasUsed())
					{
						if (buffer.logFloor(this.logMixtureWeightFloor))
						{
							buffer.logNormalizeToSum(this.logMath);
						}
						float[] values = buffer.getValues();
						this.mixtureWeights.put(j, i, values);
					}
				}
			}
		}
		
		private void updateTransitionMatrices()
		{
			if (!HMMPoolManager.assertionsDisabled && this.matrixPool.size() != this.matrixBufferPool.size())
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < this.matrixPool.size(); i++)
			{
				float[][] array = (float[][])this.matrixPool.get(i);
				Buffer[] array2 = (Buffer[])this.matrixBufferPool.get(i);
				for (int j = 0; j < array.Length; j++)
				{
					Buffer buffer = array2[j];
					if (buffer.wasUsed())
					{
						for (int k = 0; k < array[j].Length; k++)
						{
							float value = buffer.getValue(k);
							if (value != -3.40282347E+38f)
							{
								if (!HMMPoolManager.assertionsDisabled && array[j][k] == -3.40282347E+38f)
								{
									
									throw new AssertionError();
								}
								if (value < this.logTransitionProbabilityFloor)
								{
									buffer.setValue(k, this.logTransitionProbabilityFloor);
								}
							}
						}
						buffer.logNormalizeToSum(this.logMath);
						this.copyVector(buffer.getValues(), array[j]);
					}
				}
			}
		}
		
		private void copyVector(float[] array, float[] array2)
		{
			if (!HMMPoolManager.assertionsDisabled && array.Length != array2.Length)
			{
				
				throw new AssertionError();
			}
			ByteCodeHelper.arraycopy_primitive_4(array, 0, array2, 0, array.Length);
		}
		
		protected internal HMMPoolManager(Loader loader)
		{
			loader.load();
			this.hmmManager = loader.getHMMManager();
			this.indexMap = new HashMap();
			this.meansPool = loader.getMeansPool();
			this.variancePool = loader.getVariancePool();
			this.mixtureWeights = loader.getMixtureWeights();
			this.matrixPool = loader.getTransitionMatrixPool();
			this.senonePool = loader.getSenonePool();
			this.createBuffers();
			this.logLikelihood = 0f;
			this.logMath = LogMath.getLogMath();
		}
		
		protected internal void resetBuffers()
		{
			this.createBuffers();
			this.logLikelihood = 0f;
		}
		
		protected internal void accumulate(int num, TrainerScore[] array)
		{
			this.accumulate(num, array, null);
		}

		protected internal void updateLogLikelihood()
		{
		}
		
		protected internal float normalize()
		{
			this.normalizePool(this.meansBufferPool);
			this.normalizePool(this.varianceBufferPool);
			this.logNormalizePool(this.mixtureWeightsBufferPool);
			this.logNormalize2DPool(this.matrixBufferPool, this.matrixPool);
			return this.logLikelihood;
		}
		
		protected internal void update()
		{
			this.updateMeans();
			this.updateVariances();
			this.recomputeMixtureComponents();
			this.updateMixtureWeights();
			this.updateTransitionMatrices();
		}

		static HMMPoolManager()
		{
			HMMPoolManager.logger = Logger.getLogger("edu.cmu.sphinx.linguist.acoustic.HMMPoolManager");
		}

		private HMMManager hmmManager;
		
		private HashMap indexMap;

		private Pool meansPool;

		private Pool variancePool;
		
		private Pool matrixPool;

		private GaussianWeights mixtureWeights;
		
		private Pool meansBufferPool;
		
		private Pool varianceBufferPool;
		
		private Pool matrixBufferPool;

		private Pool mixtureWeightsBufferPool;
		
		private Pool senonePool;

		private LogMath logMath;

		private float logMixtureWeightFloor;

		private float logTransitionProbabilityFloor;

		private float varianceFloor;

		private float logLikelihood;

		private float currentLogLikelihood;

		private static Logger logger;

		internal static bool assertionsDisabled = !ClassLiteral<HMMPoolManager>.Value.desiredAssertionStatus();
	}
}

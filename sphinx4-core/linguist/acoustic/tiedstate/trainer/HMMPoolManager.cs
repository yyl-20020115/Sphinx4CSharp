using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer
{
	
	internal sealed class HMMPoolManager : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			44,
			115,
			115,
			115,
			114
		})]
		
		protected internal void createBuffers()
		{
			this.meansBufferPool = this.create1DPoolBuffer(this.meansPool, false);
			this.varianceBufferPool = this.create1DPoolBuffer(this.variancePool, false);
			this.matrixBufferPool = this.create2DPoolBuffer(this.matrixPool, true);
			this.mixtureWeightsBufferPool = this.createWeightsPoolBuffer(this.mixtureWeights);
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			117,
			162,
			140,
			107,
			109,
			115,
			112,
			233,
			60,
			230,
			70
		})]
		
		private Pool create1DPoolBuffer(Pool pool, bool flag)
		{
			Pool pool2 = new Pool(pool.getName());
			for (int i = 0; i < pool.size(); i++)
			{
				float[] array = (float[])pool.get(i);
				this.indexMap.put(array, Integer.valueOf(i));
				Buffer.__<clinit>();
				Buffer o = new Buffer(array.Length, flag, i);
				pool2.put(i, o);
			}
			return pool2;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			110,
			162,
			140,
			110,
			109,
			115,
			100,
			105,
			105,
			55,
			168,
			233,
			56,
			233,
			74
		})]
		
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
					Buffer.__<clinit>();
					array3[num2] = new Buffer(array[j].Length, flag, j);
				}
				pool2.put(i, array2);
			}
			return pool2;
		}

		
		[LineNumberTable(new byte[]
		{
			65,
			108,
			103,
			103,
			103,
			104,
			104,
			105,
			107,
			234,
			61,
			40,
			232,
			71
		})]
		
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

		[LineNumberTable(new byte[]
		{
			115,
			228,
			70,
			139,
			150,
			108,
			131,
			103,
			100,
			106,
			106,
			106,
			236,
			70,
			104,
			178,
			106,
			170
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			87,
			100,
			112,
			40,
			203,
			114,
			103,
			106,
			138,
			121,
			119,
			119,
			116,
			114,
			106,
			106,
			108,
			144,
			106,
			47,
			168,
			235,
			48,
			233,
			83
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			118,
			100,
			112,
			40,
			203,
			114,
			103,
			106,
			106,
			138,
			121,
			116,
			114,
			106,
			106,
			108,
			112,
			106,
			114,
			28,
			200,
			235,
			49,
			233,
			82
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			149,
			100,
			112,
			40,
			200,
			114,
			112,
			105,
			106,
			238,
			61,
			230,
			70
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			7,
			164,
			127,
			1,
			107,
			53,
			166,
			228,
			69,
			100,
			170
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			237,
			199,
			228,
			69,
			183,
			178,
			169,
			107,
			242,
			60,
			232,
			71
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			171,
			105,
			131,
			129,
			103,
			108,
			199,
			184,
			180,
			133,
			141,
			239,
			73,
			166,
			198,
			118,
			45,
			139,
			106,
			107,
			103,
			107,
			112,
			140,
			243,
			37,
			235,
			100
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			161,
			49,
			117,
			107,
			109,
			104,
			230,
			61,
			230,
			70
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			161,
			64,
			117,
			107,
			109,
			104,
			230,
			61,
			230,
			70
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			161,
			81,
			117,
			110,
			109,
			109,
			103,
			106,
			11,
			230,
			61,
			233,
			73
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			115,
			127,
			11,
			115,
			114,
			114,
			104,
			103,
			104,
			98,
			255,
			15,
			57,
			233,
			74
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			130,
			127,
			11,
			115,
			114,
			114,
			114,
			107,
			104,
			121,
			105,
			126,
			109,
			235,
			61,
			232,
			70,
			233,
			51,
			233,
			80
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			151,
			115,
			114,
			103,
			117,
			40,
			232,
			61,
			233,
			71
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			162,
			108,
			108,
			127,
			3,
			105,
			105,
			103,
			116,
			105,
			111,
			141,
			105,
			239,
			56,
			41,
			233,
			77
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			182,
			127,
			11,
			115,
			114,
			114,
			106,
			101,
			108,
			110,
			107,
			105,
			127,
			0,
			106,
			239,
			59,
			235,
			73,
			109,
			240,
			51,
			233,
			61,
			233,
			84
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			109,
			120,
			107
		})]
		
		private void copyVector(float[] array, float[] array2)
		{
			if (!HMMPoolManager.assertionsDisabled && array.Length != array2.Length)
			{
				
				throw new AssertionError();
			}
			ByteCodeHelper.arraycopy_primitive_4(array, 0, array2, 0, array.Length);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			8,
			104,
			102,
			108,
			107,
			108,
			108,
			108,
			108,
			236,
			77,
			102,
			107,
			107
		})]
		
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

		[LineNumberTable(new byte[]
		{
			36,
			102,
			107
		})]
		
		protected internal void resetBuffers()
		{
			this.createBuffers();
			this.logLikelihood = 0f;
		}

		[LineNumberTable(new byte[]
		{
			103,
			105
		})]
		
		protected internal void accumulate(int num, TrainerScore[] array)
		{
			this.accumulate(num, array, null);
		}

		protected internal void updateLogLikelihood()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			36,
			108,
			108,
			108,
			114
		})]
		
		protected internal float normalize()
		{
			this.normalizePool(this.meansBufferPool);
			this.normalizePool(this.varianceBufferPool);
			this.logNormalizePool(this.mixtureWeightsBufferPool);
			this.logNormalize2DPool(this.matrixBufferPool, this.matrixPool);
			return this.logLikelihood;
		}

		[LineNumberTable(new byte[]
		{
			161,
			95,
			102,
			102,
			102,
			102,
			102
		})]
		
		protected internal void update()
		{
			this.updateMeans();
			this.updateVariances();
			this.recomputeMixtureComponents();
			this.updateMixtureWeights();
			this.updateTransitionMatrices();
		}

		[LineNumberTable(new byte[]
		{
			159,
			168,
			245,
			88
		})]
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

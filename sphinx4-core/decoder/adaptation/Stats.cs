using edu.cmu.sphinx.api;
using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.util;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.decoder.adaptation
{
	public class Stats : Object
	{	
		public Stats(Loader loader, ClusteredDensityFileData means)
		{
			this.logMath = LogMath.getLogMath();
			this.loader = (Sphinx3Loader)loader;
			this.nClusters = means.getNumberOfClusters();
			this.means = means;
			this.varFlor = 1E-05f;
			this.invertVariances();
			this.init();
			this.nFrames = 0;
		}
		
		private void invertVariances()
		{
			for (int i = 0; i < this.loader.getNumStates(); i++)
			{
				for (int j = 0; j < this.loader.getNumGaussiansPerState(); j++)
				{
					for (int k = 0; k < this.loader.getVectorLength()[0]; k++)
					{
						if ((double)((float[])this.loader.getVariancePool().get(i * this.loader.getNumGaussiansPerState() + j))[k] <= (double)0f)
						{
							((float[])this.loader.getVariancePool().get(i * this.loader.getNumGaussiansPerState() + j))[k] = 0.5f;
						}
						else if (((float[])this.loader.getVariancePool().get(i * this.loader.getNumGaussiansPerState() + j))[k] < this.varFlor)
						{
							((float[])this.loader.getVariancePool().get(i * this.loader.getNumGaussiansPerState() + j))[k] = (float)((double)1f / (double)this.varFlor);
						}
						else
						{
							((float[])this.loader.getVariancePool().get(i * this.loader.getNumGaussiansPerState() + j))[k] = (float)((double)1f / (double)((float[])this.loader.getVariancePool().get(i * this.loader.getNumGaussiansPerState() + j))[k]);
						}
					}
				}
			}
		}		
		private void init()
		{
			int num = this.loader.getVectorLength()[0];
			this.regLs = new double[this.nClusters][][][][];
			this.regRs = new double[this.nClusters][][][];
			for (int i = 0; i < this.nClusters; i++)
			{
				this.regLs[i] = new double[this.loader.getNumStreams()][][][];
				this.regRs[i] = new double[this.loader.getNumStreams()][][];
				for (int j = 0; j < this.loader.getNumStreams(); j++)
				{
					int num2 = this.loader.getVectorLength()[j];
					double[][][][] array = this.regLs[i];
					int num3 = j;
					int num4 = num2;
					int num5 = num2 + 1;
					int num6 = num2 + 1;
					int[] array2 = new int[3];
					int num7 = num6;
					array2[2] = num7;
					num7 = num5;
					array2[1] = num7;
					num7 = num4;
					array2[0] = num7;
					array[num3] = (double[][][])ByteCodeHelper.multianewarray(typeof(double[][][]).TypeHandle, array2);
					double[][][] array3 = this.regRs[i];
					int num8 = j;
					int num9 = num2;
					int num10 = num2 + 1;
					array2 = new int[2];
					num7 = num10;
					array2[1] = num7;
					num7 = num9;
					array2[0] = num7;
					array3[num8] = (double[][])ByteCodeHelper.multianewarray(typeof(double[][]).TypeHandle, array2);
				}
			}
		}
		
		private float[] computePosterios(float[] array, int num)
		{
			int num2 = array.Length;
			int num3 = (num != -1) ? (num2 / num) : (-num2);
			int num4 = 0;
			for (int i = 0; i < num; i++)
			{
				float num5 = array[num4];
				for (int j = num4 + 1; j < num4 + num3; j++)
				{
					if (array[j] > num5)
					{
						num5 = array[j];
					}
				}
				for (int j = num4; j < num4 + num3; j++)
				{
					array[j] = (float)this.logMath.logToLinear(array[j] - num5);
				}
				num4 += num3;
			}
			return array;
		}

		public virtual ClusteredDensityFileData getClusteredData()
		{
			return this.means;
		}

		public virtual double[][][][][] getRegLs()
		{
			return this.regLs;
		}

		public virtual double[][][][] getRegRs()
		{
			return this.regRs;
		}
	
		public virtual void collect(SpeechResult result)
		{
			Token token = result.getResult().getBestToken();
			if (token == null)
			{
				string text = "Best token not found!";
				
				throw new Exception(text);
			}
			do
			{
				FloatData floatData = (FloatData)token.getData();
				SearchState searchState = token.getSearchState();
				if (!(searchState is HMMSearchState) || !searchState.isEmitting())
				{
					token = token.getPredecessor();
				}
				else
				{
					this.nFrames++;
					float[] array = token.calculateComponentScore(floatData);
					float[] values = FloatData.toFloatData(floatData).getValues();
					int num = (int)((HMMSearchState)token.getSearchState()).getHMMState().getMixtureId();
					if (this.loader is Sphinx3Loader && this.loader.hasTiedMixtures())
					{
						num = this.loader.getSenone2Ci()[num];
					}
					int[] vectorLength = this.loader.getVectorLength();
					int numStreams = this.loader.getNumStreams();
					int numGaussiansPerState = this.loader.getNumGaussiansPerState();
					float[] array2 = this.computePosterios(array, numStreams);
					int num2 = 0;
					for (int i = 0; i < numStreams; i++)
					{
						for (int j = 0; j < numGaussiansPerState; j++)
						{
							int classIndex = this.means.getClassIndex(num * numStreams * numGaussiansPerState + i * numGaussiansPerState + j);
							float num3 = array2[i * numGaussiansPerState + j];
							if ((double)num3 > (double)0f)
							{
								float[] array3 = (float[])this.loader.getMeansPool().get(num * numStreams * numGaussiansPerState + i * numGaussiansPerState + j);
								for (int k = 0; k < vectorLength[i]; k++)
								{
									float num4 = array2[i * numGaussiansPerState + j] * values[k + num2];
									float num5 = num4 * ((float[])this.loader.getVariancePool().get(num * numStreams * numGaussiansPerState + i * numGaussiansPerState + j))[k];
									float num6 = num3 * ((float[])this.loader.getVariancePool().get(num * numStreams * numGaussiansPerState + i * numGaussiansPerState + j))[k];
									int num8;
									double[] array5;
									for (int l = 0; l < vectorLength[i]; l++)
									{
										float num7 = num6 * array3[l];
										for (int m = l; m < vectorLength[i]; m++)
										{
											double[] array4 = this.regLs[classIndex][i][k][l];
											num8 = m;
											array5 = array4;
											array5[num8] += (double)(num7 * array3[m]);
										}
										double[] array6 = this.regLs[classIndex][i][k][l];
										num8 = vectorLength[i];
										array5 = array6;
										array5[num8] += (double)num7;
										double[] array7 = this.regRs[classIndex][i][k];
										num8 = l;
										array5 = array7;
										array5[num8] += (double)(num5 * array3[l]);
									}
									double[] array8 = this.regLs[classIndex][i][k][vectorLength[i]];
									num8 = vectorLength[i];
									array5 = array8;
									array5[num8] += (double)num6;
									double[] array9 = this.regRs[classIndex][i][k];
									num8 = vectorLength[i];
									array5 = array9;
									array5[num8] += (double)num5;
								}
							}
						}
						num2 += vectorLength[i];
					}
					token = token.getPredecessor();
				}
			}
			while (token != null);
		}
	
		public virtual void fillRegLowerPart()
		{
			for (int i = 0; i < this.nClusters; i++)
			{
				for (int j = 0; j < this.loader.getNumStreams(); j++)
				{
					for (int k = 0; k < this.loader.getVectorLength()[j]; k++)
					{
						for (int l = 0; l <= this.loader.getVectorLength()[j]; l++)
						{
							for (int m = l + 1; m <= this.loader.getVectorLength()[j]; m++)
							{
								this.regLs[i][j][k][m][l] = this.regLs[i][j][k][l][m];
							}
						}
					}
				}
			}
		}	
		public virtual Transform createTransform()
		{
			if (this.nFrames < 300 * this.nClusters)
			{
				return null;
			}
			Transform transform = new Transform(this.loader, this.nClusters);
			transform.update(this);
			return transform;
		}

		public virtual int getFrames()
		{
			return this.nFrames;
		}

		private const int MIN_FRAMES = 300;

		private ClusteredDensityFileData means;

		private double[][][][][] regLs;

		private double[][][][] regRs;

		private int nClusters;

		private Sphinx3Loader loader;

		private float varFlor;

		private LogMath logMath;

		private int nFrames;
	}
}

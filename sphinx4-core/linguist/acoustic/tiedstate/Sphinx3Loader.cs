using System;

using edu.cmu.sphinx.decoder.adaptation;
using edu.cmu.sphinx.linguist.acoustic.tiedstate.tiedmixture;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.acoustic.tiedstate.Loader"
	})]
	public class Sphinx3Loader : java.lang.Object, Loader, Configurable
	{
		
		public static void __<clinit>()
		{
		}

		public virtual int[] getVectorLength()
		{
			return this.vectorLength;
		}

		public virtual int getNumStreams()
		{
			return this.numStreams;
		}

		public virtual int getNumStates()
		{
			return this.numStates;
		}

		public virtual int getNumGaussiansPerState()
		{
			return this.numGaussiansPerState;
		}

		
		public virtual Pool getVariancePool()
		{
			return this.variancePool;
		}

		[LineNumberTable(new byte[]
		{
			160,
			99,
			118
		})]
		
		public virtual bool hasTiedMixtures()
		{
			string property = this.modelProps.getProperty("-model", "cont");
			return java.lang.String.instancehelper_equals(property, "ptm");
		}

		public virtual int[] getSenone2Ci()
		{
			return this.senone2ci;
		}

		
		public virtual Pool getMeansPool()
		{
			return this.meansPool;
		}

		[LineNumberTable(new byte[]
		{
			164,
			8,
			104,
			97,
			116,
			39,
			134
		})]
		
		public virtual void setGauScoresQueueLength(int scoresQueueLen)
		{
			if (this.phoneticTiedMixtures == null)
			{
				return;
			}
			MixtureComponentSet[] array = this.phoneticTiedMixtures;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				MixtureComponentSet mixtureComponentSet = array[i];
				mixtureComponentSet.setScoreQueueLength(scoresQueueLen);
			}
		}

		[LineNumberTable(new byte[]
		{
			164,
			1,
			104,
			97,
			116,
			38,
			134
		})]
		
		public virtual void clearGauScores()
		{
			if (this.phoneticTiedMixtures == null)
			{
				return;
			}
			MixtureComponentSet[] array = this.phoneticTiedMixtures;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				MixtureComponentSet mixtureComponentSet = array[i];
				mixtureComponentSet.clearStoredScores();
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			99,
			99,
			107,
			103,
			104,
			103,
			104,
			105,
			105,
			104,
			103
		})]
		
		protected internal virtual void init(URL location, UnitManager unitManager, float distFloor, float mixtureWeightFloor, float varianceFloor, int topGauNum, bool useCDUnits, Logger logger)
		{
			this.logMath = LogMath.getLogMath();
			this.location = location;
			this.logger = logger;
			this.unitManager = unitManager;
			this.distFloor = distFloor;
			this.mixtureWeightFloor = mixtureWeightFloor;
			this.varianceFloor = varianceFloor;
			this.topGauNum = topGauNum;
			this.useCDUnits = useCDUnits;
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.net.URISyntaxException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			184,
			118,
			151,
			119,
			113,
			113,
			145,
			136,
			134,
			186,
			216,
			108,
			99,
			144,
			109
		})]
		
		protected internal virtual void loadModelFiles()
		{
			this.meansPool = this.loadDensityFile("means", float.MinValue);
			this.variancePool = this.loadDensityFile("variances", this.varianceFloor);
			this.mixtureWeights = this.loadMixtureWeights("mixture_weights", this.mixtureWeightFloor);
			this.transitionsPool = this.loadTransitionMatrices("transition_matrices");
			this.transformMatrix = this.loadTransformMatrix("feature_transform");
			this.modelProps = this.loadModelProps("feat.params");
			if (this.hasTiedMixtures())
			{
				this.getSenoneToCIPhone();
				this.senonePool = this.createTiedSenonePool(this.distFloor, this.varianceFloor);
			}
			else
			{
				this.senonePool = this.createSenonePool(this.distFloor, this.varianceFloor);
			}
			InputStream dataStream = this.getDataStream("mdef");
			if (dataStream == null)
			{
				string text = "can't find model definition";
				
				throw new IOException(text);
			}
			this.loadHMMPool(this.useCDUnits, dataStream);
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.net.URISyntaxException"
		})]
		
		[LineNumberTable(new byte[]
		{
			161,
			152,
			102,
			130,
			137,
			140,
			112,
			191,
			6,
			109,
			120,
			134,
			105,
			105,
			137,
			105,
			105,
			44,
			200,
			137,
			127,
			7,
			127,
			7,
			127,
			7,
			127,
			8,
			159,
			7,
			105,
			40,
			200,
			157,
			104,
			110,
			110,
			142,
			108,
			105,
			105,
			110,
			105,
			250,
			61,
			40,
			43,
			235,
			75,
			137,
			134,
			104,
			104,
			104,
			136
		})]
		
		public virtual Pool loadDensityFile(string path, float floor)
		{
			Properties properties = new Properties();
			int num = 0;
			DataInputStream dataInputStream = this.readS3BinaryHeader(path, properties);
			string property = properties.getProperty("version");
			if (property == null || !java.lang.String.instancehelper_equals(property, "1.0"))
			{
				string text = new StringBuilder().append("Unsupported version in ").append(path).toString();
				
				throw new IOException(text);
			}
			string property2 = properties.getProperty("chksum0");
			int num2 = (property2 == null || !java.lang.String.instancehelper_equals(property2, "yes")) ? 0 : 1;
			this.resetChecksum();
			int num3 = this.readInt(dataInputStream);
			int num4 = this.readInt(dataInputStream);
			int num5 = this.readInt(dataInputStream);
			int[] array = new int[num4];
			int i;
			for (i = 0; i < num4; i++)
			{
				array[i] = this.readInt(dataInputStream);
			}
			i = this.readInt(dataInputStream);
			this.logger.fine(new StringBuilder().append("Number of states ").append(num3).toString());
			this.logger.fine(new StringBuilder().append("Number of streams ").append(num4).toString());
			this.logger.fine(new StringBuilder().append("Number of gaussians per state ").append(num5).toString());
			this.logger.fine(new StringBuilder().append("Vector length ").append(array.Length).toString());
			this.logger.fine(new StringBuilder().append("Raw length ").append(i).toString());
			for (int j = 0; j < num4; j++)
			{
				num += array[j];
			}
			if (!Sphinx3Loader.assertionsDisabled && i != num5 * num * num3)
			{
				
				throw new AssertionError();
			}
			Pool pool = new Pool(path);
			pool.setFeature(Pool.Feature.__NUM_SENONES, num3);
			pool.setFeature(Pool.Feature.__NUM_STREAMS, num4);
			pool.setFeature(Pool.Feature.__NUM_GAUSSIANS_PER_STATE, num5);
			for (int k = 0; k < num3; k++)
			{
				for (int l = 0; l < num4; l++)
				{
					for (int m = 0; m < num5; m++)
					{
						float[] array2 = this.readFloatArray(dataInputStream, array[l]);
						Utilities.floorData(array2, floor);
						pool.put(k * num4 * num5 + l * num5 + m, array2);
					}
				}
			}
			this.validateChecksum(dataInputStream, num2 != 0);
			dataInputStream.close();
			this.numStates = num3;
			this.numStreams = num4;
			this.numGaussiansPerState = num5;
			this.vectorLength = array;
			return pool;
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.net.URISyntaxException"
		})]
		[LineNumberTable(new byte[]
		{
			163,
			101,
			159,
			6,
			134,
			137,
			140,
			112,
			191,
			6,
			108,
			118,
			134,
			105,
			105,
			105,
			105,
			142,
			127,
			7,
			127,
			7,
			159,
			7,
			158,
			105,
			105,
			139,
			103,
			105,
			109,
			237,
			58,
			40,
			235,
			75,
			137,
			102
		})]
		
		protected internal virtual GaussianWeights loadMixtureWeights(string path, float floor)
		{
			this.logger.fine(new StringBuilder().append("Loading mixture weights from: ").append(path).toString());
			Properties properties = new Properties();
			DataInputStream dataInputStream = this.readS3BinaryHeader(path, properties);
			string property = properties.getProperty("version");
			if (property == null || !java.lang.String.instancehelper_equals(property, "1.0"))
			{
				string text = new StringBuilder().append("Unsupported version in ").append(path).toString();
				
				throw new IOException(text);
			}
			string property2 = properties.getProperty("chksum0");
			int num = (property2 == null || !java.lang.String.instancehelper_equals(property2, "yes")) ? 0 : 1;
			this.resetChecksum();
			int num2 = this.readInt(dataInputStream);
			int num3 = this.readInt(dataInputStream);
			int num4 = this.readInt(dataInputStream);
			int num5 = this.readInt(dataInputStream);
			GaussianWeights gaussianWeights = new GaussianWeights(path, num2, num4, num3);
			this.logger.fine(new StringBuilder().append("Number of states ").append(num2).toString());
			this.logger.fine(new StringBuilder().append("Number of streams ").append(num3).toString());
			this.logger.fine(new StringBuilder().append("Number of gaussians per state ").append(num4).toString());
			if (!Sphinx3Loader.assertionsDisabled && num5 != num2 * num3 * num4)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num3; j++)
				{
					float[] array = this.readFloatArray(dataInputStream, num4);
					Utilities.normalize(array);
					Utilities.floorData(array, floor);
					this.logMath.linearToLog(array);
					gaussianWeights.put(i, j, array);
				}
			}
			this.validateChecksum(dataInputStream, num != 0);
			dataInputStream.close();
			return gaussianWeights;
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.net.URISyntaxException"
		})]
		
		[LineNumberTable(new byte[]
		{
			163,
			158,
			159,
			6,
			102,
			137,
			140,
			112,
			191,
			6,
			108,
			118,
			134,
			136,
			105,
			105,
			105,
			137,
			158,
			108,
			137,
			110,
			146,
			105,
			110,
			111,
			106,
			240,
			60,
			232,
			70,
			235,
			52,
			235,
			79,
			137,
			102
		})]
		
		protected internal virtual Pool loadTransitionMatrices(string path)
		{
			this.logger.fine(new StringBuilder().append("Loading transition matrices from: ").append(path).toString());
			Properties properties = new Properties();
			DataInputStream dataInputStream = this.readS3BinaryHeader(path, properties);
			string property = properties.getProperty("version");
			if (property == null || !java.lang.String.instancehelper_equals(property, "1.0"))
			{
				string text = new StringBuilder().append("Unsupported version in ").append(path).toString();
				
				throw new IOException(text);
			}
			string property2 = properties.getProperty("chksum0");
			int num = (property2 == null || !java.lang.String.instancehelper_equals(property2, "yes")) ? 0 : 1;
			this.resetChecksum();
			Pool pool = new Pool(path);
			int num2 = this.readInt(dataInputStream);
			int num3 = this.readInt(dataInputStream);
			int num4 = this.readInt(dataInputStream);
			int num5 = this.readInt(dataInputStream);
			if (!Sphinx3Loader.assertionsDisabled && num5 != num4 * num3 * num2)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < num2; i++)
			{
				float[][] array = new float[num4][];
				array[num4 - 1] = new float[num4];
				this.logMath.linearToLog(array[num4 - 1]);
				for (int j = 0; j < num3; j++)
				{
					array[j] = this.readFloatArray(dataInputStream, num4);
					Utilities.nonZeroFloor(array[j], 0f);
					Utilities.normalize(array[j]);
					this.logMath.linearToLog(array[j]);
				}
				pool.put(i, array);
			}
			this.validateChecksum(dataInputStream, num != 0);
			dataInputStream.close();
			return pool;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			163,
			215,
			159,
			6,
			198,
			255,
			1,
			69,
			226,
			60,
			97,
			108,
			97,
			167,
			141,
			114,
			191,
			6,
			109,
			120,
			134,
			104,
			105,
			105,
			137,
			155,
			105,
			105,
			46,
			200,
			137,
			102
		})]
		
		protected internal virtual float[][] loadTransformMatrix(string path)
		{
			this.logger.fine(new StringBuilder().append("Loading transform matrix from: ").append(path).toString());
			Properties properties = new Properties();
			DataInputStream dataInputStream;
			URISyntaxException ex2;
			try
			{
				try
				{
					dataInputStream = this.readS3BinaryHeader(path, properties);
				}
				catch (URISyntaxException ex)
				{
					ex2 = ByteCodeHelper.MapException<URISyntaxException>(ex, 1);
					goto IL_45;
				}
			}
			catch (IOException ex3)
			{
				goto IL_48;
			}
			string property = properties.getProperty("version");
			if (property == null || !java.lang.String.instancehelper_equals(property, "0.1"))
			{
				string text = new StringBuilder().append("Unsupported version in ").append(path).toString();
				
				throw new IOException(text);
			}
			string property2 = properties.getProperty("chksum0");
			int num = (property2 == null || !java.lang.String.instancehelper_equals(property2, "yes")) ? 0 : 1;
			this.resetChecksum();
			this.readInt(dataInputStream);
			int num2 = this.readInt(dataInputStream);
			int num3 = this.readInt(dataInputStream);
			int num4 = this.readInt(dataInputStream);
			if (!Sphinx3Loader.assertionsDisabled && num4 != num2 * num3)
			{
				
				throw new AssertionError();
			}
			float[][] array = new float[num2][];
			for (int i = 0; i < num2; i++)
			{
				array[i] = this.readFloatArray(dataInputStream, num3);
			}
			this.validateChecksum(dataInputStream, num != 0);
			dataInputStream.close();
			return array;
			IL_45:
			URISyntaxException ex4 = ex2;
			Exception ex5 = ex4;
			
			throw new RuntimeException(ex5);
			IL_48:
			return (float[][])null;
		}

		[Throws(new string[]
		{
			"java.net.MalformedURLException",
			"java.io.IOException",
			"java.net.URISyntaxException"
		})]
		[LineNumberTable(new byte[]
		{
			164,
			95,
			102,
			98,
			144,
			106,
			108,
			109,
			98
		})]
		
		protected internal virtual Properties loadModelProps(string path)
		{
			Properties properties = new Properties();
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(this.getDataStream(path)));
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				string[] array = java.lang.String.instancehelper_split(text, " ");
				properties.put(array[0], array[1]);
			}
			return properties;
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.net.URISyntaxException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			218,
			108,
			99,
			144,
			170,
			159,
			11,
			139,
			113,
			139,
			108,
			139,
			108,
			139,
			109,
			139,
			141,
			108,
			139,
			109,
			139,
			148,
			127,
			2,
			191,
			2,
			146,
			104,
			39,
			136,
			141,
			107,
			52,
			168,
			139,
			248,
			53,
			235,
			78,
			102
		})]
		
		private void getSenoneToCIPhone()
		{
			InputStream dataStream = this.getDataStream("mdef");
			if (dataStream == null)
			{
				string text = "can't find model definition";
				
				throw new IOException(text);
			}
			ExtendedStreamTokenizer extendedStreamTokenizer = new ExtendedStreamTokenizer(dataStream, 35, false);
			this.logger.fine(new StringBuilder().append("Loading HMM file from ").append(this.location).toString());
			extendedStreamTokenizer.expectString("0.3");
			this.numBase = extendedStreamTokenizer.getInt("numBase");
			extendedStreamTokenizer.expectString("n_base");
			int @int = extendedStreamTokenizer.getInt("numTri");
			extendedStreamTokenizer.expectString("n_tri");
			int int2 = extendedStreamTokenizer.getInt("numStateMap");
			extendedStreamTokenizer.expectString("n_state_map");
			int int3 = extendedStreamTokenizer.getInt("numTiedState");
			extendedStreamTokenizer.expectString("n_tied_state");
			this.senone2ci = new int[int3];
			extendedStreamTokenizer.getInt("numContextIndependentTiedState");
			extendedStreamTokenizer.expectString("n_tied_ci_state");
			int int4 = extendedStreamTokenizer.getInt("numTiedTransitionMatrices");
			extendedStreamTokenizer.expectString("n_tied_tmat");
			int num = int2;
			int num2 = @int + this.numBase;
			int num3 = (num2 != -1) ? (num / num2) : (-num);
			if (!Sphinx3Loader.assertionsDisabled && int3 != this.mixtureWeights.getStatesNum())
			{
				
				throw new AssertionError();
			}
			if (!Sphinx3Loader.assertionsDisabled && int4 != this.transitionsPool.size())
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < this.numBase + @int; i++)
			{
				int j;
				for (j = 0; j < 5; j++)
				{
					extendedStreamTokenizer.getString();
				}
				j = extendedStreamTokenizer.getInt("tmat");
				for (int k = 0; k < num3 - 1; k++)
				{
					this.senone2ci[extendedStreamTokenizer.getInt("j")] = j;
				}
				extendedStreamTokenizer.expectString("N");
				if (!Sphinx3Loader.assertionsDisabled && j >= int4)
				{
					
					throw new AssertionError();
				}
			}
			extendedStreamTokenizer.close();
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			83,
			139,
			108,
			108,
			108,
			109,
			141,
			127,
			7,
			127,
			6,
			127,
			6,
			159,
			6,
			118,
			127,
			1,
			159,
			1,
			119,
			108,
			114,
			108,
			119,
			108,
			114,
			140,
			113,
			112,
			103,
			108,
			104,
			107,
			113,
			113,
			150,
			250,
			59,
			235,
			73,
			234,
			53,
			235,
			77,
			246,
			49,
			235,
			82,
			105,
			127,
			0,
			10,
			200
		})]
		
		private Pool createTiedSenonePool(float num, float num2)
		{
			Pool pool = new Pool("senones");
			int num3 = this.meansPool.size();
			int num4 = this.variancePool.size();
			int gauPerState = this.mixtureWeights.getGauPerState();
			int statesNum = this.mixtureWeights.getStatesNum();
			int streamsNum = this.mixtureWeights.getStreamsNum();
			this.logger.fine(new StringBuilder().append("Senones ").append(statesNum).toString());
			this.logger.fine(new StringBuilder().append("Gaussians Per State ").append(gauPerState).toString());
			this.logger.fine(new StringBuilder().append("Means ").append(num3).toString());
			this.logger.fine(new StringBuilder().append("Variances ").append(num4).toString());
			if (!Sphinx3Loader.assertionsDisabled && gauPerState <= 0)
			{
				
				throw new AssertionError();
			}
			if (!Sphinx3Loader.assertionsDisabled && num4 != this.numBase * gauPerState * streamsNum)
			{
				
				throw new AssertionError();
			}
			if (!Sphinx3Loader.assertionsDisabled && num3 != this.numBase * gauPerState * streamsNum)
			{
				
				throw new AssertionError();
			}
			float[][] meanTransformationMatrix = (this.meanTransformationMatrixPool != null) ? ((float[][])this.meanTransformationMatrixPool.get(0)) : ((float[][])null);
			float[] meanTransformationVector = (this.meanTransformationVectorPool != null) ? ((float[])this.meanTransformationVectorPool.get(0)) : null;
			float[][] varianceTransformationMatrix = (this.varianceTransformationMatrixPool != null) ? ((float[][])this.varianceTransformationMatrixPool.get(0)) : ((float[][])null);
			float[] varianceTransformationVector = (this.varianceTransformationVectorPool != null) ? ((float[])this.varianceTransformationVectorPool.get(0)) : null;
			this.phoneticTiedMixtures = new MixtureComponentSet[this.numBase];
			for (int i = 0; i < this.numBase; i++)
			{
				ArrayList arrayList = new ArrayList();
				for (int j = 0; j < streamsNum; j++)
				{
					PrunableMixtureComponent[] array = new PrunableMixtureComponent[gauPerState];
					for (int k = 0; k < gauPerState; k++)
					{
						int id = i * gauPerState * streamsNum + j * gauPerState + k;
						PrunableMixtureComponent[] array2 = array;
						int num5 = k;
						PrunableMixtureComponent.__<clinit>();
						array2[num5] = new PrunableMixtureComponent((float[])this.meansPool.get(id), meanTransformationMatrix, meanTransformationVector, (float[])this.variancePool.get(id), varianceTransformationMatrix, varianceTransformationVector, num, num2, k);
					}
					arrayList.add(array);
				}
				this.phoneticTiedMixtures[i] = new MixtureComponentSet(arrayList, this.topGauNum);
			}
			for (int i = 0; i < statesNum; i++)
			{
				SetBasedGaussianMixture o = new SetBasedGaussianMixture(this.mixtureWeights, this.phoneticTiedMixtures[this.senone2ci[i]], i);
				pool.put(i, o);
			}
			return pool;
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			26,
			139,
			108,
			108,
			108,
			109,
			109,
			131,
			127,
			7,
			127,
			6,
			127,
			6,
			159,
			6,
			118,
			121,
			153,
			119,
			108,
			114,
			108,
			119,
			108,
			114,
			140,
			108,
			139,
			104,
			113,
			150,
			216,
			230,
			56,
			232,
			75,
			113,
			234,
			49,
			235,
			81
		})]
		
		protected internal virtual Pool createSenonePool(float distFloor, float varianceFloor)
		{
			Pool pool = new Pool("senones");
			int num = this.meansPool.size();
			int num2 = this.variancePool.size();
			int gauPerState = this.mixtureWeights.getGauPerState();
			int statesNum = this.mixtureWeights.getStatesNum();
			int streamsNum = this.mixtureWeights.getStreamsNum();
			int num3 = 0;
			this.logger.fine(new StringBuilder().append("Senones ").append(statesNum).toString());
			this.logger.fine(new StringBuilder().append("Gaussians Per Senone ").append(gauPerState).toString());
			this.logger.fine(new StringBuilder().append("Means ").append(num).toString());
			this.logger.fine(new StringBuilder().append("Variances ").append(num2).toString());
			if (!Sphinx3Loader.assertionsDisabled && gauPerState <= 0)
			{
				
				throw new AssertionError();
			}
			if (!Sphinx3Loader.assertionsDisabled && num2 != statesNum * gauPerState)
			{
				
				throw new AssertionError();
			}
			if (!Sphinx3Loader.assertionsDisabled && num != statesNum * gauPerState)
			{
				
				throw new AssertionError();
			}
			float[][] meanTransformationMatrix = (this.meanTransformationMatrixPool != null) ? ((float[][])this.meanTransformationMatrixPool.get(0)) : ((float[][])null);
			float[] meanTransformationVector = (this.meanTransformationVectorPool != null) ? ((float[])this.meanTransformationVectorPool.get(0)) : null;
			float[][] varianceTransformationMatrix = (this.varianceTransformationMatrixPool != null) ? ((float[][])this.varianceTransformationMatrixPool.get(0)) : ((float[][])null);
			float[] varianceTransformationVector = (this.varianceTransformationVectorPool != null) ? ((float[])this.varianceTransformationVectorPool.get(0)) : null;
			for (int i = 0; i < statesNum; i++)
			{
				MixtureComponent[] array = new MixtureComponent[gauPerState * streamsNum];
				for (int j = 0; j < gauPerState; j++)
				{
					MixtureComponent[] array2 = array;
					int num4 = j;
					MixtureComponent.__<clinit>();
					array2[num4] = new MixtureComponent((float[])this.meansPool.get(num3), meanTransformationMatrix, meanTransformationVector, (float[])this.variancePool.get(num3), varianceTransformationMatrix, varianceTransformationVector, distFloor, varianceFloor);
					num3++;
				}
				GaussianMixture o = new GaussianMixture(this.mixtureWeights, array, i);
				pool.put(i, o);
			}
			return pool;
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.net.URISyntaxException"
		})]
		
		
		protected internal virtual InputStream getDataStream(string path)
		{
			URL.__<clinit>();
			return new URL(Utilities.pathJoin(this.location.toString(), path)).openStream();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			158,
			203,
			130,
			170,
			159,
			11,
			139,
			108,
			139,
			108,
			139,
			109,
			139,
			109,
			139,
			102,
			103,
			139,
			109,
			139,
			144,
			127,
			2,
			191,
			2,
			107,
			104,
			104,
			104,
			104,
			104,
			141,
			139,
			107,
			112,
			31,
			4,
			200,
			139,
			127,
			1,
			127,
			1,
			127,
			1,
			152,
			123,
			149,
			114,
			223,
			7,
			124,
			167,
			116,
			138,
			104,
			108,
			237,
			26,
			235,
			105,
			119,
			240,
			71,
			103,
			99,
			99,
			131,
			107,
			104,
			104,
			104,
			104,
			104,
			141,
			139,
			107,
			112,
			31,
			5,
			232,
			69,
			139,
			127,
			1,
			127,
			1,
			127,
			1,
			127,
			1,
			152,
			134,
			159,
			16,
			107,
			134,
			104,
			150,
			104,
			150,
			139,
			146,
			100,
			132,
			114,
			191,
			7,
			148,
			100,
			112,
			138,
			100,
			132,
			104,
			108,
			237,
			6,
			235,
			126,
			102
		})]
		
		protected internal virtual void loadHMMPool(bool useCDUnits, InputStream inputStream)
		{
			ExtendedStreamTokenizer extendedStreamTokenizer = new ExtendedStreamTokenizer(inputStream, 35, false);
			this.logger.fine(new StringBuilder().append("Loading HMM file from: ").append(this.location).toString());
			extendedStreamTokenizer.expectString("0.3");
			int @int = extendedStreamTokenizer.getInt("numBase");
			extendedStreamTokenizer.expectString("n_base");
			int int2 = extendedStreamTokenizer.getInt("numTri");
			extendedStreamTokenizer.expectString("n_tri");
			int int3 = extendedStreamTokenizer.getInt("numStateMap");
			extendedStreamTokenizer.expectString("n_state_map");
			int int4 = extendedStreamTokenizer.getInt("numTiedState");
			extendedStreamTokenizer.expectString("n_tied_state");
			int int5 = extendedStreamTokenizer.getInt("numContextIndependentTiedState");
			extendedStreamTokenizer.expectString("n_tied_ci_state");
			int int6 = extendedStreamTokenizer.getInt("numTiedTransitionMatrices");
			extendedStreamTokenizer.expectString("n_tied_tmat");
			int num = int3;
			int num2 = int2 + @int;
			int num3 = (num2 != -1) ? (num / num2) : (-num);
			if (!Sphinx3Loader.assertionsDisabled && int4 != this.mixtureWeights.getStatesNum())
			{
				
				throw new AssertionError();
			}
			if (!Sphinx3Loader.assertionsDisabled && int6 != this.transitionsPool.size())
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < @int; i++)
			{
				string @string = extendedStreamTokenizer.getString();
				string string2 = extendedStreamTokenizer.getString();
				string string3 = extendedStreamTokenizer.getString();
				string string4 = extendedStreamTokenizer.getString();
				string string5 = extendedStreamTokenizer.getString();
				int int7 = extendedStreamTokenizer.getInt("tmat");
				int[] array = new int[num3 - 1];
				for (int j = 0; j < num3 - 1; j++)
				{
					array[j] = extendedStreamTokenizer.getInt("j");
					if (!Sphinx3Loader.assertionsDisabled && (array[j] < 0 || array[j] >= int5))
					{
						
						throw new AssertionError();
					}
				}
				extendedStreamTokenizer.expectString("N");
				if (!Sphinx3Loader.assertionsDisabled && !java.lang.String.instancehelper_equals(string2, "-"))
				{
					
					throw new AssertionError();
				}
				if (!Sphinx3Loader.assertionsDisabled && !java.lang.String.instancehelper_equals(string3, "-"))
				{
					
					throw new AssertionError();
				}
				if (!Sphinx3Loader.assertionsDisabled && !java.lang.String.instancehelper_equals(string4, "-"))
				{
					
					throw new AssertionError();
				}
				if (!Sphinx3Loader.assertionsDisabled && int7 >= int6)
				{
					
					throw new AssertionError();
				}
				Unit unit = this.unitManager.getUnit(@string, java.lang.String.instancehelper_equals(string5, "filler"));
				this.contextIndependentUnits.put(unit.getName(), unit);
				if (this.logger.isLoggable(Level.FINE))
				{
					this.logger.fine(new StringBuilder().append("Loaded ").append(unit).toString());
				}
				if (unit.isFiller() && java.lang.String.instancehelper_equals(unit.getName(), "SIL"))
				{
					unit = UnitManager.__SILENCE;
				}
				float[][] transitionMatrix = (float[][])this.transitionsPool.get(int7);
				SenoneSequence senoneSequence = this.getSenoneSequence(array);
				SenoneHMM hmm = new SenoneHMM(unit, senoneSequence, transitionMatrix, HMMPosition.lookup(string4));
				this.hmmManager.put(hmm);
			}
			if (this.hmmManager.get(HMMPosition.__UNDEFINED, UnitManager.__SILENCE) == null)
			{
				string text = "Could not find SIL unit in acoustic model";
				
				throw new IOException(text);
			}
			string text2 = "";
			Unit unit2 = null;
			int[] ssid = null;
			SenoneSequence senoneSequence2 = null;
			for (int k = 0; k < int2; k++)
			{
				string string5 = extendedStreamTokenizer.getString();
				string string6 = extendedStreamTokenizer.getString();
				string string7 = extendedStreamTokenizer.getString();
				string string8 = extendedStreamTokenizer.getString();
				string string9 = extendedStreamTokenizer.getString();
				int int8 = extendedStreamTokenizer.getInt("tmat");
				int[] array2 = new int[num3 - 1];
				for (int l = 0; l < num3 - 1; l++)
				{
					array2[l] = extendedStreamTokenizer.getInt("j");
					if (!Sphinx3Loader.assertionsDisabled && (array2[l] < int5 || array2[l] >= int4))
					{
						
						throw new AssertionError();
					}
				}
				extendedStreamTokenizer.expectString("N");
				if (!Sphinx3Loader.assertionsDisabled && java.lang.String.instancehelper_equals(string6, "-"))
				{
					
					throw new AssertionError();
				}
				if (!Sphinx3Loader.assertionsDisabled && java.lang.String.instancehelper_equals(string7, "-"))
				{
					
					throw new AssertionError();
				}
				if (!Sphinx3Loader.assertionsDisabled && java.lang.String.instancehelper_equals(string8, "-"))
				{
					
					throw new AssertionError();
				}
				if (!Sphinx3Loader.assertionsDisabled && !java.lang.String.instancehelper_equals(string9, "n/a"))
				{
					
					throw new AssertionError();
				}
				if (!Sphinx3Loader.assertionsDisabled && int8 >= int6)
				{
					
					throw new AssertionError();
				}
				if (useCDUnits)
				{
					string text3 = new StringBuilder().append(string5).append(' ').append(string6).append(' ').append(string7).toString();
					Unit unit3;
					if (java.lang.String.instancehelper_equals(text3, text2))
					{
						unit3 = unit2;
					}
					else
					{
						LeftRightContext context = LeftRightContext.get(new Unit[]
						{
							(Unit)this.contextIndependentUnits.get(string6)
						}, new Unit[]
						{
							(Unit)this.contextIndependentUnits.get(string7)
						});
						unit3 = this.unitManager.getUnit(string5, false, context);
					}
					text2 = text3;
					unit2 = unit3;
					if (this.logger.isLoggable(Level.FINE))
					{
						this.logger.fine(new StringBuilder().append("Loaded ").append(unit3).toString());
					}
					float[][] transitionMatrix2 = (float[][])this.transitionsPool.get(int8);
					SenoneSequence senoneSequence3 = senoneSequence2;
					if (senoneSequence3 == null || !this.sameSenoneSequence(array2, ssid))
					{
						senoneSequence3 = this.getSenoneSequence(array2);
					}
					senoneSequence2 = senoneSequence3;
					ssid = array2;
					SenoneHMM hmm2 = new SenoneHMM(unit3, senoneSequence3, transitionMatrix2, HMMPosition.lookup(string8));
					this.hmmManager.put(hmm2);
				}
			}
			extendedStreamTokenizer.close();
		}

		[Throws(new string[]
		{
			"java.io.IOException",
			"java.net.URISyntaxException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			234,
			136,
			99,
			159,
			6,
			140,
			104,
			109,
			191,
			6,
			107,
			109,
			105,
			106,
			194,
			104,
			105,
			127,
			6,
			105,
			110,
			127,
			6,
			137,
			159,
			6
		})]
		
		public virtual DataInputStream readS3BinaryHeader(string path, Properties props)
		{
			InputStream dataStream = this.getDataStream(path);
			if (dataStream == null)
			{
				string text = new StringBuilder().append("Can't open ").append(path).toString();
				
				throw new IOException(text);
			}
			DataInputStream dataInputStream = new DataInputStream(new BufferedInputStream(dataStream));
			string text2 = this.readWord(dataInputStream);
			if (!java.lang.String.instancehelper_equals(text2, "s3"))
			{
				string text3 = new StringBuilder().append("Not proper s3 binary file ").append(path).toString();
				
				throw new IOException(text3);
			}
			string text4;
			while ((text4 = this.readWord(dataInputStream)) != null && !java.lang.String.instancehelper_equals(text4, "endhdr"))
			{
				string text5 = this.readWord(dataInputStream);
				props.setProperty(text4, text5);
			}
			int num = dataInputStream.readInt();
			if (num == 287454020)
			{
				this.logger.fine(new StringBuilder().append("Not swapping ").append(path).toString());
				this.swap = false;
			}
			else
			{
				if (Utilities.swapInteger(num) != 287454020)
				{
					string text6 = new StringBuilder().append("Corrupted S3 file ").append(path).toString();
					
					throw new IOException(text6);
				}
				this.logger.fine(new StringBuilder().append("Swapping  ").append(path).toString());
				this.swap = true;
			}
			return dataInputStream;
		}

		private void resetChecksum()
		{
			this.calculatedCheckSum = 0L;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			92,
			104,
			137,
			135,
			127,
			4
		})]
		
		public virtual int readInt(DataInputStream dis)
		{
			int num;
			if (this.swap)
			{
				num = Utilities.readLittleEndianInt(dis);
			}
			else
			{
				num = dis.readInt();
			}
			this.calculatedCheckSum = ((this.calculatedCheckSum << 20 | this.calculatedCheckSum >> 12) + (long)num & (long)((ulong)-1));
			return num;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			135,
			103,
			102,
			42,
			166
		})]
		
		public virtual float[] readFloatArray(DataInputStream dis, int size)
		{
			float[] array = new float[size];
			for (int i = 0; i < size; i++)
			{
				array[i] = this.readFloat(dis);
			}
			return array;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			158,
			224,
			66,
			99,
			97,
			104,
			104,
			100,
			117,
			117,
			154
		})]
		
		private void validateChecksum(DataInputStream dis, bool flag)
		{
			if (!flag)
			{
				return;
			}
			int num = (int)this.calculatedCheckSum;
			int num2 = this.readInt(dis);
			if (num2 != num)
			{
				string text = new StringBuilder().append("Invalid checksum ").append(Long.toHexString(this.calculatedCheckSum)).append(" must be ").append(Integer.toHexString(num2)).toString();
				
				throw new IOException(text);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			21,
			198,
			104,
			168,
			104,
			104,
			104
		})]
		
		internal virtual string readWord(DataInputStream dataInputStream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num;
			do
			{
				num = (int)this.readChar(dataInputStream);
			}
			while (Character.isWhitespace((char)num));
			do
			{
				stringBuilder.append((char)num);
				num = (int)this.readChar(dataInputStream);
			}
			while (!Character.isWhitespace((char)num));
			return stringBuilder.toString();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		
		private char readChar(DataInputStream dataInputStream)
		{
			return (char)((sbyte)dataInputStream.readByte());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			162,
			112,
			104,
			137,
			135,
			127,
			4
		})]
		
		public virtual float readFloat(DataInputStream dis)
		{
			int num;
			if (this.swap)
			{
				num = Utilities.readLittleEndianInt(dis);
			}
			else
			{
				num = dis.readInt();
			}
			this.calculatedCheckSum = ((this.calculatedCheckSum << 20 | this.calculatedCheckSum >> 12) + (long)num & (long)((ulong)-1));
			FloatConverter floatConverter;
			return FloatConverter.ToFloat(num, ref floatConverter);
		}

		[LineNumberTable(new byte[]
		{
			163,
			80,
			104,
			103,
			54,
			166
		})]
		
		protected internal virtual SenoneSequence getSenoneSequence(int[] stateid)
		{
			Senone[] array = new Senone[stateid.Length];
			for (int i = 0; i < stateid.Length; i++)
			{
				array[i] = (Senone)this.senonePool.get(stateid[i]);
			}
			return new SenoneSequence(array);
		}

		[LineNumberTable(new byte[]
		{
			163,
			60,
			102,
			103,
			104,
			2,
			230,
			69,
			130
		})]
		protected internal virtual bool sameSenoneSequence(int[] ssid1, int[] ssid2)
		{
			if (ssid1.Length == ssid2.Length)
			{
				for (int i = 0; i < ssid1.Length; i++)
				{
					if (ssid1[i] != ssid2[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		[LineNumberTable(new byte[]
		{
			159,
			104,
			67,
			232,
			162,
			11,
			232,
			157,
			247,
			143,
			15,
			165
		})]
		
		public Sphinx3Loader(URL location, UnitManager unitManager, float distFloor, float mixtureWeightFloor, float varianceFloor, int topGauNum, bool useCDUnits)
		{
			this.calculatedCheckSum = 0L;
			this.init(location, unitManager, distFloor, mixtureWeightFloor, varianceFloor, topGauNum, useCDUnits, Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName()));
		}

		[Throws(new string[]
		{
			"java.net.MalformedURLException",
			"java.lang.ClassNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			102,
			131,
			232,
			162,
			1,
			232,
			158,
			1,
			180,
			239,
			61,
			197
		})]
		
		public Sphinx3Loader(string location, UnitManager unitManager, float distFloor, float mixtureWeightFloor, float varianceFloor, int topGauNum, bool useCDUnits)
		{
			this.calculatedCheckSum = 0L;
			this.init(ConfigurationManagerUtils.resourceToURL(location), unitManager, distFloor, mixtureWeightFloor, varianceFloor, topGauNum, useCDUnits, Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName()));
		}

		[LineNumberTable(new byte[]
		{
			160,
			70,
			232,
			161,
			235,
			232,
			158,
			23
		})]
		
		public Sphinx3Loader()
		{
			this.calculatedCheckSum = 0L;
		}

		
		
		public virtual string getLocation()
		{
			return this.location.getPath();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			105,
			114,
			112,
			118,
			107,
			107,
			240,
			59,
			229,
			70
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.init(ConfigurationManagerUtils.getResource("location", ps), (UnitManager)ps.getComponent("unitManager"), ps.getFloat("mixtureComponentScoreFloor"), ps.getFloat("mixtureWeightFloor"), ps.getFloat("varianceFloor"), ps.getInt("topGaussiansNum"), ps.getBoolean("useCDUnits").booleanValue(), ps.getLogger());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			123,
			107,
			144,
			107,
			171,
			103,
			103,
			103,
			103,
			204,
			184,
			2,
			97,
			204,
			103,
			145
		})]
		
		public virtual void load()
		{
			if (!this.loaded)
			{
				TimerPool.getTimer(this, "Load AM").start();
				this.hmmManager = new HMMManager();
				this.contextIndependentUnits = new LinkedHashMap();
				this.meanTransformationMatrixPool = null;
				this.meanTransformationVectorPool = null;
				this.varianceTransformationMatrixPool = null;
				this.varianceTransformationVectorPool = null;
				this.transformMatrix = (float[][])null;
				URISyntaxException ex2;
				try
				{
					this.loadModelFiles();
				}
				catch (URISyntaxException ex)
				{
					ex2 = ByteCodeHelper.MapException<URISyntaxException>(ex, 1);
					goto IL_6E;
				}
				this.loaded = true;
				TimerPool.getTimer(this, "Load AM").stop();
				return;
				IL_6E:
				URISyntaxException ex3 = ex2;
				Exception ex4 = ex3;
				
				throw new RuntimeException(ex4);
			}
		}

		protected internal virtual HMMManager getHmmManager()
		{
			return this.hmmManager;
		}

		
		protected internal virtual Pool getMatrixPool()
		{
			return this.transitionsPool;
		}

		protected internal virtual GaussianWeights getMixtureWeightsPool()
		{
			return this.mixtureWeights;
		}

		
		public virtual Map getContextIndependentUnits()
		{
			return this.contextIndependentUnits;
		}

		
		public virtual Pool getMeansTransformationMatrixPool()
		{
			return this.meanTransformationMatrixPool;
		}

		
		public virtual Pool getMeansTransformationVectorPool()
		{
			return this.meanTransformationVectorPool;
		}

		
		public virtual Pool getVarianceTransformationMatrixPool()
		{
			return this.varianceTransformationMatrixPool;
		}

		
		public virtual Pool getVarianceTransformationVectorPool()
		{
			return this.varianceTransformationVectorPool;
		}

		public virtual GaussianWeights getMixtureWeights()
		{
			return this.mixtureWeights;
		}

		
		public virtual Pool getTransitionMatrixPool()
		{
			return this.transitionsPool;
		}

		public virtual float[][] getTransformMatrix()
		{
			return this.transformMatrix;
		}

		
		public virtual Pool getSenonePool()
		{
			return this.senonePool;
		}

		public virtual int getLeftContextSize()
		{
			return 1;
		}

		public virtual int getRightContextSize()
		{
			return 1;
		}

		public virtual HMMManager getHMMManager()
		{
			return this.hmmManager;
		}

		[LineNumberTable(new byte[]
		{
			164,
			67,
			127,
			11,
			113,
			113,
			113,
			145,
			104,
			113,
			104,
			113,
			104,
			113,
			104,
			145,
			113,
			113,
			123,
			47,
			133,
			113
		})]
		
		public virtual void logInfo()
		{
			this.logger.info(new StringBuilder().append("Loading tied-state acoustic model from: ").append(this.location).toString());
			this.meansPool.logInfo(this.logger);
			this.variancePool.logInfo(this.logger);
			this.transitionsPool.logInfo(this.logger);
			this.senonePool.logInfo(this.logger);
			if (this.meanTransformationMatrixPool != null)
			{
				this.meanTransformationMatrixPool.logInfo(this.logger);
			}
			if (this.meanTransformationVectorPool != null)
			{
				this.meanTransformationVectorPool.logInfo(this.logger);
			}
			if (this.varianceTransformationMatrixPool != null)
			{
				this.varianceTransformationMatrixPool.logInfo(this.logger);
			}
			if (this.varianceTransformationVectorPool != null)
			{
				this.varianceTransformationVectorPool.logInfo(this.logger);
			}
			this.mixtureWeights.logInfo(this.logger);
			this.senonePool.logInfo(this.logger);
			this.logger.info(new StringBuilder().append("Context Independent Unit Entries: ").append(this.contextIndependentUnits.size()).toString());
			this.hmmManager.logInfo(this.logger);
		}

		public virtual Properties getProperties()
		{
			return this.modelProps;
		}

		[LineNumberTable(new byte[]
		{
			164,
			107,
			115,
			104,
			110,
			146,
			112,
			115,
			105,
			112,
			63,
			11,
			200,
			255,
			2,
			58,
			235,
			72,
			235,
			55,
			235,
			59,
			233,
			81
		})]
		
		public virtual void update(Transform transform, ClusteredDensityFileData clusters)
		{
			for (int i = 0; i < this.meansPool.size(); i++)
			{
				int classIndex = clusters.getClassIndex(i);
				float[] array = new float[this.getVectorLength()[0]];
				float[] array2 = (float[])this.meansPool.get(i);
				for (int j = 0; j < this.numStreams; j++)
				{
					for (int k = 0; k < this.getVectorLength()[j]; k++)
					{
						array[k] = 0f;
						int num;
						float[] array4;
						for (int l = 0; l < this.getVectorLength()[j]; l++)
						{
							float[] array3 = array;
							num = k;
							array4 = array3;
							array4[num] += transform.getAs()[classIndex][j][k][l] * array2[l];
						}
						float[] array5 = array;
						num = k;
						array4 = array5;
						array4[num] += transform.getBs()[classIndex][j][k];
					}
					ByteCodeHelper.arraycopy_primitive_4(array, 0, array2, 0, array.Length);
				}
			}
		}

		
		static Sphinx3Loader()
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
				"Ledu/cmu/sphinx/linguist/acoustic/UnitManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_UNIT_MANAGER = "unitManager";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"mandatory",
			true
		})]
		public const string PROP_LOCATION = "location";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_USE_CD_UNITS = "useCDUnits";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.0
		})]
		public const string PROP_MC_FLOOR = "mixtureComponentScoreFloor";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			9.9999997473787516E-05
		})]
		public const string PROP_VARIANCE_FLOOR = "varianceFloor";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0000000116860974E-07
		})]
		public const string PROP_MW_FLOOR = "mixtureWeightFloor";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			4
		})]
		public const string PROP_TOPN = "topGaussiansNum";

		protected internal const string FILLER = "filler";

		protected internal const string SILENCE_CIPHONE = "SIL";

		protected internal const int BYTE_ORDER_MAGIC = 287454020;

		public const string MODEL_VERSION = "0.3";

		private const int CONTEXT_SIZE = 1;

		protected internal Properties modelProps;

		
		protected internal Pool meansPool;

		
		protected internal Pool variancePool;

		
		protected internal Pool transitionsPool;

		protected internal GaussianWeights mixtureWeights;

		private int numStates;

		private int numStreams;

		private int numBase;

		private int numGaussiansPerState;

		private int[] vectorLength;

		private int[] senone2ci;

		
		protected internal Pool meanTransformationMatrixPool;

		
		protected internal Pool meanTransformationVectorPool;

		
		protected internal Pool varianceTransformationMatrixPool;

		
		protected internal Pool varianceTransformationVectorPool;

		protected internal float[][] transformMatrix;

		private MixtureComponentSet[] phoneticTiedMixtures;

		
		protected internal Pool senonePool;

		
		private Map contextIndependentUnits;

		private HMMManager hmmManager;

		protected internal LogMath logMath;

		private UnitManager unitManager;

		private bool swap;

		private const string DENSITY_FILE_VERSION = "1.0";

		private const string MIXW_FILE_VERSION = "1.0";

		private const string TMAT_FILE_VERSION = "1.0";

		private const string TRANSFORM_FILE_VERSION = "0.1";

		protected internal Logger logger;

		private URL location;

		protected internal float distFloor;

		protected internal float mixtureWeightFloor;

		protected internal float varianceFloor;

		private int topGauNum;

		protected internal bool useCDUnits;

		private bool loaded;

		private long calculatedCheckSum;

		
		internal static bool assertionsDisabled = !ClassLiteral<Sphinx3Loader>.Value.desiredAssertionStatus();
	}
}

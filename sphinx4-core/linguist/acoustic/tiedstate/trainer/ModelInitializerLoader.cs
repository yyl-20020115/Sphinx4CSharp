using System;

using edu.cmu.sphinx.decoder.adaptation;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.acoustic.tiedstate.Loader"
	})]
	public class ModelInitializerLoader : java.lang.Object, Loader, Configurable
	{
		
		public static void __<clinit>()
		{
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			150,
			103,
			127,
			15,
			127,
			6,
			109,
			109,
			102,
			142,
			236,
			60,
			40,
			232,
			74,
			104
		})]
		
		private Pool createDummyMatrixPool(string text)
		{
			Pool pool = new Pool(text);
			int num = this.vectorLength;
			int num2 = this.vectorLength;
			int[] array = new int[2];
			int num3 = num2;
			array[1] = num3;
			num3 = num;
			array[0] = num3;
			float[][] array2 = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array);
			this.logger.info(new StringBuilder().append("creating dummy matrix pool ").append(text).toString());
			for (int i = 0; i < this.vectorLength; i++)
			{
				for (int j = 0; j < this.vectorLength; j++)
				{
					if (i == j)
					{
						array2[i][j] = 1f;
					}
					else
					{
						array2[i][j] = 0f;
					}
				}
			}
			pool.put(0, array2);
			return pool;
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			174,
			127,
			6,
			103,
			108,
			107,
			40,
			166,
			104
		})]
		
		private Pool createDummyVectorPool(string text)
		{
			this.logger.info(new StringBuilder().append("creating dummy vector pool ").append(text).toString());
			Pool pool = new Pool(text);
			float[] array = new float[this.vectorLength];
			for (int i = 0; i < this.vectorLength; i++)
			{
				array[i] = 0f;
			}
			pool.put(0, array);
			return pool;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			198,
			130,
			130,
			130,
			170,
			112,
			112,
			112,
			144,
			109,
			109,
			103,
			141,
			112,
			173,
			103,
			107,
			136,
			107,
			146,
			100,
			107,
			236,
			69,
			107,
			242,
			71,
			102,
			104,
			104,
			133,
			99,
			100,
			141,
			117,
			127,
			24,
			105,
			135,
			105,
			39,
			206,
			155,
			149,
			114,
			223,
			34,
			176,
			176,
			181,
			242,
			70,
			116,
			138,
			116,
			109,
			102,
			133,
			242,
			71,
			102
		})]
		
		private void loadPhoneList(PropertySheet propertySheet, bool flag, InputStream inputStream, string text)
		{
			int num = 0;
			int num2 = 1;
			int num3 = 1;
			ExtendedStreamTokenizer extendedStreamTokenizer = new ExtendedStreamTokenizer(inputStream, 35, false);
			this.meansPool = new Pool("means");
			this.variancePool = new Pool("variances");
			this.matrixPool = new Pool("transitionmatrices");
			this.senonePool = new Pool("senones");
			float @float = propertySheet.getFloat("MixtureComponentScoreFloor");
			float float2 = propertySheet.getFloat("mixtureWeightFloor");
			float num4 = 0f;
			float float3 = propertySheet.getFloat("varianceFloor");
			this.logger.info("Loading phone list file from: ");
			this.logger.info(text);
			string expecting = "0.1";
			extendedStreamTokenizer.expectString("version");
			extendedStreamTokenizer.expectString(expecting);
			extendedStreamTokenizer.expectString("same_sized_models");
			int num5 = java.lang.String.instancehelper_equals(extendedStreamTokenizer.getString(), "yes") ? 1 : 0;
			if (num5 != 0)
			{
				extendedStreamTokenizer.expectString("n_state");
				num = extendedStreamTokenizer.getInt("numBase");
			}
			extendedStreamTokenizer.expectString("tmat_skip");
			int num6 = java.lang.String.instancehelper_equals(extendedStreamTokenizer.getString(), "yes") ? 1 : 0;
			int num7 = 0;
			int num8 = 0;
			for (;;)
			{
				string @string = extendedStreamTokenizer.getString();
				if (extendedStreamTokenizer.isEOF())
				{
					break;
				}
				int num9 = num;
				if (num5 == 0)
				{
					num9 = extendedStreamTokenizer.getInt("ModelSize");
				}
				this.phoneList.put(@string, Integer.valueOf(num9));
				this.logger.fine(new StringBuilder().append("Phone: ").append(@string).append(" size: ").append(num9).toString());
				int[] array = new int[num9];
				string rep = "-";
				int i = 0;
				while (i < num9)
				{
					array[i] = num7;
					i++;
					num7++;
				}
				Unit unit = this.unitManager.getUnit(@string, java.lang.String.instancehelper_equals(@string, "SIL"));
				this.contextIndependentUnits.put(unit.getName(), unit);
				if (this.logger.isLoggable(Level.FINE))
				{
					this.logger.fine(new StringBuilder().append("Loaded ").append(unit).append(" with ").append(num9).append(" states").toString());
				}
				this.addModelToDensityPool(this.meansPool, array, num2, num3);
				this.addModelToDensityPool(this.variancePool, array, num2, num3);
				this.addModelToTransitionMatrixPool(this.matrixPool, num8, array.Length, num4, num6 != 0);
				this.addModelToSenonePool(this.senonePool, array, @float, float3);
				float[][] transitionMatrix = (float[][])this.matrixPool.get(num8);
				SenoneSequence senoneSequence = this.getSenoneSequence(array);
				SenoneHMM hmm = new SenoneHMM(unit, senoneSequence, transitionMatrix, HMMPosition.lookup(rep));
				this.hmmManager.put(hmm);
				num8++;
			}
			this.mixtureWeights = this.initMixtureWeights(num7, num2, num3, float2);
			extendedStreamTokenizer.close();
		}

		[LineNumberTable(new byte[]
		{
			96,
			111,
			111,
			111,
			111,
			111,
			111,
			111,
			111,
			111,
			111,
			111,
			111,
			111,
			111,
			111,
			111,
			111
		})]
		
		private void printPhoneListHelp()
		{
			java.lang.System.@out.println("The format for the phone list file is:");
			java.lang.System.@out.println("\tversion 0.1");
			java.lang.System.@out.println("\tsame_sized_models yes");
			java.lang.System.@out.println("\tn_state 3");
			java.lang.System.@out.println("\ttmat_skip (no|yes)");
			java.lang.System.@out.println("\tAA");
			java.lang.System.@out.println("\tAE");
			java.lang.System.@out.println("\tAH");
			java.lang.System.@out.println("\t...");
			java.lang.System.@out.println("Or:");
			java.lang.System.@out.println("\tversion 0.1");
			java.lang.System.@out.println("\tsame_sized_models no");
			java.lang.System.@out.println("\ttmat_skip (no|yes)");
			java.lang.System.@out.println("\tAA 5");
			java.lang.System.@out.println("\tAE 3");
			java.lang.System.@out.println("\tAH 4");
			java.lang.System.@out.println("\t...");
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			160,
			119,
			117,
			149,
			131,
			109,
			110,
			109,
			100,
			142,
			150,
			109,
			100,
			143,
			215,
			118,
			102,
			100,
			169,
			109,
			105,
			234,
			59,
			8,
			230,
			74
		})]
		
		private void addModelToDensityPool(Pool pool, int[] array, int num, int num2)
		{
			if (!ModelInitializerLoader.assertionsDisabled && pool == null)
			{
				
				throw new AssertionError();
			}
			if (!ModelInitializerLoader.assertionsDisabled && array == null)
			{
				
				throw new AssertionError();
			}
			int num3 = array.Length;
			int feature = pool.getFeature(Pool.Feature.__NUM_SENONES, 0);
			pool.setFeature(Pool.Feature.__NUM_SENONES, num3 + feature);
			feature = pool.getFeature(Pool.Feature.__NUM_STREAMS, -1);
			if (feature == -1)
			{
				pool.setFeature(Pool.Feature.__NUM_STREAMS, num);
			}
			else if (!ModelInitializerLoader.assertionsDisabled && feature != num)
			{
				
				throw new AssertionError();
			}
			feature = pool.getFeature(Pool.Feature.__NUM_GAUSSIANS_PER_STATE, -1);
			if (feature == -1)
			{
				pool.setFeature(Pool.Feature.__NUM_GAUSSIANS_PER_STATE, num2);
			}
			else if (!ModelInitializerLoader.assertionsDisabled && feature != num2)
			{
				
				throw new AssertionError();
			}
			if (!ModelInitializerLoader.assertionsDisabled && num != 1)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < num3; i++)
			{
				int num4 = array[i];
				for (int j = 0; j < num2; j++)
				{
					float[] o = new float[this.vectorLength];
					int id = num4 * num2 + j;
					pool.put(id, o);
				}
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			159,
			24,
			163,
			181,
			132,
			159,
			6,
			107,
			139,
			237,
			71,
			167,
			110,
			203,
			99,
			104,
			235,
			45,
			235,
			88,
			107,
			240,
			38,
			235,
			92,
			105
		})]
		
		private void addModelToTransitionMatrixPool(Pool pool, int id, int num, float num2, bool flag)
		{
			if (!ModelInitializerLoader.assertionsDisabled && pool == null)
			{
				
				throw new AssertionError();
			}
			int num3 = num + 1;
			int num4 = num3;
			int num5 = num3;
			int[] array = new int[2];
			int num6 = num5;
			array[1] = num6;
			num6 = num4;
			array[0] = num6;
			float[][] array2 = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array);
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < num3; j++)
				{
					array2[i][j] = 0f;
					if (i < num3 - 1)
					{
						if (j == i || j == i + 1)
						{
							array2[i][j] = num2;
						}
						if (flag && j == i + 2)
						{
							array2[i][j] = num2;
						}
					}
				}
				this.normalize(array2[i]);
				this.logMath.linearToLog(array2[i]);
			}
			pool.put(id, array2);
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			65,
			245,
			84,
			108,
			118,
			115,
			104,
			107,
			105,
			113,
			113,
			113,
			114,
			113,
			113,
			245,
			56,
			235,
			77,
			145,
			234,
			47,
			233,
			83
		})]
		
		private void addModelToSenonePool(Pool pool, int[] array, float num, float num2)
		{
			if (!ModelInitializerLoader.assertionsDisabled && pool == null)
			{
				
				throw new AssertionError();
			}
			int gauPerState = this.mixtureWeights.getGauPerState();
			if (!ModelInitializerLoader.assertionsDisabled && gauPerState <= 0)
			{
				
				throw new AssertionError();
			}
			int num3 = array.Length;
			for (int i = 0; i < num3; i++)
			{
				int num4 = array[i];
				MixtureComponent[] array2 = new MixtureComponent[gauPerState];
				for (int j = 0; j < gauPerState; j++)
				{
					int id = num4 * gauPerState + j;
					MixtureComponent[] array3 = array2;
					int num5 = j;
					MixtureComponent.__<clinit>();
					array3[num5] = new MixtureComponent((float[])this.meansPool.get(id), (float[][])this.meanTransformationMatrixPool.get(0), (float[])this.meanTransformationVectorPool.get(0), (float[])this.variancePool.get(id), (float[][])this.varianceTransformationMatrixPool.get(0), (float[])this.varianceTransformationVectorPool.get(0), num, num2);
				}
				GaussianMixture o = new GaussianMixture(this.mixtureWeights, array2, num4);
				pool.put(num4, o);
			}
		}

		[LineNumberTable(new byte[]
		{
			161,
			55,
			136,
			103,
			54,
			230,
			69
		})]
		
		private SenoneSequence getSenoneSequence(int[] array)
		{
			Senone[] array2 = new Senone[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = (Senone)this.senonePool.get(array[i]);
			}
			return new SenoneSequence(array2);
		}

		[LineNumberTable(new byte[]
		{
			161,
			77,
			118,
			110,
			102,
			135,
			138,
			103,
			108,
			233,
			57,
			230,
			73
		})]
		
		private GaussianWeights initMixtureWeights(int num, int num2, int num3, float num4)
		{
			if (!ModelInitializerLoader.assertionsDisabled && num2 != 1)
			{
				
				throw new AssertionError();
			}
			GaussianWeights gaussianWeights = new GaussianWeights("mixtureweights", num, num3, num2);
			for (int i = 0; i < num; i++)
			{
				float[] array = new float[num3];
				this.floorData(array, num4);
				this.normalize(array);
				this.logMath.linearToLog(array);
				gaussianWeights.put(i, 0, array);
			}
			return gaussianWeights;
		}

		[LineNumberTable(new byte[]
		{
			160,
			160,
			103,
			103,
			5,
			230,
			69
		})]
		private void floorData(float[] array, float num)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] < num)
				{
					array[i] = num;
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			173,
			102,
			112,
			38,
			198,
			136,
			105,
			105,
			43,
			200
		})]
		private void normalize(float[] array)
		{
			float num = 0f;
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				float num3 = array[i];
				num += num3;
			}
			if (num != 0f)
			{
				num = 1f / num;
				for (int j = 0; j < array.Length; j++)
				{
					array[j] *= num;
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			185,
			232,
			82
		})]
		
		public ModelInitializerLoader()
		{
			this.vectorLength = 39;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			56,
			107,
			140,
			150,
			107,
			107,
			139,
			113,
			113,
			113,
			145,
			140,
			108,
			108,
			140,
			127,
			6,
			127,
			6,
			127,
			6,
			191,
			6,
			146,
			150,
			255,
			42,
			69,
			226,
			60,
			97,
			166,
			2,
			98,
			135
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logMath = LogMath.getLogMath();
			this.logger = ps.getLogger();
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
			this.hmmManager = new HMMManager();
			this.contextIndependentUnits = new LinkedHashMap();
			this.phoneList = new LinkedHashMap();
			this.meanTransformationMatrixPool = this.createDummyMatrixPool("meanTransformationMatrix");
			this.meanTransformationVectorPool = this.createDummyVectorPool("meanTransformationMatrix");
			this.varianceTransformationMatrixPool = this.createDummyMatrixPool("varianceTransformationMatrix");
			this.varianceTransformationVectorPool = this.createDummyVectorPool("varianceTransformationMatrix");
			string @string = ps.getString("modelName");
			string string2 = ps.getString("location");
			string string3 = ps.getString("phones");
			string string4 = ps.getString("dataDir");
			this.logger.info(new StringBuilder().append("Creating Sphinx3 acoustic model: ").append(@string).toString());
			this.logger.info(new StringBuilder().append("    Path      : ").append(string2).toString());
			this.logger.info(new StringBuilder().append("    phonelist : ").append(string3).toString());
			this.logger.info(new StringBuilder().append("    dataDir   : ").append(string4).toString());
			int num = ps.getBoolean("useCDUnits").booleanValue() ? 1 : 0;
			if (!ModelInitializerLoader.assertionsDisabled && num != 0)
			{
				
				throw new AssertionError();
			}
			IOException ex3;
			try
			{
				try
				{
					this.loadPhoneList(ps, num != 0, StreamFactory.getInputStream(string2, string3), new StringBuilder().append(string2).append(File.separator).append(string3).toString());
				}
				catch (StreamCorruptedException ex)
				{
					goto IL_1C0;
				}
			}
			catch (IOException ex2)
			{
				ex3 = ByteCodeHelper.MapException<IOException>(ex2, 1);
				goto IL_1C3;
			}
			return;
			IL_1C0:
			this.printPhoneListHelp();
			return;
			IL_1C3:
			IOException ex4 = ex3;
			Throwable.instancehelper_printStackTrace(ex4);
		}

		
		public virtual Map getContextIndependentUnits()
		{
			return this.contextIndependentUnits;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		public virtual void load()
		{
		}

		
		public virtual Pool getMeansPool()
		{
			return this.meansPool;
		}

		
		public virtual Pool getMeansTransformationMatrixPool()
		{
			return this.meanTransformationMatrixPool;
		}

		
		public virtual Pool getMeansTransformationVectorPool()
		{
			return this.meanTransformationVectorPool;
		}

		
		public virtual Pool getVariancePool()
		{
			return this.variancePool;
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
			return this.matrixPool;
		}

		
		public virtual float[][] getTransformMatrix()
		{
			return (float[][])null;
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
			161,
			240,
			112,
			113,
			113,
			113,
			113,
			113,
			113,
			113,
			113,
			113,
			113,
			127,
			16,
			113
		})]
		
		public virtual void logInfo()
		{
			this.logger.info("Sphinx3Loader");
			this.meansPool.logInfo(this.logger);
			this.variancePool.logInfo(this.logger);
			this.matrixPool.logInfo(this.logger);
			this.senonePool.logInfo(this.logger);
			this.meanTransformationMatrixPool.logInfo(this.logger);
			this.meanTransformationVectorPool.logInfo(this.logger);
			this.varianceTransformationMatrixPool.logInfo(this.logger);
			this.varianceTransformationVectorPool.logInfo(this.logger);
			this.mixtureWeights.logInfo(this.logger);
			this.senonePool.logInfo(this.logger);
			this.logger.info(new StringBuilder().append("Context Independent Unit Entries: ").append(this.contextIndependentUnits.size()).toString());
			this.hmmManager.logInfo(this.logger);
		}

		
		
		public virtual Properties getProperties()
		{
			return new Properties();
		}

		public virtual void update(Transform transform, ClusteredDensityFileData clusters)
		{
		}

		
		static ModelInitializerLoader()
		{
		}

		private const string SILENCE_CIPHONE = "SIL";

		public const string MODEL_VERSION = "0.3";

		private const int CONTEXT_SIZE = 1;

		
		private Pool meansPool;

		
		private Pool variancePool;

		
		private Pool matrixPool;

		
		private Pool meanTransformationMatrixPool;

		
		private Pool meanTransformationVectorPool;

		
		private Pool varianceTransformationMatrixPool;

		
		private Pool varianceTransformationVectorPool;

		private GaussianWeights mixtureWeights;

		
		private Pool senonePool;

		private int vectorLength;

		
		private Map contextIndependentUnits;

		
		private Map phoneList;

		private HMMManager hmmManager;

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"model"
		})]
		public const string MODEL_NAME = "modelName";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"."
		})]
		public const string LOCATION = "location";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"phonelist"
		})]
		public const string PHONE_LIST = "phones";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"data"
		})]
		public const string DATA_DIR = "dataDir";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"model.props"
		})]
		public const string PROP_FILE = "propsFile";

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

		private UnitManager unitManager;

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_USE_CD_UNITS = "useCDUnits";

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
			0.0
		})]
		public const string PROP_MC_FLOOR = "MixtureComponentScoreFloor";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0000000116860974E-07
		})]
		public const string PROP_MW_FLOOR = "mixtureWeightFloor";

		private LogMath logMath;

		private Logger logger;

		
		internal static bool assertionsDisabled = !ClassLiteral<ModelInitializerLoader>.Value.desiredAssertionStatus();
	}
}

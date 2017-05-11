using edu.cmu.sphinx.decoder.adaptation;
using edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public class HTKLoader : java.lang.Object, Loader, Configurable
	{		
		internal static float access_000(HTKLoader htkloader)
		{
			return htkloader.varianceFloor;
		}		
		
		internal static float access_100(HTKLoader htkloader)
		{
			return htkloader.mixtureWeightFloor;
		}		
		
		internal static LogMath access_200(HTKLoader htkloader)
		{
			return htkloader.logMath;
		}
		
		private void loadProperties()
		{
			if (this.properties == null)
			{
				this.properties = new Properties();
				try
				{
					URL resource = java.lang.Object.instancehelper_getClass(this).getResource(this.propsFile);
					if (resource != null)
					{
						this.properties.load(resource.openStream());
					}
				}
				catch (IOException ex)
				{
					Throwable.instancehelper_printStackTrace(ex);
				}
			}
		}
		
		private void loadModelFiles(string text)
		{
			this.logger.config(new StringBuilder().append("Loading HTK acoustic model: ").append(text).toString());
			this.logger.config(new StringBuilder().append("    Path      : ").append(this.location).toString());
			this.logger.config(new StringBuilder().append("    modellName: ").append(this.model).toString());
			this.logger.config(new StringBuilder().append("    dataDir   : ").append(this.dataDir).toString());
			HTKLoader.HTKStruct htkstruct = new HTKLoader.HTKStruct(this);
			htkstruct.load(text);
			this.meansPool = htkstruct.htkMeans(text);
			this.variancePool = htkstruct.htkVars(text, this.varianceFloor);
			this.mixtureWeights = htkstruct.htkWeights(text, this.mixtureWeightFloor);
			this.matrixPool = htkstruct.htkTrans(text);
			this.senonePool = this.createSenonePool(this.distFloor, this.varianceFloor);
			this.loadHMMPool(this.useCDUnits, htkstruct, new StringBuilder().append(this.location).append(File.separator).append(this.model).toString());
		}
		
		private Pool createSenonePool(float num, float num2)
		{
			Pool pool = new Pool("senones");
			int num3 = this.meansPool.size();
			int num4 = this.variancePool.size();
			int gauPerState = this.mixtureWeights.getGauPerState();
			int statesNum = this.mixtureWeights.getStatesNum();
			int num5 = 0;
			this.logger.fine(new StringBuilder().append("NG ").append(gauPerState).toString());
			this.logger.fine(new StringBuilder().append("NS ").append(statesNum).toString());
			this.logger.fine(new StringBuilder().append("NMNS ").append(num3).toString());
			this.logger.fine(new StringBuilder().append("NMNS ").append(num4).toString());
			if (!HTKLoader.assertionsDisabled && gauPerState <= 0)
			{
				
				throw new AssertionError();
			}
			if (!HTKLoader.assertionsDisabled && num4 != statesNum * gauPerState)
			{
				
				throw new AssertionError();
			}
			if (!HTKLoader.assertionsDisabled && num3 != statesNum * gauPerState)
			{
				
				throw new AssertionError();
			}
			float[][] meanTransformationMatrix = (this.meanTransformationMatrixPool != null) ? ((float[][])this.meanTransformationMatrixPool.get(0)) : ((float[][])null);
			float[] meanTransformationVector = (this.meanTransformationVectorPool != null) ? ((float[])this.meanTransformationVectorPool.get(0)) : null;
			float[][] varianceTransformationMatrix = (this.varianceTransformationMatrixPool != null) ? ((float[][])this.varianceTransformationMatrixPool.get(0)) : ((float[][])null);
			float[] varianceTransformationVector = (this.varianceTransformationVectorPool != null) ? ((float[])this.varianceTransformationVectorPool.get(0)) : null;
			for (int i = 0; i < statesNum; i++)
			{
				MixtureComponent[] array = new MixtureComponent[gauPerState];
				for (int j = 0; j < gauPerState; j++)
				{
					MixtureComponent[] array2 = array;
					int num6 = j;
					array2[num6] = new MixtureComponent((float[])this.meansPool.get(num5), meanTransformationMatrix, meanTransformationVector, (float[])this.variancePool.get(num5), varianceTransformationMatrix, varianceTransformationVector, num, num2);
					num5++;
				}
				GaussianMixture o = new GaussianMixture(this.mixtureWeights, array, i);
				pool.put(i, o);
			}
			return pool;
		}
		
		protected internal virtual void loadHMMPool(bool useCDUnits, HTKLoader.HTKStruct htkModels, string path)
		{
			if (!this.tie1ph)
			{
				Iterator iterator = htkModels.hmmsHTK.get1phIt();
				while (iterator.hasNext())
				{
					SingleHMM singleHMM = (SingleHMM)iterator.next();
					if (singleHMM == null)
					{
						break;
					}
					string baseName = singleHMM.getName();
					string text;
					if (java.lang.String.instancehelper_equals(baseName, "sil") || java.lang.String.instancehelper_equals(baseName, "sp") || java.lang.String.instancehelper_equals(baseName, "bb") || java.lang.String.instancehelper_equals(baseName, "xx") || java.lang.String.instancehelper_equals(baseName, "hh"))
					{
						text = "filler";
					}
					else
					{
						text = "nofiller";
					}
					int trIdx = singleHMM.trIdx;
					int nstates = singleHMM.getNstates();
					int[] array = new int[singleHMM.getNbEmittingStates()];
					int num = 0;
					for (int i = 0; i < nstates; i++)
					{
						if (singleHMM.isEmitting(i))
						{
							HTK.HMMState state = singleHMM.getState(i);
							array[num] = htkModels.hmmsHTK.getStateIdx(state);
							num++;
						}
					}
					Unit unit = this.unitManager.getUnit(baseName, java.lang.String.instancehelper_equals(text, "filler"));
					this.contextIndependentUnits.put(unit.getName(), unit);
					if (this.logger.isLoggable(Level.FINE))
					{
						this.logger.fine(new StringBuilder().append("Loaded ").append(unit).toString());
					}
					if (unit.isFiller() && java.lang.String.instancehelper_equals(unit.getName(), "SIL"))
					{
						unit = UnitManager.__SILENCE;
					}
					float[][] transitionMatrix = (float[][])this.matrixPool.get(trIdx);
					SenoneSequence senoneSequence = this.getSenoneSequence(array);
					SenoneHMM hmm = new SenoneHMM(unit, senoneSequence, transitionMatrix, HMMPosition.lookup("-"));
					this.hmmManager.put(hmm);
				}
			}
			else
			{
				for (int j = 0; j < htkModels.hmmsHTK.getNhmms(); j++)
				{
					SingleHMM singleHMM = htkModels.hmmsHTK.getHMM(j);
					if (singleHMM == null)
					{
						break;
					}
					string baseName = singleHMM.getBaseName();
					if (!this.contextIndependentUnits.containsKey(baseName))
					{
						string text;
						if (java.lang.String.instancehelper_equals(baseName, "SIL") || java.lang.String.instancehelper_equals(baseName, "SP") || java.lang.String.instancehelper_equals(baseName, "BB") || java.lang.String.instancehelper_equals(baseName, "XX") || java.lang.String.instancehelper_equals(baseName, "HH"))
						{
							text = "filler";
						}
						else
						{
							text = "nofiller";
						}
						int trIdx = singleHMM.trIdx;
						int nstates = singleHMM.getNstates();
						int[] array = new int[singleHMM.getNbEmittingStates()];
						int num = 0;
						for (int i = 0; i < nstates; i++)
						{
							if (singleHMM.isEmitting(i))
							{
								HTK.HMMState state = singleHMM.getState(i);
								array[num] = htkModels.hmmsHTK.getStateIdx(state);
								num++;
							}
						}
						Unit unit = this.unitManager.getUnit(baseName, java.lang.String.instancehelper_equals(text, "filler"));
						this.contextIndependentUnits.put(unit.getName(), unit);
						if (this.logger.isLoggable(Level.FINE))
						{
							this.logger.fine(new StringBuilder().append("Loaded ").append(unit).toString());
						}
						if (unit.isFiller() && java.lang.String.instancehelper_equals(unit.getName(), "SIL"))
						{
							unit = UnitManager.__SILENCE;
						}
						float[][] transitionMatrix = (float[][])this.matrixPool.get(trIdx);
						SenoneSequence senoneSequence = this.getSenoneSequence(array);
						SenoneHMM hmm = new SenoneHMM(unit, senoneSequence, transitionMatrix, HMMPosition.lookup("-"));
						this.hmmManager.put(hmm);
					}
				}
			}
			string text2 = "";
			Unit unit2 = null;
			int[] ssid = null;
			SenoneSequence senoneSequence2 = null;
			ArrayList arrayList = new ArrayList();
			Iterator iterator2 = htkModels.hmmsHTK.get3phIt();
			while (iterator2.hasNext())
			{
				SingleHMM singleHMM2 = (SingleHMM)iterator2.next();
				if (singleHMM2 == null)
				{
					break;
				}
				string baseName2 = singleHMM2.getBaseName();
				string text3 = singleHMM2.getLeft();
				string text4 = singleHMM2.getRight();
				if (java.lang.String.instancehelper_equals(text3, "-"))
				{
					text3 = "SIL";
				}
				if (java.lang.String.instancehelper_equals(text4, "-"))
				{
					text4 = "SIL";
				}
				string text5 = new StringBuilder().append(text3).append(' ').append(baseName2).append(' ').append(text4).toString();
				if (!arrayList.contains(text5))
				{
					arrayList.add(text5);
					text5 = "i";
					int trIdx2 = singleHMM2.trIdx;
					int nstates = singleHMM2.getNstates();
					int[] array2 = new int[singleHMM2.getNbEmittingStates()];
					int num2 = 0;
					for (int k = 0; k < nstates; k++)
					{
						if (singleHMM2.isEmitting(k))
						{
							HTK.HMMState state2 = singleHMM2.getState(k);
							array2[num2] = htkModels.hmmsHTK.getStateIdx(state2);
							num2++;
						}
					}
					if (useCDUnits)
					{
						string text6 = new StringBuilder().append(baseName2).append(' ').append(text3).append(' ').append(text4).toString();
						Unit unit3;
						if (java.lang.String.instancehelper_equals(text6, text2))
						{
							unit3 = unit2;
						}
						else
						{
							LeftRightContext context = LeftRightContext.get(new Unit[]
							{
								(Unit)this.contextIndependentUnits.get(text3)
							}, new Unit[]
							{
								(Unit)this.contextIndependentUnits.get(text4)
							});
							unit3 = this.unitManager.getUnit(baseName2, false, context);
						}
						text2 = text6;
						unit2 = unit3;
						if (this.logger.isLoggable(Level.FINE))
						{
							this.logger.fine(new StringBuilder().append("Loaded ").append(unit3).toString());
						}
						float[][] transitionMatrix2 = (float[][])this.matrixPool.get(trIdx2);
						SenoneSequence senoneSequence3 = senoneSequence2;
						if (senoneSequence3 == null || !this.sameSenoneSequence(array2, ssid))
						{
							senoneSequence3 = this.getSenoneSequence(array2);
						}
						senoneSequence2 = senoneSequence3;
						ssid = array2;
						SenoneHMM hmm2 = new SenoneHMM(unit3, senoneSequence3, transitionMatrix2, HMMPosition.lookup(text5));
						this.hmmManager.put(hmm2);
					}
				}
			}
		}	
		
		private char readChar(DataInputStream dataInputStream)
		{
			return (char)((sbyte)dataInputStream.readByte());
		}
		
		protected internal virtual float readFloat(DataInputStream dis)
		{
			float result;
			if (this.swap)
			{
				result = Utilities.readLittleEndianFloat(dis);
			}
			else
			{
				result = dis.readFloat();
			}
			return result;
		}

		protected internal virtual SenoneSequence getSenoneSequence(int[] stateId)
		{
			Senone[] array = new Senone[stateId.Length];
			for (int i = 0; i < stateId.Length; i++)
			{
				array[i] = (Senone)this.senonePool.get(stateId[i]);
			}
			return new SenoneSequence(array);
		}

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
		
		public HTKLoader(string propsFile, UnitManager unitManager, bool isBinary, int vectorLength, string model, bool tie1ph, float distFloor, float mixtureWeightFloor, float varianceFloor)
		{
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
			this.propsFile = propsFile;
			this.loadProperties();
			this.logMath = LogMath.getLogMath();
			this.unitManager = unitManager;
			this.model = model;
			this.tie1ph = tie1ph;
			this.distFloor = distFloor;
			this.mixtureWeightFloor = mixtureWeightFloor;
			this.varianceFloor = varianceFloor;
		}
		
		public HTKLoader()
		{
		}
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.propsFile = ps.getString("propertiesFile");
			this.loadProperties();
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
			string text = (string)this.properties.get("modelDefinition");
			this.model = ((text == null) ? ps.getString("modelDefinition") : text);
			this.tie1ph = ps.getBoolean("tie1ph").booleanValue();
			this.distFloor = ps.getFloat("MixtureComponentScoreFloor");
			this.mixtureWeightFloor = ps.getFloat("mixtureWeightFloor");
			this.varianceFloor = ps.getFloat("varianceFloor");
		}

		public virtual void load()
		{
			if (!this.loaded)
			{
				this.hmmManager = new HMMManager();
				this.contextIndependentUnits = new LinkedHashMap();
				this.meanTransformationMatrixPool = null;
				this.meanTransformationVectorPool = null;
				this.varianceTransformationMatrixPool = null;
				this.varianceTransformationVectorPool = null;
				this.loadModelFiles(this.model);
				java.lang.System.err.println("HTK -> S4 conversion finished");
				this.loaded = true;
			}
		}

		public virtual string getName()
		{
			return this.name;
		}
		
		public virtual Properties getProperties()
		{
			if (this.properties == null)
			{
				this.loadProperties();
			}
			return this.properties;
		}

		protected internal virtual string getLocation()
		{
			return this.location;
		}
		
		public virtual Map getContextIndependentUnits()
		{
			return this.contextIndependentUnits;
		}
		
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
		
		protected internal virtual int readInt(DataInputStream dis)
		{
			if (this.swap)
			{
				return Utilities.readLittleEndianInt(dis);
			}
			return dis.readInt();
		}
		
		protected internal virtual float[] readFloatArray(DataInputStream dis, int size)
		{
			float[] array = new float[size];
			for (int i = 0; i < size; i++)
			{
				array[i] = this.readFloat(dis);
			}
			return array;
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
		
		public virtual void logInfo()
		{
			this.logger.info("HTKLoader");
			this.meansPool.logInfo(this.logger);
			this.variancePool.logInfo(this.logger);
			this.matrixPool.logInfo(this.logger);
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
			this.senonePool.logInfo(this.logger);
			this.logger.info(new StringBuilder().append("Context Independent Unit Entries: ").append(this.contextIndependentUnits.size()).toString());
			this.hmmManager.logInfo(this.logger);
		}
		public virtual void update(Transform transform, ClusteredDensityFileData clusters)
		{
		}
		protected internal void loadHMMPool(bool flag, object obj, string path)
		{
			this.loadHMMPool(flag, (HTKLoader.HTKStruct)obj, path);
		}

		protected internal void _0(bool flag, object obj, string path)
		{
			this.loadHMMPool(flag, (HTKLoader.HTKStruct)obj, path);
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

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_IS_BINARY = "isBinary";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"mandatory",
			true,
			"defaultValue",
			"hmmdefs"
		})]
		public const string PROP_MODEL = "modelDefinition";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_TIE_1PH = "tie1ph";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"model.props"
		})]
		public const string PROP_PROPERTIES_FILE = "propertiesFile";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			39
		})]
		public const string PROP_VECTOR_LENGTH = "vectorLength";

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

		protected internal const string FILLER = "filler";

		protected internal const string SILENCE_CIPHONE = "SIL";

		protected internal const int BYTE_ORDER_MAGIC = 287454020;

		public const string MODEL_VERSION = "0.3";

		protected internal const int CONTEXT_SIZE = 1;
		
		private Pool meansPool;

		private Pool variancePool;
		
		private Pool matrixPool;
		
		private Pool meanTransformationMatrixPool;
		
		private Pool meanTransformationVectorPool;
		
		private Pool varianceTransformationMatrixPool;
		
		private Pool varianceTransformationVectorPool;

		private GaussianWeights mixtureWeights;
		
		private Pool senonePool;
		
		private Map contextIndependentUnits;

		private HMMManager hmmManager;

		private LogMath logMath;

		private UnitManager unitManager;

		private Properties properties;

		private bool swap;

		protected internal const string DENSITY_FILE_VERSION = "1.0";

		protected internal const string MIXW_FILE_VERSION = "1.0";

		protected internal const string TMAT_FILE_VERSION = "1.0";

		private string name;

		private Logger logger;

		private string location;

		private string model;

		private string dataDir;

		private string propsFile;

		private float distFloor;

		private float mixtureWeightFloor;

		private float varianceFloor;

		private bool useCDUnits;

		private bool loaded;

		private bool tie1ph;
		
		internal static bool assertionsDisabled = !ClassLiteral<HTKLoader>.Value.desiredAssertionStatus();
		
		public sealed class HTKStruct : java.lang.Object
		{
						
			internal int getNumStates()
			{
				return this.hmmsHTK.getNstates();
			}
			
			internal int getGMMSize()
			{
				GMMDiag gmmdiag = (GMMDiag)this.hmmsHTK.__gmms.get(0);
				return gmmdiag.getNgauss();
			}
			
			internal int getNcoefs()
			{
				GMMDiag gmmdiag = (GMMDiag)this.hmmsHTK.__gmms.get(0);
				return gmmdiag.getNcoefs();
			}
			
			internal int getNumHMMs()
			{
				return this.hmmsHTK.getNhmms();
			}
			
			internal HTKStruct(HTKLoader htkloader)
			{
				this.this_0 = htkloader;
			}
			
			public void load(string nomFich)
			{
				java.lang.System.err.println("HTK loading...");
				this.hmmsHTK = new HMMSet();
				this.hmmsHTK.loadHTK(nomFich);
				java.lang.System.err.println("HTK loading finished");
			}
			
			public Pool htkMeans(string name)
			{
				Pool pool = new Pool(name);
				int numStates = this.getNumStates();
				int value = 1;
				int gmmsize = this.getGMMSize();
				pool.setFeature(Pool.Feature.__NUM_SENONES, numStates);
				pool.setFeature(Pool.Feature.__NUM_STREAMS, value);
				pool.setFeature(Pool.Feature.__NUM_GAUSSIANS_PER_STATE, gmmsize);
				int ncoefs = this.getNcoefs();
				for (int i = 0; i < numStates; i++)
				{
					GMMDiag gmmdiag = (GMMDiag)this.hmmsHTK.__gmms.get(i);
					for (int j = 0; j < gmmsize; j++)
					{
						float[] array = new float[ncoefs];
						int k;
						for (k = 0; k < ncoefs; k++)
						{
							array[k] = gmmdiag.getMean(j, k);
						}
						k = i * gmmsize + j;
						pool.put(k, array);
					}
				}
				return pool;
			}
			
			public Pool htkVars(string name, float num)
			{
				Pool pool = new Pool(name);
				int numStates = this.getNumStates();
				int value = 1;
				int gmmsize = this.getGMMSize();
				pool.setFeature(Pool.Feature.__NUM_SENONES, numStates);
				pool.setFeature(Pool.Feature.__NUM_STREAMS, value);
				pool.setFeature(Pool.Feature.__NUM_GAUSSIANS_PER_STATE, gmmsize);
				int ncoefs = this.getNcoefs();
				for (int i = 0; i < numStates; i++)
				{
					GMMDiag gmmdiag = (GMMDiag)this.hmmsHTK.__gmms.get(i);
					for (int j = 0; j < gmmsize; j++)
					{
						float[] array = new float[ncoefs];
						int k;
						for (k = 0; k < ncoefs; k++)
						{
							array[k] = gmmdiag.getVar(j, k);
						}
						Utilities.floorData(array, HTKLoader.access_000(this.this_0));
						k = i * gmmsize + j;
						pool.put(k, array);
					}
				}
				return pool;
			}
			
			public GaussianWeights htkWeights(string name, float num)
			{
				int numStates = this.getNumStates();
				int numStreams = 1;
				int gmmsize = this.getGMMSize();
				GaussianWeights gaussianWeights = new GaussianWeights(name, numStates, gmmsize, numStreams);
				for (int i = 0; i < numStates; i++)
				{
					GMMDiag gmmdiag = (GMMDiag)this.hmmsHTK.__gmms.get(i);
					float[] array = new float[gmmsize];
					for (int j = 0; j < gmmsize; j++)
					{
						array[j] = gmmdiag.getWeight(j);
					}
					Utilities.floorData(array, HTKLoader.access_100(this.this_0));
					HTKLoader.access_200(this.this_0).linearToLog(array);
					gaussianWeights.put(i, 0, array);
				}
				return gaussianWeights;
			}
			
			public Pool htkTrans(string name)
			{
				Pool pool = new Pool(name);
				int numHMMs = this.getNumHMMs();
				int i = 0;
				if (this.hmmsHTK.__transitions != null)
				{
					while (i < this.hmmsHTK.__transitions.size())
					{
						float[][] array = (float[][])this.hmmsHTK.__transitions.get(i);
						int num = array.Length;
						int num2 = array[0].Length;
						int[] array2 = new int[2];
						int num3 = num2;
						array2[1] = num3;
						num3 = num;
						array2[0] = num3;
						float[][] array3 = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array2);
						for (int j = 0; j < array3.Length; j++)
						{
							for (int k = 0; k < array3[j].Length; k++)
							{
								array3[j][k] = HTKLoader.access_200(this.this_0).linearToLog((double)array[j][k]);
							}
						}
						pool.put(i, array3);
						i++;
					}
				}
				for (int l = 0; l < numHMMs; l++)
				{
					SingleHMM hmm = this.hmmsHTK.getHMM(l);
					if (hmm.trans != null)
					{
						float[][] trans = hmm.trans;
						int num4 = trans.Length;
						int num5 = trans[0].Length;
						int[] array2 = new int[2];
						int num3 = num5;
						array2[1] = num3;
						num3 = num4;
						array2[0] = num3;
						float[][] array4 = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array2);
						for (int m = 0; m < array4.Length; m++)
						{
							for (int n = 0; n < array4[m].Length; n++)
							{
								array4[m][n] = HTKLoader.access_200(this.this_0).linearToLog((double)trans[m][n]);
							}
						}
						hmm.trIdx = i;
						Pool pool2 = pool;
						int id = i;
						i++;
						pool2.put(id, array4);
					}
					else
					{
						hmm.trIdx = hmm.getTransIdx();
					}
				}
				return pool;
			}

			internal HMMSet hmmsHTK;

			
			internal HTKLoader this_0;
		}
	}
}

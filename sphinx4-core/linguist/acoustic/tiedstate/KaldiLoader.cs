using edu.cmu.sphinx.decoder.adaptation;
using edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi;
using edu.cmu.sphinx.util.props;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public class KaldiLoader : Object, Loader, Configurable
	{
		public virtual void init(string location, UnitManager unitManager)
		{
			this.location = location;
			this.unitManager = unitManager;
		}
		
		private void loadTransform()
		{
			URL url = new URL(new File(this.location, "final.mat").getPath());
			InputStreamReader inputStreamReader = new InputStreamReader(url.openStream());
			BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
			ArrayList arrayList = new ArrayList();
			int num = 0;
			int num2 = 0;
			string text;
			while (null != (text = bufferedReader.readLine()))
			{
				int num3 = 0;
				string[] array = String.instancehelper_split(text, "\\s+");
				int i = array.Length;
				for (int j = 0; j < i; j++)
				{
					string text2 = array[j];
					if (!String.instancehelper_isEmpty(text2) && !String.instancehelper_equals("[", text2))
					{
						if (!String.instancehelper_equals("]", text2))
						{
							arrayList.add(Float.valueOf(Float.parseFloat(text2)));
							num3++;
						}
					}
				}
				if (num3 > 0)
				{
					num++;
				}
				num2 = num3;
			}
			int num4 = num;
			int num5 = num2;
			int[] array2 = new int[2];
			int num6 = num5;
			array2[1] = num6;
			num6 = num4;
			array2[0] = num6;
			this.transform = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array2);
			Iterator iterator = arrayList.iterator();
			for (int k = 0; k < num; k++)
			{
				for (int i = 0; i < num2; i++)
				{
					this.transform[k][i] = ((Float)iterator.next()).floatValue();
				}
			}
		}
		
		private void loadProperties()
		{
			File file = new File(this.location, "feat.params");
			InputStream inputStream = new URL(file.getPath()).openStream();
			InputStreamReader inputStreamReader = new InputStreamReader(inputStream);
			BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
			this.modelProperties = new Properties();
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				string[] array = String.instancehelper_split(text, " ");
				this.modelProperties.put(array[0], array[1]);
			}
		}

		public KaldiLoader()
		{
		}
		
		public KaldiLoader(string location, UnitManager unitManager)
		{
			this.init(location, unitManager);
		}

		public virtual void newProperties(PropertySheet ps)
		{
			this.init(ps.getString("location"), (UnitManager)ps.getComponent("unitManager"));
		}

		public virtual void load()
		{
			KaldiTextParser parser = new KaldiTextParser(this.location);
			TransitionModel transitionModel = new TransitionModel(parser);
			this.senonePool = new KaldiGmmPool(parser);
			File file = new File(this.location, "phones.txt");
			InputStream inputStream = new URL(file.getPath()).openStream();
			InputStreamReader inputStreamReader = new InputStreamReader(inputStream);
			BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
			HashMap hashMap = new HashMap();
			string text;
			while (null != (text = bufferedReader.readLine()))
			{
				string[] array = String.instancehelper_split(text, " ");
				if (Character.isLetter(String.instancehelper_charAt(array[0], 0)))
				{
					hashMap.put(array[0], Integer.valueOf(Integer.parseInt(array[1])));
				}
			}
			this.contextIndependentUnits = new HashMap();
			this.hmmManager = new LazyHmmManager(parser, transitionModel, this.senonePool, hashMap);
			Iterator iterator = hashMap.keySet().iterator();
			while (iterator.hasNext())
			{
				string text2 = (string)iterator.next();
				Unit unit = this.unitManager.getUnit(text2, String.instancehelper_equals("SIL", text2));
				this.contextIndependentUnits.put(unit.getName(), unit);
				this.hmmManager.get(HMMPosition.__UNDEFINED, unit);
			}
			this.loadTransform();
			this.loadProperties();
		}

		public virtual Pool getSenonePool()
		{
			return this.senonePool;
		}

		public virtual HMMManager getHMMManager()
		{
			return this.hmmManager;
		}

		public virtual Map getContextIndependentUnits()
		{
			return this.contextIndependentUnits;
		}

		public virtual int getLeftContextSize()
		{
			return 1;
		}

		public virtual int getRightContextSize()
		{
			return 1;
		}

		public virtual Properties getProperties()
		{
			return this.modelProperties;
		}

		public virtual void logInfo()
		{
		}

		public virtual Pool getMeansPool()
		{
			return null;
		}

		public virtual Pool getMeansTransformationMatrixPool()
		{
			return null;
		}

		public virtual Pool getMeansTransformationVectorPool()
		{
			return null;
		}

		public virtual Pool getVariancePool()
		{
			return null;
		}

		public virtual Pool getVarianceTransformationMatrixPool()
		{
			return null;
		}

		public virtual Pool getVarianceTransformationVectorPool()
		{
			return null;
		}

		public virtual GaussianWeights getMixtureWeights()
		{
			return null;
		}
		
		public virtual Pool getTransitionMatrixPool()
		{
			return null;
		}

		public virtual float[][] getTransformMatrix()
		{
			return this.transform;
		}

		public virtual void update(Transform transform, ClusteredDensityFileData clusters)
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

		private string location;

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"mandatory",
			true
		})]
		public const string PROP_LOCATION = "location";

		private UnitManager unitManager;

		private Pool senonePool;

		private HMMManager hmmManager;

		private Properties modelProperties;

		private Map contextIndependentUnits;

		private float[][] transform;
	}
}

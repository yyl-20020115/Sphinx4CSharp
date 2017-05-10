using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.acoustic.AcousticModel"
	})]
	public class TiedStateAcousticModel : java.lang.Object, AcousticModel, Configurable
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			54,
			104,
			139,
			123,
			47,
			133
		})]
		
		protected internal virtual void logInfo()
		{
			if (this.loader != null)
			{
				this.loader.logInfo();
			}
			this.logger.info(new StringBuilder().append("CompositeSenoneSequences: ").append(this.compositeSenoneSequenceCache.size()).toString());
		}

		[LineNumberTable(new byte[]
		{
			160,
			184,
			135,
			146,
			114,
			223,
			26,
			99,
			226,
			70,
			135,
			166,
			116,
			110,
			106,
			105,
			112,
			114,
			159,
			12,
			174,
			165,
			104,
			121,
			111,
			238,
			74,
			99,
			127,
			0,
			108,
			138,
			226,
			71,
			103,
			103,
			108,
			103,
			127,
			0,
			108,
			108,
			138,
			98,
			241,
			56,
			235,
			76,
			104,
			179,
			114,
			127,
			28,
			114,
			171
		})]
		
		public virtual SenoneSequence getCompositeSenoneSequence(Unit unit, HMMPosition position)
		{
			string text = unit.toString();
			SenoneSequence senoneSequence = (SenoneSequence)this.compositeSenoneSequenceCache.get(text);
			if (this.logger.isLoggable(Level.FINE))
			{
				this.logger.fine((new StringBuilder().append("getCompositeSenoneSequence: ").append(unit).append(senoneSequence).toString() != null) ? "Cached" : "");
			}
			if (senoneSequence != null)
			{
				return senoneSequence;
			}
			Context context = unit.getContext();
			ArrayList arrayList = new ArrayList();
			Iterator hmmiterator = this.getHMMIterator();
			while (hmmiterator.hasNext())
			{
				SenoneHMM senoneHMM = (SenoneHMM)hmmiterator.next();
				if (senoneHMM.getPosition() == position)
				{
					Unit unit2 = senoneHMM.getUnit();
					if (unit2.isPartialMatch(unit.getName(), context))
					{
						if (this.logger.isLoggable(Level.FINE))
						{
							this.logger.fine(new StringBuilder().append("collected: ").append(senoneHMM.getUnit()).toString());
						}
						arrayList.add(senoneHMM.getSenoneSequence());
					}
				}
			}
			if (arrayList.isEmpty())
			{
				Unit unit3 = this.unitManager.getUnit(unit.getName(), unit.isFiller());
				SenoneHMM senoneHMM = this.lookupHMM(unit3, HMMPosition.__UNDEFINED);
				arrayList.add(senoneHMM.getSenoneSequence());
			}
			int num = 0;
			Iterator iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				SenoneSequence senoneSequence2 = (SenoneSequence)iterator.next();
				if (senoneSequence2.getSenones().Length > num)
				{
					num = senoneSequence2.getSenones().Length;
				}
			}
			ArrayList arrayList2 = new ArrayList();
			float weight = 0f;
			for (int i = 0; i < num; i++)
			{
				HashSet hashSet = new HashSet();
				Iterator iterator2 = arrayList.iterator();
				while (iterator2.hasNext())
				{
					SenoneSequence senoneSequence3 = (SenoneSequence)iterator2.next();
					if (i < senoneSequence3.getSenones().Length)
					{
						Senone senone = senoneSequence3.getSenones()[i];
						hashSet.add(senone);
					}
				}
				arrayList2.add(CompositeSenone.create(hashSet, weight));
			}
			senoneSequence = SenoneSequence.create(arrayList2);
			this.compositeSenoneSequenceCache.put(unit.toString(), senoneSequence);
			if (this.logger.isLoggable(Level.FINE))
			{
				this.logger.fine(new StringBuilder().append(unit).append(" consists of ").append(arrayList2.size()).append(" composite senones").toString());
				if (this.logger.isLoggable(Level.FINEST))
				{
					senoneSequence.dump("am");
				}
			}
			return senoneSequence;
		}

		[LineNumberTable(new byte[]
		{
			159,
			99,
			98,
			99,
			137,
			108,
			137,
			104,
			199,
			112,
			137,
			105,
			104,
			236,
			69,
			104,
			168,
			99,
			201,
			102,
			141,
			122,
			99,
			159,
			11,
			117,
			154,
			173,
			245,
			69
		})]
		
		public virtual HMM lookupNearestHMM(Unit unit, HMMPosition position, bool exactMatch)
		{
			if (exactMatch)
			{
				return this.lookupHMM(unit, position);
			}
			HMMManager hmmmanager = this.loader.getHMMManager();
			object obj = hmmmanager.get(position, unit);
			if ((HMM)obj != null)
			{
				return (HMM)obj;
			}
			if (this.useComposites && (HMM)obj == null && this.isComposite(unit))
			{
				obj = this.getCompositeHMM(unit, position);
				if ((HMM)obj != null)
				{
					hmmmanager.put((HMM)obj);
				}
			}
			if ((HMM)obj == null)
			{
				obj = this.getHMMAtAnyPosition(unit);
			}
			if (obj == null)
			{
				obj = this.getHMMInSilenceContext(unit, position);
			}
			if (obj == null)
			{
				Unit unit2 = this.lookupUnit(unit.getName());
				if (!TiedStateAcousticModel.assertionsDisabled && !unit.isContextDependent())
				{
					
					throw new AssertionError();
				}
				if (unit2 == null)
				{
					this.logger.severe(new StringBuilder().append("Can't find HMM for ").append(unit.getName()).toString());
				}
				if (!TiedStateAcousticModel.assertionsDisabled && unit2 == null)
				{
					
					throw new AssertionError();
				}
				if (!TiedStateAcousticModel.assertionsDisabled && unit2.isContextDependent())
				{
					
					throw new AssertionError();
				}
				obj = hmmmanager.get(HMMPosition.__UNDEFINED, unit2);
			}
			if (!TiedStateAcousticModel.assertionsDisabled && obj == null)
			{
				
				throw new AssertionError();
			}
			object obj2 = obj;
			HMM result;
			if (obj2 != null)
			{
				if ((result = (obj2 as HMM)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		
		
		private SenoneHMM lookupHMM(Unit unit, HMMPosition position)
		{
			return (SenoneHMM)this.loader.getHMMManager().get(position, unit);
		}

		[LineNumberTable(new byte[]
		{
			160,
			119,
			104,
			162,
			103,
			104,
			103,
			104,
			130,
			104,
			162
		})]
		
		private bool isComposite(Unit unit)
		{
			if (unit.isFiller())
			{
				return false;
			}
			Context context = unit.getContext();
			if (context is LeftRightContext)
			{
				LeftRightContext leftRightContext = (LeftRightContext)context;
				if (leftRightContext.getRightContext() == null)
				{
					return true;
				}
				if (leftRightContext.getLeftContext() == null)
				{
					return true;
				}
			}
			return false;
		}

		[LineNumberTable(new byte[]
		{
			97,
			189,
			169,
			147,
			103
		})]
		
		private HMM getCompositeHMM(Unit unit, HMMPosition position)
		{
			Unit unit2 = this.unitManager.getUnit(unit.getName(), unit.isFiller(), Context.__EMPTY_CONTEXT);
			SenoneSequence compositeSenoneSequence = this.getCompositeSenoneSequence(unit, position);
			SenoneHMM senoneHMM = (SenoneHMM)this.lookupNearestHMM(unit2, HMMPosition.__UNDEFINED, true);
			float[][] transitionMatrix = senoneHMM.getTransitionMatrix();
			return new SenoneHMM(unit, compositeSenoneSequence, transitionMatrix, position);
		}

		[LineNumberTable(new byte[]
		{
			161,
			69,
			108,
			116,
			112,
			100,
			227,
			61,
			230,
			69
		})]
		
		private SenoneHMM getHMMAtAnyPosition(Unit unit)
		{
			HMMManager hmmmanager = this.loader.getHMMManager();
			HMMPosition[] array = HMMPosition.values();
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				HMMPosition position = array[i];
				SenoneHMM senoneHMM = (SenoneHMM)hmmmanager.get(position, unit);
				if (senoneHMM != null)
				{
					return senoneHMM;
				}
			}
			return null;
		}

		[LineNumberTable(new byte[]
		{
			161,
			87,
			98,
			108,
			135,
			107,
			135,
			104,
			232,
			69,
			106,
			140,
			164,
			106,
			140,
			164,
			108,
			107,
			109,
			39,
			135,
			111,
			99,
			201
		})]
		
		private SenoneHMM getHMMInSilenceContext(Unit unit, HMMPosition position)
		{
			SenoneHMM senoneHMM = null;
			HMMManager hmmmanager = this.loader.getHMMManager();
			Context context = unit.getContext();
			if (context is LeftRightContext)
			{
				LeftRightContext leftRightContext = (LeftRightContext)context;
				Unit[] leftContext = leftRightContext.getLeftContext();
				Unit[] rightContext = leftRightContext.getRightContext();
				Unit[] array;
				if (this.hasNonSilenceFiller(leftContext))
				{
					array = this.replaceNonSilenceFillerWithSilence(leftContext);
				}
				else
				{
					array = leftContext;
				}
				Unit[] array2;
				if (this.hasNonSilenceFiller(rightContext))
				{
					array2 = this.replaceNonSilenceFillerWithSilence(rightContext);
				}
				else
				{
					array2 = rightContext;
				}
				if (array != leftContext || array2 != rightContext)
				{
					LeftRightContext context2 = LeftRightContext.get(array, array2);
					Unit unit2 = this.unitManager.getUnit(unit.getName(), unit.isFiller(), context2);
					senoneHMM = (SenoneHMM)hmmmanager.get(position, unit2);
					if (senoneHMM == null)
					{
						senoneHMM = this.getHMMAtAnyPosition(unit2);
					}
				}
			}
			return senoneHMM;
		}

		
		
		private Unit lookupUnit(string text)
		{
			return (Unit)this.loader.getContextIndependentUnits().get(text);
		}

		
		
		
		public virtual Iterator getHMMIterator()
		{
			return this.loader.getHMMManager().iterator();
		}

		[LineNumberTable(new byte[]
		{
			161,
			133,
			99,
			162,
			111,
			110,
			103,
			226,
			61,
			230,
			70
		})]
		
		private bool hasNonSilenceFiller(Unit[] array)
		{
			if (array == null)
			{
				return false;
			}
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Unit unit = array[i];
				if (unit.isFiller() && !unit.equals(UnitManager.__SILENCE))
				{
					return true;
				}
			}
			return false;
		}

		[LineNumberTable(new byte[]
		{
			161,
			154,
			104,
			103,
			114,
			103,
			138,
			230,
			59,
			230,
			72
		})]
		
		private Unit[] replaceNonSilenceFillerWithSilence(Unit[] array)
		{
			Unit[] array2 = new Unit[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].isFiller() && !array[i].equals(UnitManager.__SILENCE))
				{
					array2[i] = UnitManager.__SILENCE;
				}
				else
				{
					array2[i] = array[i];
				}
			}
			return array2;
		}

		[LineNumberTable(new byte[]
		{
			159,
			120,
			98,
			232,
			61,
			203,
			103,
			103,
			103,
			118
		})]
		
		public TiedStateAcousticModel(Loader loader, UnitManager unitManager, bool useComposites)
		{
			this.compositeSenoneSequenceCache = new HashMap();
			this.loader = loader;
			this.unitManager = unitManager;
			this.useComposites = useComposites;
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
		}

		[LineNumberTable(new byte[]
		{
			46,
			232,
			54,
			235,
			76
		})]
		
		public TiedStateAcousticModel()
		{
			this.compositeSenoneSequenceCache = new HashMap();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			51,
			118,
			118,
			118,
			108
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.loader = (Loader)ps.getComponent("loader");
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
			this.useComposites = ps.getBoolean("useComposites").booleanValue();
			this.logger = ps.getLogger();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			63,
			104,
			107,
			102,
			135
		})]
		
		public virtual void allocate()
		{
			if (!this.allocated)
			{
				this.loader.load();
				this.logInfo();
				this.allocated = true;
			}
		}

		public virtual void deallocate()
		{
		}

		public virtual string getName()
		{
			return this.name;
		}

		
		
		
		public virtual Iterator getContextIndependentUnitIterator()
		{
			return this.loader.getContextIndependentUnits().values().iterator();
		}

		
		
		public virtual int getLeftContextSize()
		{
			return this.loader.getLeftContextSize();
		}

		
		
		public virtual int getRightContextSize()
		{
			return this.loader.getRightContextSize();
		}

		
		
		public virtual Senone getSenone(long id)
		{
			return (Senone)this.loader.getSenonePool().get((int)id);
		}

		[LineNumberTable(new byte[]
		{
			161,
			173,
			104,
			139,
			112,
			191,
			2,
			2,
			97,
			166
		})]
		
		public virtual Properties getProperties()
		{
			if (this.properties == null)
			{
				this.properties = new Properties();
				IOException ex2;
				try
				{
					this.properties.load(ClassLiteral<TiedStateAcousticModel>.Value.getResource("model.props").openStream());
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_41;
				}
				goto IL_4D;
				IL_41:
				IOException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
			}
			IL_4D:
			return this.properties;
		}

		
		static TiedStateAcousticModel()
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
				"Ledu/cmu/sphinx/linguist/acoustic/tiedstate/Loader, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_LOADER = "loader";

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
		public const string PROP_USE_COMPOSITES = "useComposites";

		protected internal string name;

		protected internal Logger logger;

		protected internal Loader loader;

		protected internal UnitManager unitManager;

		private bool useComposites;

		private Properties properties;

		
		
		[NonSerialized]
		private Map compositeSenoneSequenceCache;

		private bool allocated;

		
		internal static bool assertionsDisabled = !ClassLiteral<TiedStateAcousticModel>.Value.desiredAssertionStatus();
	}
}

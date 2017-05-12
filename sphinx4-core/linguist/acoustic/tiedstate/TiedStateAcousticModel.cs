using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public class TiedStateAcousticModel : AcousticModelBase
	{		
		protected internal virtual void logInfo()
		{
			if (this.loader != null)
			{
				this.loader.logInfo();
			}
			this.logger.info(new StringBuilder().append("CompositeSenoneSequences: ").append(this.compositeSenoneSequenceCache.size()).toString());
		}
		
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
		
		public override HMM lookupNearestHMM(Unit unit, HMMPosition position, bool exactMatch)
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
		
		private HMM getCompositeHMM(Unit unit, HMMPosition position)
		{
			Unit unit2 = this.unitManager.getUnit(unit.getName(), unit.isFiller(), Context.__EMPTY_CONTEXT);
			SenoneSequence compositeSenoneSequence = this.getCompositeSenoneSequence(unit, position);
			SenoneHMM senoneHMM = (SenoneHMM)this.lookupNearestHMM(unit2, HMMPosition.__UNDEFINED, true);
			float[][] transitionMatrix = senoneHMM.getTransitionMatrix();
			return new SenoneHMM(unit, compositeSenoneSequence, transitionMatrix, position);
		}
		
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
		
		public override Iterator getHMMIterator()
		{
			return this.loader.getHMMManager().iterator();
		}
		
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

		public TiedStateAcousticModel(Loader loader, UnitManager unitManager, bool useComposites)
		{
			this.compositeSenoneSequenceCache = new HashMap();
			this.loader = loader;
			this.unitManager = unitManager;
			this.useComposites = useComposites;
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
		}
		
		public TiedStateAcousticModel()
		{
			this.compositeSenoneSequenceCache = new HashMap();
		}
		
		public override void newProperties(PropertySheet ps)
		{
			this.loader = (Loader)ps.getComponent("loader");
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
			this.useComposites = ps.getBoolean("useComposites").booleanValue();
			this.logger = ps.getLogger();
		}
		
		public override void allocate()
		{
			if (!this.allocated)
			{
				this.loader.load();
				this.logInfo();
				this.allocated = true;
			}
		}

		public override void deallocate()
		{
		}

		public override string getName()
		{
			return this.name;
		}
		
		public override Iterator getContextIndependentUnitIterator()
		{
			return this.loader.getContextIndependentUnits().values().iterator();
		}
		
		public override int getLeftContextSize()
		{
			return this.loader.getLeftContextSize();
		}

		public override int getRightContextSize()
		{
			return this.loader.getRightContextSize();
		}

		public virtual Senone getSenone(long id)
		{
			return (Senone)this.loader.getSenonePool().get((int)id);
		}
		
		public override Properties getProperties()
		{
			if (this.properties == null)
			{
				this.properties = new Properties();
				try
				{
					this.properties.load(ClassLiteral<TiedStateAcousticModel>.Value.getResource("model.props").openStream());
				}
				catch (IOException ex)
				{
					Throwable.instancehelper_printStackTrace(ex);
				}
			}
			return this.properties;
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

		[System.NonSerialized]
		private Map compositeSenoneSequenceCache;

		private bool allocated;

		internal static bool assertionsDisabled = !ClassLiteral<TiedStateAcousticModel>.Value.desiredAssertionStatus();
	}
}

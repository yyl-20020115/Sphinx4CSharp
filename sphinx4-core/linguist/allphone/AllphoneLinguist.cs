using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.linguist.allphone
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.Linguist"
	})]
	public class AllphoneLinguist : java.lang.Object, Linguist, Configurable
	{		
		private void createContextDependentSuccessors()
		{
			this.cdHMMs = new HashMap();
			this.senonesToUnits = new HashMap();
			this.fillerHMMs = new ArrayList();
			this.leftContextSilHMMs = new ArrayList();
			Iterator hmmiterator = this.acousticModel.getHMMIterator();
			while (hmmiterator.hasNext())
			{
				HMM hmm = (HMM)hmmiterator.next();
				SenoneSequence senoneSequence = ((SenoneHMM)hmm).getSenoneSequence();
				ArrayList arrayList;
				if ((arrayList = (ArrayList)this.senonesToUnits.get(senoneSequence)) == null)
				{
					arrayList = new ArrayList();
					this.senonesToUnits.put(senoneSequence, arrayList);
				}
				arrayList.add(hmm.getUnit());
				if (hmm.getUnit().isFiller())
				{
					this.fillerHMMs.add(hmm);
				}
				else if (hmm.getUnit().isContextDependent())
				{
					LeftRightContext leftRightContext = (LeftRightContext)hmm.getUnit().getContext();
					Unit unit = leftRightContext.getLeftContext()[0];
					if (unit == UnitManager.__SILENCE)
					{
						this.leftContextSilHMMs.add(hmm);
					}
					else
					{
						Unit baseUnit = hmm.getUnit().getBaseUnit();
						HashMap hashMap;
						if ((hashMap = (HashMap)this.cdHMMs.get(unit)) == null)
						{
							hashMap = new HashMap();
							this.cdHMMs.put(unit, hashMap);
						}
						ArrayList arrayList2;
						if ((arrayList2 = (ArrayList)hashMap.get(baseUnit)) == null)
						{
							arrayList2 = new ArrayList();
							hashMap.put(baseUnit, arrayList2);
						}
						arrayList2.add(hmm);
					}
				}
			}
			this.leftContextSilHMMs.addAll(this.fillerHMMs);
		}
		
		private void createContextIndependentSuccessors()
		{
			Iterator hmmiterator = this.acousticModel.getHMMIterator();
			this.ciHMMs = new ArrayList();
			this.senonesToUnits = new HashMap();
			while (hmmiterator.hasNext())
			{
				HMM hmm = (HMM)hmmiterator.next();
				if (!hmm.getUnit().isContextDependent())
				{
					SenoneSequence senoneSequence = ((SenoneHMM)hmm).getSenoneSequence();
					ArrayList arrayList;
					if ((arrayList = (ArrayList)this.senonesToUnits.get(senoneSequence)) == null)
					{
						arrayList = new ArrayList();
						this.senonesToUnits.put(senoneSequence, arrayList);
					}
					arrayList.add(hmm.getUnit());
					this.ciHMMs.add(hmm);
				}
			}
		}
		
		public AllphoneLinguist()
		{
		}
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.acousticModel = (AcousticModel)ps.getComponent("acousticModel");
			this.pip = LogMath.getLogMath().linearToLog((double)ps.getFloat("phoneInsertionProbability"));
			this.useCD = ps.getBoolean("useContextDependentPhones").booleanValue();
			if (this.useCD)
			{
				this.createContextDependentSuccessors();
			}
			else
			{
				this.createContextIndependentSuccessors();
			}
		}
		
		public virtual SearchGraph getSearchGraph()
		{
			return new AllphoneSearchGraph(this);
		}

		public virtual void startRecognition()
		{
		}

		public virtual void stopRecognition()
		{
		}

		public virtual void allocate()
		{
		}

		public virtual void deallocate()
		{
		}

		public virtual AcousticModel getAcousticModel()
		{
			return this.acousticModel;
		}

		public virtual float getPhoneInsertionProb()
		{
			return this.pip;
		}

		public virtual bool useContextDependentPhones()
		{
			return this.useCD;
		}

		public virtual ArrayList getCISuccessors()
		{
			return this.ciHMMs;
		}
		
		public virtual ArrayList getCDSuccessors(Unit lc, Unit @base)
		{
			if (lc.isFiller())
			{
				return this.leftContextSilHMMs;
			}
			if (@base == UnitManager.__SILENCE)
			{
				return this.fillerHMMs;
			}
			return (ArrayList)((HashMap)this.cdHMMs.get(lc)).get(@base);
		}		
		
		public virtual ArrayList getUnits(SenoneSequence senoneSeq)
		{
			return (ArrayList)this.senonesToUnits.get(senoneSeq);
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/AcousticModel, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_ACOUSTIC_MODEL = "acousticModel";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.05
		})]
		public const string PROP_PIP = "phoneInsertionProbability";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_CD = "useContextDependentPhones";

		private AcousticModel acousticModel;
		
		private ArrayList ciHMMs;
		
		private ArrayList fillerHMMs;
		
		private ArrayList leftContextSilHMMs;
		
		private HashMap senonesToUnits;
		
		private HashMap cdHMMs;

		private float pip;

		private bool useCD;
	}
}

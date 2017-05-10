using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.allphone
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.Linguist"
	})]
	public class AllphoneLinguist : java.lang.Object, Linguist, Configurable
	{
		[LineNumberTable(new byte[]
		{
			83,
			107,
			107,
			107,
			107,
			108,
			107,
			140,
			108,
			117,
			102,
			142,
			109,
			109,
			109,
			133,
			112,
			114,
			107,
			105,
			109,
			133,
			141,
			119,
			103,
			176,
			115,
			103,
			140,
			137,
			101,
			114
		})]
		
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

		[LineNumberTable(new byte[]
		{
			64,
			108,
			107,
			107,
			107,
			108,
			141,
			108,
			117,
			102,
			142,
			109,
			141,
			101
		})]
		
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

		[LineNumberTable(new byte[]
		{
			4,
			134
		})]
		
		public AllphoneLinguist()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			9,
			118,
			156,
			118,
			104,
			136,
			102
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		public virtual void allocate()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
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

		
		[LineNumberTable(new byte[]
		{
			52,
			104,
			103,
			104,
			103
		})]
		
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

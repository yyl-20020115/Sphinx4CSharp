using System;

using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.aflat
{
	public class PhoneLoop : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			1,
			104,
			103,
			104,
			104,
			104,
			108,
			108,
			108,
			108,
			110
		})]
		
		public PhoneLoop(AcousticModel model, float logOutOfGrammarBranchProbability, float logPhoneInsertionProbability, SearchStateArc[] toGrammarSearchState)
		{
			this.acousticModel = model;
			this.logOutOfGrammarBranchProbability = logOutOfGrammarBranchProbability;
			this.logPhoneInsertionProbability = logPhoneInsertionProbability;
			this.toGrammarSearchState = toGrammarSearchState;
			this.fbs = new PhoneLoop.FirstBranchState(this);
			this.lbs = new PhoneLoop.LastBranchState(this);
			this.uws = new PhoneLoop.UnknownWordState(this);
			this.lbsArcSet = new SearchStateArc[1];
			this.lbsArcSet[0] = this.lbs;
		}

		public virtual SearchStateArc getPhoneLoop()
		{
			return this.uws;
		}

		
		internal static SearchStateArc[] access_600()
		{
			return PhoneLoop.EMPTY_ARCS;
		}

		
		
		internal static PhoneLoop.FirstBranchState access_000(PhoneLoop phoneLoop)
		{
			return phoneLoop.fbs;
		}

		
		
		internal static float access_100(PhoneLoop phoneLoop)
		{
			return phoneLoop.logOutOfGrammarBranchProbability;
		}

		
		
		internal static AcousticModel access_200(PhoneLoop phoneLoop)
		{
			return phoneLoop.acousticModel;
		}

		
		
		internal static float access_300(PhoneLoop phoneLoop)
		{
			return phoneLoop.logPhoneInsertionProbability;
		}

		
		
		internal static SearchStateArc[] access_400(PhoneLoop phoneLoop)
		{
			return phoneLoop.lbsArcSet;
		}

		
		
		internal static SearchStateArc[] access_500(PhoneLoop phoneLoop)
		{
			return phoneLoop.toGrammarSearchState;
		}

		
		static PhoneLoop()
		{
		}

		
		private AcousticModel acousticModel;

		
		private float logOutOfGrammarBranchProbability;

		
		private float logPhoneInsertionProbability;

		
		private static SearchStateArc[] EMPTY_ARCS = new SearchStateArc[0];

		
		private PhoneLoop.FirstBranchState fbs;

		
		private PhoneLoop.LastBranchState lbs;

		
		private PhoneLoop.UnknownWordState uws;

		
		private SearchStateArc[] lbsArcSet;

		
		private SearchStateArc[] toGrammarSearchState;

		
		.
		
		internal sealed class FinalState : PhoneLoop.OogSearchState
		{
			
			
			internal FinalState(PhoneLoop phoneLoop) : base(phoneLoop)
			{
			}

			public override int getOrder()
			{
				return 2;
			}

			public override string getSignature()
			{
				return "oogFinal";
			}

			public override bool isFinal()
			{
				return true;
			}

			
			
			public override SearchStateArc[] getSuccessors()
			{
				return PhoneLoop.access_600();
			}

			
			internal new PhoneLoop this_0 = phoneLoop;
		}

		
		.
		
		internal sealed class FirstBranchState : PhoneLoop.OogSearchState
		{
			[LineNumberTable(new byte[]
			{
				110,
				112,
				102,
				116,
				108,
				104,
				105,
				137,
				98,
				124
			})]
			
			internal FirstBranchState(PhoneLoop phoneLoop) : base(phoneLoop)
			{
				ArrayList arrayList = new ArrayList();
				Iterator contextIndependentUnitIterator = PhoneLoop.access_200(phoneLoop).getContextIndependentUnitIterator();
				while (contextIndependentUnitIterator.hasNext())
				{
					Unit unit = (Unit)contextIndependentUnitIterator.next();
					if (!unit.isFiller())
					{
						PhoneLoop.OogHMM oogHMM = new PhoneLoop.OogHMM(phoneLoop, unit);
						arrayList.add(oogHMM);
					}
				}
				this.successors = (SearchStateArc[])arrayList.toArray(new SearchStateArc[arrayList.size()]);
			}

			public override int getOrder()
			{
				return 2;
			}

			public override string getSignature()
			{
				return "oogFBS";
			}

			public override SearchStateArc[] getSuccessors()
			{
				return this.successors;
			}

			
			private SearchStateArc[] successors;

			
			internal new PhoneLoop this_0 = phoneLoop;
		}

		
		.
		
		internal sealed class LastBranchState : PhoneLoop.OogSearchState
		{
			[LineNumberTable(new byte[]
			{
				161,
				37,
				112,
				108,
				110,
				112
			})]
			
			internal LastBranchState(PhoneLoop phoneLoop) : base(phoneLoop)
			{
				this.successors = new SearchStateArc[2];
				this.successors[0] = PhoneLoop.access_000(phoneLoop);
				this.successors[1] = PhoneLoop.access_500(phoneLoop)[0];
			}

			public override int getOrder()
			{
				return 1;
			}

			public override string getSignature()
			{
				return "oogLBS";
			}

			public override SearchStateArc[] getSuccessors()
			{
				return this.successors;
			}

			
			private SearchStateArc[] successors;

			
			internal new PhoneLoop this_0 = phoneLoop;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.UnitSearchState"
		})]
		.
		
		internal sealed class OogHMM : PhoneLoop.OogSearchState, UnitSearchState, SearchState
		{
			[LineNumberTable(new byte[]
			{
				160,
				104,
				112,
				120,
				108,
				126
			})]
			
			internal OogHMM(PhoneLoop phoneLoop, Unit u) : base(phoneLoop)
			{
				this.hmm = PhoneLoop.access_200(phoneLoop).lookupNearestHMM(u, HMMPosition.__UNDEFINED, false);
				this.successors = new SearchStateArc[1];
				this.successors[0] = new PhoneLoop.OogHMMState(phoneLoop, this.hmm.getInitialState(), 0f);
			}

			
			
			public Unit getUnit()
			{
				return this.hmm.getBaseUnit();
			}

			public override int getOrder()
			{
				return 3;
			}

			
			
			public override string getSignature()
			{
				return new StringBuilder().append("oogHMM-").append(this.getUnit()).toString();
			}

			public override SearchStateArc[] getSuccessors()
			{
				return this.successors;
			}

			
			
			public override float getInsertionProbability()
			{
				return PhoneLoop.access_300(this.this_0);
			}

			
			private HMM hmm;

			
			private SearchStateArc[] successors;

			
			internal new PhoneLoop this_0 = phoneLoop;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.HMMSearchState",
			"edu.cmu.sphinx.decoder.scorer.ScoreProvider"
		})]
		.
		
		internal sealed class OogHMMState : PhoneLoop.OogSearchState, HMMSearchState, SearchState, ScoreProvider
		{
			[LineNumberTable(new byte[]
			{
				160,
				178,
				112,
				103,
				104
			})]
			
			internal OogHMMState(PhoneLoop phoneLoop, HMMState hmmstate, float num) : base(phoneLoop)
			{
				this.hmmState = hmmstate;
				this.logProbability = num;
			}

			
			
			public override bool isEmitting()
			{
				return this.hmmState.isEmitting();
			}

			
			
			public override string getSignature()
			{
				return new StringBuilder().append("oog-").append(this.hmmState).toString();
			}

			public HMMState getHMMState()
			{
				return this.hmmState;
			}

			
			
			public override int hashCode()
			{
				return 191 + java.lang.Object.instancehelper_hashCode(this.hmmState);
			}

			[LineNumberTable(new byte[]
			{
				160,
				235,
				100,
				98,
				104,
				103,
				143
			})]
			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is PhoneLoop.OogHMMState)
				{
					PhoneLoop.OogHMMState oogHMMState = (PhoneLoop.OogHMMState)obj;
					return oogHMMState.hmmState == this.hmmState;
				}
				return false;
			}

			
			
			public override int getOrder()
			{
				return (!this.isEmitting()) ? 0 : 4;
			}

			[LineNumberTable(new byte[]
			{
				161,
				8,
				109,
				140,
				108,
				104,
				103,
				115,
				11,
				198
			})]
			
			public override SearchStateArc[] getSuccessors()
			{
				if (this.hmmState.isExitState())
				{
					return PhoneLoop.access_400(this.this_0);
				}
				HMMStateArc[] successors = this.hmmState.getSuccessors();
				SearchStateArc[] array = new SearchStateArc[successors.Length];
				for (int i = 0; i < successors.Length; i++)
				{
					array[i] = new PhoneLoop.OogHMMState(this.this_0, successors[i].getHMMState(), successors[i].getLogProbability());
				}
				return array;
			}

			
			
			public float getScore(Data d)
			{
				return this.hmmState.getScore(d);
			}

			
			
			public float[] getComponentScore(Data d)
			{
				return this.hmmState.calculateComponentScore(d);
			}

			
			internal HMMState hmmState;

			
			internal float logProbability;

			
			internal new PhoneLoop this_0 = phoneLoop;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.SearchState",
			"edu.cmu.sphinx.linguist.SearchStateArc"
		})]
		.
		internal abstract class OogSearchState : java.lang.Object, SearchState, SearchStateArc
		{
			
			
			public override string toString()
			{
				return this.getSignature();
			}

			public abstract string getSignature();

			public virtual float getLanguageProbability()
			{
				return 0f;
			}

			public virtual float getInsertionProbability()
			{
				return 0f;
			}

			
			
			internal OogSearchState(PhoneLoop phoneLoop)
			{
			}

			public abstract SearchStateArc[] getSuccessors();

			public abstract int getOrder();

			public virtual bool isEmitting()
			{
				return false;
			}

			public virtual bool isFinal()
			{
				return false;
			}

			public virtual object getLexState()
			{
				return null;
			}

			
			
			public virtual string toPrettyString()
			{
				return this.toString();
			}

			public virtual WordSequence getWordHistory()
			{
				return null;
			}

			public virtual SearchState getState()
			{
				return this;
			}

			
			
			public virtual float getProbability()
			{
				return this.getLanguageProbability() + this.getInsertionProbability();
			}

			internal const int ANY = 0;

			
			internal PhoneLoop this_0 = phoneLoop;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.WordSearchState"
		})]
		.
		
		internal sealed class UnknownWordState : PhoneLoop.OogSearchState, WordSearchState, SearchState
		{
			[LineNumberTable(new byte[]
			{
				31,
				112,
				108,
				110
			})]
			
			internal UnknownWordState(PhoneLoop phoneLoop) : base(phoneLoop)
			{
				this.successors = new SearchStateArc[1];
				this.successors[0] = PhoneLoop.access_000(phoneLoop);
			}

			
			
			public Pronunciation getPronunciation()
			{
				return Word.__UNKNOWN.getPronunciations()[0];
			}

			public override int getOrder()
			{
				return 1;
			}

			public override string getSignature()
			{
				return "oogUNK";
			}

			public override SearchStateArc[] getSuccessors()
			{
				return this.successors;
			}

			
			
			public override float getLanguageProbability()
			{
				return PhoneLoop.access_100(this.this_0);
			}

			public bool isWordStart()
			{
				return true;
			}

			
			private SearchStateArc[] successors;

			
			internal new PhoneLoop this_0 = phoneLoop;
		}
	}
}

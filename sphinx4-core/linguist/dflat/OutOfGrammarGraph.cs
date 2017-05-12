using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.dflat
{
	public class OutOfGrammarGraph : java.lang.Object
	{
		public OutOfGrammarGraph(AcousticModel model, float logOutOfGrammarBranchProbability, float logPhoneInsertionProbability)
		{
			this.acousticModel = model;
			this.logOutOfGrammarBranchProbability = logOutOfGrammarBranchProbability;
			this.logPhoneInsertionProbability = logPhoneInsertionProbability;
			this.fbs = new OutOfGrammarGraph.FirstBranchState(this);
			this.lbs = new OutOfGrammarGraph.LastBranchState(this);
			this.uws = new OutOfGrammarGraph.UnknownWordState(this);
			this.lbsArcSet = new SearchStateArc[1];
			this.lbsArcSet[0] = this.lbs;
		}

		public virtual SearchStateArc getOutOfGrammarGraph()
		{
			return this.uws;
		}
	
		internal static SearchStateArc[] access_500()
		{
			return OutOfGrammarGraph.EMPTY_ARCS;
		}		
		
		internal static OutOfGrammarGraph.FirstBranchState access_000(OutOfGrammarGraph outOfGrammarGraph)
		{
			return outOfGrammarGraph.fbs;
		}
		
		internal static float access_100(OutOfGrammarGraph outOfGrammarGraph)
		{
			return outOfGrammarGraph.logOutOfGrammarBranchProbability;
		}
		
		internal static AcousticModel access_200(OutOfGrammarGraph outOfGrammarGraph)
		{
			return outOfGrammarGraph.acousticModel;
		}
		
		internal static float access_300(OutOfGrammarGraph outOfGrammarGraph)
		{
			return outOfGrammarGraph.logPhoneInsertionProbability;
		}		
		
		internal static SearchStateArc[] access_400(OutOfGrammarGraph outOfGrammarGraph)
		{
			return outOfGrammarGraph.lbsArcSet;
		}
		
		private AcousticModel acousticModel;
		
		private float logOutOfGrammarBranchProbability;
		
		private float logPhoneInsertionProbability;
		
		private static SearchStateArc[] EMPTY_ARCS = new SearchStateArc[0];
		
		private OutOfGrammarGraph.FirstBranchState fbs;
		
		private OutOfGrammarGraph.LastBranchState lbs;
		
		private OutOfGrammarGraph.UnknownWordState uws;
		
		private SearchStateArc[] lbsArcSet;
		
		internal sealed class FinalState : OutOfGrammarGraph.OogSearchState
		{			
			internal FinalState(OutOfGrammarGraph outOfGrammarGraph) : base(outOfGrammarGraph)
			{
				this.this_0 = outOfGrammarGraph;
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
				return OutOfGrammarGraph.access_500();
			}
		}

		internal sealed class FirstBranchState : OutOfGrammarGraph.OogSearchState
		{			
			internal FirstBranchState(OutOfGrammarGraph outOfGrammarGraph) : base(outOfGrammarGraph)
			{
				ArrayList arrayList = new ArrayList();
				Iterator contextIndependentUnitIterator = OutOfGrammarGraph.access_200(outOfGrammarGraph).getContextIndependentUnitIterator();
				while (contextIndependentUnitIterator.hasNext())
				{
					Unit unit = (Unit)contextIndependentUnitIterator.next();
					OutOfGrammarGraph.OogHMM oogHMM = new OutOfGrammarGraph.OogHMM(outOfGrammarGraph, unit);
					arrayList.add(oogHMM);
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
		}
	
		internal sealed class LastBranchState : OutOfGrammarGraph.OogSearchState
		{
			internal LastBranchState(OutOfGrammarGraph outOfGrammarGraph) : base(outOfGrammarGraph)
			{
				this.successors = new SearchStateArc[2];
				this.successors[0] = OutOfGrammarGraph.access_000(outOfGrammarGraph);
				this.successors[1] = new OutOfGrammarGraph.FinalState(outOfGrammarGraph);
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
		}

		internal sealed class OogHMM : OutOfGrammarGraph.OogSearchState, UnitSearchState, SearchState
		{			
			internal OogHMM(OutOfGrammarGraph outOfGrammarGraph, Unit u) : base(outOfGrammarGraph)
			{
				this.hmm = OutOfGrammarGraph.access_200(outOfGrammarGraph).lookupNearestHMM(u, HMMPosition.__UNDEFINED, false);
				this.successors = new SearchStateArc[1];
				this.successors[0] = new OutOfGrammarGraph.OogHMMState(outOfGrammarGraph, this.hmm.getInitialState(), 0f);
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
				return OutOfGrammarGraph.access_300(this.this_0);
			}

			private HMM hmm;
			
			private SearchStateArc[] successors;
		}
		
		internal sealed class OogHMMState : OutOfGrammarGraph.OogSearchState, HMMSearchState, SearchState, ScoreProvider
		{			
			internal OogHMMState(OutOfGrammarGraph outOfGrammarGraph, HMMState hmmstate, float num) : base(outOfGrammarGraph)
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

			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is OutOfGrammarGraph.OogHMMState)
				{
					OutOfGrammarGraph.OogHMMState oogHMMState = (OutOfGrammarGraph.OogHMMState)obj;
					return oogHMMState.hmmState == this.hmmState;
				}
				return false;
			}
			
			public override int getOrder()
			{
				return (!this.isEmitting()) ? 0 : 4;
			}
			
			public override SearchStateArc[] getSuccessors()
			{
				if (this.hmmState.isExitState())
				{
					return OutOfGrammarGraph.access_400(this.this_0);
				}
				HMMStateArc[] successors = this.hmmState.getSuccessors();
				SearchStateArc[] array = new SearchStateArc[successors.Length];
				for (int i = 0; i < successors.Length; i++)
				{
					array[i] = new OutOfGrammarGraph.OogHMMState(this.this_0, successors[i].getHMMState(), successors[i].getLogProbability());
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
		}

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
			
			internal OogSearchState(OutOfGrammarGraph outOfGrammarGraph)
			{
				this.this_0 = outOfGrammarGraph;
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

			internal OutOfGrammarGraph this_0;
		}
		
		internal sealed class UnknownWordState : OutOfGrammarGraph.OogSearchState, WordSearchState, SearchState
		{			
			internal UnknownWordState(OutOfGrammarGraph outOfGrammarGraph) : base(outOfGrammarGraph)
			{
				this.successors = new SearchStateArc[1];
				this.successors[0] = OutOfGrammarGraph.access_000(outOfGrammarGraph);
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
				return OutOfGrammarGraph.access_100(this.this_0);
			}

			public bool isWordStart()
			{
				return true;
			}

			private SearchStateArc[] successors;
		}
	}
}

using System;

using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.dflat
{
	public class OutOfGrammarGraph : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			191,
			104,
			103,
			104,
			104,
			108,
			108,
			108,
			108,
			142
		})]
		
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

		
		internal static SearchStateArc[] access$500()
		{
			return OutOfGrammarGraph.EMPTY_ARCS;
		}

		
		
		internal static OutOfGrammarGraph.FirstBranchState access_000(OutOfGrammarGraph outOfGrammarGraph)
		{
			return outOfGrammarGraph.fbs;
		}

		
		
		internal static float access$100(OutOfGrammarGraph outOfGrammarGraph)
		{
			return outOfGrammarGraph.logOutOfGrammarBranchProbability;
		}

		
		
		internal static AcousticModel access$200(OutOfGrammarGraph outOfGrammarGraph)
		{
			return outOfGrammarGraph.acousticModel;
		}

		
		
		internal static float access$300(OutOfGrammarGraph outOfGrammarGraph)
		{
			return outOfGrammarGraph.logPhoneInsertionProbability;
		}

		
		
		internal static SearchStateArc[] access$400(OutOfGrammarGraph outOfGrammarGraph)
		{
			return outOfGrammarGraph.lbsArcSet;
		}

		
		static OutOfGrammarGraph()
		{
		}

		
		private AcousticModel acousticModel;

		
		private float logOutOfGrammarBranchProbability;

		
		private float logPhoneInsertionProbability;

		
		private static SearchStateArc[] EMPTY_ARCS = new SearchStateArc[0];

		
		private OutOfGrammarGraph.FirstBranchState fbs;

		
		private OutOfGrammarGraph.LastBranchState lbs;

		
		private OutOfGrammarGraph.UnknownWordState uws;

		
		private SearchStateArc[] lbsArcSet;

		
		[SourceFile("OutOfGrammarGraph.java")]
		
		internal sealed class FinalState : OutOfGrammarGraph.OogSearchState
		{
			
			
			internal FinalState(OutOfGrammarGraph outOfGrammarGraph) : base(outOfGrammarGraph)
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
				return OutOfGrammarGraph.access$500();
			}

			
			internal new OutOfGrammarGraph this$0 = outOfGrammarGraph;
		}

		
		[SourceFile("OutOfGrammarGraph.java")]
		
		internal sealed class FirstBranchState : OutOfGrammarGraph.OogSearchState
		{
			[LineNumberTable(new byte[]
			{
				108,
				112,
				102,
				116,
				108,
				105,
				105,
				98,
				124
			})]
			
			internal FirstBranchState(OutOfGrammarGraph outOfGrammarGraph) : base(outOfGrammarGraph)
			{
				ArrayList arrayList = new ArrayList();
				Iterator contextIndependentUnitIterator = OutOfGrammarGraph.access$200(outOfGrammarGraph).getContextIndependentUnitIterator();
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

			
			internal new OutOfGrammarGraph this$0 = outOfGrammarGraph;
		}

		
		[SourceFile("OutOfGrammarGraph.java")]
		
		internal sealed class LastBranchState : OutOfGrammarGraph.OogSearchState
		{
			[LineNumberTable(new byte[]
			{
				161,
				37,
				112,
				108,
				110,
				110
			})]
			
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

			
			internal new OutOfGrammarGraph this$0 = outOfGrammarGraph;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.UnitSearchState"
		})]
		[SourceFile("OutOfGrammarGraph.java")]
		
		internal sealed class OogHMM : OutOfGrammarGraph.OogSearchState, UnitSearchState, SearchState
		{
			[LineNumberTable(new byte[]
			{
				160,
				100,
				112,
				120,
				108,
				158
			})]
			
			internal OogHMM(OutOfGrammarGraph outOfGrammarGraph, Unit u) : base(outOfGrammarGraph)
			{
				this.hmm = OutOfGrammarGraph.access$200(outOfGrammarGraph).lookupNearestHMM(u, HMMPosition.__UNDEFINED, false);
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
				return OutOfGrammarGraph.access$300(this.this$0);
			}

			
			private HMM hmm;

			
			private SearchStateArc[] successors;

			
			internal new OutOfGrammarGraph this$0 = outOfGrammarGraph;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.HMMSearchState",
			"edu.cmu.sphinx.decoder.scorer.ScoreProvider"
		})]
		[SourceFile("OutOfGrammarGraph.java")]
		
		internal sealed class OogHMMState : OutOfGrammarGraph.OogSearchState, HMMSearchState, SearchState, ScoreProvider
		{
			[LineNumberTable(new byte[]
			{
				160,
				176,
				112,
				103,
				104
			})]
			
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

			[LineNumberTable(new byte[]
			{
				160,
				233,
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

			[LineNumberTable(new byte[]
			{
				161,
				6,
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
					return OutOfGrammarGraph.access$400(this.this$0);
				}
				HMMStateArc[] successors = this.hmmState.getSuccessors();
				SearchStateArc[] array = new SearchStateArc[successors.Length];
				for (int i = 0; i < successors.Length; i++)
				{
					array[i] = new OutOfGrammarGraph.OogHMMState(this.this$0, successors[i].getHMMState(), successors[i].getLogProbability());
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

			
			internal new OutOfGrammarGraph this$0 = outOfGrammarGraph;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.SearchState",
			"edu.cmu.sphinx.linguist.SearchStateArc"
		})]
		[SourceFile("OutOfGrammarGraph.java")]
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

			
			internal OutOfGrammarGraph this$0 = outOfGrammarGraph;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.WordSearchState"
		})]
		[SourceFile("OutOfGrammarGraph.java")]
		
		internal sealed class UnknownWordState : OutOfGrammarGraph.OogSearchState, WordSearchState, SearchState
		{
			[LineNumberTable(new byte[]
			{
				29,
				112,
				108,
				110
			})]
			
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
				return OutOfGrammarGraph.access$100(this.this$0);
			}

			public bool isWordStart()
			{
				return true;
			}

			
			private SearchStateArc[] successors;

			
			internal new OutOfGrammarGraph this$0 = outOfGrammarGraph;
		}
	}
}

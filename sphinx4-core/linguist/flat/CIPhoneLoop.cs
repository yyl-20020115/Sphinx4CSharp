using System;

using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.flat
{
	public class CIPhoneLoop : java.lang.Object
	{
		
		
		internal static float access_000(CIPhoneLoop ciphoneLoop)
		{
			return ciphoneLoop.logPhoneInsertionProbability;
		}

		[LineNumberTable(new byte[]
		{
			159,
			191,
			232,
			54,
			235,
			75,
			103,
			136
		})]
		
		public CIPhoneLoop(AcousticModel model, float logPhoneInsertionProbability)
		{
			this.__logOne = 0f;
			this.__model = model;
			this.logPhoneInsertionProbability = logPhoneInsertionProbability;
		}

		
		
		public virtual SearchGraph getSearchGraph()
		{
			return new CIPhoneLoop.PhoneLoopSearchGraph(this);
		}

		
		public AcousticModel model
		{
			
			get
			{
				return this.__model;
			}
			
			private set
			{
				this.__model = value;
			}
		}

		
		public float logOne
		{
			
			get
			{
				return this.__logOne;
			}
			
			private set
			{
				this.__logOne = value;
			}
		}

		internal AcousticModel __model;

		
		private float logPhoneInsertionProbability;

		internal float __logOne;

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.SearchGraph"
		})]
		[SourceFile("CIPhoneLoop.java")]
		public class PhoneLoopSearchGraph : java.lang.Object, SearchGraph
		{
			[LineNumberTable(new byte[]
			{
				160,
				75,
				204,
				103
			})]
			
			protected internal virtual void attachState(SentenceHMMState prevState, SentenceHMMState nextState, float logLanguageProbability, float logInsertionProbability)
			{
				SentenceHMMStateArc arc = new SentenceHMMStateArc(nextState, logLanguageProbability, logInsertionProbability);
				prevState.connect(arc);
			}

			[LineNumberTable(new byte[]
			{
				98,
				115
			})]
			
			protected internal virtual void addStateToCache(SentenceHMMState state)
			{
				this.__existingStates.put(state.getSignature(), state);
			}

			[LineNumberTable(new byte[]
			{
				111,
				98,
				157,
				110,
				104,
				142,
				104,
				140,
				106,
				105,
				100,
				146,
				112,
				104,
				234,
				48,
				233,
				83
			})]
			
			protected internal virtual HMMStateState expandHMMTree(UnitState parent, HMMStateState tree)
			{
				HMMStateState result = tree;
				HMMStateArc[] successors = tree.getHMMState().getSuccessors();
				int num = successors.Length;
				for (int i = 0; i < num; i++)
				{
					HMMStateArc hmmstateArc = successors[i];
					HMMStateState hmmstateState;
					if (hmmstateArc.getHMMState().isEmitting())
					{
						HMMStateState.__<clinit>();
						hmmstateState = new HMMStateState(parent, hmmstateArc.getHMMState());
					}
					else
					{
						NonEmittingHMMState.__<clinit>();
						hmmstateState = new NonEmittingHMMState(parent, hmmstateArc.getHMMState());
					}
					SentenceHMMState existingState = this.getExistingState(hmmstateState);
					float logProbability = hmmstateArc.getLogProbability();
					if (existingState != null)
					{
						this.attachState(tree, existingState, 0f, logProbability);
					}
					else
					{
						this.attachState(tree, hmmstateState, 0f, logProbability);
						this.addStateToCache(hmmstateState);
						result = this.expandHMMTree(parent, hmmstateState);
					}
				}
				return result;
			}

			
			
			private SentenceHMMState getExistingState(SentenceHMMState sentenceHMMState)
			{
				return (SentenceHMMState)this.__existingStates.get(sentenceHMMState.getSignature());
			}

			[LineNumberTable(new byte[]
			{
				23,
				111,
				107,
				107,
				113,
				151,
				113,
				103,
				146,
				119,
				188,
				106,
				37,
				165,
				104,
				116,
				105,
				107,
				168,
				180,
				172,
				115,
				101
			})]
			
			public PhoneLoopSearchGraph(CIPhoneLoop this$0)
			{
				this.__existingStates = new HashMap();
				this.__firstState = new UnknownWordState();
				BranchOutState.__<clinit>();
				BranchOutState branchOutState = new BranchOutState(this.__firstState);
				this.attachState(this.__firstState, branchOutState, 0f, 0f);
				LoopBackState.__<clinit>();
				LoopBackState loopBackState = new LoopBackState(this.__firstState);
				loopBackState.setFinalState(true);
				this.attachState(loopBackState, branchOutState, 0f, 0f);
				Iterator contextIndependentUnitIterator = this$0.__model.getContextIndependentUnitIterator();
				while (contextIndependentUnitIterator.hasNext())
				{
					UnitState.__<clinit>();
					UnitState unitState = new UnitState((Unit)contextIndependentUnitIterator.next(), HMMPosition.__UNDEFINED);
					this.attachState(branchOutState, unitState, 0f, CIPhoneLoop.access_000(this$0));
					HMM hmm = this$0.__model.lookupNearestHMM(unitState.getUnit(), unitState.getPosition(), false);
					HMMState initialState = hmm.getInitialState();
					HMMStateState hmmstateState = new HMMStateState(unitState, initialState);
					this.addStateToCache(hmmstateState);
					this.attachState(unitState, hmmstateState, 0f, 0f);
					HMMStateState prevState = this.expandHMMTree(unitState, hmmstateState);
					this.attachState(prevState, loopBackState, 0f, 0f);
				}
			}

			public virtual SearchState getInitialState()
			{
				return this.__firstState;
			}

			public virtual int getNumStateOrder()
			{
				return 5;
			}

			public virtual bool getWordTokenFirst()
			{
				return false;
			}

			
			protected internal Map existingStates
			{
				
				get
				{
					return this.__existingStates;
				}
				
				private set
				{
					this.__existingStates = value;
				}
			}

			
			protected internal SentenceHMMState firstState
			{
				
				get
				{
					return this.__firstState;
				}
				
				private set
				{
					this.__firstState = value;
				}
			}

			
			internal Map __existingStates;

			internal SentenceHMMState __firstState;

			
			internal CIPhoneLoop this$0 = this$0;
		}
	}
}

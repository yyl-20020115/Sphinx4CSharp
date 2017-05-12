using edu.cmu.sphinx.linguist.acoustic;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	public class CIPhoneLoop : Object
	{
		internal static float access_000(CIPhoneLoop ciphoneLoop)
		{
			return ciphoneLoop.logPhoneInsertionProbability;
		}
		
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

		public class PhoneLoopSearchGraph : Object, SearchGraph
		{			
			protected internal virtual void attachState(SentenceHMMState prevState, SentenceHMMState nextState, float logLanguageProbability, float logInsertionProbability)
			{
				SentenceHMMStateArc arc = new SentenceHMMStateArc(nextState, logLanguageProbability, logInsertionProbability);
				prevState.connect(arc);
			}
			
			protected internal virtual void addStateToCache(SentenceHMMState state)
			{
				this.__existingStates.put(state.getSignature(), state);
			}

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
						hmmstateState = new HMMStateState(parent, hmmstateArc.getHMMState());
					}
					else
					{
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
			
			public PhoneLoopSearchGraph(CIPhoneLoop this_0)
			{
				this.this_0 = this_0;
				this.__existingStates = new HashMap();
				this.__firstState = new UnknownWordState();
				BranchOutState branchOutState = new BranchOutState(this.__firstState);
				this.attachState(this.__firstState, branchOutState, 0f, 0f);
				LoopBackState loopBackState = new LoopBackState(this.__firstState);
				loopBackState.setFinalState(true);
				this.attachState(loopBackState, branchOutState, 0f, 0f);
				Iterator contextIndependentUnitIterator = this_0.__model.getContextIndependentUnitIterator();
				while (contextIndependentUnitIterator.hasNext())
				{
					UnitState unitState = new UnitState((Unit)contextIndependentUnitIterator.next(), HMMPosition.__UNDEFINED);
					this.attachState(branchOutState, unitState, 0f, CIPhoneLoop.access_000(this_0));
					HMM hmm = this_0.__model.lookupNearestHMM(unitState.getUnit(), unitState.getPosition(), false);
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

			
			internal CIPhoneLoop this_0;
		}
	}
}

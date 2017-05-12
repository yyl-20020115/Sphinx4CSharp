using edu.cmu.sphinx.decoder.pruner;
using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.decoder.search
{
	public class WordPruningBreadthFirstSearchManager : TokenSearchManager
	{	
		protected internal virtual void localStart()
		{
			SearchGraph searchGraph = this.linguist.getSearchGraph();
			this.currentFrameNumber = 0;
			this.curTokensScored.value = (double)0f;
			this.numStateOrder = searchGraph.getNumStateOrder();
			this.activeListManager.setNumStateOrder(this.numStateOrder);
			if (this.buildWordLattice)
			{
				this.loserManager = new AlternateHypothesisManager(this.maxLatticeEdges);
			}
			SearchState initialState = searchGraph.getInitialState();
			this.activeList = this.activeListManager.getEmittingList();
			this.activeList.add(new Token(initialState, -1L));
			this.clearCollectors();
			this.growBranches();
			this.growNonEmittingBranches();
		}
		
		protected internal virtual bool recognize()
		{
			this.activeList = this.activeListManager.getEmittingList();
			int num = this.scoreTokens() ? 1 : 0;
			if (num != 0)
			{
				this.pruneBranches();
				this.currentFrameNumber++;
				if (this.growSkipInterval != 0)
				{
					bool flag = this.currentFrameNumber != 0;
					int num2 = this.growSkipInterval;
					if (num2 == -1 || (flag ? 1 : 0) % num2 == 0)
					{
						goto IL_61;
					}
				}
				this.clearCollectors();
				this.growEmittingBranches();
				this.growNonEmittingBranches();
			}
			IL_61:
			return num == 0;
		}
		
		protected internal virtual void showTokenCount()
		{
			HashSet hashSet = new HashSet();
			Iterator iterator = this.activeList.iterator();
			while (iterator.hasNext())
			{
				for (Token token = (Token)iterator.next(); token != null; token = token.getPredecessor())
				{
					hashSet.add(token);
				}
			}
			java.lang.System.@out.println(new StringBuilder().append("Token Lattice size: ").append(hashSet.size()).toString());
			hashSet = new HashSet();
			iterator = this.resultList.iterator();
			while (iterator.hasNext())
			{
				for (Token token = (Token)iterator.next(); token != null; token = token.getPredecessor())
				{
					hashSet.add(token);
				}
			}
			java.lang.System.@out.println(new StringBuilder().append("Result Lattice size: ").append(hashSet.size()).toString());
		}
		
		protected internal virtual bool scoreTokens()
		{
			this.scoreTimer.start();
			Data data = this.scorer.calculateScores(this.activeList.getTokens());
			this.scoreTimer.stop();
			Token token = null;
			if (data is Token)
			{
				token = (Token)data;
			}
			else if (data == null)
			{
				this.streamEnd = true;
			}
			if (token != null)
			{
				this.currentCollectTime = token.getCollectTime();
			}
			int result = (token == null) ? 0 : 1;
			this.activeList.setBestToken(token);
			this.monitorStates(this.activeList);
			StatisticsVariable statisticsVariable = this.curTokensScored;
			statisticsVariable.value += (double)this.activeList.size();
			StatisticsVariable statisticsVariable2 = this.totalTokensScored;
			statisticsVariable2.value += (double)this.activeList.size();
			return result != 0;
		}
		
		protected internal virtual void pruneBranches()
		{
			this.pruneTimer.start();
			this.activeList = this.pruner.prune(this.activeList);
			this.pruneTimer.stop();
		}
		
		private void clearCollectors()
		{
			this.resultList = new LinkedList();
			this.createBestTokenMap();
			this.activeListManager.clearEmittingList();
		}
		
		protected internal virtual void growEmittingBranches()
		{
			if (this.acousticLookaheadFrames <= 0f)
			{
				this.growBranches();
				return;
			}
			this.growTimer.start();
			float num = float.MinValue;
			Iterator iterator = this.activeList.iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				float num2 = token.getScore() + token.getAcousticScore() * this.acousticLookaheadFrames;
				if (num2 > num)
				{
					num = num2;
				}
			}
			float num3 = num + this.relativeBeamWidth;
			Iterator iterator2 = this.activeList.iterator();
			while (iterator2.hasNext())
			{
				Token token2 = (Token)iterator2.next();
				if (token2.getScore() + token2.getAcousticScore() * this.acousticLookaheadFrames > num3)
				{
					this.collectSuccessorTokens(token2);
				}
			}
			this.growTimer.stop();
		}
		
		private void growNonEmittingBranches()
		{
			Iterator nonEmittingListIterator = this.activeListManager.getNonEmittingListIterator();
			while (nonEmittingListIterator.hasNext())
			{
				this.activeList = (ActiveList)nonEmittingListIterator.next();
				if (this.activeList != null)
				{
					nonEmittingListIterator.remove();
					this.pruneBranches();
					this.growBranches();
				}
			}
		}
		
		protected internal virtual void createBestTokenMap()
		{
			int num = this.activeList.size() * 10;
			if (num == 0)
			{
				num = 1;
			}
			this.bestTokenMap = new HashMap(num, 0.3f);
		}
		protected internal virtual void localStop()
		{
		}
		
		protected internal virtual void growBranches()
		{
			this.growTimer.start();
			float beamThreshold = this.activeList.getBeamThreshold();
			if (this.logger.isLoggable(Level.FINE))
			{
				this.logger.fine(new StringBuilder().append("Frame: ").append(this.currentFrameNumber).append(" thresh : ").append(beamThreshold).append(" bs ").append(this.activeList.getBestScore()).append(" tok ").append(this.activeList.getBestToken()).toString());
			}
			Iterator iterator = this.activeList.iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				if (token.getScore() >= beamThreshold && this.allowExpansion(token))
				{
					this.collectSuccessorTokens(token);
				}
			}
			this.growTimer.stop();
		}

		protected internal virtual bool allowExpansion(Token t)
		{
			return true;
		}
		
		protected internal virtual void collectSuccessorTokens(Token token)
		{
			if (token.isFinal())
			{
				this.resultList.add(this.getResultListPredecessor(token));
				return;
			}
			if (!token.isEmitting() && this.keepAllTokens && this.isVisited(token))
			{
				return;
			}
			SearchState searchState = token.getSearchState();
			SearchStateArc[] successors = searchState.getSuccessors();
			Token resultListPredecessor = this.getResultListPredecessor(token);
			SearchStateArc[] array = successors;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				SearchStateArc searchStateArc = array[i];
				SearchState state = searchStateArc.getState();
				if (this._checkStateOrder)
				{
					this.checkStateOrder(searchState, state);
				}
				float num2 = token.getScore() + searchStateArc.getProbability();
				Token bestToken = this.getBestToken(state);
				if (bestToken == null)
				{
					Token token2 = new Token(resultListPredecessor, state, num2, searchStateArc.getInsertionProbability(), searchStateArc.getLanguageProbability(), this.currentCollectTime);
					StatisticsVariable statisticsVariable = this.tokensCreated;
					statisticsVariable.value += (double)1f;
					this.setBestToken(token2, state);
					this.activeListAdd(token2);
				}
				else if (bestToken.getScore() < num2)
				{
					Token token2 = bestToken.getPredecessor();
					bestToken.update(resultListPredecessor, state, num2, searchStateArc.getInsertionProbability(), searchStateArc.getLanguageProbability(), this.currentCollectTime);
					if (this.buildWordLattice && state is WordSearchState)
					{
						this.loserManager.addAlternatePredecessor(bestToken, token2);
					}
				}
				else if (this.buildWordLattice && state is WordSearchState && resultListPredecessor != null)
				{
					this.loserManager.addAlternatePredecessor(bestToken, resultListPredecessor);
				}
			}
		}
		
		protected internal virtual void monitorStates(ActiveList activeList)
		{
			this.tokenSum += (long)activeList.size();
			this.tokenCount++;
			bool flag = this.tokenCount != 0;
			int num = 1000;
			if (num == -1 || (flag ? 1 : 0) % num == 0)
			{
				Logger logger = this.logger;
				StringBuilder stringBuilder = new StringBuilder().append("Average Tokens/State: ");
				long num2 = this.tokenSum;
				long num3 = (long)this.tokenCount;
				logger.info(stringBuilder.append((num3 != -1L) ? (num2 / num3) : (-num2)).toString());
			}
		}
		
		protected internal virtual bool isVisited(Token t)
		{
			SearchState searchState = t.getSearchState();
			t = t.getPredecessor();
			while (t != null && !t.isEmitting())
			{
				if (java.lang.Object.instancehelper_equals(searchState, t.getSearchState()))
				{
					java.lang.System.@out.println(new StringBuilder().append("CS ").append(searchState).append(" match ").append(t.getSearchState()).toString());
					return true;
				}
				t = t.getPredecessor();
			}
			return false;
		}
		
		protected internal virtual void checkStateOrder(SearchState fromState, SearchState toState)
		{
			if (fromState.getOrder() == this.numStateOrder - 1)
			{
				return;
			}
			if (fromState.getOrder() > toState.getOrder())
			{
				string text = new StringBuilder().append("IllegalState order: from ").append(java.lang.Object.instancehelper_getClass(fromState).getName()).append(' ').append(fromState.toPrettyString()).append(" order: ").append(fromState.getOrder()).append(" to ").append(java.lang.Object.instancehelper_getClass(toState).getName()).append(' ').append(toState.toPrettyString()).append(" order: ").append(toState.getOrder()).toString();
				
				throw new Error(text);
			}
		}
		
		protected internal virtual Token getBestToken(SearchState state)
		{
			return (Token)this.bestTokenMap.get(state);
		}
		
		protected internal virtual void setBestToken(Token token, SearchState state)
		{
			this.bestTokenMap.put(state, token);
		}
		
		protected internal virtual void activeListAdd(Token token)
		{
			this.activeListManager.add(token);
		}
		
		public WordPruningBreadthFirstSearchManager(Linguist linguist, Pruner pruner, AcousticScorer scorer, ActiveListManager activeListManager, bool showTokenCount, double relativeWordBeamWidth, int growSkipInterval, bool checkStateOrder, bool buildWordLattice, int maxLatticeEdges, float acousticLookaheadFrames, bool keepAllTokens)
		{
			this.maxLatticeEdges = 100;
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
			this.logMath = LogMath.getLogMath();
			this.linguist = linguist;
			this.pruner = pruner;
			this.scorer = scorer;
			this.activeListManager = activeListManager;
			this._showTokenCount = showTokenCount;
			this.growSkipInterval = growSkipInterval;
			this._checkStateOrder = checkStateOrder;
			this.buildWordLattice = buildWordLattice;
			this.maxLatticeEdges = maxLatticeEdges;
			this.acousticLookaheadFrames = acousticLookaheadFrames;
			this.keepAllTokens = keepAllTokens;
			this.relativeBeamWidth = this.logMath.linearToLog(relativeWordBeamWidth);
		}
		
		public WordPruningBreadthFirstSearchManager()
		{
			this.maxLatticeEdges = 100;
		}
	
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logMath = LogMath.getLogMath();
			this.logger = ps.getLogger();
			this.linguist = (Linguist)ps.getComponent("linguist");
			this.pruner = (Pruner)ps.getComponent("pruner");
			this.scorer = (AcousticScorer)ps.getComponent("scorer");
			this.activeListManager = (ActiveListManager)ps.getComponent("activeListManager");
			this._showTokenCount = ps.getBoolean("showTokenCount").booleanValue();
			this.growSkipInterval = ps.getInt("growSkipInterval");
			this._checkStateOrder = ps.getBoolean("checkStateOrder").booleanValue();
			this.maxLatticeEdges = ps.getInt("maxLatticeEdges");
			this.acousticLookaheadFrames = ps.getFloat("acousticLookaheadFrames");
			this.relativeBeamWidth = this.logMath.linearToLog(ps.getDouble("relativeBeamWidth"));
		}
		
		public override void allocate()
		{
			this.scoreTimer = TimerPool.getTimer(this, "Score");
			this.pruneTimer = TimerPool.getTimer(this, "Prune");
			this.growTimer = TimerPool.getTimer(this, "Grow");
			this.totalTokensScored = StatisticsVariable.getStatisticsVariable("totalTokensScored");
			this.curTokensScored = StatisticsVariable.getStatisticsVariable("curTokensScored");
			this.tokensCreated = StatisticsVariable.getStatisticsVariable("tokensCreated");
			try
			{
				this.linguist.allocate();
				this.pruner.allocate();
				this.scorer.allocate();
			}
			catch (IOException ex)
			{
				throw new RuntimeException("Allocation of search manager resources failed", ex);
			}
		}
		
		public override void deallocate()
		{
			try
			{
				this.scorer.deallocate();
				this.pruner.deallocate();
				this.linguist.deallocate();
			}
			catch (IOException ex)
			{
				throw new RuntimeException("Deallocation of search manager resources failed", ex);
			}
		}
		
		public override void startRecognition()
		{
			this.linguist.startRecognition();
			this.pruner.startRecognition();
			this.scorer.startRecognition();
			this.localStart();
		}
		
		public override Result recognize(int nFrames)
		{
			int num = 0;
			Result result = null;
			this.streamEnd = false;
			int num2 = 0;
			while (num2 < nFrames && num == 0)
			{
				num = (this.recognize() ? 1 : 0);
				num2++;
			}
			if (!this.streamEnd)
			{
				result = new Result(this.loserManager, this.activeList, this.resultList, this.currentCollectTime, num != 0, this.linguist.getSearchGraph().getWordTokenFirst(), true);
			}
			if (this._showTokenCount)
			{
				this.showTokenCount();
			}
			return result;
		}
		
		public override void stopRecognition()
		{
			this.localStop();
			this.scorer.stopRecognition();
			this.pruner.stopRecognition();
			this.linguist.stopRecognition();
		}

		private void monitorWords(ActiveList activeList)
		{
		}

		public virtual ActiveList getActiveList()
		{
			return this.activeList;
		}

		public virtual void setActiveList(ActiveList activeList)
		{
			this.activeList = activeList;
		}
		
		public virtual List getResultList()
		{
			return this.resultList;
		}
		
		public virtual void setResultList(List resultList)
		{
			this.resultList = resultList;
		}

		public virtual int getCurrentFrameNumber()
		{
			return this.currentFrameNumber;
		}

		public virtual util.Timer getGrowTimer()
		{
			return this.growTimer;
		}

		public virtual StatisticsVariable getTokensCreated()
		{
			return this.tokensCreated;
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/Linguist, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_LINGUIST = "linguist";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/decoder/pruner/Pruner, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_PRUNER = "pruner";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/decoder/scorer/AcousticScorer, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_SCORER = "scorer";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_TOKEN_COUNT = "showTokenCount";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			0
		})]
		public const string PROP_GROW_SKIP_INTERVAL = "growSkipInterval";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/decoder/search/ActiveListManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_ACTIVE_LIST_MANAGER = "activeListManager";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_CHECK_STATE_ORDER = "checkStateOrder";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			100
		})]
		public const string PROP_MAX_LATTICE_EDGES = "maxLatticeEdges";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.0
		})]
		public const string PROP_ACOUSTIC_LOOKAHEAD_FRAMES = "acousticLookaheadFrames";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.0
		})]
		public const string PROP_RELATIVE_BEAM_WIDTH = "relativeBeamWidth";

		protected internal Linguist linguist;

		protected internal Pruner pruner;

		protected internal AcousticScorer scorer;

		private ActiveListManager activeListManager;

		protected internal LogMath logMath;

		protected internal Logger logger;

		protected internal bool _showTokenCount;

		protected internal bool _checkStateOrder;

		private int growSkipInterval;

		protected internal float relativeBeamWidth;

		protected internal float acousticLookaheadFrames;

		private int maxLatticeEdges;

		protected internal util.Timer scoreTimer;

		protected internal util.Timer pruneTimer;

		protected internal util.Timer growTimer;

		protected internal StatisticsVariable totalTokensScored;

		protected internal StatisticsVariable curTokensScored;

		protected internal StatisticsVariable tokensCreated;

		private long tokenSum;

		private int tokenCount;

		protected internal int currentFrameNumber;

		protected internal long currentCollectTime;

		protected internal ActiveList activeList;

		protected internal List resultList;
		
		protected internal Map bestTokenMap;

		protected internal AlternateHypothesisManager loserManager;

		private int numStateOrder;

		protected internal bool streamEnd;
	}
}

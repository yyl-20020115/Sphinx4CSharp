using System;

using edu.cmu.sphinx.decoder.pruner;
using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.decoder.search
{
	public class WordPruningBreadthFirstSearchManager : TokenSearchManager
	{
		[LineNumberTable(new byte[]
		{
			160,
			241,
			108,
			103,
			113,
			108,
			113,
			104,
			182,
			135,
			113,
			147,
			134,
			102,
			166
		})]
		
		protected internal virtual void localStart()
		{
			SearchGraph searchGraph = this.linguist.getSearchGraph();
			this.currentFrameNumber = 0;
			this.curTokensScored.value = (double)0f;
			this.numStateOrder = searchGraph.getNumStateOrder();
			this.activeListManager.setNumStateOrder(this.numStateOrder);
			if (this.buildWordLattice)
			{
				AlternateHypothesisManager.__<clinit>();
				this.loserManager = new AlternateHypothesisManager(this.maxLatticeEdges);
			}
			SearchState initialState = searchGraph.getInitialState();
			this.activeList = this.activeListManager.getEmittingList();
			this.activeList.add(new Token(initialState, -1L));
			this.clearCollectors();
			this.growBranches();
			this.growNonEmittingBranches();
		}

		[LineNumberTable(new byte[]
		{
			160,
			193,
			113,
			135,
			99,
			102,
			110,
			127,
			1,
			102,
			102,
			166
		})]
		
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

		[LineNumberTable(new byte[]
		{
			162,
			82,
			134,
			127,
			1,
			99,
			104,
			137,
			130,
			159,
			10,
			134,
			127,
			1,
			99,
			104,
			137,
			130,
			127,
			10
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			80,
			107,
			119,
			140,
			98,
			104,
			105,
			99,
			167,
			99,
			172,
			104,
			172,
			204,
			127,
			0,
			159,
			0
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			168,
			107,
			119,
			108
		})]
		
		protected internal virtual void pruneBranches()
		{
			this.pruneTimer.start();
			this.activeList = this.pruner.prune(this.activeList);
			this.pruneTimer.stop();
		}

		[LineNumberTable(new byte[]
		{
			160,
			212,
			107,
			102,
			107
		})]
		
		private void clearCollectors()
		{
			this.resultList = new LinkedList();
			this.createBestTokenMap();
			this.activeListManager.clearEmittingList();
		}

		[LineNumberTable(new byte[]
		{
			161,
			37,
			109,
			102,
			129,
			107,
			102,
			127,
			1,
			119,
			100,
			130,
			98,
			107,
			127,
			5,
			124,
			104,
			98,
			108
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			61,
			116,
			113,
			104,
			102,
			102,
			168
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			221,
			111,
			99,
			130,
			113
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			18,
			107,
			108,
			114,
			127,
			33,
			63,
			10,
			165,
			127,
			1,
			114,
			135,
			98,
			108
		})]
		
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

		[LineNumberTable(new byte[]
		{
			161,
			229,
			104,
			115,
			225,
			76,
			121,
			161,
			103,
			103,
			232,
			75,
			120,
			137,
			104,
			233,
			69,
			145,
			138,
			100,
			115,
			114,
			120,
			106,
			104,
			176,
			105,
			112,
			43,
			133,
			113,
			143,
			115,
			99,
			238,
			34,
			235,
			98
		})]
		
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
				if (this.checkStateOrder)
				{
					this.checkStateOrder(searchState, state);
				}
				float num2 = token.getScore() + searchStateArc.getProbability();
				Token bestToken = this.getBestToken(state);
				if (bestToken == null)
				{
					Token.__<clinit>();
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

		[LineNumberTable(new byte[]
		{
			161,
			158,
			116,
			142,
			119,
			159,
			28
		})]
		
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

		[LineNumberTable(new byte[]
		{
			162,
			48,
			135,
			136,
			107,
			110,
			127,
			26,
			130,
			138
		})]
		
		protected internal virtual bool isVisited(Token t)
		{
			SearchState searchState = t.getSearchState();
			t = t.getPredecessor();
			while (t != null && !t.isEmitting())
			{
				if (Object.instancehelper_equals(searchState, t.getSearchState()))
				{
					java.lang.System.@out.println(new StringBuilder().append("CS ").append(searchState).append(" match ").append(t.getSearchState()).toString());
					return true;
				}
				t = t.getPredecessor();
			}
			return false;
		}

		[LineNumberTable(new byte[]
		{
			161,
			203,
			112,
			161,
			113,
			127,
			29,
			127,
			34,
			154
		})]
		
		protected internal virtual void checkStateOrder(SearchState fromState, SearchState toState)
		{
			if (fromState.getOrder() == this.numStateOrder - 1)
			{
				return;
			}
			if (fromState.getOrder() > toState.getOrder())
			{
				string text = new StringBuilder().append("IllegalState order: from ").append(Object.instancehelper_getClass(fromState).getName()).append(' ').append(fromState.toPrettyString()).append(" order: ").append(fromState.getOrder()).append(" to ").append(Object.instancehelper_getClass(toState).getName()).append(' ').append(toState.toPrettyString()).append(" order: ").append(toState.getOrder()).toString();
				
				throw new Error(text);
			}
		}

		
		
		protected internal virtual Token getBestToken(SearchState state)
		{
			return (Token)this.bestTokenMap.get(state);
		}

		[LineNumberTable(new byte[]
		{
			161,
			193,
			110
		})]
		
		protected internal virtual void setBestToken(Token token, SearchState state)
		{
			this.bestTokenMap.put(state, token);
		}

		[LineNumberTable(new byte[]
		{
			162,
			63,
			108
		})]
		
		protected internal virtual void activeListAdd(Token token)
		{
			this.activeListManager.add(token);
		}

		[LineNumberTable(new byte[]
		{
			159,
			99,
			140,
			233,
			18,
			232,
			112,
			118,
			107,
			103,
			103,
			103,
			104,
			103,
			104,
			103,
			103,
			104,
			105,
			135,
			116
		})]
		
		public WordPruningBreadthFirstSearchManager(Linguist linguist, Pruner pruner, AcousticScorer scorer, ActiveListManager activeListManager, bool showTokenCount, double relativeWordBeamWidth, int growSkipInterval, bool checkStateOrder, bool buildWordLattice, int maxLatticeEdges, float acousticLookaheadFrames, bool keepAllTokens)
		{
			this.maxLatticeEdges = 100;
			this.logger = Logger.getLogger(Object.instancehelper_getClass(this).getName());
			this.logMath = LogMath.getLogMath();
			this.linguist = linguist;
			this.pruner = pruner;
			this.scorer = scorer;
			this.activeListManager = activeListManager;
			this.showTokenCount = showTokenCount;
			this.growSkipInterval = growSkipInterval;
			this.checkStateOrder = checkStateOrder;
			this.buildWordLattice = buildWordLattice;
			this.maxLatticeEdges = maxLatticeEdges;
			this.acousticLookaheadFrames = acousticLookaheadFrames;
			this.keepAllTokens = keepAllTokens;
			this.relativeBeamWidth = this.logMath.linearToLog(relativeWordBeamWidth);
		}

		[LineNumberTable(new byte[]
		{
			160,
			79,
			232,
			159,
			191,
			232,
			160,
			67
		})]
		
		public WordPruningBreadthFirstSearchManager()
		{
			this.maxLatticeEdges = 100;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			92,
			135,
			107,
			140,
			118,
			118,
			118,
			118,
			118,
			145,
			118,
			113,
			145,
			124
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logMath = LogMath.getLogMath();
			this.logger = ps.getLogger();
			this.linguist = (Linguist)ps.getComponent("linguist");
			this.pruner = (Pruner)ps.getComponent("pruner");
			this.scorer = (AcousticScorer)ps.getComponent("scorer");
			this.activeListManager = (ActiveListManager)ps.getComponent("activeListManager");
			this.showTokenCount = ps.getBoolean("showTokenCount").booleanValue();
			this.growSkipInterval = ps.getInt("growSkipInterval");
			this.checkStateOrder = ps.getBoolean("checkStateOrder").booleanValue();
			this.maxLatticeEdges = ps.getInt("maxLatticeEdges");
			this.acousticLookaheadFrames = ps.getFloat("acousticLookaheadFrames");
			this.relativeBeamWidth = this.logMath.linearToLog(ps.getDouble("relativeBeamWidth"));
		}

		[LineNumberTable(new byte[]
		{
			160,
			120,
			113,
			113,
			145,
			112,
			112,
			176,
			107,
			107,
			189,
			2,
			97,
			145
		})]
		
		public override void allocate()
		{
			this.scoreTimer = TimerPool.getTimer(this, "Score");
			this.pruneTimer = TimerPool.getTimer(this, "Prune");
			this.growTimer = TimerPool.getTimer(this, "Grow");
			this.totalTokensScored = StatisticsVariable.getStatisticsVariable("totalTokensScored");
			this.curTokensScored = StatisticsVariable.getStatisticsVariable("curTokensScored");
			this.tokensCreated = StatisticsVariable.getStatisticsVariable("tokensCreated");
			IOException ex2;
			try
			{
				this.linguist.allocate();
				this.pruner.allocate();
				this.scorer.allocate();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_93;
			}
			return;
			IL_93:
			IOException ex3 = ex2;
			string text = "Allocation of search manager resources failed";
			Exception ex4 = ex3;
			
			throw new RuntimeException(text, ex4);
		}

		[LineNumberTable(new byte[]
		{
			160,
			144,
			107,
			107,
			189,
			2,
			97,
			145
		})]
		
		public override void deallocate()
		{
			IOException ex2;
			try
			{
				this.scorer.deallocate();
				this.pruner.deallocate();
				this.linguist.deallocate();
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_30;
			}
			return;
			IL_30:
			IOException ex3 = ex2;
			string text = "Deallocation of search manager resources failed";
			Exception ex4 = ex3;
			
			throw new RuntimeException(text, ex4);
		}

		[LineNumberTable(new byte[]
		{
			160,
			157,
			107,
			107,
			107,
			102
		})]
		
		public override void startRecognition()
		{
			this.linguist.startRecognition();
			this.pruner.startRecognition();
			this.scorer.startRecognition();
			this.localStart();
		}

		[LineNumberTable(new byte[]
		{
			160,
			171,
			98,
			98,
			135,
			105,
			39,
			198,
			104,
			127,
			5,
			204,
			104,
			134
		})]
		
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
			if (this.showTokenCount)
			{
				this.showTokenCount();
			}
			return result;
		}

		[LineNumberTable(new byte[]
		{
			160,
			230,
			102,
			107,
			107,
			107
		})]
		
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

		public virtual Timer getGrowTimer()
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

		protected internal bool showTokenCount;

		protected internal bool checkStateOrder;

		private int growSkipInterval;

		protected internal float relativeBeamWidth;

		protected internal float acousticLookaheadFrames;

		private int maxLatticeEdges;

		protected internal Timer scoreTimer;

		protected internal Timer pruneTimer;

		protected internal Timer growTimer;

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

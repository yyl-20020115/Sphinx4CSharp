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
	public class SimpleBreadthFirstSearchManager : TokenSearchManager
	{
		public virtual ActiveList getActiveList()
		{
			return this.activeList;
		}

		[LineNumberTable(new byte[]
		{
			160,
			186,
			103,
			113,
			108,
			113,
			110,
			135,
			102
		})]
		
		protected internal virtual void localStart()
		{
			this.currentFrameNumber = 0;
			this.curTokensScored.value = (double)0f;
			ActiveList activeList = this.activeListFactory.newInstance();
			SearchState initialState = this.linguist.getSearchGraph().getInitialState();
			activeList.add(new Token(initialState, -1L));
			this.activeList = activeList;
			this.growBranches();
		}

		[LineNumberTable(new byte[]
		{
			160,
			171,
			103,
			99,
			102,
			110,
			159,
			1,
			166
		})]
		
		protected internal virtual bool recognize()
		{
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
						goto IL_44;
					}
				}
				this.growBranches();
			}
			IL_44:
			return num == 0;
		}

		[LineNumberTable(new byte[]
		{
			160,
			134,
			140,
			127,
			4,
			167,
			105,
			125,
			112,
			111,
			172,
			103,
			133
		})]
		
		protected internal virtual ActiveList undoLastGrowStep()
		{
			ActiveList activeList = this.activeList.newInstance();
			Iterator iterator = this.activeList.iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				Token predecessor = token.getPredecessor();
				while (predecessor.getPredecessor() != null && ((predecessor.isFinal() && predecessor.getPredecessor() != null && !predecessor.getPredecessor().isFinal()) || (predecessor.isEmitting() && predecessor.getData() == null) || (!predecessor.isFinal() && !predecessor.isEmitting())))
				{
					predecessor = predecessor.getPredecessor();
				}
				activeList.add(predecessor);
			}
			return activeList;
		}

		[LineNumberTable(new byte[]
		{
			161,
			176,
			117,
			102,
			127,
			1,
			99,
			104,
			137,
			98,
			127,
			11,
			102,
			127,
			1,
			99,
			104,
			137,
			98,
			159,
			11
		})]
		
		protected internal virtual void showTokenCount()
		{
			if (this.logger.isLoggable(Level.INFO))
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
				this.logger.info(new StringBuilder().append("Token Lattice size: ").append(hashSet.size()).toString());
				hashSet = new HashSet();
				iterator = this.resultList.iterator();
				while (iterator.hasNext())
				{
					for (Token token = (Token)iterator.next(); token != null; token = token.getPredecessor())
					{
						hashSet.add(token);
					}
				}
				this.logger.info(new StringBuilder().append("Result Lattice size: ").append(hashSet.size()).toString());
			}
		}

		protected internal virtual void localStop()
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			238,
			130,
			107,
			119,
			140,
			98,
			104,
			105,
			99,
			167,
			99,
			98,
			108,
			204,
			127,
			0,
			127,
			0,
			254,
			72
		})]
		
		protected internal virtual bool scoreTokens()
		{
			int result = 0;
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
				result = 1;
				this.currentCollectTime = token.getCollectTime();
				this.activeList.setBestToken(token);
			}
			StatisticsVariable statisticsVariable = this.curTokensScored;
			statisticsVariable.value += (double)this.activeList.size();
			StatisticsVariable statisticsVariable2 = this.totalTokensScored;
			statisticsVariable2.value += (double)this.activeList.size();
			this.tokensPerSecond.value = this.totalTokensScored.value / this.getTotalTime();
			return result != 0;
		}

		[LineNumberTable(new byte[]
		{
			161,
			28,
			108,
			107,
			119,
			127,
			2,
			108
		})]
		
		protected internal virtual void pruneBranches()
		{
			int num = this.activeList.size();
			this.pruneTimer.start();
			this.activeList = this.pruner.prune(this.activeList);
			StatisticsVariable statisticsVariable = this.beamPruned;
			statisticsVariable.value += (double)(num - this.activeList.size());
			this.pruneTimer.stop();
		}

		[LineNumberTable(new byte[]
		{
			160,
			207,
			111,
			99,
			130,
			107,
			108,
			103,
			107,
			113,
			108,
			148,
			123,
			103,
			98,
			108,
			114,
			109,
			111,
			191,
			49
		})]
		
		protected internal virtual void growBranches()
		{
			int num = this.activeList.size() * 10;
			if (num == 0)
			{
				num = 1;
			}
			this.growTimer.start();
			this.bestTokenMap = new HashMap(num);
			ActiveList activeList = this.activeList;
			this.resultList = new LinkedList();
			this.activeList = this.activeListFactory.newInstance();
			this.threshold = activeList.getBeamThreshold();
			this.wordThreshold = activeList.getBestScore() + this.logRelativeWordBeamWidth;
			Iterator iterator = activeList.iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				this.collectSuccessorTokens(token);
			}
			this.growTimer.stop();
			if (this.logger.isLoggable(Level.FINE))
			{
				int num2 = this.activeList.size();
				this.totalHmms += num2;
				this.logger.fine(new StringBuilder().append("Frame: ").append(this.currentFrameNumber).append(" Hmms: ").append(num2).append("  total ").append(this.totalHmms).toString());
			}
		}

		[LineNumberTable(new byte[]
		{
			161,
			74,
			135,
			104,
			141,
			110,
			129,
			105,
			109,
			129,
			231,
			73,
			118,
			169,
			113,
			104,
			106,
			133,
			147,
			165,
			233,
			72,
			105,
			109,
			103,
			146,
			120,
			109,
			237,
			69,
			106,
			100,
			109,
			103,
			147,
			120,
			107,
			109,
			98,
			107,
			106,
			103,
			11,
			197,
			154,
			248,
			13,
			235,
			119
		})]
		
		protected internal virtual void collectSuccessorTokens(Token token)
		{
			SearchState searchState = token.getSearchState();
			if (token.isFinal())
			{
				this.resultList.add(token);
			}
			if (token.getScore() < this.threshold)
			{
				return;
			}
			if (searchState is WordSearchState && token.getScore() < this.wordThreshold)
			{
				return;
			}
			SearchStateArc[] successors = searchState.getSuccessors();
			SearchStateArc[] array = successors;
			int num = array.Length;
			int i = 0;
			while (i < num)
			{
				SearchStateArc searchStateArc = array[i];
				SearchState state = searchStateArc.getState();
				float num2 = token.getScore() + searchStateArc.getProbability();
				if (!this.wantEntryPruning)
				{
					goto IL_A8;
				}
				if (num2 >= this.threshold)
				{
					if (!(state is WordSearchState) || num2 >= this.wordThreshold)
					{
						goto IL_A8;
					}
				}
				IL_1D7:
				i++;
				continue;
				IL_A8:
				Token resultListPredecessor = this.getResultListPredecessor(token);
				if (!state.isEmitting())
				{
					Token.__<clinit>();
					Token token2 = new Token(resultListPredecessor, state, num2, searchStateArc.getInsertionProbability(), searchStateArc.getLanguageProbability(), this.currentCollectTime);
					StatisticsVariable statisticsVariable = this.tokensCreated;
					statisticsVariable.value += (double)1f;
					if (!this.isVisited(token2))
					{
						this.collectSuccessorTokens(token2);
						goto IL_1D7;
					}
					goto IL_1D7;
				}
				else
				{
					Token token2 = this.getBestToken(state);
					if (token2 == null)
					{
						Token.__<clinit>();
						Token token3 = new Token(resultListPredecessor, state, num2, searchStateArc.getInsertionProbability(), searchStateArc.getLanguageProbability(), (long)this.currentFrameNumber);
						StatisticsVariable statisticsVariable2 = this.tokensCreated;
						statisticsVariable2.value += (double)1f;
						this.setBestToken(token3, state);
						this.activeList.add(token3);
						goto IL_1D7;
					}
					if (token2.getScore() <= num2)
					{
						token2.update(resultListPredecessor, state, num2, searchStateArc.getInsertionProbability(), searchStateArc.getLanguageProbability(), this.currentCollectTime);
						StatisticsVariable statisticsVariable3 = this.viterbiPruned;
						statisticsVariable3.value += (double)1f;
						goto IL_1D7;
					}
					StatisticsVariable statisticsVariable4 = this.viterbiPruned;
					statisticsVariable4.value += (double)1f;
					goto IL_1D7;
				}
			}
		}

		
		
		private double getTotalTime()
		{
			return ((double)System.currentTimeMillis() - this.startTime) / 1000.0;
		}

		[LineNumberTable(new byte[]
		{
			161,
			160,
			135,
			136,
			107,
			110,
			130,
			138
		})]
		
		private bool isVisited(Token predecessor)
		{
			SearchState searchState = predecessor.getSearchState();
			predecessor = predecessor.getPredecessor();
			while (predecessor != null && !predecessor.isEmitting())
			{
				if (java.lang.Object.instancehelper_equals(searchState, predecessor.getSearchState()))
				{
					return true;
				}
				predecessor = predecessor.getPredecessor();
			}
			return false;
		}

		[LineNumberTable(new byte[]
		{
			161,
			43,
			114,
			117,
			159,
			22
		})]
		
		protected internal virtual Token getBestToken(SearchState state)
		{
			Token token = (Token)this.bestTokenMap.get(state);
			if (this.logger.isLoggable(Level.FINER) && token != null)
			{
				this.logger.finer(new StringBuilder().append("BT ").append(token).append(" for state ").append(state).toString());
			}
			return token;
		}

		
		
		protected internal virtual Token setBestToken(Token token, SearchState state)
		{
			return (Token)this.bestTokenMap.put(state, token);
		}

		[LineNumberTable(new byte[]
		{
			85,
			134
		})]
		
		public SimpleBreadthFirstSearchManager()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			104,
			134,
			104,
			113,
			113,
			107,
			103,
			103,
			103,
			104,
			103,
			104,
			103,
			116,
			103
		})]
		
		public SimpleBreadthFirstSearchManager(Linguist linguist, Pruner pruner, AcousticScorer scorer, ActiveListFactory activeListFactory, bool showTokenCount, double relativeWordBeamWidth, int growSkipInterval, bool wantEntryPruning)
		{
			this.name = java.lang.Object.instancehelper_getClass(this).getName();
			this.logger = Logger.getLogger(this.name);
			this.logMath = LogMath.getLogMath();
			this.linguist = linguist;
			this.pruner = pruner;
			this.scorer = scorer;
			this.activeListFactory = activeListFactory;
			this.showTokenCount = showTokenCount;
			this.growSkipInterval = growSkipInterval;
			this.wantEntryPruning = wantEntryPruning;
			this.logRelativeWordBeamWidth = this.logMath.linearToLog(relativeWordBeamWidth);
			this.keepAllTokens = true;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			121,
			135,
			107,
			108,
			140,
			118,
			118,
			118,
			118,
			150,
			109,
			113,
			118,
			146,
			103
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logMath = LogMath.getLogMath();
			this.logger = ps.getLogger();
			this.name = ps.getInstanceName();
			this.linguist = (Linguist)ps.getComponent("linguist");
			this.pruner = (Pruner)ps.getComponent("pruner");
			this.scorer = (AcousticScorer)ps.getComponent("scorer");
			this.activeListFactory = (ActiveListFactory)ps.getComponent("activeListFactory");
			this.showTokenCount = ps.getBoolean("showTokenCount").booleanValue();
			double linearValue = ps.getDouble("relativeWordBeamWidth");
			this.growSkipInterval = ps.getInt("growSkipInterval");
			this.wantEntryPruning = ps.getBoolean("wantEntryPruning").booleanValue();
			this.logRelativeWordBeamWidth = this.logMath.linearToLog(linearValue);
			this.keepAllTokens = true;
		}

		[LineNumberTable(new byte[]
		{
			160,
			80,
			144,
			107,
			107,
			107,
			102,
			109,
			141
		})]
		
		public override void startRecognition()
		{
			this.logger.finer("starting recognition");
			this.linguist.startRecognition();
			this.pruner.startRecognition();
			this.scorer.startRecognition();
			this.localStart();
			if (this.startTime == (double)0f)
			{
				this.startTime = (double)System.currentTimeMillis();
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			99,
			98,
			98,
			135,
			105,
			39,
			230,
			70,
			141,
			167,
			104,
			117,
			177,
			104,
			166
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
			if (this.activeList.getBestToken() != null)
			{
				ActiveList activeList = this.undoLastGrowStep();
				if (!this.streamEnd)
				{
					result = new Result(activeList, this.resultList, (long)this.currentFrameNumber, num != 0, this.linguist.getSearchGraph().getWordTokenFirst(), false);
				}
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
			156,
			102,
			107,
			107,
			139,
			112
		})]
		
		public override void stopRecognition()
		{
			this.localStop();
			this.scorer.stopRecognition();
			this.pruner.stopRecognition();
			this.linguist.stopRecognition();
			this.logger.finer("recognition stopped");
		}

		
		protected internal virtual Map getBestTokenMap()
		{
			return this.bestTokenMap;
		}

		
		protected internal virtual void setBestTokenMap(Map bestTokenMap)
		{
			this.bestTokenMap = bestTokenMap;
		}

		
		public virtual List getResultList()
		{
			return this.resultList;
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

		[LineNumberTable(new byte[]
		{
			162,
			7,
			102,
			106,
			102,
			106,
			102,
			106,
			102,
			106,
			102,
			106,
			208,
			107,
			107,
			189,
			2,
			97,
			177,
			113,
			113,
			113
		})]
		
		public override void allocate()
		{
			this.totalTokensScored = StatisticsVariable.getStatisticsVariable("totalTokensScored");
			this.tokensPerSecond = StatisticsVariable.getStatisticsVariable("tokensScoredPerSecond");
			this.curTokensScored = StatisticsVariable.getStatisticsVariable("curTokensScored");
			this.tokensCreated = StatisticsVariable.getStatisticsVariable("tokensCreated");
			this.viterbiPruned = StatisticsVariable.getStatisticsVariable("viterbiPruned");
			this.beamPruned = StatisticsVariable.getStatisticsVariable("beamPruned");
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
				goto IL_90;
			}
			this.scoreTimer = TimerPool.getTimer(this, "Score");
			this.pruneTimer = TimerPool.getTimer(this, "Prune");
			this.growTimer = TimerPool.getTimer(this, "Grow");
			return;
			IL_90:
			IOException ex3 = ex2;
			string text = "Allocation of search manager resources failed";
			Exception ex4 = ex3;
			
			throw new RuntimeException(text, ex4);
		}

		[LineNumberTable(new byte[]
		{
			162,
			41,
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

		public override string toString()
		{
			return this.name;
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

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/decoder/search/ActiveListFactory, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_ACTIVE_LIST_FACTORY = "activeListFactory";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_TOKEN_COUNT = "showTokenCount";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.0
		})]
		public const string PROP_RELATIVE_WORD_BEAM_WIDTH = "relativeWordBeamWidth";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_WANT_ENTRY_PRUNING = "wantEntryPruning";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			0
		})]
		public const string PROP_GROW_SKIP_INTERVAL = "growSkipInterval";

		protected internal Linguist linguist;

		private Pruner pruner;

		private AcousticScorer scorer;

		protected internal int currentFrameNumber;

		protected internal long currentCollectTime;

		protected internal ActiveList activeList;

		
		protected internal List resultList;

		protected internal LogMath logMath;

		private Logger logger;

		private string name;

		private Timer scoreTimer;

		private Timer pruneTimer;

		protected internal Timer growTimer;

		private StatisticsVariable totalTokensScored;

		private StatisticsVariable tokensPerSecond;

		private StatisticsVariable curTokensScored;

		private StatisticsVariable tokensCreated;

		private StatisticsVariable viterbiPruned;

		private StatisticsVariable beamPruned;

		protected internal bool showTokenCount;

		private bool wantEntryPruning;

		
		protected internal Map bestTokenMap;

		private float logRelativeWordBeamWidth;

		private int totalHmms;

		private double startTime;

		private float threshold;

		private float wordThreshold;

		private int growSkipInterval;

		protected internal ActiveListFactory activeListFactory;

		protected internal bool streamEnd;
	}
}

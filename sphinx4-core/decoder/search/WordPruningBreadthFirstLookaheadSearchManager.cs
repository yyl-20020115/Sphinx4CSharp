﻿using edu.cmu.sphinx.decoder.pruner;
using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.linguist.allphone;
using edu.cmu.sphinx.linguist.lextree;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	public class WordPruningBreadthFirstLookaheadSearchManager : WordPruningBreadthFirstSearchManager
	{		
		private void fastMatchRecognize()
		{
			int num = this.scoreFastMatchTokens() ? 1 : 0;
			if (num != 0)
			{
				this.pruneFastMatchBranches();
				this.currentFastMatchFrameNumber++;
				this.createFastMatchBestTokenMap();
				this.growFastmatchBranches();
			}
		}
		
		protected internal virtual bool scoreFastMatchTokens()
		{
			this.scoreTimer.start();
			Data data = this.scorer.calculateScoresAndStoreData(this.fastmatchActiveList.getTokens());
			this.scoreTimer.stop();
			Token token = null;
			if (data is Token)
			{
				token = (Token)data;
			}
			else
			{
				this.fastmatchStreamEnd = true;
			}
			int result = (token == null) ? 0 : 1;
			this.fastmatchActiveList.setBestToken(token);
			this.monitorStates(this.fastmatchActiveList);
			StatisticsVariable curTokensScored = this.curTokensScored;
			curTokensScored.value += (double)this.fastmatchActiveList.size();
			StatisticsVariable totalTokensScored = this.totalTokensScored;
			totalTokensScored.value += (double)this.fastmatchActiveList.size();
			return result != 0;
		}
		
		protected internal virtual void pruneFastMatchBranches()
		{
			this.pruneTimer.start();
			this.fastmatchActiveList = this.pruner.prune(this.fastmatchActiveList);
			this.pruneTimer.stop();
		}
		
		protected internal virtual void createFastMatchBestTokenMap()
		{
			int num = this.fastmatchActiveList.size() * 10;
			if (num == 0)
			{
				num = 1;
			}
			this.fastMatchBestTokenMap = new HashMap(num);
		}
		
		protected internal virtual void growFastmatchBranches()
		{
			this.growTimer.start();
			ActiveList activeList = this.fastmatchActiveList;
			this.fastmatchActiveList = this.fastmatchActiveListFactory.newInstance();
			float beamThreshold = activeList.getBeamThreshold();
			float[] array = new float[1024];
			Arrays.fill(array, float.MinValue);
			float num = float.MinValue;
			Iterator iterator = activeList.iterator();
			while (iterator.hasNext())
			{
				Token token = (Token)iterator.next();
				float score = token.getScore();
				if (score >= beamThreshold)
				{
					if (token.getSearchState() is PhoneHmmSearchState)
					{
						int baseId = ((PhoneHmmSearchState)token.getSearchState()).getBaseId();
						if (array[baseId] < score)
						{
							array[baseId] = score;
						}
						if (num < score)
						{
							num = score;
						}
					}
					this.collectFastMatchSuccessorTokens(token);
				}
			}
			this.ciScores.add(new WordPruningBreadthFirstLookaheadSearchManager.FrameCiScores(this, array, num));
			this.growTimer.stop();
		}
		
		protected internal virtual void collectFastMatchSuccessorTokens(Token token)
		{
			SearchState searchState = token.getSearchState();
			SearchStateArc[] successors = searchState.getSuccessors();
			SearchStateArc[] array = successors;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				SearchStateArc searchStateArc = array[i];
				SearchState state = searchStateArc.getState();
				float num2 = token.getScore() + searchStateArc.getProbability();
				Token resultListPredecessor = this.getResultListPredecessor(token);
				if (!state.isEmitting())
				{
					Token token2 = new Token(resultListPredecessor, state, num2, searchStateArc.getInsertionProbability(), searchStateArc.getLanguageProbability(), (long)this.currentFastMatchFrameNumber);
					StatisticsVariable tokensCreated = this.tokensCreated;
					tokensCreated.value += (double)1f;
					if (!this.isVisited(token2))
					{
						this.collectFastMatchSuccessorTokens(token2);
					}
				}
				else
				{
					Token token2 = this.getFastMatchBestToken(state);
					if (token2 == null)
					{
						Token token3 = new Token(resultListPredecessor, state, num2, searchStateArc.getInsertionProbability(), searchStateArc.getLanguageProbability(), (long)this.currentFastMatchFrameNumber);
						StatisticsVariable tokensCreated2 = this.tokensCreated;
						tokensCreated2.value += (double)1f;
						this.setFastMatchBestToken(token3, state);
						this.fastmatchActiveList.add(token3);
					}
					else if (token2.getScore() <= num2)
					{
						token2.update(resultListPredecessor, state, num2, searchStateArc.getInsertionProbability(), searchStateArc.getLanguageProbability(), (long)this.currentFastMatchFrameNumber);
					}
				}
			}
		}
		
		protected internal virtual Token getFastMatchBestToken(SearchState state)
		{
			return (Token)this.fastMatchBestTokenMap.get(state);
		}
		
		protected internal virtual void setFastMatchBestToken(Token token, SearchState state)
		{
			this.fastMatchBestTokenMap.put(state, token);
		}
		
		private Float updateLookaheadPenalty(int num)
		{
			if (this.ciScores.isEmpty())
			{
				return Float.valueOf(0f);
			}
			float num2 = float.MinValue;
			Iterator iterator = this.ciScores.iterator();
			while (iterator.hasNext())
			{
				WordPruningBreadthFirstLookaheadSearchManager.FrameCiScores frameCiScores = (WordPruningBreadthFirstLookaheadSearchManager.FrameCiScores)iterator.next();
				float num3 = frameCiScores.scores[num] - frameCiScores.maxScore;
				if (num3 > num2)
				{
					num2 = num3;
				}
			}
			this.penalties.put(Integer.valueOf(num), Float.valueOf(num2));
			return Float.valueOf(num2);
		}
		
		public WordPruningBreadthFirstLookaheadSearchManager(Linguist linguist, Linguist fastmatchLinguist, Loader loader, Pruner pruner, AcousticScorer scorer, ActiveListManager activeListManager, ActiveListFactory fastmatchActiveListFactory, bool showTokenCount, double relativeWordBeamWidth, int growSkipInterval, bool checkStateOrder, bool buildWordLattice, int lookaheadWindow, float lookaheadWeight, int maxLatticeEdges, float acousticLookaheadFrames, bool keepAllTokens) : base(linguist, pruner, scorer, activeListManager, showTokenCount, relativeWordBeamWidth, growSkipInterval, checkStateOrder, buildWordLattice, maxLatticeEdges, acousticLookaheadFrames, keepAllTokens)
		{
			this.loader = loader;
			this.fastmatchLinguist = fastmatchLinguist;
			this.fastmatchActiveListFactory = fastmatchActiveListFactory;
			this.lookaheadWindow = lookaheadWindow;
			this.lookaheadWeight = lookaheadWeight;
			if (lookaheadWindow < 1 || lookaheadWindow > 10)
			{
				string text = new StringBuilder().append("Unsupported lookahead window size: ").append(lookaheadWindow).append(". Value in range [1..10] is expected").toString();
				
				throw new IllegalArgumentException(text);
			}
			this.ciScores = new LinkedList();
			this.penalties = new HashMap();
			if (loader is Sphinx3Loader && ((Sphinx3Loader)loader).hasTiedMixtures())
			{
				((Sphinx3Loader)loader).setGauScoresQueueLength(lookaheadWindow + 2);
			}
		}
		
		public WordPruningBreadthFirstLookaheadSearchManager()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.fastmatchLinguist = (Linguist)ps.getComponent("fastmatchLinguist");
			this.fastmatchActiveListFactory = (ActiveListFactory)ps.getComponent("fastmatchActiveListFactory");
			this.loader = (Loader)ps.getComponent("loader");
			this.lookaheadWindow = ps.getInt("lookaheadWindow");
			this.lookaheadWeight = ps.getFloat("lookaheadPenaltyWeight");
			if (this.lookaheadWindow < 1 || this.lookaheadWindow > 10)
			{
				string name = ClassLiteral<WordPruningBreadthFirstLookaheadSearchManager>.Value.getName();
				string propertyName = "lookaheadWindow";
				string msg = new StringBuilder().append("Unsupported lookahead window size: ").append(this.lookaheadWindow).append(". Value in range [1..10] is expected").toString();
				
				throw new PropertyException(name, propertyName, msg);
			}
			this.ciScores = new LinkedList();
			this.penalties = new HashMap();
			if (this.loader is Sphinx3Loader && ((Sphinx3Loader)this.loader).hasTiedMixtures())
			{
				((Sphinx3Loader)this.loader).setGauScoresQueueLength(this.lookaheadWindow + 2);
			}
		}
		
		public override Result recognize(int nFrames)
		{
			int num = 0;
			Result result = null;
			this.streamEnd = false;
			int num2 = 0;
			while (num2 < nFrames && num == 0)
			{
				if (!this.fastmatchStreamEnd)
				{
					this.fastMatchRecognize();
				}
				this.penalties.clear();
				this.ciScores.poll();
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
		
		protected internal override void localStart()
		{
			this.currentFastMatchFrameNumber = 0;
			if (this.loader is Sphinx3Loader && ((Sphinx3Loader)this.loader).hasTiedMixtures())
			{
				((Sphinx3Loader)this.loader).clearGauScores();
			}
			this.fastmatchActiveList = this.fastmatchActiveListFactory.newInstance();
			SearchState initialState = this.fastmatchLinguist.getSearchGraph().getInitialState();
			ActiveList activeList = this.fastmatchActiveList;
			activeList.add(new Token(initialState, (long)this.currentFastMatchFrameNumber));
			this.createFastMatchBestTokenMap();
			this.growFastmatchBranches();
			this.fastmatchStreamEnd = false;
			int num = 0;
			while (num < this.lookaheadWindow - 1 && !this.fastmatchStreamEnd)
			{
				this.fastMatchRecognize();
				num++;
			}
			base.localStart();
		}
		
		protected internal override void collectSuccessorTokens(Token token)
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
			float score = token.getScore();
			float beamThreshold = this.activeList.getBeamThreshold();
			int num = (!(searchState is LexTreeLinguist.LexTreeNonEmittingHMMState) && !(searchState is LexTreeLinguist.LexTreeWordState) && !(searchState is LexTreeLinguist.LexTreeEndUnitState)) ? 0 : 1;
			SearchStateArc[] array = successors;
			int num2 = array.Length;
			int i = 0;
			while (i < num2)
			{
				SearchStateArc searchStateArc = array[i];
				SearchState state = searchStateArc.getState();
				if (num == 0 || !(state is LexTreeLinguist.LexTreeHMMState))
				{
					goto IL_110;
				}
				int baseID = ((LexTreeLinguist.LexTreeHMMState)state).getHMMState().getHMM().getBaseUnit().getBaseID();
				Float @float = ((Float)this.penalties.get(Integer.valueOf(baseID))) ?? this.updateLookaheadPenalty(baseID);
				if (score + this.lookaheadWeight * @float.floatValue() >= beamThreshold)
				{
					goto IL_110;
				}
				IL_207:
				i++;
				continue;
				IL_110:
				if (this._checkStateOrder)
				{
					this.checkStateOrder(searchState, state);
				}
				float num3 = score + searchStateArc.getProbability();
				Token bestToken = this.getBestToken(state);
				if (bestToken == null)
				{
					Token token2 = new Token(resultListPredecessor, state, num3, searchStateArc.getInsertionProbability(), searchStateArc.getLanguageProbability(), this.currentCollectTime);
					StatisticsVariable tokensCreated = this.tokensCreated;
					tokensCreated.value += (double)1f;
					this.setBestToken(token2, state);
					this.activeListAdd(token2);
					goto IL_207;
				}
				if (bestToken.getScore() < num3)
				{
					Token token2 = bestToken.getPredecessor();
					bestToken.update(resultListPredecessor, state, num3, searchStateArc.getInsertionProbability(), searchStateArc.getLanguageProbability(), this.currentCollectTime);
					if (this.buildWordLattice && state is WordSearchState)
					{
						this.loserManager.addAlternatePredecessor(bestToken, token2);
					}
					goto IL_207;
				}
				if (this.buildWordLattice && state is WordSearchState && resultListPredecessor != null)
				{
					this.loserManager.addAlternatePredecessor(bestToken, resultListPredecessor);
					goto IL_207;
				}
				goto IL_207;
			}
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/tiedstate/Loader, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_LOADER = "loader";

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
		public const string PROP_FASTMATCH_LINGUIST = "fastmatchLinguist";

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
		public const string PROP_FM_ACTIVE_LIST_FACTORY = "fastmatchActiveListFactory";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_LOOKAHEAD_PENALTY_WEIGHT = "lookaheadPenaltyWeight";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			5
		})]
		public const string PROP_LOOKAHEAD_WINDOW = "lookaheadWindow";

		private Linguist fastmatchLinguist;

		private Loader loader;

		private ActiveListFactory fastmatchActiveListFactory;

		private int lookaheadWindow;

		private float lookaheadWeight;
		
		private HashMap penalties;
		
		private LinkedList ciScores;

		private int currentFastMatchFrameNumber;

		protected internal ActiveList fastmatchActiveList;

		protected internal Map fastMatchBestTokenMap;

		private bool fastmatchStreamEnd;
		
		internal sealed class FrameCiScores : Object
		{			
			public FrameCiScores(WordPruningBreadthFirstLookaheadSearchManager wordPruningBreadthFirstLookaheadSearchManager, float[] array, float num)
			{
				this_0 = wordPruningBreadthFirstLookaheadSearchManager;
				this.scores = array;
				this.maxScore = num;
			}
			
			public float[] scores;
			
			public float maxScore;
			
			internal WordPruningBreadthFirstLookaheadSearchManager this_0;
		}
	}
}

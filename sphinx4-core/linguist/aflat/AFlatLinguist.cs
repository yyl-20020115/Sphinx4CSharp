﻿using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.grammar;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.aflat
{
	public class AFlatLinguist : LinguistBase
	{		
		internal static Grammar access_1500(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.grammar;
		}
		
		protected internal virtual void allocateAcousticModel()
		{
			this.acousticModel.allocate();
			if (this.addOutOfGrammarBranch)
			{
				this.phoneLoopAcousticModel.allocate();
			}
		}
		
		private void compileGrammar()
		{
			this.logger.info("Compiling Grammar");
			Set grammarNodes = this.grammar.getGrammarNodes();
			Iterator iterator = grammarNodes.iterator();
			while (iterator.hasNext())
			{
				GrammarNode grammarNode = (GrammarNode)iterator.next();
				this.initUnitMaps(grammarNode);
			}
			this.logger.info(new StringBuilder().append("Free Memory before generating Search Graph= ").append(this.runtime.freeMemory() / (long)((ulong)1048576)).append(" MB").toString());
			this.searchGraph = new AFlatLinguist.DynamicFlatSearchGraph(this);
		}
		
		private void initUnitMaps(GrammarNode grammarNode)
		{
			if (this.nodeToNextUnitArrayMap.get(grammarNode) == null)
			{
				HashSet hashSet = new HashSet();
				HashSet hashSet2 = new HashSet();
				GrammarArc[] successors = grammarNode.getSuccessors();
				GrammarArc[] array = successors;
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					GrammarArc grammarArc = array[i];
					GrammarNode grammarNode2 = grammarArc.getGrammarNode();
					this.collectNextUnits(grammarNode2, hashSet, hashSet2);
				}
				int[] array2 = new int[hashSet2.size()];
				num = 0;
				Iterator iterator = hashSet2.iterator();
				while (iterator.hasNext())
				{
					Unit unit = (Unit)iterator.next();
					int[] array3 = array2;
					int num2 = num;
					num++;
					array3[num2] = unit.getBaseID();
				}
				this.nodeToNextUnitArrayMap.put(grammarNode, array2);
			}
			if (this.nodeToUnitSetMap.get(grammarNode) == null)
			{
				HashSet hashSet = new HashSet();
				HashSet hashSet2 = new HashSet();
				this.collectNextUnits(grammarNode, hashSet, hashSet2);
				this.nodeToUnitSetMap.put(grammarNode, hashSet2);
			}
		}
		
		private void collectNextUnits(GrammarNode grammarNode, Set set, Set set2)
		{
			if (set.contains(grammarNode))
			{
				return;
			}
			set.add(grammarNode);
			if (grammarNode.isFinalNode())
			{
				set2.add(UnitManager.__SILENCE);
			}
			else if (!grammarNode.isEmpty())
			{
				Word word = grammarNode.getWord();
				Pronunciation[] pronunciations = word.getPronunciations();
				Pronunciation[] array = pronunciations;
				int i = array.Length;
				for (int j = 0; j < i; j++)
				{
					Pronunciation pronunciation = array[j];
					set2.add(pronunciation.getUnits()[0]);
				}
			}
			else
			{
				GrammarArc[] successors = grammarNode.getSuccessors();
				GrammarArc[] array2 = successors;
				int num = array2.Length;
				for (int i = 0; i < num; i++)
				{
					GrammarArc grammarArc = array2[i];
					GrammarNode grammarNode2 = grammarArc.getGrammarNode();
					this.collectNextUnits(grammarNode2, set, set2);
				}
			}
		}
		
		public AFlatLinguist(AcousticModel acousticModel, Grammar grammar, UnitManager unitManager, double wordInsertionProbability, double silenceInsertionProbability, double unitInsertionProbability, double fillerInsertionProbability, float languageWeight, bool addOutOfGrammarBranch, double outOfGrammarBranchProbability, double phoneInsertionProbability, AcousticModel phoneLoopAcousticModel)
		{
			this.runtime = Runtime.getRuntime();
			this.counterForMemoryLogging = 0L;
			this.EMPTY_ARCS = new SearchStateArc[0];
			this.successorCache = new HashMap();
			this.logger = Logger.getLogger(Object.instancehelper_getClass(this).getName());
			this.acousticModel = acousticModel;
			this.grammar = grammar;
			this.unitManager = unitManager;
			LogMath logMath = LogMath.getLogMath();
			this.logWordInsertionProbability = logMath.linearToLog(wordInsertionProbability);
			this.logSilenceInsertionProbability = logMath.linearToLog(silenceInsertionProbability);
			this.logUnitInsertionProbability = logMath.linearToLog(unitInsertionProbability);
			this.logFillerInsertionProbability = logMath.linearToLog(fillerInsertionProbability);
			this.languageWeight = languageWeight;
			this.addOutOfGrammarBranch = addOutOfGrammarBranch;
			this.logOutOfGrammarBranchProbability = logMath.linearToLog(outOfGrammarBranchProbability);
			this.logPhoneInsertionProbability = logMath.linearToLog((double)this.logPhoneInsertionProbability);
			if (addOutOfGrammarBranch)
			{
				this.phoneLoopAcousticModel = phoneLoopAcousticModel;
			}
		}
		
		public AFlatLinguist()
		{
			this.runtime = Runtime.getRuntime();
			this.counterForMemoryLogging = 0L;
			this.EMPTY_ARCS = new SearchStateArc[0];
			this.successorCache = new HashMap();
		}
		
		public override void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.acousticModel = (AcousticModel)ps.getComponent("acousticModel");
			this.grammar = (Grammar)ps.getComponent("grammar");
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
			LogMath logMath = LogMath.getLogMath();
			this.logWordInsertionProbability = logMath.linearToLog(ps.getDouble("wordInsertionProbability"));
			this.logSilenceInsertionProbability = logMath.linearToLog(ps.getDouble("silenceInsertionProbability"));
			this.logUnitInsertionProbability = logMath.linearToLog(ps.getDouble("unitInsertionProbability"));
			this.logFillerInsertionProbability = logMath.linearToLog(ps.getDouble("fillerInsertionProbability"));
			this.languageWeight = ps.getFloat("languageWeight");
			this.addOutOfGrammarBranch = ps.getBoolean("addOutOfGrammarBranch").booleanValue();
			this.logOutOfGrammarBranchProbability = logMath.linearToLog(ps.getDouble("outOfGrammarProbability"));
			this.logPhoneInsertionProbability = logMath.linearToLog(ps.getDouble("phoneInsertionProbability"));
			if (this.addOutOfGrammarBranch)
			{
				this.phoneLoopAcousticModel = (AcousticModel)ps.getComponent("phoneLoopAcousticModel");
			}
		}

		public override SearchGraph getSearchGraph()
		{
			this.logger.info("Generated Search Graph");
			this.logger.info(new StringBuilder().append("Total Memory= ").append(this.runtime.totalMemory() / (long)((ulong)1048576)).append(" MB").toString());
			this.logger.info(new StringBuilder().append("Free Memory = ").append(this.runtime.freeMemory() / (long)((ulong)1048576)).append(" MB").toString());
			return this.searchGraph;
		}
		
		protected internal virtual void setupAcousticModel(PropertySheet ps)
		{
			this.acousticModel = (AcousticModel)ps.getComponent("acousticModel");
		}
		
		public override void allocate()
		{
			this.logger.info("Allocating DFLAT");
			this.allocateAcousticModel();
			this.grammar.allocate();
			this.hmmPool = new HMMPool(this.acousticModel, this.logger, this.unitManager);
			this.nodeToNextUnitArrayMap = new HashMap();
			this.nodeToUnitSetMap = new HashMap();
			sphinx.util.Timer timer = TimerPool.getTimer(this, "compileGrammar");
			timer.start();
			this.compileGrammar();
			timer.stop();
			this.logger.info("Done allocating  DFLAT");
		}
		
		public override void deallocate()
		{
			if (this.acousticModel != null)
			{
				this.acousticModel.deallocate();
			}
			this.grammar.deallocate();
		}

		public virtual float getLogSilenceInsertionProbability()
		{
			return this.logSilenceInsertionProbability;
		}

		public override void startRecognition()
		{
		}

		public override void stopRecognition()
		{
		}
		
		internal static float access_000(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.languageWeight;
		}
		
		internal static long access_108(AFlatLinguist aflatLinguist)
		{
			long num = aflatLinguist.counterForMemoryLogging;
			long result = num;
			aflatLinguist.counterForMemoryLogging = num + 1L;
			return result;
		}
				
		internal static long access_100(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.counterForMemoryLogging;
		}		
		
		internal static Logger access_200(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.logger;
		}		
		
		internal static SearchStateArc[] access_300(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.EMPTY_ARCS;
		}		
		
		internal static bool access_400(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.addOutOfGrammarBranch;
		}
				
		internal static AcousticModel access_500(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.acousticModel;
		}
				
		internal static float access_600(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.logOutOfGrammarBranchProbability;
		}

		internal static float access_700(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.logPhoneInsertionProbability;
		}
		
		internal static HMMPool access_800(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.hmmPool;
		}
		
		internal static Map access_900(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.nodeToUnitSetMap;
		}
		
		internal static Map access_1000(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.nodeToNextUnitArrayMap;
		}
		
		internal static float access_1100(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.logWordInsertionProbability;
		}
		
		internal static float access_1200(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.logSilenceInsertionProbability;
		}
		
		internal static float access_1300(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.logFillerInsertionProbability;
		}
		
		internal static float access_1400(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.logUnitInsertionProbability;
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/language/grammar/Grammar, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string GRAMMAR = "grammar";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/UnitManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string UNIT_MANAGER = "unitManager";

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
		public const string ACOUSTIC_MODEL = "acousticModel";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string ADD_OUT_OF_GRAMMAR_BRANCH = "addOutOfGrammarBranch";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string OUT_OF_GRAMMAR_PROBABILITY = "outOfGrammarProbability";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PHONE_INSERTION_PROBABILITY = "phoneInsertionProbability";

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
		public const string PHONE_LOOP_ACOUSTIC_MODEL = "phoneLoopAcousticModel";

		private Grammar grammar;

		private AcousticModel acousticModel;

		private AcousticModel phoneLoopAcousticModel;

		private UnitManager unitManager;

		private float logWordInsertionProbability;

		private float logSilenceInsertionProbability;

		private float logUnitInsertionProbability;

		private float logFillerInsertionProbability;

		private float languageWeight;

		private float logOutOfGrammarBranchProbability;

		private float logPhoneInsertionProbability;

		private bool addOutOfGrammarBranch;

		private SearchGraph searchGraph;

		private Logger logger;

		private HMMPool hmmPool;

		//internal SearchStateArc outOfGrammarGraph = null;

		public Runtime runtime;

		private long counterForMemoryLogging;
		
		private Map nodeToNextUnitArrayMap;
		
		private Map nodeToUnitSetMap;
		
		private SearchStateArc[] EMPTY_ARCS;		
		
		internal Map successorCache;
	
		internal sealed class DynamicFlatSearchGraph : Object, SearchGraph
		{
			internal DynamicFlatSearchGraph(AFlatLinguist aflatLinguist)
			{
				this.this_0 = aflatLinguist;
			}
			
			public SearchState getInitialState()
			{
				AFlatLinguist.InitialState initialState = new AFlatLinguist.InitialState(this.this_0);
				initialState.addArc(new AFlatLinguist.GrammarState(this.this_0, AFlatLinguist.access_1500(this.this_0).getInitialNode()));
				return initialState;
			}

			public int getNumStateOrder()
			{
				return 5;
			}

			public bool getWordTokenFirst()
			{
				return true;
			}
		
			internal AFlatLinguist this_0;
		}

		internal abstract class FlatSearchState : Object, SearchState, SearchStateArc
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
			
			internal FlatSearchState(AFlatLinguist aflatLinguist)
			{
				this.this_0 = aflatLinguist;
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

			internal virtual SearchStateArc[] getCachedSuccessors()
			{
				return (SearchStateArc[])this.this_0.successorCache.get(this);
			}
			
			internal virtual void cacheSuccessors(SearchStateArc[] array)
			{
				this.this_0.successorCache.put(this, array);
			}

			internal const int ANY = 0;
			
			internal AFlatLinguist this_0;
		}

		internal sealed class FullHMMSearchState : AFlatLinguist.FlatSearchState, UnitSearchState, SearchState
		{
			internal FullHMMSearchState(AFlatLinguist aflatLinguist, AFlatLinguist.PronunciationState pronunciationState, int num, int leftID, int rightID) : base(aflatLinguist)
			{
				this.pState = pronunciationState;
				this.index = num;
				this.lc = leftID;
				this.rc = rightID;
				int baseID = pronunciationState.getPronunciation().getUnits()[num].getBaseID();
				int unitID = AFlatLinguist.access_800(aflatLinguist).buildID(baseID, leftID, rightID);
				this.hmm = AFlatLinguist.access_800(aflatLinguist).getHMM(unitID, this.getPosition());
				this._isLastUnitOfWord = (num == pronunciationState.getPronunciation().getUnits().Length - 1);
			}
			
			internal HMMPosition getPosition()
			{
				int num = this.pState.getPronunciation().getUnits().Length;
				if (num == 1)
				{
					return HMMPosition.__SINGLE;
				}
				if (this.index == 0)
				{
					return HMMPosition.__BEGIN;
				}
				if (this.index == num - 1)
				{
					return HMMPosition.__END;
				}
				return HMMPosition.__INTERNAL;
			}

			internal HMM getHMM()
			{
				return this.hmm;
			}

			internal bool isLastUnitOfWord()
			{
				return this._isLastUnitOfWord;
			}

			internal int getRC()
			{
				return this.rc;
			}
			
			internal FullHMMSearchState(AFlatLinguist aflatLinguist, AFlatLinguist.PronunciationState pronunciationState, int num, int num2) : this(aflatLinguist, pronunciationState, num, num2, pronunciationState.getPronunciation().getUnits()[num + 1].getBaseID())
			{
			}
			
			public override float getInsertionProbability()
			{
				Unit baseUnit = this.hmm.getBaseUnit();
				if (baseUnit.isSilence())
				{
					return AFlatLinguist.access_1200(this.this_0);
				}
				if (baseUnit.isFiller())
				{
					return AFlatLinguist.access_1300(this.this_0);
				}
				return AFlatLinguist.access_1400(this.this_0);
			}

			public override string toString()
			{
				return this.hmm.getUnit().toString();
			}

			public override int hashCode()
			{
				return Object.instancehelper_hashCode(this.pState.getGrammarState().getGrammarNode()) * 29 + Object.instancehelper_hashCode(this.pState.getPronunciation()) * 19 + this.index * 7 + 43 * this.lc + this.rc;
			}
			
			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is AFlatLinguist.FullHMMSearchState)
				{
					AFlatLinguist.FullHMMSearchState fullHMMSearchState = (AFlatLinguist.FullHMMSearchState)obj;
					return this.pState.getGrammarState().getGrammarNode() == fullHMMSearchState.pState.getGrammarState().getGrammarNode() && this.pState.getPronunciation() == fullHMMSearchState.pState.getPronunciation() && this.index == fullHMMSearchState.index && this.lc == fullHMMSearchState.lc && this.rc == fullHMMSearchState.rc;
				}
				return false;
			}

			public Unit getUnit()
			{
				return this.hmm.getBaseUnit();
			}
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedSuccessors();
				if (array == null)
				{
					array = new SearchStateArc[]
					{
						new AFlatLinguist.HMMStateSearchState(this.this_0, this, this.hmm.getInitialState())
					};
					this.cacheSuccessors(array);
				}
				return array;
			}

			public override int getOrder()
			{
				return 3;
			}
			
			public override string getSignature()
			{
				return new StringBuilder().append("HSS ").append(this.pState.getGrammarState().getGrammarNode()).append(this.pState.getPronunciation()).append(this.index).append('-').append(this.rc).append('-').append(this.lc).toString();
			}
			
			internal SearchStateArc[] getNextArcs()
			{
				int baseID = this.getHMM().getBaseUnit().getBaseID();
				SearchStateArc[] result;
				if (!this.isLastUnitOfWord())
				{
					result = this.pState.getSuccessors(baseID, this.index + 1);
				}
				else
				{
					AFlatLinguist.GrammarState grammarState = this.pState.getGrammarState();
					result = grammarState.getNextGrammarStates(baseID, this.getRC());
				}
				return result;
			}
			
			private AFlatLinguist.PronunciationState pState;
			
			private int index;
			
			private int lc;
			
			private int rc;
			
			private HMM hmm;

			private bool _isLastUnitOfWord;
		}

		internal class GrammarState : AFlatLinguist.FlatSearchState
		{
			internal GrammarState(AFlatLinguist aflatLinguist, GrammarNode grammarNode) : this(aflatLinguist, grammarNode, 0f, UnitManager.__SILENCE.getBaseID())
			{
			}

			internal virtual GrammarNode getGrammarNode()
			{
				return this.node;
			}

			internal virtual SearchStateArc[] getNextGrammarStates(int num, int num2)
			{
				GrammarArc[] array = this.node.getSuccessors();
				array = this.filter(array, num2);
				SearchStateArc[] array2 = new SearchStateArc[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					GrammarArc grammarArc = array[i];
					array2[i] = new AFlatLinguist.GrammarState(this.this_0, grammarArc.getGrammarNode(), grammarArc.getProbability(), num, num2);
				}
				return array2;
			}

			internal GrammarState(AFlatLinguist aflatLinguist, GrammarNode grammarNode, float num, int num2) : this(aflatLinguist, grammarNode, num, num2, 0)
			{
			}

			internal GrammarState(AFlatLinguist aflatLinguist, GrammarNode grammarNode, float num, int num2, int num3) : base(aflatLinguist)
			{
				this.lc = num2;
				this.nextBaseID = num3;
				this.node = grammarNode;
				this.languageProbability = num;
			}

			public override bool isFinal()
			{
				return this.node.isFinalNode();
			}

			internal virtual Pronunciation[] filter(Pronunciation[] result, int num)
			{
				return result;
			}

			internal virtual GrammarArc[] filter(GrammarArc[] array, int num)
			{
				if (num != 0)
				{
					ArrayList arrayList = new ArrayList();
					GrammarArc[] array2 = array;
					int num2 = array2.Length;
					for (int i = 0; i < num2; i++)
					{
						GrammarArc grammarArc = array2[i];
						GrammarNode grammarNode = grammarArc.getGrammarNode();
						if (this.hasEntryContext(grammarNode, num))
						{
							arrayList.add(grammarArc);
						}
					}
					array = (GrammarArc[])arrayList.toArray(new GrammarArc[arrayList.size()]);
				}
				return array;
			}

			private bool hasEntryContext(GrammarNode grammarNode, int unitID)
			{
				Set set = (Set)AFlatLinguist.access_900(this.this_0).get(grammarNode);
				return set.contains(AFlatLinguist.access_800(this.this_0).getUnit(unitID));
			}

			public override float getLanguageProbability()
			{
				return this.languageProbability * AFlatLinguist.access_000(this.this_0);
			}

			public override int hashCode()
			{
				return Object.instancehelper_hashCode(this.node) * 17 + this.lc * 7 + this.nextBaseID;
			}

			public override bool equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				if (o is AFlatLinguist.GrammarState)
				{
					AFlatLinguist.GrammarState grammarState = (AFlatLinguist.GrammarState)o;
					return grammarState.node == this.node && this.lc == grammarState.lc && this.nextBaseID == grammarState.nextBaseID;
				}
				return false;
			}

			public override SearchStateArc[] getSuccessors()
			{
				AFlatLinguist.access_108(this.this_0);
				ulong num = (ulong)AFlatLinguist.access_100(this.this_0);
				ulong num2 = (ulong)500000;
				if (((num2 != 18446744073709551615UL) ? (num % num2) : 0UL) == 0UL)
				{
					AFlatLinguist.access_200(this.this_0).info(new StringBuilder().append("Free Memory= ").append(this.this_0.runtime.freeMemory() / (long)((ulong)1048576)).append(" MB\tMax Memory= ").append(this.this_0.runtime.maxMemory() / (long)((ulong)1048576)).append("MB").toString());
				}
				SearchStateArc[] array = this.getCachedSuccessors();
				if (array == null)
				{
					if (this.isFinal())
					{
						array = AFlatLinguist.access_300(this.this_0);
					}
					else if (this.node.isEmpty())
					{
						array = this.getNextGrammarStates(this.lc, this.nextBaseID);
					}
					else
					{
						Word word = this.node.getWord();
						Pronunciation[] array2 = word.getPronunciations();
						array2 = this.filter(array2, this.nextBaseID);
						SearchStateArc[] array3;
						if (AFlatLinguist.access_400(this.this_0))
						{
							array3 = new SearchStateArc[array2.Length + 1];
						}
						else
						{
							array3 = new SearchStateArc[array2.Length];
						}
						for (int i = 0; i < array2.Length; i++)
						{
							array3[i] = new AFlatLinguist.PronunciationState(this.this_0, this, array2[i]);
						}
						SearchStateArc[] toGrammarSearchState = new SearchStateArc[]
						{
							this
						};
						if (AFlatLinguist.access_400(this.this_0))
						{
							PhoneLoop phoneLoop = new PhoneLoop(AFlatLinguist.access_500(this.this_0), AFlatLinguist.access_600(this.this_0), AFlatLinguist.access_700(this.this_0), toGrammarSearchState);
							array3[array2.Length] = phoneLoop.getPhoneLoop();
						}
						array = array3;
					}
					this.cacheSuccessors(array);
				}
				return array;
			}

			public override string getSignature()
			{
				return new StringBuilder().append("GS ").append(this.node).append("-lc-").append(AFlatLinguist.access_800(this.this_0).getUnit(this.lc)).append("-rc-").append(AFlatLinguist.access_800(this.this_0).getUnit(this.nextBaseID)).toString();
			}

			public override int getOrder()
			{
				return 1;
			}

			internal virtual int getLC()
			{
				return this.lc;
			}

			internal virtual int getNextBaseID()
			{
				return this.nextBaseID;
			}

			internal virtual int[] getNextUnits()
			{
				return (int[])AFlatLinguist.access_1000(this.this_0).get(this.node);
			}

			public override string toString()
			{
				return new StringBuilder().append(this.node).append("[").append(AFlatLinguist.access_800(this.this_0).getUnit(this.lc)).append(',').append(AFlatLinguist.access_800(this.this_0).getUnit(this.nextBaseID)).append(']').toString();
			}

			public override float getInsertionProbability()
			{
				return base.getInsertionProbability();
			}

			public override float getProbability()
			{
				return base.getProbability();
			}

			public override SearchState getState()
			{
				return base.getState();
			}

			public override WordSequence getWordHistory()
			{
				return base.getWordHistory();
			}

			public override string toPrettyString()
			{
				return base.toPrettyString();
			}

			public override object getLexState()
			{
				return base.getLexState();
			}

			public override bool isEmitting()
			{
				return base.isEmitting();
			}

			private GrammarNode node;

			private int lc;

			private int nextBaseID;

			private float languageProbability;
		}
		
		internal sealed class HMMStateSearchState : AFlatLinguist.FlatSearchState, HMMSearchState, SearchState, ScoreProvider
		{			
			internal HMMStateSearchState(AFlatLinguist aflatLinguist, AFlatLinguist.FullHMMSearchState fullHMMSearchState, HMMState hmmstate) : this(aflatLinguist, fullHMMSearchState, hmmstate, 0f)
			{
			}
			
			internal HMMStateSearchState(AFlatLinguist aflatLinguist, AFlatLinguist.FullHMMSearchState fullHMMSearchState, HMMState hmmstate, float num) : base(aflatLinguist)
			{
				this.probability = num;
				this.fullHMMSearchState = fullHMMSearchState;
				this.hmmState = hmmstate;
			}			
			
			public override bool isEmitting()
			{
				return this.hmmState.isEmitting();
			}

			public override float getInsertionProbability()
			{
				return this.probability;
			}

			public override int hashCode()
			{
				return 7 * this.fullHMMSearchState.hashCode() + Object.instancehelper_hashCode(this.hmmState);
			}
			
			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is AFlatLinguist.HMMStateSearchState)
				{
					AFlatLinguist.HMMStateSearchState hmmstateSearchState = (AFlatLinguist.HMMStateSearchState)obj;
					return hmmstateSearchState.fullHMMSearchState.equals(this.fullHMMSearchState) && Object.instancehelper_equals(hmmstateSearchState.hmmState, this.hmmState);
				}
				return false;
			}
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedSuccessors();
				if (array == null)
				{
					if (this.hmmState.isExitState())
					{
						array = this.fullHMMSearchState.getNextArcs();
					}
					else
					{
						HMMStateArc[] successors = this.hmmState.getSuccessors();
						array = new SearchStateArc[successors.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = new AFlatLinguist.HMMStateSearchState(this.this_0, this.fullHMMSearchState, successors[i].getHMMState(), successors[i].getLogProbability());
						}
					}
					this.cacheSuccessors(array);
				}
				return array;
			}
			
			public override int getOrder()
			{
				return (!this.isEmitting()) ? 0 : 4;
			}
						
			public override string getSignature()
			{
				return new StringBuilder().append("HSSS ").append(this.fullHMMSearchState.getSignature()).append('-').append(this.hmmState).toString();
			}

			public HMMState getHMMState()
			{
				return this.hmmState;
			}			
			
			public float getScore(Data d)
			{
				return this.hmmState.getScore(d);
			}
			
			public float[] getComponentScore(Data d)
			{
				return this.hmmState.calculateComponentScore(d);
			}
			
			private AFlatLinguist.FullHMMSearchState fullHMMSearchState;
			
			private HMMState hmmState;

			private float probability;
			
		}

		internal sealed class InitialState : AFlatLinguist.FlatSearchState
		{			
			internal InitialState(AFlatLinguist aflatLinguist) : base(aflatLinguist)
			{
				this.nextArcs = new ArrayList();
			}
			
			public void addArc(SearchStateArc searchStateArc)
			{
				this.nextArcs.add(searchStateArc);
			}

			public override string getSignature()
			{
				return "initialState";
			}
			
			public override SearchStateArc[] getSuccessors()
			{
				return (SearchStateArc[])this.nextArcs.toArray(new SearchStateArc[this.nextArcs.size()]);
			}

			public override int getOrder()
			{
				return 1;
			}

			public override string toString()
			{
				return this.getSignature();
			}
			
			private List nextArcs;
		}

		internal sealed class PronunciationState : AFlatLinguist.FlatSearchState, WordSearchState, SearchState
		{
			public Pronunciation getPronunciation()
			{
				return this.pronunciation;
			}

			internal AFlatLinguist.GrammarState getGrammarState()
			{
				return this.gs;
			}
			
			internal SearchStateArc[] getSuccessors(int num, int num2)
			{
				SearchStateArc[] array;
				if (num2 == this.pronunciation.getUnits().Length - 1)
				{
					if (this.isContextIndependentUnit(this.pronunciation.getUnits()[num2]))
					{
						array = new SearchStateArc[]
						{
							new AFlatLinguist.FullHMMSearchState(this.this_0, this, num2, num, 0)
						};
					}
					else
					{
						int[] nextUnits = this.gs.getNextUnits();
						array = new SearchStateArc[nextUnits.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = new AFlatLinguist.FullHMMSearchState(this.this_0, this, num2, num, nextUnits[i]);
						}
					}
				}
				else
				{
					array = new SearchStateArc[]
					{
						new AFlatLinguist.FullHMMSearchState(this.this_0, this, num2, num)
					};
				}
				return array;
			}
			
			internal PronunciationState(AFlatLinguist aflatLinguist, AFlatLinguist.GrammarState grammarState, Pronunciation pronunciation) : base(aflatLinguist)
			{
				this.gs = grammarState;
				this.pronunciation = pronunciation;
			}
			
			private bool isContextIndependentUnit(Unit unit)
			{
				return unit.isFiller();
			}

			public override float getInsertionProbability()
			{
				if (this.pronunciation.getWord().isFiller())
				{
					return 0f;
				}
				return AFlatLinguist.access_1100(this.this_0);
			}

			public override int hashCode()
			{
				return 13 * this.gs.hashCode() + Object.instancehelper_hashCode(this.pronunciation);
			}
			
			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is AFlatLinguist.PronunciationState)
				{
					AFlatLinguist.PronunciationState pronunciationState = (AFlatLinguist.PronunciationState)obj;
					return pronunciationState.gs.equals(this.gs) && Object.instancehelper_equals(pronunciationState.pronunciation, this.pronunciation);
				}
				return false;
			}
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedSuccessors();
				if (array == null)
				{
					array = this.getSuccessors(this.gs.getLC(), 0);
					this.cacheSuccessors(array);
				}
				return array;
			}

			public override string getSignature()
			{
				return new StringBuilder().append("PS ").append(this.gs.getSignature()).append('-').append(this.pronunciation).toString();
			}

			public override string toString()
			{
				return this.pronunciation.getWord().getSpelling();
			}

			public override int getOrder()
			{
				return 2;
			}

			public bool isWordStart()
			{
				return true;
			}

			private AFlatLinguist.GrammarState gs;

			private Pronunciation pronunciation;

		}
	}
}

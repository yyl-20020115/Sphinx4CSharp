using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.grammar;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.dflat
{
	public class DynamicFlatLinguist : java.lang.Object, Linguist, Configurable
	{		
		internal static Grammar access_900(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.grammar;
		}
		
		internal static bool access_1000(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.addOutOfGrammarBranch;
		}		
		
		internal static AcousticModel access_1100(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.phoneLoopAcousticModel;
		}
		
		internal static float access_1200(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.logOutOfGrammarBranchProbability;
		}
		
		internal static float access_1300(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.logPhoneInsertionProbability;
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
			this.initialGrammarState = this.grammar.getInitialNode();
			Iterator iterator = this.grammar.getGrammarNodes().iterator();
			while (iterator.hasNext())
			{
				GrammarNode grammarNode = (GrammarNode)iterator.next();
				this.initUnitMaps(grammarNode);
			}
			this.searchGraph = new DynamicFlatLinguist.DynamicFlatSearchGraph(this);
		}
		
		private bool grammarHasChanged()
		{
			return this.initialGrammarState == null || this.initialGrammarState != this.grammar.getInitialNode();
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
		
		public DynamicFlatLinguist(AcousticModel acousticModel, Grammar grammar, UnitManager unitManager, double wordInsertionProbability, double silenceInsertionProbability, double unitInsertionProbability, double fillerInsertionProbability, float languageWeight, bool addOutOfGrammarBranch, double outOfGrammarBranchProbability, double phoneInsertionProbability, AcousticModel phoneLoopAcousticModel)
		{
			this.EMPTY_ARCS = new SearchStateArc[0];
			this.successorCache = new HashMap();
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
			this.acousticModel = acousticModel;
			this.logMath = LogMath.getLogMath();
			this.grammar = grammar;
			this.unitManager = unitManager;
			this.logWordInsertionProbability = this.logMath.linearToLog(wordInsertionProbability);
			this.logSilenceInsertionProbability = this.logMath.linearToLog(silenceInsertionProbability);
			this.logUnitInsertionProbability = this.logMath.linearToLog(unitInsertionProbability);
			this.logFillerInsertionProbability = this.logMath.linearToLog(fillerInsertionProbability);
			this.languageWeight = languageWeight;
			this.addOutOfGrammarBranch = addOutOfGrammarBranch;
			this.logOutOfGrammarBranchProbability = this.logMath.linearToLog(outOfGrammarBranchProbability);
			this.logPhoneInsertionProbability = this.logMath.linearToLog((double)this.logPhoneInsertionProbability);
			if (addOutOfGrammarBranch)
			{
				this.phoneLoopAcousticModel = phoneLoopAcousticModel;
			}
		}
		
		public DynamicFlatLinguist()
		{
			this.EMPTY_ARCS = new SearchStateArc[0];
			this.successorCache = new HashMap();
		}
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.logMath = LogMath.getLogMath();
			this.acousticModel = (AcousticModel)ps.getComponent("acousticModel");
			this.grammar = (Grammar)ps.getComponent("grammar");
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
			this.logWordInsertionProbability = this.logMath.linearToLog(ps.getDouble("wordInsertionProbability"));
			this.logSilenceInsertionProbability = this.logMath.linearToLog(ps.getDouble("silenceInsertionProbability"));
			this.logUnitInsertionProbability = this.logMath.linearToLog(ps.getDouble("unitInsertionProbability"));
			this.logFillerInsertionProbability = this.logMath.linearToLog(ps.getDouble("fillerInsertionProbability"));
			this.languageWeight = ps.getFloat("languageWeight");
			this.addOutOfGrammarBranch = ps.getBoolean("addOutOfGrammarBranch").booleanValue();
			this.logOutOfGrammarBranchProbability = this.logMath.linearToLog(ps.getDouble("outOfGrammarProbability"));
			this.logPhoneInsertionProbability = this.logMath.linearToLog(ps.getDouble("phoneInsertionProbability"));
			if (this.addOutOfGrammarBranch)
			{
				this.phoneLoopAcousticModel = (AcousticModel)ps.getComponent("phoneLoopAcousticModel");
			}
		}

		public virtual SearchGraph getSearchGraph()
		{
			return this.searchGraph;
		}
		
		public virtual void allocate()
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
		
		public virtual void deallocate()
		{
			if (this.acousticModel != null)
			{
				this.acousticModel.deallocate();
			}
			this.grammar.deallocate();
		}
		
		public virtual void startRecognition()
		{
			if (this.grammarHasChanged())
			{
				this.compileGrammar();
			}
		}

		public virtual void stopRecognition()
		{
		}

		public virtual float getLogSilenceInsertionProbability()
		{
			return this.logSilenceInsertionProbability;
		}		
		
		internal static float access_000(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.languageWeight;
		}
				
		internal static SearchStateArc[] access_100(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.EMPTY_ARCS;
		}		
		
		internal static HMMPool access_200(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.hmmPool;
		}		
		
		internal static Map access_300(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.nodeToUnitSetMap;
		}
		
		internal static Map access_400(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.nodeToNextUnitArrayMap;
		}
		
		internal static float access_500(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.logWordInsertionProbability;
		}

		internal static float access_600(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.logSilenceInsertionProbability;
		}

		internal static float access_700(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.logFillerInsertionProbability;
		}

		internal static float access_800(DynamicFlatLinguist dynamicFlatLinguist)
		{
			return dynamicFlatLinguist.logUnitInsertionProbability;
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

		private LogMath logMath;

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

		internal SearchStateArc outOfGrammarGraph;

		private GrammarNode initialGrammarState;
		
		private Map nodeToNextUnitArrayMap;

		private Map nodeToUnitSetMap;
		
		private SearchStateArc[] EMPTY_ARCS;	
		
		internal Map successorCache;
				
		internal sealed class DynamicFlatSearchGraph : java.lang.Object, SearchGraph
		{
			internal DynamicFlatSearchGraph(DynamicFlatLinguist dynamicFlatLinguist)
			{
				this.this_0 = dynamicFlatLinguist;
			}
			
			public SearchState getInitialState()
			{
				DynamicFlatLinguist.InitialState initialState = new DynamicFlatLinguist.InitialState(this.this_0);
				initialState.addArc(new DynamicFlatLinguist.GrammarState(this.this_0, DynamicFlatLinguist.access_900(this.this_0).getInitialNode()));
				if (DynamicFlatLinguist.access_1000(this.this_0))
				{
					OutOfGrammarGraph outOfGrammarGraph = new OutOfGrammarGraph(DynamicFlatLinguist.access_1100(this.this_0), DynamicFlatLinguist.access_1200(this.this_0), DynamicFlatLinguist.access_1300(this.this_0));
					initialState.addArc(outOfGrammarGraph.getOutOfGrammarGraph());
				}
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
			
			internal DynamicFlatLinguist this_0;
		}

		internal abstract class FlatSearchState : java.lang.Object, SearchState, SearchStateArc
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
			
			internal FlatSearchState(DynamicFlatLinguist dynamicFlatLinguist)
			{
				this.this_0 = dynamicFlatLinguist;
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
			
			internal DynamicFlatLinguist this_0;
		}

		internal sealed class FullHMMSearchState : DynamicFlatLinguist.FlatSearchState, UnitSearchState, SearchState
		{		
			internal FullHMMSearchState(DynamicFlatLinguist dynamicFlatLinguist, DynamicFlatLinguist.PronunciationState pronunciationState, int num, int leftID, int rightID) : base(dynamicFlatLinguist)
			{
				this.pState = pronunciationState;
				this.index = num;
				this.lc = leftID;
				this.rc = rightID;
				int baseID = pronunciationState.getPronunciation().getUnits()[num].getBaseID();
				int unitID = DynamicFlatLinguist.access_200(dynamicFlatLinguist).buildID(baseID, leftID, rightID);
				this.hmm = DynamicFlatLinguist.access_200(dynamicFlatLinguist).getHMM(unitID, this.getPosition());
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
			
			internal FullHMMSearchState(DynamicFlatLinguist dynamicFlatLinguist, DynamicFlatLinguist.PronunciationState pronunciationState, int num, int num2) : this(dynamicFlatLinguist, pronunciationState, num, num2, pronunciationState.getPronunciation().getUnits()[num + 1].getBaseID())
			{
			}
			
			public override float getInsertionProbability()
			{
				Unit baseUnit = this.hmm.getBaseUnit();
				if (baseUnit.isSilence())
				{
					return DynamicFlatLinguist.access_600(this.this_0);
				}
				if (baseUnit.isFiller())
				{
					return DynamicFlatLinguist.access_700(this.this_0);
				}
				return DynamicFlatLinguist.access_800(this.this_0);
			}
			
			public override string toString()
			{
				return this.hmm.getUnit().toString();
			}

			public override int hashCode()
			{
				return java.lang.Object.instancehelper_hashCode(this.pState.getGrammarState().getGrammarNode()) * 29 + java.lang.Object.instancehelper_hashCode(this.pState.getPronunciation()) * 19 + this.index * 7 + 43 * this.lc + this.rc;
			}
			
			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is DynamicFlatLinguist.FullHMMSearchState)
				{
					DynamicFlatLinguist.FullHMMSearchState fullHMMSearchState = (DynamicFlatLinguist.FullHMMSearchState)obj;
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
						new DynamicFlatLinguist.HMMStateSearchState(this.this_0, this, this.hmm.getInitialState())
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
					DynamicFlatLinguist.GrammarState grammarState = this.pState.getGrammarState();
					result = grammarState.getNextGrammarStates(baseID, this.getRC());
				}
				return result;
			}
			
			private DynamicFlatLinguist.PronunciationState pState;
			
			private int index;
			
			private int lc;
			
			private int rc;
			
			private HMM hmm;
			
			private bool _isLastUnitOfWord;
		}

		internal sealed class GrammarState : DynamicFlatLinguist.FlatSearchState
		{			
			internal GrammarState(DynamicFlatLinguist dynamicFlatLinguist, GrammarNode grammarNode) : this(dynamicFlatLinguist, grammarNode, 0f, UnitManager.__SILENCE.getBaseID())
			{
			}

			internal GrammarNode getGrammarNode()
			{
				return this.node;
			}

			internal SearchStateArc[] getNextGrammarStates(int num, int num2)
			{
				GrammarArc[] array = this.node.getSuccessors();
				array = this.filter(array, num2);
				SearchStateArc[] array2 = new SearchStateArc[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					GrammarArc grammarArc = array[i];
					array2[i] = new DynamicFlatLinguist.GrammarState(this.this_0, grammarArc.getGrammarNode(), grammarArc.getProbability(), num, num2);
				}
				return array2;
			}
			
			internal GrammarState(DynamicFlatLinguist dynamicFlatLinguist, GrammarNode grammarNode, float num, int num2) : this(dynamicFlatLinguist, grammarNode, num, num2, 0)
			{
			}
			
			internal GrammarState(DynamicFlatLinguist dynamicFlatLinguist, GrammarNode grammarNode, float num, int num2, int num3) : base(dynamicFlatLinguist)
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
			
			internal GrammarArc[] filter(GrammarArc[] array, int num)
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
				Set set = (Set)DynamicFlatLinguist.access_300(this.this_0).get(grammarNode);
				return set.contains(DynamicFlatLinguist.access_200(this.this_0).getUnit(unitID));
			}
			
			public override float getLanguageProbability()
			{
				return this.languageProbability * DynamicFlatLinguist.access_000(this.this_0);
			}
			
			public override int hashCode()
			{
				return java.lang.Object.instancehelper_hashCode(this.node) * 17 + this.lc * 7 + this.nextBaseID;
			}

			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is DynamicFlatLinguist.GrammarState)
				{
					DynamicFlatLinguist.GrammarState grammarState = (DynamicFlatLinguist.GrammarState)obj;
					return grammarState.node == this.node && this.lc == grammarState.lc && this.nextBaseID == grammarState.nextBaseID;
				}
				return false;
			}
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedSuccessors();
				if (array != null)
				{
					return array;
				}
				if (this.isFinal())
				{
					array = DynamicFlatLinguist.access_100(this.this_0);
				}
				else if (this.node.isEmpty())
				{
					array = this.getNextGrammarStates(this.lc, this.nextBaseID);
				}
				else
				{
					Word word = this.node.getWord();
					Pronunciation[] pronunciations = word.getPronunciations();
					SearchStateArc[] array2 = new SearchStateArc[pronunciations.Length];
					for (int i = 0; i < pronunciations.Length; i++)
					{
						array2[i] = new DynamicFlatLinguist.PronunciationState(this.this_0, this, pronunciations[i]);
					}
					array = array2;
				}
				this.cacheSuccessors(array);
				return array;
			}
			
			public override string getSignature()
			{
				return new StringBuilder().append("GS ").append(this.node).append("-lc-").append(DynamicFlatLinguist.access_200(this.this_0).getUnit(this.lc)).append("-rc-").append(DynamicFlatLinguist.access_200(this.this_0).getUnit(this.nextBaseID)).toString();
			}

			public override int getOrder()
			{
				return 1;
			}
			
			internal Pronunciation[] filter(Pronunciation[] array, int num)
			{
				if (num == 0)
				{
					return array;
				}
				ArrayList arrayList = new ArrayList(array.Length);
				int num2 = array.Length;
				for (int i = 0; i < num2; i++)
				{
					Pronunciation pronunciation = array[i];
					if (pronunciation.getUnits()[0].getBaseID() == num)
					{
						arrayList.add(pronunciation);
					}
				}
				return (Pronunciation[])arrayList.toArray(new Pronunciation[arrayList.size()]);
			}

			internal int getLC()
			{
				return this.lc;
			}

			internal int getNextBaseID()
			{
				return this.nextBaseID;
			}			
			
			internal int[] getNextUnits()
			{
				return (int[])DynamicFlatLinguist.access_400(this.this_0).get(this.node);
			}
						
			public override string toString()
			{
				return new StringBuilder().append(this.node).append("[").append(DynamicFlatLinguist.access_200(this.this_0).getUnit(this.lc)).append(',').append(DynamicFlatLinguist.access_200(this.this_0).getUnit(this.nextBaseID)).append(']').toString();
			}
			
			private GrammarNode node;
			
			private int lc;
			
			private int nextBaseID;
			
			private float languageProbability;
		}
		
		internal sealed class HMMStateSearchState : DynamicFlatLinguist.FlatSearchState, HMMSearchState, SearchState, ScoreProvider
		{			
			internal HMMStateSearchState(DynamicFlatLinguist dynamicFlatLinguist, DynamicFlatLinguist.FullHMMSearchState fullHMMSearchState, HMMState hmmstate) : this(dynamicFlatLinguist, fullHMMSearchState, hmmstate, 0f)
			{
			}
			
			internal HMMStateSearchState(DynamicFlatLinguist dynamicFlatLinguist, DynamicFlatLinguist.FullHMMSearchState fullHMMSearchState, HMMState hmmstate, float num) : base(dynamicFlatLinguist)
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
				return 7 * this.fullHMMSearchState.hashCode() + java.lang.Object.instancehelper_hashCode(this.hmmState);
			}
			
			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is DynamicFlatLinguist.HMMStateSearchState)
				{
					DynamicFlatLinguist.HMMStateSearchState hmmstateSearchState = (DynamicFlatLinguist.HMMStateSearchState)obj;
					return hmmstateSearchState.fullHMMSearchState.equals(this.fullHMMSearchState) && java.lang.Object.instancehelper_equals(hmmstateSearchState.hmmState, this.hmmState);
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
							array[i] = new DynamicFlatLinguist.HMMStateSearchState(this.this_0, this.fullHMMSearchState, successors[i].getHMMState(), successors[i].getLogProbability());
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
			
			private DynamicFlatLinguist.FullHMMSearchState fullHMMSearchState;

			private HMMState hmmState;

			private float probability;
		}
		
		internal sealed class InitialState : DynamicFlatLinguist.FlatSearchState
		{			
			internal InitialState(DynamicFlatLinguist dynamicFlatLinguist) : base(dynamicFlatLinguist)
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
		
		internal sealed class PronunciationState : DynamicFlatLinguist.FlatSearchState, WordSearchState, SearchState
		{
			public Pronunciation getPronunciation()
			{
				return this.pronunciation;
			}

			internal DynamicFlatLinguist.GrammarState getGrammarState()
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
							new DynamicFlatLinguist.FullHMMSearchState(this.this_0, this, num2, num, 0)
						};
					}
					else
					{
						int[] nextUnits = this.gs.getNextUnits();
						array = new SearchStateArc[nextUnits.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = new DynamicFlatLinguist.FullHMMSearchState(this.this_0, this, num2, num, nextUnits[i]);
						}
					}
				}
				else
				{
					array = new SearchStateArc[]
					{
						new DynamicFlatLinguist.FullHMMSearchState(this.this_0, this, num2, num)
					};
				}
				return array;
			}
			
			internal PronunciationState(DynamicFlatLinguist dynamicFlatLinguist, DynamicFlatLinguist.GrammarState grammarState, Pronunciation pronunciation) : base(dynamicFlatLinguist)
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
				return DynamicFlatLinguist.access_500(this.this_0);
			}
		
			public override int hashCode()
			{
				return 13 * this.gs.hashCode() + java.lang.Object.instancehelper_hashCode(this.pronunciation);
			}
			
			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is DynamicFlatLinguist.PronunciationState)
				{
					DynamicFlatLinguist.PronunciationState pronunciationState = (DynamicFlatLinguist.PronunciationState)obj;
					return pronunciationState.gs.equals(this.gs) && java.lang.Object.instancehelper_equals(pronunciationState.pronunciation, this.pronunciation);
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

			private DynamicFlatLinguist.GrammarState gs;

			private Pronunciation pronunciation;
		}
	}
}

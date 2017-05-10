using System;

using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.grammar;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.aflat
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.Linguist",
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public class AFlatLinguist : java.lang.Object, Linguist, Configurable
	{
		
		
		internal static Grammar access_1500(AFlatLinguist aflatLinguist)
		{
			return aflatLinguist.grammar;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			151,
			107,
			104,
			139
		})]
		
		protected internal virtual void allocateAcousticModel()
		{
			this.acousticModel.allocate();
			if (this.addOutOfGrammarBranch)
			{
				this.phoneLoopAcousticModel.allocate();
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			181,
			144,
			140,
			123,
			103,
			98,
			123,
			63,
			1,
			133,
			108
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			209,
			113,
			102,
			134,
			103,
			117,
			105,
			10,
			200,
			109,
			99,
			127,
			0,
			114,
			98,
			239,
			69,
			110,
			102,
			102,
			105,
			142
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			160,
			250,
			105,
			161,
			104,
			104,
			113,
			104,
			103,
			103,
			115,
			48,
			168,
			98,
			104,
			118,
			105,
			10,
			230,
			69
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			105,
			131,
			232,
			38,
			107,
			232,
			79,
			236,
			160,
			246,
			235,
			159,
			22,
			118,
			103,
			103,
			135,
			102,
			101,
			106,
			101,
			106,
			101,
			106,
			101,
			106,
			105,
			103,
			101,
			138,
			105,
			106,
			99,
			136
		})]
		
		public AFlatLinguist(AcousticModel acousticModel, Grammar grammar, UnitManager unitManager, double wordInsertionProbability, double silenceInsertionProbability, double unitInsertionProbability, double fillerInsertionProbability, float languageWeight, bool addOutOfGrammarBranch, double outOfGrammarBranchProbability, double phoneInsertionProbability, AcousticModel phoneLoopAcousticModel)
		{
			this.runtime = Runtime.getRuntime();
			this.counterForMemoryLogging = 0L;
			this.EMPTY_ARCS = new SearchStateArc[0];
			this.successorCache = new HashMap();
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
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

		[LineNumberTable(new byte[]
		{
			160,
			64,
			232,
			10,
			107,
			232,
			79,
			236,
			160,
			246,
			235,
			159,
			49
		})]
		
		public AFlatLinguist()
		{
			this.runtime = Runtime.getRuntime();
			this.counterForMemoryLogging = 0L;
			this.EMPTY_ARCS = new SearchStateArc[0];
			this.successorCache = new HashMap();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			76,
			108,
			150,
			118,
			182,
			102,
			104,
			37,
			138,
			104,
			37,
			138,
			104,
			37,
			138,
			104,
			37,
			138,
			113,
			118,
			104,
			37,
			170,
			104,
			37,
			138,
			104,
			103,
			143
		})]
		
		public virtual void newProperties(PropertySheet ps)
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

		[LineNumberTable(new byte[]
		{
			160,
			111,
			112,
			159,
			33,
			159,
			33
		})]
		
		public virtual SearchGraph getSearchGraph()
		{
			this.logger.info("Generated Search Graph");
			this.logger.info(new StringBuilder().append("Total Memory= ").append(this.runtime.totalMemory() / (long)((ulong)1048576)).append(" MB").toString());
			this.logger.info(new StringBuilder().append("Free Memory = ").append(this.runtime.freeMemory() / (long)((ulong)1048576)).append(" MB").toString());
			return this.searchGraph;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			128,
			118
		})]
		
		protected internal virtual void setupAcousticModel(PropertySheet ps)
		{
			this.acousticModel = (AcousticModel)ps.getComponent("acousticModel");
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			132,
			112,
			102,
			107,
			127,
			3,
			107,
			107,
			108,
			102,
			102,
			103,
			112
		})]
		
		public virtual void allocate()
		{
			this.logger.info("Allocating DFLAT");
			this.allocateAcousticModel();
			this.grammar.allocate();
			HMMPool.__<clinit>();
			this.hmmPool = new HMMPool(this.acousticModel, this.logger, this.unitManager);
			this.nodeToNextUnitArrayMap = new HashMap();
			this.nodeToUnitSetMap = new HashMap();
			Timer timer = TimerPool.getTimer(this, "compileGrammar");
			timer.start();
			this.compileGrammar();
			timer.stop();
			this.logger.info("Done allocating  DFLAT");
		}

		[LineNumberTable(new byte[]
		{
			160,
			163,
			104,
			139,
			107
		})]
		
		public virtual void deallocate()
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

		public virtual void startRecognition()
		{
		}

		public virtual void stopRecognition()
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

		internal SearchStateArc outOfGrammarGraph;

		public Runtime runtime;

		private long counterForMemoryLogging;

		
		private Map nodeToNextUnitArrayMap;

		
		private Map nodeToUnitSetMap;

		
		private SearchStateArc[] EMPTY_ARCS;

		
		
		internal Map successorCache;

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.SearchGraph"
		})]
		[SourceFile("AFlatLinguist.java")]
		
		internal sealed class DynamicFlatSearchGraph : java.lang.Object, SearchGraph
		{
			
			
			internal DynamicFlatSearchGraph(AFlatLinguist aflatLinguist)
			{
			}

			[LineNumberTable(new byte[]
			{
				165,
				89,
				108,
				127,
				2
			})]
			
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

			
			internal AFlatLinguist this_0 = aflatLinguist;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.SearchState",
			"edu.cmu.sphinx.linguist.SearchStateArc"
		})]
		[SourceFile("AFlatLinguist.java")]
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

			
			
			internal FlatSearchState(AFlatLinguist aflatLinguist)
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

			
			
			internal virtual SearchStateArc[] getCachedSuccessors()
			{
				return (SearchStateArc[])this.this_0.successorCache.get(this);
			}

			[LineNumberTable(new byte[]
			{
				161,
				154,
				115
			})]
			
			internal virtual void cacheSuccessors(SearchStateArc[] array)
			{
				this.this_0.successorCache.put(this, array);
			}

			internal const int ANY = 0;

			
			internal AFlatLinguist this_0 = aflatLinguist;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.UnitSearchState"
		})]
		[SourceFile("AFlatLinguist.java")]
		
		internal sealed class FullHMMSearchState : AFlatLinguist.FlatSearchState, UnitSearchState, SearchState
		{
			[LineNumberTable(new byte[]
			{
				163,
				239,
				112,
				103,
				103,
				104,
				104,
				115,
				113,
				120,
				119
			})]
			
			internal FullHMMSearchState(AFlatLinguist aflatLinguist, AFlatLinguist.PronunciationState pronunciationState, int num, int leftID, int rightID) : base(aflatLinguist)
			{
				this.pState = pronunciationState;
				this.index = num;
				this.lc = leftID;
				this.rc = rightID;
				int baseID = pronunciationState.getPronunciation().getUnits()[num].getBaseID();
				int unitID = AFlatLinguist.access_800(aflatLinguist).buildID(baseID, leftID, rightID);
				this.hmm = AFlatLinguist.access_800(aflatLinguist).getHMM(unitID, this.getPosition());
				this.isLastUnitOfWord = (num == pronunciationState.getPronunciation().getUnits().Length - 1);
			}

			[LineNumberTable(new byte[]
			{
				164,
				104,
				114,
				100,
				102,
				104,
				102,
				107,
				134
			})]
			
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
				return this.isLastUnitOfWord;
			}

			internal int getRC()
			{
				return this.rc;
			}

			[LineNumberTable(new byte[]
			{
				163,
				223,
				117,
				37,
				133
			})]
			
			internal FullHMMSearchState(AFlatLinguist aflatLinguist, AFlatLinguist.PronunciationState pronunciationState, int num, int num2) : this(aflatLinguist, pronunciationState, num, num2, pronunciationState.getPronunciation().getUnits()[num + 1].getBaseID())
			{
			}

			[LineNumberTable(new byte[]
			{
				164,
				1,
				140,
				104,
				108,
				104,
				140
			})]
			
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

			[LineNumberTable(new byte[]
			{
				164,
				29,
				126,
				63,
				9
			})]
			
			public override int hashCode()
			{
				return java.lang.Object.instancehelper_hashCode(this.pState.getGrammarState().getGrammarNode()) * 29 + java.lang.Object.instancehelper_hashCode(this.pState.getPronunciation()) * 19 + this.index * 7 + 43 * this.lc + this.rc;
			}

			[LineNumberTable(new byte[]
			{
				164,
				43,
				100,
				98,
				107,
				231,
				71,
				118,
				114,
				107,
				255,
				22,
				61,
				225,
				71
			})]
			
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

			[LineNumberTable(new byte[]
			{
				164,
				80,
				103,
				99,
				103,
				122,
				135
			})]
			
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

			[LineNumberTable(new byte[]
			{
				164,
				143,
				127,
				11,
				63,
				31
			})]
			
			public override string getSignature()
			{
				return new StringBuilder().append("HSS ").append(this.pState.getGrammarState().getGrammarNode()).append(this.pState.getPronunciation()).append(this.index).append('-').append(this.rc).append('-').append(this.lc).toString();
			}

			[LineNumberTable(new byte[]
			{
				164,
				170,
				145,
				104,
				215,
				108,
				142
			})]
			
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

			
			private bool isLastUnitOfWord;

			
			internal new AFlatLinguist this_0 = aflatLinguist;
		}

		
		[SourceFile("AFlatLinguist.java")]
		public class GrammarState : AFlatLinguist.FlatSearchState
		{
			[LineNumberTable(new byte[]
			{
				161,
				177,
				119
			})]
			
			internal GrammarState(AFlatLinguist aflatLinguist, GrammarNode grammarNode) : this(aflatLinguist, grammarNode, 0f, UnitManager.__SILENCE.getBaseID())
			{
			}

			internal virtual GrammarNode getGrammarNode()
			{
				return this.node;
			}

			[LineNumberTable(new byte[]
			{
				162,
				69,
				108,
				105,
				136,
				103,
				100,
				111,
				237,
				61,
				230,
				69
			})]
			
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

			[LineNumberTable(new byte[]
			{
				161,
				192,
				109
			})]
			
			internal GrammarState(AFlatLinguist aflatLinguist, GrammarNode grammarNode, float num, int num2) : this(aflatLinguist, grammarNode, num, num2, 0)
			{
			}

			[LineNumberTable(new byte[]
			{
				161,
				209,
				112,
				104,
				104,
				103,
				104
			})]
			
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

			[LineNumberTable(new byte[]
			{
				162,
				113,
				102,
				102,
				112,
				105,
				107,
				233,
				61,
				230,
				70,
				152
			})]
			
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

			[LineNumberTable(new byte[]
			{
				162,
				135,
				119
			})]
			
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
				return java.lang.Object.instancehelper_hashCode(this.node) * 17 + this.lc * 7 + this.nextBaseID;
			}

			[LineNumberTable(new byte[]
			{
				161,
				246,
				100,
				98,
				104,
				103,
				191,
				16
			})]
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

			[LineNumberTable(new byte[]
			{
				162,
				18,
				108,
				127,
				2,
				159,
				44,
				31,
				1,
				197,
				103,
				102,
				104,
				113,
				109,
				152,
				108,
				103,
				142,
				109,
				140,
				136,
				105,
				52,
				200,
				104,
				101,
				109,
				118,
				107,
				110,
				139,
				130,
				135
			})]
			
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
							PhoneLoop.__<clinit>();
							PhoneLoop phoneLoop = new PhoneLoop(AFlatLinguist.access_500(this.this_0), AFlatLinguist.access_600(this.this_0), AFlatLinguist.access_700(this.this_0), toGrammarSearchState);
							array3[array2.Length] = phoneLoop.getPhoneLoop();
						}
						array = array3;
					}
					this.cacheSuccessors(array);
				}
				return array;
			}

			[LineNumberTable(new byte[]
			{
				162,
				89,
				127,
				48,
				58
			})]
			
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

			[LineNumberTable(new byte[]
			{
				162,
				212,
				127,
				35,
				63,
				2
			})]
			
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

			
			internal new AFlatLinguist this_0 = aflatLinguist;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.HMMSearchState",
			"edu.cmu.sphinx.decoder.scorer.ScoreProvider"
		})]
		[SourceFile("AFlatLinguist.java")]
		
		internal sealed class HMMStateSearchState : AFlatLinguist.FlatSearchState, HMMSearchState, SearchState, ScoreProvider
		{
			[LineNumberTable(new byte[]
			{
				164,
				201,
				110
			})]
			
			internal HMMStateSearchState(AFlatLinguist aflatLinguist, AFlatLinguist.FullHMMSearchState fullHMMSearchState, HMMState hmmstate) : this(aflatLinguist, fullHMMSearchState, hmmstate, 0f)
			{
			}

			[LineNumberTable(new byte[]
			{
				164,
				215,
				112,
				105,
				103,
				103
			})]
			
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
				return 7 * this.fullHMMSearchState.hashCode() + java.lang.Object.instancehelper_hashCode(this.hmmState);
			}

			[LineNumberTable(new byte[]
			{
				164,
				250,
				100,
				98,
				104,
				103,
				127,
				0,
				43,
				161
			})]
			
			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is AFlatLinguist.HMMStateSearchState)
				{
					AFlatLinguist.HMMStateSearchState hmmstateSearchState = (AFlatLinguist.HMMStateSearchState)obj;
					return hmmstateSearchState.fullHMMSearchState.equals(this.fullHMMSearchState) && java.lang.Object.instancehelper_equals(hmmstateSearchState.hmmState, this.hmmState);
				}
				return false;
			}

			[LineNumberTable(new byte[]
			{
				165,
				23,
				103,
				102,
				109,
				142,
				108,
				104,
				103,
				113,
				104,
				235,
				61,
				230,
				70,
				135
			})]
			
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

			
			internal new AFlatLinguist this_0 = aflatLinguist;
		}

		
		[SourceFile("AFlatLinguist.java")]
		
		internal sealed class InitialState : AFlatLinguist.FlatSearchState
		{
			[LineNumberTable(new byte[]
			{
				162,
				227,
				144
			})]
			
			internal InitialState(AFlatLinguist aflatLinguist) : base(aflatLinguist)
			{
				this.nextArcs = new ArrayList();
			}

			[LineNumberTable(new byte[]
			{
				162,
				242,
				109
			})]
			
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

			
			internal new AFlatLinguist this_0 = aflatLinguist;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.WordSearchState"
		})]
		[SourceFile("AFlatLinguist.java")]
		
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

			[LineNumberTable(new byte[]
			{
				163,
				111,
				116,
				117,
				103,
				151,
				108,
				104,
				103,
				52,
				198,
				130,
				103,
				145
			})]
			
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

			[LineNumberTable(new byte[]
			{
				163,
				35,
				112,
				103,
				103
			})]
			
			internal PronunciationState(AFlatLinguist aflatLinguist, AFlatLinguist.GrammarState grammarState, Pronunciation pronunciation) : base(aflatLinguist)
			{
				this.gs = grammarState;
				this.pronunciation = pronunciation;
			}

			
			
			private bool isContextIndependentUnit(Unit unit)
			{
				return unit.isFiller();
			}

			[LineNumberTable(new byte[]
			{
				163,
				47,
				114,
				134
			})]
			
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
				return 13 * this.gs.hashCode() + java.lang.Object.instancehelper_hashCode(this.pronunciation);
			}

			[LineNumberTable(new byte[]
			{
				163,
				73,
				100,
				98,
				104,
				103,
				127,
				0,
				43,
				161
			})]
			
			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is AFlatLinguist.PronunciationState)
				{
					AFlatLinguist.PronunciationState pronunciationState = (AFlatLinguist.PronunciationState)obj;
					return pronunciationState.gs.equals(this.gs) && java.lang.Object.instancehelper_equals(pronunciationState.pronunciation, this.pronunciation);
				}
				return false;
			}

			[LineNumberTable(new byte[]
			{
				163,
				91,
				103,
				99,
				115,
				135
			})]
			
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

			
			internal new AFlatLinguist this_0 = aflatLinguist;
		}
	}
}

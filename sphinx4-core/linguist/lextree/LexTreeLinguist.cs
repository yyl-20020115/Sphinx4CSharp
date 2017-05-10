using System;

using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.ngram;
using edu.cmu.sphinx.linguist.util;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.lextree
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.Linguist"
	})]
	public class LexTreeLinguist : java.lang.Object, Linguist, Configurable
	{
		
		public static void __<clinit>()
		{
		}

		
		
		internal static float access_000(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.logOne;
		}

		
		
		internal static Word access$100(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.sentenceEndWord;
		}

		
		
		internal static int access$200(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.maxDepth;
		}

		
		
		internal static LanguageModel access$300(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.languageModel;
		}

		
		
		
		internal static float access$400(LexTreeLinguist lexTreeLinguist, WordSequence wordSequence)
		{
			return lexTreeLinguist.getSmearTermFromLanguageModel(wordSequence);
		}

		
		
		
		internal static float access$500(LexTreeLinguist lexTreeLinguist, UnitNode unitNode)
		{
			return lexTreeLinguist.calculateInsertionProbability(unitNode);
		}

		
		
		
		internal static float access$600(LexTreeLinguist lexTreeLinguist, Node node)
		{
			return lexTreeLinguist.getUnigramSmear(node);
		}

		
		
		internal static bool access$700(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.generateUnitStates;
		}

		
		
		internal static bool access$800(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.cacheEnabled;
		}

		
		
		internal static LRUCache access$900(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.arcCache;
		}

		
		
		internal static int access$1008(LexTreeLinguist lexTreeLinguist)
		{
			int num = lexTreeLinguist.cacheHits;
			int result = num;
			lexTreeLinguist.cacheHits = num + 1;
			return result;
		}

		
		
		internal static int access$1104(LexTreeLinguist lexTreeLinguist)
		{
			int num = lexTreeLinguist.cacheTrys + 1;
			int result = num;
			lexTreeLinguist.cacheTrys = num;
			return result;
		}

		
		
		internal static int access$1000(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.cacheHits;
		}

		
		
		internal static int access$1100(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.cacheTrys;
		}

		
		
		
		internal static HMMNode[] access$1200(LexTreeLinguist lexTreeLinguist, EndNode endNode)
		{
			return lexTreeLinguist.getHMMNodes(endNode);
		}

		
		
		private HMMNode[] getHMMNodes(EndNode endNode)
		{
			return this.__hmmTree.getHMMNodes(endNode);
		}

		[LineNumberTable(new byte[]
		{
			165,
			176,
			104,
			145,
			135
		})]
		
		private float getUnigramSmear(Node node)
		{
			float result;
			if (this.wantUnigramSmear)
			{
				result = node.getUnigramProbability() * this.unigramSmearWeight;
			}
			else
			{
				result = this.logOne;
			}
			return result;
		}

		[LineNumberTable(new byte[]
		{
			165,
			155,
			135,
			100,
			103,
			100,
			111,
			100,
			135
		})]
		
		private float calculateInsertionProbability(UnitNode unitNode)
		{
			int type = unitNode.getType();
			if (type == 1)
			{
				return this.logUnitInsertionProbability;
			}
			if (type == 2)
			{
				return this.logUnitInsertionProbability + this.logWordInsertionProbability;
			}
			if (type == 3)
			{
				return this.logSilenceInsertionProbability;
			}
			return this.logFillerInsertionProbability;
		}

		
		
		private float getSmearTermFromLanguageModel(WordSequence ws)
		{
			return this.languageModel.getSmear(ws);
		}

		[LineNumberTable(new byte[]
		{
			161,
			39,
			144,
			113,
			108,
			115,
			145,
			134,
			209,
			114
		})]
		
		private void compileGrammar()
		{
			TimerPool.getTimer(this, "Compile").start();
			this.sentenceEndWord = this.dictionary.getSentenceEndWord();
			this.sentenceStartWordArray = new Word[1];
			this.sentenceStartWordArray[0] = this.dictionary.getSentenceStartWord();
			this.maxDepth = this.languageModel.getMaxDepth();
			this.generateHmmTree();
			TimerPool.getTimer(this, "Compile").stop();
			this.searchGraph = new LexTreeLinguist.LexTreeSearchGraph(this, this.getInitialSearchState());
		}

		[LineNumberTable(new byte[]
		{
			161,
			57,
			127,
			3,
			191,
			15,
			107
		})]
		
		protected internal virtual void generateHmmTree()
		{
			HMMPool.__<clinit>();
			this.hmmPool = new HMMPool(this.acousticModel, this.logger, this.unitManager);
			HMMTree.__<clinit>();
			this.__hmmTree = new HMMTree(this.hmmPool, this.dictionary, this.languageModel, this.addFillerWords, this.languageWeight);
			this.hmmPool.dumpInfo();
		}

		[LineNumberTable(new byte[]
		{
			161,
			27,
			140,
			99,
			144
		})]
		
		private SearchState getInitialSearchState()
		{
			InitialWordNode initialNode = this.__hmmTree.getInitialNode();
			if (initialNode == null)
			{
				string text = "Language model has no entry for initial word <s>";
				
				throw new RuntimeException(text);
			}
			WordNode wordNode = initialNode;
			HMMNode parent = initialNode.getParent();
			WordSequence.__<clinit>();
			return new LexTreeLinguist.LexTreeWordState(this, wordNode, parent, new WordSequence(this.sentenceStartWordArray).trim(this.maxDepth - 1), 0f, this.logOne, this.logOne);
		}

		[LineNumberTable(new byte[]
		{
			159,
			77,
			169,
			232,
			32,
			103,
			235,
			97,
			150,
			103,
			107,
			103,
			103,
			136,
			103,
			116,
			116,
			116,
			116,
			105,
			103,
			103,
			105,
			136,
			107,
			104,
			141
		})]
		
		public LexTreeLinguist(AcousticModel acousticModel, UnitManager unitManager, LanguageModel languageModel, Dictionary dictionary, bool fullWordHistories, bool wantUnigramSmear, double wordInsertionProbability, double silenceInsertionProbability, double fillerInsertionProbability, double unitInsertionProbability, float languageWeight, bool addFillerWords, bool generateUnitStates, float unigramSmearWeight, int maxArcCacheSize)
		{
			this.wantUnigramSmear = true;
			this.unigramSmearWeight = 1f;
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
			this.acousticModel = acousticModel;
			this.logMath = LogMath.getLogMath();
			this.unitManager = unitManager;
			this.languageModel = languageModel;
			this.dictionary = dictionary;
			this.wantUnigramSmear = wantUnigramSmear;
			this.logWordInsertionProbability = this.logMath.linearToLog(wordInsertionProbability);
			this.logSilenceInsertionProbability = this.logMath.linearToLog(silenceInsertionProbability);
			this.logFillerInsertionProbability = this.logMath.linearToLog(fillerInsertionProbability);
			this.logUnitInsertionProbability = this.logMath.linearToLog(unitInsertionProbability);
			this.languageWeight = languageWeight;
			this.addFillerWords = addFillerWords;
			this.generateUnitStates = generateUnitStates;
			this.unigramSmearWeight = unigramSmearWeight;
			this.maxArcCacheSize = maxArcCacheSize;
			this.cacheEnabled = (maxArcCacheSize > 0);
			if (this.cacheEnabled)
			{
				this.arcCache = new LRUCache(maxArcCacheSize);
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			176,
			232,
			5,
			103,
			235,
			124
		})]
		
		public LexTreeLinguist()
		{
			this.wantUnigramSmear = true;
			this.unigramSmearWeight = 1f;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			186,
			108,
			139,
			118,
			118,
			118,
			150,
			118,
			124,
			124,
			124,
			124,
			113,
			118,
			118,
			113,
			145,
			111,
			104,
			145
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.logMath = LogMath.getLogMath();
			this.acousticModel = (AcousticModel)ps.getComponent("acousticModel");
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
			this.languageModel = (LanguageModel)ps.getComponent("languageModel");
			this.dictionary = (Dictionary)ps.getComponent("dictionary");
			this.wantUnigramSmear = ps.getBoolean("wantUnigramSmear").booleanValue();
			this.logWordInsertionProbability = this.logMath.linearToLog(ps.getDouble("wordInsertionProbability"));
			this.logSilenceInsertionProbability = this.logMath.linearToLog(ps.getDouble("silenceInsertionProbability"));
			this.logFillerInsertionProbability = this.logMath.linearToLog(ps.getDouble("fillerInsertionProbability"));
			this.logUnitInsertionProbability = this.logMath.linearToLog(ps.getDouble("unitInsertionProbability"));
			this.languageWeight = ps.getFloat("languageWeight");
			this.addFillerWords = ps.getBoolean("addFillerWords").booleanValue();
			this.generateUnitStates = ps.getBoolean("generateUnitStates").booleanValue();
			this.unigramSmearWeight = ps.getFloat("unigramSmearWeight");
			this.maxArcCacheSize = ps.getInt("cacheSize");
			this.cacheEnabled = (this.maxArcCacheSize > 0);
			if (this.cacheEnabled)
			{
				this.arcCache = new LRUCache(this.maxArcCacheSize);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			218,
			107,
			107,
			107,
			102
		})]
		
		public virtual void allocate()
		{
			this.dictionary.allocate();
			this.acousticModel.allocate();
			this.languageModel.allocate();
			this.compileGrammar();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			231,
			104,
			107,
			104,
			107,
			104,
			107,
			103
		})]
		
		public virtual void deallocate()
		{
			if (this.acousticModel != null)
			{
				this.acousticModel.deallocate();
			}
			if (this.dictionary != null)
			{
				this.dictionary.deallocate();
			}
			if (this.languageModel != null)
			{
				this.languageModel.deallocate();
			}
			this.__hmmTree = null;
		}

		public virtual SearchGraph getSearchGraph()
		{
			return this.searchGraph;
		}

		public virtual void startRecognition()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			2,
			107
		})]
		
		public virtual void stopRecognition()
		{
			this.languageModel.onUtteranceEnd();
		}

		public virtual LanguageModel getLanguageModel()
		{
			return this.languageModel;
		}

		public virtual Dictionary getDictionary()
		{
			return this.dictionary;
		}

		
		internal static SearchStateArc[] access$1300()
		{
			return LexTreeLinguist.EMPTY_ARC;
		}

		
		static LexTreeLinguist()
		{
		}

		
		protected internal object hmmTree
		{
			
			get
			{
				return this.__hmmTree;
			}
			
			set
			{
				this.__hmmTree = (HMMTree)value;
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
				"Ledu/cmu/sphinx/linguist/language/grammar/Grammar, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_GRAMMAR = "grammar";

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
		public const string PROP_ACOUSTIC_MODEL = "acousticModel";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/UnitManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			},
			"defaultClass",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/UnitManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_UNIT_MANAGER = "unitManager";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_FULL_WORD_HISTORIES = "fullWordHistories";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/language/ngram/LanguageModel, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_LANGUAGE_MODEL = "languageModel";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/dictionary/Dictionary, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_DICTIONARY = "dictionary";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			0
		})]
		public const string PROP_CACHE_SIZE = "cacheSize";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_ADD_FILLER_WORDS = "addFillerWords";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_GENERATE_UNIT_STATES = "generateUnitStates";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_WANT_UNIGRAM_SMEAR = "wantUnigramSmear";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_UNIGRAM_SMEAR_WEIGHT = "unigramSmearWeight";

		
		private static SearchStateArc[] EMPTY_ARC = new SearchStateArc[0];

		private LanguageModel languageModel;

		private AcousticModel acousticModel;

		private LogMath logMath;

		private Dictionary dictionary;

		private UnitManager unitManager;

		private Logger logger;

		protected internal bool addFillerWords;

		private bool generateUnitStates;

		private bool wantUnigramSmear;

		private float unigramSmearWeight;

		private bool cacheEnabled;

		private int maxArcCacheSize;

		protected internal float languageWeight;

		private float logWordInsertionProbability;

		private float logUnitInsertionProbability;

		private float logFillerInsertionProbability;

		private float logSilenceInsertionProbability;

		private float logOne;

		private Word sentenceEndWord;

		private Word[] sentenceStartWordArray;

		private SearchGraph searchGraph;

		private HMMPool hmmPool;

		
		private LRUCache arcCache;

		private int maxDepth;

		internal HMMTree __hmmTree;

		private int cacheTrys;

		private int cacheHits;

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.UnitSearchState"
		})]
		[SourceFile("LexTreeLinguist.java")]
		public class LexTreeEndUnitState : LexTreeLinguist.LexTreeState, UnitSearchState, SearchState
		{
			[LineNumberTable(new byte[]
			{
				162,
				235,
				103,
				113,
				105,
				169
			})]
			
			internal LexTreeEndUnitState(LexTreeLinguist lexTreeLinguist, EndNode endNode, WordSequence wordSequence, float num, float num2, float num3, float num4) : base(lexTreeLinguist, endNode, wordSequence, num, num2)
			{
				this.logLanguageProbability = num3;
				this.logInsertionProbability = num4;
			}

			
			
			private EndNode getEndNode()
			{
				return (EndNode)this.getNode();
			}

			
			
			
			public override WordSequence getWordHistory()
			{
				return base.getWordHistory();
			}

			
			
			
			public override float getSmearTerm()
			{
				return base.getSmearTerm();
			}

			
			
			
			public override float getSmearProb()
			{
				return base.getSmearProb();
			}

			
			
			public virtual Unit getUnit()
			{
				return this.getEndNode().getBaseUnit();
			}

			
			
			public override int hashCode()
			{
				return base.hashCode() * 17 + 423;
			}

			public override float getInsertionProbability()
			{
				return this.logInsertionProbability;
			}

			public override float getLanguageProbability()
			{
				return this.logLanguageProbability;
			}

			
			
			public override bool equals(object o)
			{
				return o == this || (o is LexTreeLinguist.LexTreeEndUnitState && base.equals(o));
			}

			[LineNumberTable(new byte[]
			{
				163,
				60,
				103,
				102,
				114,
				136,
				109,
				103,
				108,
				108,
				124,
				235,
				60,
				235,
				71,
				103,
				105,
				108,
				108,
				113,
				252,
				59,
				230,
				72,
				135
			})]
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedArcs();
				if (array == null)
				{
					HMMNode[] array2 = LexTreeLinguist.access$1200(this.this$0, this.getEndNode());
					array = new SearchStateArc[array2.Length];
					if (LexTreeLinguist.access$700(this.this$0))
					{
						for (int i = 0; i < array2.Length; i++)
						{
							array[i] = new LexTreeLinguist.LexTreeUnitState(this.this$0, array2[i], this.getWordHistory(), this.getSmearTerm(), this.getSmearProb(), LexTreeLinguist.access_000(this.this$0), LexTreeLinguist.access_000(this.this$0), this.getNode());
						}
					}
					else
					{
						for (int i = 0; i < array2.Length; i++)
						{
							HMM hmm = array2[i].getHMM();
							array[i] = new LexTreeLinguist.LexTreeHMMState(this.this$0, array2[i], this.getWordHistory(), this.getSmearTerm(), this.getSmearProb(), hmm.getInitialState(), LexTreeLinguist.access_000(this.this$0), LexTreeLinguist.access_000(this.this$0), this.getNode());
						}
					}
					this.putCachedArcs(array);
				}
				return array;
			}

			
			
			public override string toString()
			{
				return new StringBuilder().append(base.toString()).append(" EndUnit").toString();
			}

			public override int getOrder()
			{
				return 3;
			}

			
			
			
			public override string toPrettyString()
			{
				return base.toPrettyString();
			}

			
			
			
			public override Node getLexState()
			{
				return base.getLexState();
			}

			
			
			
			public override bool isFinal()
			{
				return base.isFinal();
			}

			
			
			
			public override bool isEmitting()
			{
				return base.isEmitting();
			}

			
			
			
			public override float getProbability()
			{
				return base.getProbability();
			}

			
			
			
			public override SearchState getState()
			{
				return base.getState();
			}

			
			
			
			public override string getSignature()
			{
				return base.getSignature();
			}

			
			[NameSig("getLexState", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			public object <bridge>getLexState()
			{
				return this.getLexState();
			}

			
			[NameSig("getLexState", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			protected internal object _0()
			{
				return this.getLexState();
			}

			[HideFromReflection]
			[NameSig("<accessstub>1|getNode", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			protected internal new object getNode()
			{
				return this.getNode();
			}

			
			protected internal object _1()
			{
				return base.getNode();
			}

			[HideFromReflection]
			[NameSig("<accessstub>2|getSuccessors", "(Ledu.cmu.sphinx.linguist.lextree.Node;)[Ledu.cmu.sphinx.linguist.SearchStateArc;")]
			protected internal SearchStateArc[] getSuccessors(object obj)
			{
				return this.getSuccessors((Node)obj);
			}

			
			protected internal SearchStateArc[] _2(object obj)
			{
				return base.getSuccessors((Node)obj);
			}

			[HideFromReflection]
			[NameSig("<accessstub>3|createWordStateArc", "(Ledu.cmu.sphinx.linguist.lextree.WordNode;Ledu.cmu.sphinx.linguist.lextree.HMMNode;Ledu.cmu.sphinx.linguist.lextree.LexTreeLinguist$LexTreeState;)Ledu.cmu.sphinx.linguist.SearchStateArc;")]
			protected internal SearchStateArc createWordStateArc(object obj, object obj2, object obj3)
			{
				return this.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			
			protected internal SearchStateArc <nonvirtual>3(object obj, object obj2, object obj3)
			{
				return base.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			[HideFromReflection]
			[NameSig("<accessstub>4|getLexState", "()Ljava.lang.Object;")]
			public object <bridge>getLexState()
			{
				return this.getLexState();
			}

			
			protected internal object <nonvirtual>4()
			{
				return base.getLexState();
			}

			internal float logLanguageProbability;

			internal float logInsertionProbability;

			
			internal new LexTreeLinguist this$0 = lexTreeLinguist;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.WordSearchState"
		})]
		[SourceFile("LexTreeLinguist.java")]
		public class LexTreeEndWordState : LexTreeLinguist.LexTreeWordState, WordSearchState, SearchState
		{
			[LineNumberTable(new byte[]
			{
				165,
				134,
				103,
				148
			})]
			
			internal LexTreeEndWordState(LexTreeLinguist lexTreeLinguist, WordNode wordNode, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, float num3) : base(lexTreeLinguist, wordNode, hmmnode, wordSequence, num, num2, num3)
			{
			}

			public override int getOrder()
			{
				return 2;
			}

			
			internal new LexTreeLinguist this$0 = lexTreeLinguist;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.HMMSearchState",
			"edu.cmu.sphinx.decoder.scorer.ScoreProvider"
		})]
		[SourceFile("LexTreeLinguist.java")]
		public class LexTreeHMMState : LexTreeLinguist.LexTreeState, HMMSearchState, SearchState, ScoreProvider
		{
			public virtual HMMState getHMMState()
			{
				return this.hmmState;
			}

			[LineNumberTable(new byte[]
			{
				164,
				9,
				103,
				241,
				49,
				231,
				80,
				104,
				104,
				105,
				105
			})]
			
			internal LexTreeHMMState(LexTreeLinguist lexTreeLinguist, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, HMMState hmmstate, float num3, float num4, Node node) : base(lexTreeLinguist, hmmnode, wordSequence, num, num2)
			{
				this.hashCode = -1;
				this.hmmState = hmmstate;
				this.parentNode = node;
				this.logLanguageProbability = num3;
				this.logInsertionProbability = num4;
			}

			
			
			
			public override WordSequence getWordHistory()
			{
				return base.getWordHistory();
			}

			
			
			
			public override float getSmearTerm()
			{
				return base.getSmearTerm();
			}

			
			
			
			public override float getSmearProb()
			{
				return base.getSmearProb();
			}

			
			
			public override string getSignature()
			{
				return new StringBuilder().append(base.getSignature()).append("-HMM-").append(this.hmmState.getState()).toString();
			}

			[LineNumberTable(new byte[]
			{
				164,
				46,
				105,
				125,
				104,
				114,
				184
			})]
			
			public override int hashCode()
			{
				if (this.hashCode == -1)
				{
					this.hashCode = base.hashCode() * 29 + (this.hmmState.getState() + 1);
					if (this.parentNode != null)
					{
						this.hashCode *= 377;
						this.hashCode += java.lang.Object.instancehelper_hashCode(this.parentNode);
					}
				}
				return this.hashCode;
			}

			[LineNumberTable(new byte[]
			{
				164,
				65,
				100,
				98,
				104,
				103,
				126,
				43,
				161
			})]
			
			public override bool equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				if (o is LexTreeLinguist.LexTreeHMMState)
				{
					LexTreeLinguist.LexTreeHMMState lexTreeHMMState = (LexTreeLinguist.LexTreeHMMState)o;
					return this.hmmState == lexTreeHMMState.hmmState && this.parentNode == lexTreeHMMState.parentNode && base.equals(o);
				}
				return false;
			}

			public override float getLanguageProbability()
			{
				return this.logLanguageProbability;
			}

			public override float getInsertionProbability()
			{
				return this.logInsertionProbability;
			}

			[LineNumberTable(new byte[]
			{
				164,
				106,
				103,
				230,
				69,
				109,
				104,
				140,
				242,
				70,
				108,
				104,
				106,
				100,
				176,
				149,
				103,
				137,
				105,
				113,
				108,
				113,
				179,
				105,
				113,
				108,
				102,
				241,
				43,
				233,
				89,
				135
			})]
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedArcs();
				if (array == null)
				{
					if (this.hmmState.isExitState())
					{
						if (this.parentNode == null)
						{
							array = base.getSuccessors();
						}
						else
						{
							array = base.getSuccessors(this.parentNode);
						}
					}
					else
					{
						HMMStateArc[] successors = this.hmmState.getSuccessors();
						array = new SearchStateArc[successors.Length];
						for (int i = 0; i < successors.Length; i++)
						{
							HMMStateArc hmmstateArc = successors[i];
							if (hmmstateArc.getHMMState().isEmitting())
							{
								if (hmmstateArc.getHMMState() == this.hmmState && this.logInsertionProbability == hmmstateArc.getLogProbability())
								{
									array[i] = this;
								}
								else
								{
									array[i] = new LexTreeLinguist.LexTreeHMMState(this.this$0, (HMMNode)this.getNode(), this.getWordHistory(), this.getSmearTerm(), this.getSmearProb(), hmmstateArc.getHMMState(), LexTreeLinguist.access_000(this.this$0), hmmstateArc.getLogProbability(), this.parentNode);
								}
							}
							else
							{
								array[i] = new LexTreeLinguist.LexTreeNonEmittingHMMState(this.this$0, (HMMNode)this.getNode(), this.getWordHistory(), this.getSmearTerm(), this.getSmearProb(), hmmstateArc.getHMMState(), hmmstateArc.getLogProbability(), this.parentNode);
							}
						}
					}
					this.putCachedArcs(array);
				}
				return array;
			}

			
			
			public override bool isEmitting()
			{
				return this.hmmState.isEmitting();
			}

			
			
			public override string toString()
			{
				return new StringBuilder().append(base.toString()).append(" hmm:").append(this.hmmState).toString();
			}

			public override int getOrder()
			{
				return 5;
			}

			
			
			public virtual float getScore(Data data)
			{
				return this.hmmState.getScore(data);
			}

			
			
			public virtual float[] getComponentScore(Data feature)
			{
				return this.hmmState.calculateComponentScore(feature);
			}

			
			
			
			public override string toPrettyString()
			{
				return base.toPrettyString();
			}

			
			
			
			public override Node getLexState()
			{
				return base.getLexState();
			}

			
			
			
			public override bool isFinal()
			{
				return base.isFinal();
			}

			
			
			
			public override float getProbability()
			{
				return base.getProbability();
			}

			
			
			
			public override SearchState getState()
			{
				return base.getState();
			}

			
			[NameSig("getLexState", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			public object <bridge>getLexState()
			{
				return this.getLexState();
			}

			
			[NameSig("getLexState", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			protected internal object _0()
			{
				return this.getLexState();
			}

			[HideFromReflection]
			[NameSig("<accessstub>1|getNode", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			protected internal new object getNode()
			{
				return this.getNode();
			}

			
			protected internal object _1()
			{
				return base.getNode();
			}

			[HideFromReflection]
			[NameSig("<accessstub>2|getSuccessors", "(Ledu.cmu.sphinx.linguist.lextree.Node;)[Ledu.cmu.sphinx.linguist.SearchStateArc;")]
			protected internal SearchStateArc[] getSuccessors(object obj)
			{
				return this.getSuccessors((Node)obj);
			}

			
			protected internal SearchStateArc[] _2(object obj)
			{
				return base.getSuccessors((Node)obj);
			}

			[HideFromReflection]
			[NameSig("<accessstub>3|createWordStateArc", "(Ledu.cmu.sphinx.linguist.lextree.WordNode;Ledu.cmu.sphinx.linguist.lextree.HMMNode;Ledu.cmu.sphinx.linguist.lextree.LexTreeLinguist$LexTreeState;)Ledu.cmu.sphinx.linguist.SearchStateArc;")]
			protected internal SearchStateArc createWordStateArc(object obj, object obj2, object obj3)
			{
				return this.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			
			protected internal SearchStateArc <nonvirtual>3(object obj, object obj2, object obj3)
			{
				return base.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			[HideFromReflection]
			[NameSig("<accessstub>4|getLexState", "()Ljava.lang.Object;")]
			public object <bridge>getLexState()
			{
				return this.getLexState();
			}

			
			protected internal object <nonvirtual>4()
			{
				return base.getLexState();
			}

			
			private HMMState hmmState;

			private float logLanguageProbability;

			private float logInsertionProbability;

			
			private Node parentNode;

			internal new int hashCode;

			
			internal new LexTreeLinguist this$0 = lexTreeLinguist;
		}

		
		[SourceFile("LexTreeLinguist.java")]
		public class LexTreeNonEmittingHMMState : LexTreeLinguist.LexTreeHMMState
		{
			[LineNumberTable(new byte[]
			{
				164,
				200,
				103,
				109,
				42,
				133
			})]
			
			internal LexTreeNonEmittingHMMState(LexTreeLinguist lexTreeLinguist, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, HMMState hmmstate, float num3, Node node) : base(lexTreeLinguist, hmmnode, wordSequence, num, num2, hmmstate, LexTreeLinguist.access_000(lexTreeLinguist), num3, node)
			{
			}

			public override int getOrder()
			{
				return 0;
			}

			
			internal new LexTreeLinguist this$0 = lexTreeLinguist;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.SearchGraph"
		})]
		[SourceFile("LexTreeLinguist.java")]
		
		internal sealed class LexTreeSearchGraph : java.lang.Object, SearchGraph
		{
			[LineNumberTable(new byte[]
			{
				161,
				77,
				111,
				103
			})]
			
			internal LexTreeSearchGraph(LexTreeLinguist lexTreeLinguist, SearchState searchState)
			{
				this.initialState = searchState;
			}

			public SearchState getInitialState()
			{
				return this.initialState;
			}

			public int getNumStateOrder()
			{
				return 6;
			}

			public bool getWordTokenFirst()
			{
				return false;
			}

			private SearchState initialState;

			
			internal LexTreeLinguist this$0 = lexTreeLinguist;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.SearchState",
			"edu.cmu.sphinx.linguist.SearchStateArc"
		})]
		[SourceFile("LexTreeLinguist.java")]
		internal abstract class LexTreeState : java.lang.Object, SearchState, SearchStateArc
		{
			
			
			public virtual float getLanguageProbability()
			{
				return LexTreeLinguist.access_000(this.this$0);
			}

			
			
			public virtual float getInsertionProbability()
			{
				return LexTreeLinguist.access_000(this.this$0);
			}

			[LineNumberTable(new byte[]
			{
				162,
				183,
				112,
				119,
				99,
				140,
				124,
				127,
				21,
				119,
				31,
				6,
				197,
				130
			})]
			
			internal virtual SearchStateArc[] getCachedArcs()
			{
				if (LexTreeLinguist.access$800(this.this$0))
				{
					SearchStateArc[] array = (SearchStateArc[])LexTreeLinguist.access$900(this.this$0).get(this);
					if (array != null)
					{
						LexTreeLinguist.access$1008(this.this$0);
					}
					bool flag = LexTreeLinguist.access$1104(this.this$0) != 0;
					int num = 1000000;
					if (num == -1 || (flag ? 1 : 0) % num == 0)
					{
						java.lang.System.@out.println(new StringBuilder().append("Hits: ").append(LexTreeLinguist.access$1000(this.this$0)).append(" of ").append(LexTreeLinguist.access$1100(this.this$0)).append(' ').append((float)LexTreeLinguist.access$1000(this.this$0) / (float)LexTreeLinguist.access$1100(this.this$0) * 100f).toString());
					}
					return array;
				}
				return null;
			}

			[LineNumberTable(new byte[]
			{
				162,
				45,
				103,
				136,
				98,
				152,
				105,
				107,
				43,
				136,
				105,
				147,
				145,
				228,
				54,
				235,
				76
			})]
			
			protected internal virtual SearchStateArc[] getSuccessors(Node node)
			{
				Node[] successors = node.getSuccessors();
				SearchStateArc[] array = new SearchStateArc[successors.Length];
				int num = 0;
				Node[] array2 = successors;
				int num2 = array2.Length;
				for (int i = 0; i < num2; i++)
				{
					Node node2 = array2[i];
					if (node2 is WordNode)
					{
						array[num] = this.createWordStateArc((WordNode)node2, (HMMNode)this.getNode(), this);
					}
					else if (node2 is EndNode)
					{
						array[num] = this.createEndUnitArc((EndNode)node2, this);
					}
					else
					{
						array[num] = this.createUnitStateArc((HMMNode)node2, this);
					}
					num++;
				}
				return array;
			}

			[LineNumberTable(new byte[]
			{
				162,
				206,
				109,
				147
			})]
			
			internal virtual void putCachedArcs(SearchStateArc[] array)
			{
				if (LexTreeLinguist.access$800(this.this$0))
				{
					LexTreeLinguist.access$900(this.this$0).put(this, array);
				}
			}

			protected internal virtual Node getNode()
			{
				return this.node;
			}

			[LineNumberTable(new byte[]
			{
				162,
				75,
				108,
				103,
				135,
				118,
				149,
				11,
				225,
				69,
				120,
				127,
				1,
				205,
				139,
				110,
				111,
				115,
				11,
				225,
				69,
				111,
				115,
				11
			})]
			
			protected internal virtual SearchStateArc createWordStateArc(WordNode wordNode, HMMNode hmmnode, LexTreeLinguist.LexTreeState lexTreeState)
			{
				float num = LexTreeLinguist.access_000(this.this$0);
				Word word = wordNode.getWord();
				float num2 = lexTreeState.getSmearTerm();
				if (word.isFiller() && word != LexTreeLinguist.access$100(this.this$0))
				{
					return new LexTreeLinguist.LexTreeWordState(this.this$0, wordNode, hmmnode, this.wordSequence, num2, LexTreeLinguist.access_000(this.this$0), num);
				}
				WordSequence wordSequence = this.wordSequence.addWord(word, LexTreeLinguist.access$200(this.this$0));
				float num3 = LexTreeLinguist.access$300(this.this$0).getProbability(wordSequence) * this.this$0.languageWeight;
				num2 = LexTreeLinguist.access$400(this.this$0, wordSequence);
				num = num3 - lexTreeState.getSmearProb();
				if (word == LexTreeLinguist.access$100(this.this$0))
				{
					return new LexTreeLinguist.LexTreeEndWordState(this.this$0, wordNode, hmmnode, wordSequence.trim(LexTreeLinguist.access$200(this.this$0) - 1), num2, LexTreeLinguist.access_000(this.this$0), num);
				}
				return new LexTreeLinguist.LexTreeWordState(this.this$0, wordNode, hmmnode, wordSequence.trim(LexTreeLinguist.access$200(this.this$0) - 1), num2, LexTreeLinguist.access_000(this.this$0), num);
			}

			[LineNumberTable(new byte[]
			{
				162,
				145,
				109,
				104,
				106,
				109,
				110,
				45
			})]
			
			internal virtual SearchStateArc createEndUnitArc(EndNode endNode, LexTreeLinguist.LexTreeState lexTreeState)
			{
				float num = LexTreeLinguist.access$600(this.this$0, endNode) + lexTreeState.getSmearTerm();
				float num2 = num - lexTreeState.getSmearProb();
				float num3 = LexTreeLinguist.access$500(this.this$0, endNode);
				return new LexTreeLinguist.LexTreeEndUnitState(this.this$0, endNode, this.getWordHistory(), lexTreeState.getSmearTerm(), num, num2, num3);
			}

			[LineNumberTable(new byte[]
			{
				162,
				115,
				109,
				109,
				104,
				234,
				69,
				109,
				110,
				176,
				104,
				110,
				182
			})]
			
			internal virtual SearchStateArc createUnitStateArc(HMMNode hmmnode, LexTreeLinguist.LexTreeState lexTreeState)
			{
				float num = LexTreeLinguist.access$500(this.this$0, hmmnode);
				float num2 = LexTreeLinguist.access$600(this.this$0, hmmnode) + lexTreeState.getSmearTerm();
				float num3 = num2 - lexTreeState.getSmearProb();
				LexTreeLinguist.LexTreeState result;
				if (LexTreeLinguist.access$700(this.this$0))
				{
					result = new LexTreeLinguist.LexTreeUnitState(this.this$0, hmmnode, this.getWordHistory(), lexTreeState.getSmearTerm(), num2, num3, num);
				}
				else
				{
					HMM hmm = hmmnode.getHMM();
					result = new LexTreeLinguist.LexTreeHMMState(this.this$0, hmmnode, this.getWordHistory(), lexTreeState.getSmearTerm(), num2, hmm.getInitialState(), num3, num, null);
				}
				return result;
			}

			public virtual float getSmearTerm()
			{
				return this.currentSmearTerm;
			}

			public virtual float getSmearProb()
			{
				return this.currentSmearProb;
			}

			public virtual WordSequence getWordHistory()
			{
				return this.wordSequence;
			}

			
			
			public virtual float getProbability()
			{
				return this.getLanguageProbability() + this.getInsertionProbability();
			}

			
			
			public override string toString()
			{
				return new StringBuilder().append("lt-").append(this.node).append(' ').append(this.getProbability()).append('{').append(this.wordSequence).append('}').toString();
			}

			public virtual Node getLexState()
			{
				return this.node;
			}

			[LineNumberTable(new byte[]
			{
				161,
				131,
				111,
				103,
				103,
				105,
				105
			})]
			
			internal LexTreeState(LexTreeLinguist lexTreeLinguist, Node node, WordSequence wordSequence, float num, float num2)
			{
				this.node = node;
				this.wordSequence = wordSequence;
				this.currentSmearTerm = num;
				this.currentSmearProb = num2;
			}

			
			
			public virtual string getSignature()
			{
				return new StringBuilder().append("lts-").append(java.lang.Object.instancehelper_hashCode(this.node)).append("-ws-").append(this.wordSequence).toString();
			}

			[LineNumberTable(new byte[]
			{
				161,
				167,
				111,
				110
			})]
			
			public override int hashCode()
			{
				int num = this.wordSequence.hashCode() * 37;
				return num + java.lang.Object.instancehelper_hashCode(this.node);
			}

			[LineNumberTable(new byte[]
			{
				161,
				181,
				100,
				98,
				104,
				103,
				110,
				98,
				146
			})]
			
			public override bool equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj is LexTreeLinguist.LexTreeState)
				{
					LexTreeLinguist.LexTreeState lexTreeState = (LexTreeLinguist.LexTreeState)obj;
					return this.node == lexTreeState.node && this.wordSequence.equals(lexTreeState.wordSequence);
				}
				return false;
			}

			public virtual SearchState getState()
			{
				return this;
			}

			public virtual bool isEmitting()
			{
				return false;
			}

			public virtual bool isFinal()
			{
				return false;
			}

			[LineNumberTable(new byte[]
			{
				162,
				29,
				103,
				99,
				109,
				135
			})]
			
			public virtual SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedArcs();
				if (array == null)
				{
					array = this.getSuccessors(this.node);
					this.putCachedArcs(array);
				}
				return array;
			}

			
			
			public virtual string toPrettyString()
			{
				return this.toString();
			}

			public abstract int getOrder();

			
			
			
			public virtual object getLexState()
			{
				return this.getLexState();
			}

			
			private Node node;

			
			private WordSequence wordSequence;

			
			internal float currentSmearTerm;

			
			internal float currentSmearProb;

			
			internal LexTreeLinguist this$0 = lexTreeLinguist;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.UnitSearchState"
		})]
		[SourceFile("LexTreeLinguist.java")]
		public class LexTreeUnitState : LexTreeLinguist.LexTreeState, UnitSearchState, SearchState
		{
			[LineNumberTable(new byte[]
			{
				163,
				117,
				150
			})]
			
			internal LexTreeUnitState(LexTreeLinguist lexTreeLinguist, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, float num3, float num4) : this(lexTreeLinguist, hmmnode, wordSequence, num, num2, num3, num4, null)
			{
			}

			[LineNumberTable(new byte[]
			{
				163,
				129,
				103,
				241,
				40,
				231,
				89,
				105,
				105,
				104
			})]
			
			internal LexTreeUnitState(LexTreeLinguist lexTreeLinguist, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, float num3, float num4, Node node) : base(lexTreeLinguist, hmmnode, wordSequence, num, num2)
			{
				this.hashCode = -1;
				this.logInsertionProbability = num4;
				this.logLanguageProbability = num3;
				this.parentNode = node;
			}

			
			
			private HMMNode getHMMNode()
			{
				return (HMMNode)this.getNode();
			}

			
			
			
			public override WordSequence getWordHistory()
			{
				return base.getWordHistory();
			}

			
			
			
			public override float getSmearTerm()
			{
				return base.getSmearTerm();
			}

			
			
			
			public override float getSmearProb()
			{
				return base.getSmearProb();
			}

			
			
			public virtual Unit getUnit()
			{
				return this.getHMMNode().getBaseUnit();
			}

			[LineNumberTable(new byte[]
			{
				163,
				154,
				105,
				117,
				104,
				114,
				184
			})]
			
			public override int hashCode()
			{
				if (this.hashCode == -1)
				{
					this.hashCode = base.hashCode() * 17 + 421;
					if (this.parentNode != null)
					{
						this.hashCode *= 432;
						this.hashCode += java.lang.Object.instancehelper_hashCode(this.parentNode);
					}
				}
				return this.hashCode;
			}

			[LineNumberTable(new byte[]
			{
				163,
				173,
				100,
				98,
				104,
				103,
				156
			})]
			
			public override bool equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				if (o is LexTreeLinguist.LexTreeUnitState)
				{
					LexTreeLinguist.LexTreeUnitState lexTreeUnitState = (LexTreeLinguist.LexTreeUnitState)o;
					return this.parentNode == lexTreeUnitState.parentNode && base.equals(o);
				}
				return false;
			}

			[LineNumberTable(new byte[]
			{
				163,
				201,
				103,
				108,
				117,
				119,
				124
			})]
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = new SearchStateArc[1];
				HMM hmm = this.getHMMNode().getHMM();
				array[0] = new LexTreeLinguist.LexTreeHMMState(this.this$0, this.getHMMNode(), this.getWordHistory(), this.getSmearTerm(), this.getSmearProb(), hmm.getInitialState(), LexTreeLinguist.access_000(this.this$0), LexTreeLinguist.access_000(this.this$0), this.parentNode);
				return array;
			}

			
			
			public override string toString()
			{
				return new StringBuilder().append(base.toString()).append(" unit").toString();
			}

			public override float getInsertionProbability()
			{
				return this.logInsertionProbability;
			}

			public override float getLanguageProbability()
			{
				return this.logLanguageProbability;
			}

			public override int getOrder()
			{
				return 4;
			}

			
			
			
			public override string toPrettyString()
			{
				return base.toPrettyString();
			}

			
			
			
			public override Node getLexState()
			{
				return base.getLexState();
			}

			
			
			
			public override bool isFinal()
			{
				return base.isFinal();
			}

			
			
			
			public override bool isEmitting()
			{
				return base.isEmitting();
			}

			
			
			
			public override float getProbability()
			{
				return base.getProbability();
			}

			
			
			
			public override SearchState getState()
			{
				return base.getState();
			}

			
			
			
			public override string getSignature()
			{
				return base.getSignature();
			}

			
			[NameSig("getLexState", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			public object <bridge>getLexState()
			{
				return this.getLexState();
			}

			
			[NameSig("getLexState", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			protected internal object _0()
			{
				return this.getLexState();
			}

			[HideFromReflection]
			[NameSig("<accessstub>1|getNode", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			protected internal new object getNode()
			{
				return this.getNode();
			}

			
			protected internal object _1()
			{
				return base.getNode();
			}

			[HideFromReflection]
			[NameSig("<accessstub>2|getSuccessors", "(Ledu.cmu.sphinx.linguist.lextree.Node;)[Ledu.cmu.sphinx.linguist.SearchStateArc;")]
			protected internal SearchStateArc[] getSuccessors(object obj)
			{
				return this.getSuccessors((Node)obj);
			}

			
			protected internal SearchStateArc[] _2(object obj)
			{
				return base.getSuccessors((Node)obj);
			}

			[HideFromReflection]
			[NameSig("<accessstub>3|createWordStateArc", "(Ledu.cmu.sphinx.linguist.lextree.WordNode;Ledu.cmu.sphinx.linguist.lextree.HMMNode;Ledu.cmu.sphinx.linguist.lextree.LexTreeLinguist$LexTreeState;)Ledu.cmu.sphinx.linguist.SearchStateArc;")]
			protected internal SearchStateArc createWordStateArc(object obj, object obj2, object obj3)
			{
				return this.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			
			protected internal SearchStateArc <nonvirtual>3(object obj, object obj2, object obj3)
			{
				return base.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			[HideFromReflection]
			[NameSig("<accessstub>4|getLexState", "()Ljava.lang.Object;")]
			public object <bridge>getLexState()
			{
				return this.getLexState();
			}

			
			protected internal object <nonvirtual>4()
			{
				return base.getLexState();
			}

			private float logInsertionProbability;

			private float logLanguageProbability;

			private Node parentNode;

			private new int hashCode;

			
			internal new LexTreeLinguist this$0 = lexTreeLinguist;
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.WordSearchState"
		})]
		[SourceFile("LexTreeLinguist.java")]
		public class LexTreeWordState : LexTreeLinguist.LexTreeState, WordSearchState, SearchState
		{
			[LineNumberTable(new byte[]
			{
				164,
				230,
				135,
				146,
				103,
				105
			})]
			
			internal LexTreeWordState(LexTreeLinguist lexTreeLinguist, WordNode wordNode, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, float num3) : base(lexTreeLinguist, wordNode, wordSequence, num, num2)
			{
				this.lastNode = hmmnode;
				this.logLanguageProbability = num3;
			}

			
			
			public virtual Pronunciation getPronunciation()
			{
				return ((WordNode)this.getNode()).getPronunciation();
			}

			
			
			public override bool isFinal()
			{
				return ((WordNode)this.getNode()).isFinal();
			}

			
			
			public override int hashCode()
			{
				return base.hashCode() * 41 + java.lang.Object.instancehelper_hashCode(this.lastNode);
			}

			
			
			public override string getSignature()
			{
				return new StringBuilder().append(base.getSignature()).append("-ln-").append(java.lang.Object.instancehelper_hashCode(this.lastNode)).toString();
			}

			[LineNumberTable(new byte[]
			{
				165,
				35,
				100,
				98,
				104,
				103,
				156
			})]
			
			public override bool equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				if (o is LexTreeLinguist.LexTreeWordState)
				{
					LexTreeLinguist.LexTreeWordState lexTreeWordState = (LexTreeLinguist.LexTreeWordState)o;
					return this.lastNode == lexTreeWordState.lastNode && base.equals(o);
				}
				return false;
			}

			public override float getLanguageProbability()
			{
				return this.logLanguageProbability;
			}

			[LineNumberTable(new byte[]
			{
				165,
				64,
				103,
				102,
				102,
				140,
				118,
				98,
				102,
				109,
				136,
				124,
				118,
				121,
				41,
				8,
				235,
				73,
				110,
				127,
				0,
				117,
				194,
				159,
				5,
				135
			})]
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedArcs();
				if (array == null)
				{
					array = LexTreeLinguist.access$1300();
					WordNode wordNode = (WordNode)this.getNode();
					if (wordNode.getWord() != LexTreeLinguist.access$100(this.this$0))
					{
						int num = 0;
						ArrayList arrayList = new ArrayList();
						Unit[] rc = this.lastNode.getRC();
						Unit lastUnit = wordNode.getLastUnit();
						Unit[] array2 = rc;
						int num2 = array2.Length;
						for (int i = 0; i < num2; i++)
						{
							Unit unit = array2[i];
							Node[] entryPoint = this.this$0.__hmmTree.getEntryPoint(lastUnit, unit);
							Node[] array3 = entryPoint;
							int num3 = array3.Length;
							for (int j = 0; j < num3; j++)
							{
								Node node = array3[j];
								arrayList.add(node);
							}
						}
						array = new SearchStateArc[arrayList.size() + 1];
						Iterator iterator = arrayList.iterator();
						while (iterator.hasNext())
						{
							Node node2 = (Node)iterator.next();
							SearchStateArc[] array4 = array;
							int num4 = num;
							num++;
							array4[num4] = this.createUnitStateArc((HMMNode)node2, this);
						}
						SearchStateArc[] array5 = array;
						int num5 = num;
						num++;
						array5[num5] = this.createWordStateArc(this.this$0.__hmmTree.getSentenceEndWordNode(), this.lastNode, this);
					}
					this.putCachedArcs(array);
				}
				return array;
			}

			public override int getOrder()
			{
				return 1;
			}

			public virtual bool isWordStart()
			{
				return false;
			}

			
			
			
			public override string toPrettyString()
			{
				return base.toPrettyString();
			}

			
			
			
			public override string toString()
			{
				return base.toString();
			}

			
			
			
			public override Node getLexState()
			{
				return base.getLexState();
			}

			
			
			
			public override WordSequence getWordHistory()
			{
				return base.getWordHistory();
			}

			
			
			
			public override bool isEmitting()
			{
				return base.isEmitting();
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

			
			
			
			public override float getSmearProb()
			{
				return base.getSmearProb();
			}

			
			
			
			public override float getSmearTerm()
			{
				return base.getSmearTerm();
			}

			
			[NameSig("getLexState", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			public object <bridge>getLexState()
			{
				return this.getLexState();
			}

			
			[NameSig("getLexState", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			protected internal object _0()
			{
				return this.getLexState();
			}

			[HideFromReflection]
			[NameSig("<accessstub>1|getNode", "()Ledu.cmu.sphinx.linguist.lextree.Node;")]
			protected internal new object getNode()
			{
				return this.getNode();
			}

			
			protected internal object _1()
			{
				return base.getNode();
			}

			[HideFromReflection]
			[NameSig("<accessstub>2|getSuccessors", "(Ledu.cmu.sphinx.linguist.lextree.Node;)[Ledu.cmu.sphinx.linguist.SearchStateArc;")]
			protected internal SearchStateArc[] getSuccessors(object obj)
			{
				return this.getSuccessors((Node)obj);
			}

			
			protected internal SearchStateArc[] _2(object obj)
			{
				return base.getSuccessors((Node)obj);
			}

			[HideFromReflection]
			[NameSig("<accessstub>3|createWordStateArc", "(Ledu.cmu.sphinx.linguist.lextree.WordNode;Ledu.cmu.sphinx.linguist.lextree.HMMNode;Ledu.cmu.sphinx.linguist.lextree.LexTreeLinguist$LexTreeState;)Ledu.cmu.sphinx.linguist.SearchStateArc;")]
			protected internal SearchStateArc createWordStateArc(object obj, object obj2, object obj3)
			{
				return this.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			
			protected internal SearchStateArc <nonvirtual>3(object obj, object obj2, object obj3)
			{
				return base.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			[HideFromReflection]
			[NameSig("<accessstub>4|getLexState", "()Ljava.lang.Object;")]
			public object <bridge>getLexState()
			{
				return this.getLexState();
			}

			
			protected internal object <nonvirtual>4()
			{
				return base.getLexState();
			}

			private HMMNode lastNode;

			private float logLanguageProbability;

			
			internal new LexTreeLinguist this$0 = lexTreeLinguist;
		}
	}
}

using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.ngram;
using edu.cmu.sphinx.linguist.util;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.lextree
{
	public class LexTreeLinguist : LinguistBase
	{			
		internal static float access_000(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.logOne;
		}
				
		internal static Word access_100(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.sentenceEndWord;
		}
		
		internal static int access_200(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.maxDepth;
		}
		
		internal static LanguageModel access_300(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.languageModel;
		}		
		
		internal static float access_400(LexTreeLinguist lexTreeLinguist, WordSequence wordSequence)
		{
			return lexTreeLinguist.getSmearTermFromLanguageModel(wordSequence);
		}
				
		internal static float access_500(LexTreeLinguist lexTreeLinguist, UnitNode unitNode)
		{
			return lexTreeLinguist.calculateInsertionProbability(unitNode);
		}		
		
		internal static float access_600(LexTreeLinguist lexTreeLinguist, Node node)
		{
			return lexTreeLinguist.getUnigramSmear(node);
		}
		
		internal static bool access_700(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.generateUnitStates;
		}		
		
		internal static bool access_800(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.cacheEnabled;
		}
		
		internal static LRUCache access_900(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.arcCache;
		}
		
		internal static int access_1008(LexTreeLinguist lexTreeLinguist)
		{
			int num = lexTreeLinguist.cacheHits;
			int result = num;
			lexTreeLinguist.cacheHits = num + 1;
			return result;
		}		
		
		internal static int access_1104(LexTreeLinguist lexTreeLinguist)
		{
			int num = lexTreeLinguist.cacheTrys + 1;
			int result = num;
			lexTreeLinguist.cacheTrys = num;
			return result;
		}
		
		internal static int access_1000(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.cacheHits;
		}
		
		internal static int access_1100(LexTreeLinguist lexTreeLinguist)
		{
			return lexTreeLinguist.cacheTrys;
		}
		
		internal static HMMNode[] access_1200(LexTreeLinguist lexTreeLinguist, EndNode endNode)
		{
			return lexTreeLinguist.getHMMNodes(endNode);
		}
		
		private HMMNode[] getHMMNodes(EndNode endNode)
		{
			return this.__hmmTree.getHMMNodes(endNode);
		}
		
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
		
		protected internal virtual void generateHmmTree()
		{
			this.hmmPool = new HMMPool(this.acousticModel, this.logger, this.unitManager);
			this.__hmmTree = new HMMTree(this.hmmPool, this.dictionary, this.languageModel, this.addFillerWords, this.languageWeight);
			this.hmmPool.dumpInfo();
		}
		
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
			return new LexTreeLinguist.LexTreeWordState(this, wordNode, parent, new WordSequence(this.sentenceStartWordArray).trim(this.maxDepth - 1), 0f, this.logOne, this.logOne);
		}
		
		public LexTreeLinguist(AcousticModel acousticModel, UnitManager unitManager, LanguageModel languageModel, dictionary.Dictionary dictionary, bool fullWordHistories, bool wantUnigramSmear, double wordInsertionProbability, double silenceInsertionProbability, double fillerInsertionProbability, double unitInsertionProbability, float languageWeight, bool addFillerWords, bool generateUnitStates, float unigramSmearWeight, int maxArcCacheSize)
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
		
		public LexTreeLinguist()
		{
			this.wantUnigramSmear = true;
			this.unigramSmearWeight = 1f;
		}
		
		public override void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.logMath = LogMath.getLogMath();
			this.acousticModel = (AcousticModel)ps.getComponent("acousticModel");
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
			this.languageModel = (LanguageModel)ps.getComponent("languageModel");
			this.dictionary = (dictionary.Dictionary)ps.getComponent("dictionary");
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
		
		public override void allocate()
		{
			this.dictionary.allocate();
			this.acousticModel.allocate();
			this.languageModel.allocate();
			this.compileGrammar();
		}
		
		public override void deallocate()
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

		public override SearchGraph getSearchGraph()
		{
			return this.searchGraph;
		}

		public override void startRecognition()
		{
		}
		
		public override void stopRecognition()
		{
			this.languageModel.onUtteranceEnd();
		}

		public virtual LanguageModel getLanguageModel()
		{
			return this.languageModel;
		}

		public virtual dictionary.Dictionary getDictionary()
		{
			return this.dictionary;
		}
		
		internal static SearchStateArc[] access_1300()
		{
			return LexTreeLinguist.EMPTY_ARC;
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

		private dictionary.Dictionary dictionary;

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

		internal class LexTreeEndUnitState : LexTreeState, UnitSearchState, SearchState
		{	
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
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedArcs();
				if (array == null)
				{
					HMMNode[] array2 = LexTreeLinguist.access_1200(this.this_0, this.getEndNode());
					array = new SearchStateArc[array2.Length];
					if (LexTreeLinguist.access_700(this.this_0))
					{
						for (int i = 0; i < array2.Length; i++)
						{
							array[i] = new LexTreeLinguist.LexTreeUnitState(this.this_0, array2[i], this.getWordHistory(), this.getSmearTerm(), this.getSmearProb(), LexTreeLinguist.access_000(this.this_0), LexTreeLinguist.access_000(this.this_0), this.getNode());
						}
					}
					else
					{
						for (int i = 0; i < array2.Length; i++)
						{
							HMM hmm = array2[i].getHMM();
							array[i] = new LexTreeLinguist.LexTreeHMMState(this.this_0, array2[i], this.getWordHistory(), this.getSmearTerm(), this.getSmearProb(), hmm.getInitialState(), LexTreeLinguist.access_000(this.this_0), LexTreeLinguist.access_000(this.this_0), this.getNode());
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

			object SearchState.getLexState()
			{
				return this.getLexState();
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

			protected internal SearchStateArc[] getSuccessors(object obj)
			{
				return this.getSuccessors((Node)obj);
			}
			
			protected internal SearchStateArc createWordStateArc(object obj, object obj2, object obj3)
			{
				return this.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			internal float logLanguageProbability;

			internal float logInsertionProbability;
		}

		internal class LexTreeEndWordState : LexTreeWordState, WordSearchState, SearchState
		{			
			internal LexTreeEndWordState(LexTreeLinguist lexTreeLinguist, WordNode wordNode, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, float num3) : base(lexTreeLinguist, wordNode, hmmnode, wordSequence, num, num2, num3)
			{
			}

			public override int getOrder()
			{
				return 2;
			}
		}

		internal class LexTreeHMMState : LexTreeState, HMMSearchState, SearchState, ScoreProvider
		{
			public virtual HMMState getHMMState()
			{
				return this._hmmState;
			}
		
			internal LexTreeHMMState(LexTreeLinguist lexTreeLinguist, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, HMMState hmmstate, float num3, float num4, Node node) : base(lexTreeLinguist, hmmnode, wordSequence, num, num2)
			{
				this._hashCode = -1;
				this._hmmState = hmmstate;
				this._parentNode = node;
				this._logLanguageProbability = num3;
				this._logInsertionProbability = num4;
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
				return new StringBuilder().append(base.getSignature()).append("-HMM-").append(this._hmmState.getState()).toString();
			}
			
			public override int hashCode()
			{
				if (this._hashCode == -1)
				{
					this._hashCode = base.hashCode() * 29 + (this._hmmState.getState() + 1);
					if (this._parentNode != null)
					{
						this._hashCode *= 377;
						this._hashCode += java.lang.Object.instancehelper_hashCode(this._parentNode);
					}
				}
				return this._hashCode;
			}
			
			public override bool equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				if (o is LexTreeLinguist.LexTreeHMMState)
				{
					LexTreeLinguist.LexTreeHMMState lexTreeHMMState = (LexTreeLinguist.LexTreeHMMState)o;
					return this._hmmState == lexTreeHMMState._hmmState && this._parentNode == lexTreeHMMState._parentNode && base.equals(o);
				}
				return false;
			}

			public override float getLanguageProbability()
			{
				return this._logLanguageProbability;
			}

			public override float getInsertionProbability()
			{
				return this._logInsertionProbability;
			}
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedArcs();
				if (array == null)
				{
					if (this._hmmState.isExitState())
					{
						if (this._parentNode == null)
						{
							array = base.getSuccessors();
						}
						else
						{
							array = base.getSuccessors(this._parentNode);
						}
					}
					else
					{
						HMMStateArc[] successors = this._hmmState.getSuccessors();
						array = new SearchStateArc[successors.Length];
						for (int i = 0; i < successors.Length; i++)
						{
							HMMStateArc hmmstateArc = successors[i];
							if (hmmstateArc.getHMMState().isEmitting())
							{
								if (hmmstateArc.getHMMState() == this._hmmState && this._logInsertionProbability == hmmstateArc.getLogProbability())
								{
									array[i] = this;
								}
								else
								{
									array[i] = new LexTreeLinguist.LexTreeHMMState(this.this_0, (HMMNode)this.getNode(), this.getWordHistory(), this.getSmearTerm(), this.getSmearProb(), hmmstateArc.getHMMState(), LexTreeLinguist.access_000(this.this_0), hmmstateArc.getLogProbability(), this._parentNode);
								}
							}
							else
							{
								array[i] = new LexTreeLinguist.LexTreeNonEmittingHMMState(this.this_0, (HMMNode)this.getNode(), this.getWordHistory(), this.getSmearTerm(), this.getSmearProb(), hmmstateArc.getHMMState(), hmmstateArc.getLogProbability(), this._parentNode);
							}
						}
					}
					this.putCachedArcs(array);
				}
				return array;
			}
			
			public override bool isEmitting()
			{
				return this._hmmState.isEmitting();
			}
						
			public override string toString()
			{
				return new StringBuilder().append(base.toString()).append(" hmm:").append(this._hmmState).toString();
			}

			public override int getOrder()
			{
				return 5;
			}
		
			public virtual float getScore(Data data)
			{
				return this._hmmState.getScore(data);
			}
			
			public virtual float[] getComponentScore(Data feature)
			{
				return this._hmmState.calculateComponentScore(feature);
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

			protected internal SearchStateArc[] getSuccessors(object obj)
			{
				return this.getSuccessors((Node)obj);
			}
			
			protected internal SearchStateArc[] _2(object obj)
			{
				return base.getSuccessors((Node)obj);
			}
			protected internal SearchStateArc createWordStateArc(object obj, object obj2, object obj3)
			{
				return this.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			private HMMState _hmmState;

			private float _logLanguageProbability;

			private float _logInsertionProbability;

			private Node _parentNode;

			internal int _hashCode;			
		}

		internal class LexTreeNonEmittingHMMState : LexTreeLinguist.LexTreeHMMState
		{			
			internal LexTreeNonEmittingHMMState(LexTreeLinguist lexTreeLinguist, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, HMMState hmmstate, float num3, Node node) : base(lexTreeLinguist, hmmnode, wordSequence, num, num2, hmmstate, LexTreeLinguist.access_000(lexTreeLinguist), num3, node)
			{
			}

			public override int getOrder()
			{
				return 0;
			}
		}

		internal sealed class LexTreeSearchGraph : java.lang.Object, SearchGraph
		{
			internal LexTreeSearchGraph(LexTreeLinguist lexTreeLinguist, SearchState searchState)
			{
				this.this_0 = lexTreeLinguist;
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
			
			internal LexTreeLinguist this_0;
		}

		internal abstract class LexTreeState : java.lang.Object, SearchState, SearchStateArc
		{
			public virtual float getLanguageProbability()
			{
				return LexTreeLinguist.access_000(this.this_0);
			}
			
			public virtual float getInsertionProbability()
			{
				return LexTreeLinguist.access_000(this.this_0);
			}

			internal virtual SearchStateArc[] getCachedArcs()
			{
				if (LexTreeLinguist.access_800(this.this_0))
				{
					SearchStateArc[] array = (SearchStateArc[])LexTreeLinguist.access_900(this.this_0).get(this);
					if (array != null)
					{
						LexTreeLinguist.access_1008(this.this_0);
					}
					bool flag = LexTreeLinguist.access_1104(this.this_0) != 0;
					int num = 1000000;
					if (num == -1 || (flag ? 1 : 0) % num == 0)
					{
						java.lang.System.@out.println(new StringBuilder().append("Hits: ").append(LexTreeLinguist.access_1000(this.this_0)).append(" of ").append(LexTreeLinguist.access_1100(this.this_0)).append(' ').append((float)LexTreeLinguist.access_1000(this.this_0) / (float)LexTreeLinguist.access_1100(this.this_0) * 100f).toString());
					}
					return array;
				}
				return null;
			}
	
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
		
			internal virtual void putCachedArcs(SearchStateArc[] array)
			{
				if (LexTreeLinguist.access_800(this.this_0))
				{
					LexTreeLinguist.access_900(this.this_0).put(this, array);
				}
			}

			protected internal virtual Node getNode()
			{
				return this.node;
			}
			
			protected internal virtual SearchStateArc createWordStateArc(WordNode wordNode, HMMNode hmmnode, LexTreeLinguist.LexTreeState lexTreeState)
			{
				float num = LexTreeLinguist.access_000(this.this_0);
				Word word = wordNode.getWord();
				float num2 = lexTreeState.getSmearTerm();
				if (word.isFiller() && word != LexTreeLinguist.access_100(this.this_0))
				{
					return new LexTreeLinguist.LexTreeWordState(this.this_0, wordNode, hmmnode, this.wordSequence, num2, LexTreeLinguist.access_000(this.this_0), num);
				}
				WordSequence wordSequence = this.wordSequence.addWord(word, LexTreeLinguist.access_200(this.this_0));
				float num3 = LexTreeLinguist.access_300(this.this_0).getProbability(wordSequence) * this.this_0.languageWeight;
				num2 = LexTreeLinguist.access_400(this.this_0, wordSequence);
				num = num3 - lexTreeState.getSmearProb();
				if (word == LexTreeLinguist.access_100(this.this_0))
				{
					return new LexTreeLinguist.LexTreeEndWordState(this.this_0, wordNode, hmmnode, wordSequence.trim(LexTreeLinguist.access_200(this.this_0) - 1), num2, LexTreeLinguist.access_000(this.this_0), num);
				}
				return new LexTreeLinguist.LexTreeWordState(this.this_0, wordNode, hmmnode, wordSequence.trim(LexTreeLinguist.access_200(this.this_0) - 1), num2, LexTreeLinguist.access_000(this.this_0), num);
			}
		
			internal virtual SearchStateArc createEndUnitArc(EndNode endNode, LexTreeLinguist.LexTreeState lexTreeState)
			{
				float num = LexTreeLinguist.access_600(this.this_0, endNode) + lexTreeState.getSmearTerm();
				float num2 = num - lexTreeState.getSmearProb();
				float num3 = LexTreeLinguist.access_500(this.this_0, endNode);
				return new LexTreeLinguist.LexTreeEndUnitState(this.this_0, endNode, this.getWordHistory(), lexTreeState.getSmearTerm(), num, num2, num3);
			}
			
			internal virtual SearchStateArc createUnitStateArc(HMMNode hmmnode, LexTreeLinguist.LexTreeState lexTreeState)
			{
				float num = LexTreeLinguist.access_500(this.this_0, hmmnode);
				float num2 = LexTreeLinguist.access_600(this.this_0, hmmnode) + lexTreeState.getSmearTerm();
				float num3 = num2 - lexTreeState.getSmearProb();
				LexTreeLinguist.LexTreeState result;
				if (LexTreeLinguist.access_700(this.this_0))
				{
					result = new LexTreeLinguist.LexTreeUnitState(this.this_0, hmmnode, this.getWordHistory(), lexTreeState.getSmearTerm(), num2, num3, num);
				}
				else
				{
					HMM hmm = hmmnode.getHMM();
					result = new LexTreeLinguist.LexTreeHMMState(this.this_0, hmmnode, this.getWordHistory(), lexTreeState.getSmearTerm(), num2, hmm.getInitialState(), num3, num, null);
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
			object SearchState.getLexState()
			{
				return this.getLexState();
			}

			internal LexTreeState(LexTreeLinguist lexTreeLinguist, Node node, WordSequence wordSequence, float num, float num2)
			{
				this.this_0 = lexTreeLinguist;
				this.node = node;
				this.wordSequence = wordSequence;
				this.currentSmearTerm = num;
				this.currentSmearProb = num2;
			}
		
			public virtual string getSignature()
			{
				return new StringBuilder().append("lts-").append(java.lang.Object.instancehelper_hashCode(this.node)).append("-ws-").append(this.wordSequence).toString();
			}
			
			public override int hashCode()
			{
				int num = this.wordSequence.hashCode() * 37;
				return num + java.lang.Object.instancehelper_hashCode(this.node);
			}
			
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
			
			private Node node;

			protected HMMNode lastNode;
			
			private WordSequence wordSequence;
			
			internal float currentSmearTerm;
			
			internal float currentSmearProb;
			
			internal LexTreeLinguist this_0;
		}

		internal class LexTreeUnitState : LexTreeState, UnitSearchState, SearchState
		{
			internal LexTreeUnitState(LexTreeLinguist lexTreeLinguist, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, float num3, float num4) : this(lexTreeLinguist, hmmnode, wordSequence, num, num2, num3, num4, null)
			{
			}

			internal LexTreeUnitState(LexTreeLinguist lexTreeLinguist, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, float num3, float num4, Node node) : base(lexTreeLinguist, hmmnode, wordSequence, num, num2)
			{
				this._hashCode = -1;
				this._logInsertionProbability = num4;
				this._logLanguageProbability = num3;
				this._parentNode = node;
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
			
			public override int hashCode()
			{
				if (this._hashCode == -1)
				{
					this._hashCode = base.hashCode() * 17 + 421;
					if (this._parentNode != null)
					{
						this._hashCode *= 432;
						this._hashCode += java.lang.Object.instancehelper_hashCode(this._parentNode);
					}
				}
				return this._hashCode;
			}
			
			public override bool equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				if (o is LexTreeLinguist.LexTreeUnitState)
				{
					LexTreeLinguist.LexTreeUnitState lexTreeUnitState = (LexTreeLinguist.LexTreeUnitState)o;
					return this._parentNode == lexTreeUnitState._parentNode && base.equals(o);
				}
				return false;
			}
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = new SearchStateArc[1];
				HMM hmm = this.getHMMNode().getHMM();
				array[0] = new LexTreeLinguist.LexTreeHMMState(this.this_0, this.getHMMNode(), this.getWordHistory(), this.getSmearTerm(), this.getSmearProb(), hmm.getInitialState(), LexTreeLinguist.access_000(this.this_0), LexTreeLinguist.access_000(this.this_0), this._parentNode);
				return array;
			}
			
			public override string toString()
			{
				return new StringBuilder().append(base.toString()).append(" unit").toString();
			}

			public override float getInsertionProbability()
			{
				return this._logInsertionProbability;
			}

			public override float getLanguageProbability()
			{
				return this._logLanguageProbability;
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

			protected internal new object getNode()
			{
				return this.getNode();
			}

			protected internal SearchStateArc[] getSuccessors(object obj)
			{
				return this.getSuccessors((Node)obj);
			}

			protected internal SearchStateArc createWordStateArc(object obj, object obj2, object obj3)
			{
				return this.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			private float _logInsertionProbability;

			private float _logLanguageProbability;

			private Node _parentNode;

			private int _hashCode;
		}

		internal class LexTreeWordState : LexTreeState, WordSearchState, SearchState
		{
			internal LexTreeWordState(LexTreeLinguist lexTreeLinguist, WordNode wordNode, HMMNode hmmnode, WordSequence wordSequence, float num, float num2, float num3) : base(lexTreeLinguist, wordNode, wordSequence, num, num2)
			{
				this._lastNode = hmmnode;
				this._logLanguageProbability = num3;
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
			
			public override bool equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				if (o is LexTreeLinguist.LexTreeWordState)
				{
					LexTreeLinguist.LexTreeWordState lexTreeWordState = (LexTreeLinguist.LexTreeWordState)o;
					return this._lastNode == lexTreeWordState._lastNode && base.equals(o);
				}
				return false;
			}

			public override float getLanguageProbability()
			{
				return this._logLanguageProbability;
			}
			
			public override SearchStateArc[] getSuccessors()
			{
				SearchStateArc[] array = this.getCachedArcs();
				if (array == null)
				{
					array = LexTreeLinguist.access_1300();
					WordNode wordNode = (WordNode)this.getNode();
					if (wordNode.getWord() != LexTreeLinguist.access_100(this.this_0))
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
							Node[] entryPoint = this.this_0.__hmmTree.getEntryPoint(lastUnit, unit);
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
						array5[num5] = this.createWordStateArc(this.this_0.__hmmTree.getSentenceEndWordNode(), this.lastNode, this);
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

			object SearchState.getLexState()
			{
				return this.getLexState();
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

			protected internal SearchStateArc[] getSuccessors(object obj)
			{
				return this.getSuccessors((Node)obj);
			}

			protected internal SearchStateArc createWordStateArc(object obj, object obj2, object obj3)
			{
				return this.createWordStateArc((WordNode)obj, (HMMNode)obj2, (LexTreeLinguist.LexTreeState)obj3);
			}

			private HMMNode _lastNode;

			private float _logLanguageProbability;
		}
	}
}

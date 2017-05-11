using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.grammar;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.flat
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.Linguist",
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public class FlatLinguist : java.lang.Object, Linguist, Configurable
	{
		private void T(string text)
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			164,
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

		public virtual string getName()
		{
			return this.name;
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			219,
			145,
			107,
			139,
			102,
			208,
			112,
			127,
			6,
			104,
			104,
			98,
			113,
			230,
			70,
			112,
			124,
			105,
			209,
			112,
			124,
			105,
			209,
			112,
			124,
			105,
			145,
			168,
			104,
			115,
			98,
			113,
			186,
			110,
			177,
			104,
			127,
			10,
			106,
			103,
			130,
			103,
			103
		})]
		
		protected internal virtual Collection compileGrammar()
		{
			this.initialGrammarState = this.grammar.getInitialNode();
			this.nodeStateMap = new HashMap();
			this.arcPool = new Cache();
			ArrayList arrayList = new ArrayList();
			TimerPool.getTimer(this, "Compile").start();
			TimerPool.getTimer(this, "Create States").start();
			Iterator iterator = this.grammar.getGrammarNodes().iterator();
			while (iterator.hasNext())
			{
				GrammarNode grammarNode = (GrammarNode)iterator.next();
				FlatLinguist.GState gstate = this.createGState(grammarNode);
				arrayList.add(gstate);
			}
			TimerPool.getTimer(this, "Create States").stop();
			this.addStartingPath();
			TimerPool.getTimer(this, "Collect Contexts").start();
			iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				FlatLinguist.GState gstate2 = (FlatLinguist.GState)iterator.next();
				gstate2.collectContexts();
			}
			TimerPool.getTimer(this, "Collect Contexts").stop();
			TimerPool.getTimer(this, "Expand States").start();
			iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				FlatLinguist.GState gstate2 = (FlatLinguist.GState)iterator.next();
				gstate2.expand();
			}
			TimerPool.getTimer(this, "Expand States").stop();
			TimerPool.getTimer(this, "Connect Nodes").start();
			iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				FlatLinguist.GState gstate2 = (FlatLinguist.GState)iterator.next();
				gstate2.connect();
			}
			TimerPool.getTimer(this, "Connect Nodes").stop();
			SentenceHMMState sentenceHMMState = this.findStartingState();
			if (this.addOutOfGrammarBranch)
			{
				CIPhoneLoop ciphoneLoop = new CIPhoneLoop(this.phoneLoopAcousticModel, this.logPhoneInsertionProbability);
				SentenceHMMState nextState = (SentenceHMMState)ciphoneLoop.getSearchGraph().getInitialState();
				sentenceHMMState.connect(this.getArc(nextState, 0f, this.logOutOfGrammarBranchProbability));
			}
			this.searchGraph = new FlatLinguist.FlatSearchGraph(this, sentenceHMMState);
			TimerPool.getTimer(this, "Compile").stop();
			if (this.dumpGStates)
			{
				Iterator iterator2 = this.grammar.getGrammarNodes().iterator();
				while (iterator2.hasNext())
				{
					GrammarNode node = (GrammarNode)iterator2.next();
					FlatLinguist.GState gstate3 = this.getGState(node);
					gstate3.dumpInfo();
				}
			}
			this.nodeStateMap = null;
			this.arcPool = null;
			return SentenceHMMState.collectStates(sentenceHMMState);
		}

		[LineNumberTable(new byte[]
		{
			161,
			68,
			116,
			43
		})]
		
		protected internal virtual bool grammarHasChanged()
		{
			return this.initialGrammarState == null || this.initialGrammarState != this.grammar.getInitialNode();
		}

		
		
		protected internal virtual FlatLinguist.GState createGState(GrammarNode grammarNode)
		{
			return new FlatLinguist.GState(this, grammarNode);
		}

		[LineNumberTable(new byte[]
		{
			161,
			46,
			113
		})]
		
		protected internal virtual void addStartingPath()
		{
			this.addStartingPath(this.grammar.getInitialNode());
		}

		[LineNumberTable(new byte[]
		{
			161,
			79,
			108,
			104
		})]
		
		protected internal virtual SentenceHMMState findStartingState()
		{
			GrammarNode initialNode = this.grammar.getInitialNode();
			FlatLinguist.GState gstate = this.getGState(initialNode);
			return gstate.getEntryPoint();
		}

		[LineNumberTable(new byte[]
		{
			161,
			96,
			179,
			114,
			120,
			127,
			5
		})]
		
		protected internal virtual SentenceHMMStateArc getArc(SentenceHMMState nextState, float logLanguageProbability, float logInsertionProbability)
		{
			SentenceHMMStateArc sentenceHMMStateArc = new SentenceHMMStateArc(nextState, logLanguageProbability * this.languageWeight, logInsertionProbability);
			SentenceHMMStateArc sentenceHMMStateArc2 = (SentenceHMMStateArc)this.arcPool.cache(sentenceHMMStateArc);
			this.actualArcs.value = (double)this.arcPool.getMisses();
			this.totalArcs.value = (double)(this.arcPool.getHits() + this.arcPool.getMisses());
			return (sentenceHMMStateArc2 != null) ? sentenceHMMStateArc2 : sentenceHMMStateArc;
		}

		
		
		protected internal virtual FlatLinguist.GState getGState(GrammarNode node)
		{
			return (FlatLinguist.GState)this.nodeStateMap.get(node);
		}

		[LineNumberTable(new byte[]
		{
			161,
			56,
			98,
			104,
			107
		})]
		
		protected internal virtual void addStartingPath(GrammarNode initialNode)
		{
			FlatLinguist.GState gstate = this.getGState(initialNode);
			FlatLinguist.GState.access_000(gstate, UnitContext.SILENCE);
		}

		public virtual SearchGraph getSearchGraph()
		{
			return this.searchGraph;
		}

		[LineNumberTable(new byte[]
		{
			159,
			97,
			108,
			233,
			24,
			231,
			106,
			103,
			107,
			103,
			135,
			116,
			116,
			116,
			116,
			137,
			103,
			103,
			135,
			135,
			99,
			116,
			116,
			168,
			103
		})]
		
		public FlatLinguist(AcousticModel acousticModel, Grammar grammar, UnitManager unitManager, double wordInsertionProbability, double silenceInsertionProbability, double fillerInsertionProbability, double unitInsertionProbability, float languageWeight, bool dumpGStates, bool showCompilationProgress, bool spreadWordProbabilitiesAcrossPronunciations, bool addOutOfGrammarBranch, double outOfGrammarBranchProbability, double phoneInsertionProbability, AcousticModel phoneLoopAcousticModel)
		{
			this.showCompilationProgress = true;
			this.acousticModel = acousticModel;
			this.logMath = LogMath.getLogMath();
			this.grammar = grammar;
			this.unitManager = unitManager;
			this.logWordInsertionProbability = this.logMath.linearToLog(wordInsertionProbability);
			this.logSilenceInsertionProbability = this.logMath.linearToLog(silenceInsertionProbability);
			this.logFillerInsertionProbability = this.logMath.linearToLog(fillerInsertionProbability);
			this.logUnitInsertionProbability = this.logMath.linearToLog(unitInsertionProbability);
			this.languageWeight = languageWeight;
			this.dumpGStates = dumpGStates;
			this.showCompilationProgress = showCompilationProgress;
			this.spreadWordProbabilitiesAcrossPronunciations = spreadWordProbabilitiesAcrossPronunciations;
			this.addOutOfGrammarBranch = addOutOfGrammarBranch;
			if (addOutOfGrammarBranch)
			{
				this.logOutOfGrammarBranchProbability = this.logMath.linearToLog(outOfGrammarBranchProbability);
				this.logPhoneInsertionProbability = this.logMath.linearToLog(phoneInsertionProbability);
				this.phoneLoopAcousticModel = phoneLoopAcousticModel;
			}
			this.name = null;
		}

		[LineNumberTable(new byte[]
		{
			160,
			95,
			232,
			159,
			188,
			231,
			160,
			70
		})]
		
		public FlatLinguist()
		{
			this.showCompilationProgress = true;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			105,
			139,
			118,
			118,
			182,
			124,
			124,
			124,
			124,
			113,
			118,
			118,
			150,
			150,
			104,
			124,
			124,
			182,
			108
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logMath = LogMath.getLogMath();
			this.acousticModel = (AcousticModel)ps.getComponent("acousticModel");
			this.grammar = (Grammar)ps.getComponent("grammar");
			this.unitManager = (UnitManager)ps.getComponent("unitManager");
			this.logWordInsertionProbability = this.logMath.linearToLog(ps.getDouble("wordInsertionProbability"));
			this.logSilenceInsertionProbability = this.logMath.linearToLog(ps.getDouble("silenceInsertionProbability"));
			this.logFillerInsertionProbability = this.logMath.linearToLog(ps.getDouble("fillerInsertionProbability"));
			this.logUnitInsertionProbability = this.logMath.linearToLog(ps.getDouble("unitInsertionProbability"));
			this.languageWeight = ps.getFloat("languageWeight");
			this.dumpGStates = ps.getBoolean("dumpGstates").booleanValue();
			this.showCompilationProgress = ps.getBoolean("showCompilationProgress").booleanValue();
			this.spreadWordProbabilitiesAcrossPronunciations = ps.getBoolean("spreadWordProbabilitiesAcrossPronunciations").booleanValue();
			this.addOutOfGrammarBranch = ps.getBoolean("addOutOfGrammarBranch").booleanValue();
			if (this.addOutOfGrammarBranch)
			{
				this.logOutOfGrammarBranchProbability = this.logMath.linearToLog(ps.getDouble("outOfGrammarProbability"));
				this.logPhoneInsertionProbability = this.logMath.linearToLog(ps.getDouble("phoneInsertionProbability"));
				this.phoneLoopAcousticModel = (AcousticModel)ps.getComponent("phoneLoopAcousticModel");
			}
			this.name = ps.getInstanceName();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			149,
			102,
			107,
			118,
			118,
			118,
			108,
			120
		})]
		
		public virtual void allocate()
		{
			this.allocateAcousticModel();
			this.grammar.allocate();
			this.totalStates = StatisticsVariable.getStatisticsVariable(this.getName(), "totalStates");
			this.totalArcs = StatisticsVariable.getStatisticsVariable(this.getName(), "totalArcs");
			this.actualArcs = StatisticsVariable.getStatisticsVariable(this.getName(), "actualArcs");
			this.stateSet = this.compileGrammar();
			this.totalStates.value = (double)this.stateSet.size();
		}

		[LineNumberTable(new byte[]
		{
			160,
			177,
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

		[LineNumberTable(new byte[]
		{
			160,
			188,
			104,
			108,
			152
		})]
		
		public virtual void startRecognition()
		{
			if (this.grammarHasChanged())
			{
				this.stateSet = this.compileGrammar();
				this.totalStates.value = (double)this.stateSet.size();
			}
		}

		public virtual void stopRecognition()
		{
		}

		public virtual float getLogSilenceInsertionProbability()
		{
			return this.logSilenceInsertionProbability;
		}

		
		
		
		internal static void access_100(FlatLinguist flatLinguist, string text)
		{
			flatLinguist.T(text);
		}

		
		
		internal static UnitManager access_200(FlatLinguist flatLinguist)
		{
			return flatLinguist.unitManager;
		}

		
		
		internal static float access_300(FlatLinguist flatLinguist)
		{
			return flatLinguist.logSilenceInsertionProbability;
		}

		
		
		internal static float access_400(FlatLinguist flatLinguist)
		{
			return flatLinguist.logFillerInsertionProbability;
		}

		
		
		internal static float access_500(FlatLinguist flatLinguist)
		{
			return flatLinguist.logWordInsertionProbability;
		}

		
		
		internal static float access_600(FlatLinguist flatLinguist)
		{
			return flatLinguist.logUnitInsertionProbability;
		}

		
		
		internal static AcousticModel access_700(FlatLinguist flatLinguist)
		{
			return flatLinguist.acousticModel;
		}

		
		
		internal static bool access_800(FlatLinguist flatLinguist)
		{
			return flatLinguist.spreadWordProbabilitiesAcrossPronunciations;
		}

		
		
		internal static bool access_900(FlatLinguist flatLinguist)
		{
			return flatLinguist.showCompilationProgress;
		}

		
		
		internal static int access_1008(FlatLinguist flatLinguist)
		{
			int num = flatLinguist.totalStateCounter;
			int result = num;
			flatLinguist.totalStateCounter = num + 1;
			return result;
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
				"Ledu/cmu/sphinx/linguist/acoustic/UnitManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_UNIT_MANAGER = "unitManager";

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

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_DUMP_GSTATES = "dumpGstates";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_ADD_OUT_OF_GRAMMAR_BRANCH = "addOutOfGrammarBranch";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_OUT_OF_GRAMMAR_PROBABILITY = "outOfGrammarProbability";

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
		public const string PROP_PHONE_LOOP_ACOUSTIC_MODEL = "phoneLoopAcousticModel";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_PHONE_INSERTION_PROBABILITY = "phoneInsertionProbability";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_COMPILATION_PROGRESS = "showCompilationProgress";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SPREAD_WORD_PROBABILITIES_ACROSS_PRONUNCIATIONS = "spreadWordProbabilitiesAcrossPronunciations";

		protected internal const float logOne = 0f;

		protected internal Grammar grammar;

		private AcousticModel acousticModel;

		private UnitManager unitManager;

		protected internal LogMath logMath;

		protected internal AcousticModel phoneLoopAcousticModel;

		protected internal bool addOutOfGrammarBranch;

		protected internal float logOutOfGrammarBranchProbability;

		protected internal float logPhoneInsertionProbability;

		private float logWordInsertionProbability;

		private float logSilenceInsertionProbability;

		private float logFillerInsertionProbability;

		private float logUnitInsertionProbability;

		private bool showCompilationProgress;

		private bool spreadWordProbabilitiesAcrossPronunciations;

		private bool dumpGStates;

		private float languageWeight;

		protected internal StatisticsVariable totalStates;

		protected internal StatisticsVariable totalArcs;

		protected internal StatisticsVariable actualArcs;

		[NonSerialized]
		private int totalStateCounter;

		private const bool tracing = false;

		
		[NonSerialized]
		private Collection stateSet;

		private string name;

		
		protected internal Map nodeStateMap;

		
		protected internal Cache arcPool;

		protected internal GrammarNode initialGrammarState;

		protected internal SearchGraph searchGraph;

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.linguist.SearchGraph"
		})]
		.
		public class FlatSearchGraph : java.lang.Object, SearchGraph
		{
			[LineNumberTable(new byte[]
			{
				161,
				133,
				111,
				103
			})]
			
			public FlatSearchGraph(FlatLinguist this_0, SearchState initialState)
			{
				this.initialState = initialState;
			}

			public virtual bool getWordTokenFirst()
			{
				return true;
			}

			public virtual SearchState getInitialState()
			{
				return this.initialState;
			}

			public virtual int getNumStateOrder()
			{
				return 7;
			}

			
			private SearchState initialState;

			
			internal FlatLinguist this_0 = this_0;
		}

		
		.
		public class GState : java.lang.Object
		{
			[LineNumberTable(new byte[]
			{
				162,
				116,
				102,
				102
			})]
			
			public virtual void collectContexts()
			{
				this.pullRightContexts();
				this.pushLeftContexts();
			}

			[LineNumberTable(new byte[]
			{
				162,
				127,
				127,
				1,
				127,
				1,
				105,
				115,
				98,
				162,
				109,
				114,
				127,
				10,
				106,
				98,
				210,
				127,
				1,
				103,
				231,
				74,
				127,
				10,
				110,
				110,
				115,
				124,
				106,
				106,
				133,
				102
			})]
			
			public virtual void expand()
			{
				Iterator iterator = this.leftContexts.iterator();
				while (iterator.hasNext())
				{
					UnitContext unitContext = (UnitContext)iterator.next();
					Iterator iterator2 = this.getStartingContexts().iterator();
					while (iterator2.hasNext())
					{
						UnitContext unitContext2 = (UnitContext)iterator2.next();
						ContextPair contextPair = ContextPair.get(unitContext, unitContext2);
						this.entryPoints.put(contextPair, new ArrayList());
					}
				}
				if (this.node.isFinalNode())
				{
					GrammarState.__<clinit>();
					GrammarState grammarState = new GrammarState(this.node);
					Iterator iterator3 = this.entryPoints.values().iterator();
					while (iterator3.hasNext())
					{
						List list = (List)iterator3.next();
						list.add(grammarState);
					}
				}
				else if (!this.node.isEmpty())
				{
					iterator = this.leftContexts.iterator();
					while (iterator.hasNext())
					{
						UnitContext unitContext = (UnitContext)iterator.next();
						this.expandWord(unitContext);
					}
				}
				else
				{
					iterator = this.entryPoints.entrySet().iterator();
					while (iterator.hasNext())
					{
						Map.Entry entry = (Map.Entry)iterator.next();
						ContextPair contextPair2 = (ContextPair)entry.getKey();
						List list2 = (List)entry.getValue();
						BranchState.__<clinit>();
						BranchState branchState = new BranchState(contextPair2.getLeftContext().toString(), contextPair2.getRightContext().toString(), this.node.getID());
						list2.add(branchState);
						this.addExitPoint(contextPair2, branchState);
					}
				}
				this.addEmptyEntryPoints();
			}

			[LineNumberTable(new byte[]
			{
				164,
				75,
				119,
				115,
				112,
				155,
				133,
				232,
				69,
				123,
				116,
				153,
				100,
				127,
				10,
				117,
				100,
				110,
				140,
				226,
				41,
				233,
				89
			})]
			
			public virtual void connect()
			{
				GrammarArc[] successors = this.getSuccessors();
				int num = successors.Length;
				for (int i = 0; i < num; i++)
				{
					GrammarArc grammarArc = successors[i];
					FlatLinguist.GState gstate = this.this_0.getGState(grammarArc.getGrammarNode());
					if (gstate.getNode().isEmpty() || !java.lang.String.instancehelper_equals(gstate.getNode().getWord().getSpelling(), "<s>"))
					{
						float num2 = grammarArc.getProbability();
						if (FlatLinguist.access_800(this.this_0) && !gstate.getNode().isEmpty())
						{
							int num3 = gstate.getNode().getWord().getPronunciations().Length;
							num2 -= this.this_0.logMath.linearToLog((double)num3);
						}
						float num4 = num2;
						Iterator iterator = this.exitPoints.entrySet().iterator();
						while (iterator.hasNext())
						{
							Map.Entry entry = (Map.Entry)iterator.next();
							List list = gstate.getEntryPoints((ContextPair)entry.getKey());
							if (list != null)
							{
								List list2 = (List)entry.getValue();
								this.connect(list2, list, num4);
							}
						}
					}
				}
			}

			[LineNumberTable(new byte[]
			{
				164,
				184,
				127,
				15,
				127,
				10,
				109,
				145,
				159,
				15,
				127,
				15,
				127,
				15,
				127,
				10,
				127,
				15,
				127,
				15,
				127,
				15,
				127,
				15,
				102
			})]
			
			internal virtual void dumpInfo()
			{
				java.lang.System.@out.println(new StringBuilder().append(" ==== ").append(this).append(" ========").toString());
				java.lang.System.@out.print(new StringBuilder().append("Node: ").append(this.node).toString());
				if (this.node.isEmpty())
				{
					java.lang.System.@out.print("  (Empty)");
				}
				else
				{
					java.lang.System.@out.print(new StringBuilder().append(" ").append(this.node.getWord()).toString());
				}
				java.lang.System.@out.print(new StringBuilder().append(" ep: ").append(this.entryPoints.size()).toString());
				java.lang.System.@out.print(new StringBuilder().append(" exit: ").append(this.exitPoints.size()).toString());
				java.lang.System.@out.print(new StringBuilder().append(" cons: ").append(this.exitConnections).toString());
				java.lang.System.@out.print(new StringBuilder().append(" tot: ").append(this.getStates().size()).toString());
				java.lang.System.@out.print(new StringBuilder().append(" sc: ").append(this.getStartingContexts().size()).toString());
				java.lang.System.@out.print(new StringBuilder().append(" rc: ").append(this.leftContexts.size()).toString());
				java.lang.System.@out.println(new StringBuilder().append(" lc: ").append(this.rightContexts.size()).toString());
				this.dumpDetails();
			}

			[LineNumberTable(new byte[]
			{
				161,
				187,
				239,
				45,
				107,
				107,
				203,
				107,
				235,
				77,
				103,
				110
			})]
			
			protected internal GState(FlatLinguist this_0, GrammarNode node)
			{
				this.entryPoints = new HashMap();
				this.exitPoints = new HashMap();
				this.existingStates = new HashMap();
				this.rightContexts = new HashSet();
				this.leftContexts = new HashSet();
				this.node = node;
				this_0.nodeStateMap.put(node, this);
			}

			
			
			
			internal static void access_000(FlatLinguist.GState gstate, UnitContext unitContext)
			{
				gstate.addLeftContext(unitContext);
			}

			[LineNumberTable(new byte[]
			{
				162,
				105,
				112,
				104
			})]
			
			public virtual SentenceHMMState getEntryPoint()
			{
				ContextPair contextPair = ContextPair.get(UnitContext.SILENCE, UnitContext.SILENCE);
				List list = this.getEntryPoints(contextPair);
				return (list != null && !list.isEmpty()) ? ((SentenceHMMState)list.get(0)) : null;
			}

			[LineNumberTable(new byte[]
			{
				162,
				80,
				109
			})]
			
			private void addLeftContext(UnitContext unitContext)
			{
				this.leftContexts.add(unitContext);
			}

			
			
			private GrammarArc[] getSuccessors()
			{
				return this.node.getSuccessors();
			}

			
			[LineNumberTable(new byte[]
			{
				161,
				200,
				107,
				235,
				69,
				109,
				103,
				112,
				116,
				19,
				198,
				130,
				109,
				105,
				119,
				106,
				14,
				232,
				70
			})]
			
			private Set getStartingContexts()
			{
				if (this.startingContexts == null)
				{
					this.startingContexts = new HashSet();
					if (this.node.isEmpty())
					{
						GrammarArc[] successors = this.getSuccessors();
						GrammarArc[] array = successors;
						int num = array.Length;
						for (int i = 0; i < num; i++)
						{
							GrammarArc grammarArc = array[i];
							FlatLinguist.GState gstate = this.this_0.getGState(grammarArc.getGrammarNode());
							this.startingContexts.addAll(gstate.getStartingContexts());
						}
					}
					else
					{
						Word word = this.node.getWord();
						Pronunciation[] pronunciations = word.getPronunciations();
						Pronunciation[] array2 = pronunciations;
						int i = array2.Length;
						for (int j = 0; j < i; j++)
						{
							Pronunciation pronunciation = array2[j];
							UnitContext startingContext = this.getStartingContext(pronunciation);
							this.startingContexts.add(startingContext);
						}
					}
				}
				return this.startingContexts;
			}

			[LineNumberTable(new byte[]
			{
				161,
				233,
				103,
				103,
				117
			})]
			
			private UnitContext getStartingContext(Pronunciation pronunciation)
			{
				int rightContextSize = this.getRightContextSize();
				Unit[] units = pronunciation.getUnits();
				Unit[] array = (units.Length <= rightContextSize) ? units : ((Unit[])Arrays.copyOf(units, rightContextSize));
				return UnitContext.get(array);
			}

			
			
			protected internal virtual int getRightContextSize()
			{
				return FlatLinguist.access_700(this.this_0).getRightContextSize();
			}

			
			
			protected internal virtual int getLeftContextSize()
			{
				return FlatLinguist.access_700(this.this_0).getLeftContextSize();
			}

			
			[LineNumberTable(new byte[]
			{
				161,
				246,
				102,
				112,
				103,
				108,
				103,
				120,
				105,
				101,
				125,
				238,
				60,
				235,
				71
			})]
			
			internal virtual Collection getEndingContexts()
			{
				ArrayList arrayList = new ArrayList();
				if (!this.node.isEmpty())
				{
					int leftContextSize = this.getLeftContextSize();
					Word word = this.node.getWord();
					Pronunciation[] pronunciations = word.getPronunciations();
					Pronunciation[] array = pronunciations;
					int num = array.Length;
					for (int i = 0; i < num; i++)
					{
						Pronunciation pronunciation = array[i];
						Unit[] units = pronunciation.getUnits();
						int num2 = units.Length;
						Unit[] array2 = (num2 <= leftContextSize) ? units : ((Unit[])Arrays.copyOfRange(units, num2 - leftContextSize, num2));
						arrayList.add(UnitContext.get(array2));
					}
				}
				return arrayList;
			}

			
			[LineNumberTable(new byte[]
			{
				162,
				47,
				110,
				129,
				141,
				116,
				115,
				168,
				110,
				233,
				58,
				230,
				73
			})]
			
			internal virtual void pushLeftContexts(Set set, Collection collection)
			{
				if (set.contains(this.getNode()))
				{
					return;
				}
				set.add(this.getNode());
				GrammarArc[] successors = this.getSuccessors();
				int num = successors.Length;
				for (int i = 0; i < num; i++)
				{
					GrammarArc grammarArc = successors[i];
					FlatLinguist.GState gstate = this.this_0.getGState(grammarArc.getGrammarNode());
					gstate.addLeftContext(collection);
					if (gstate.getNode().isEmpty())
					{
						gstate.pushLeftContexts(set, collection);
					}
				}
			}

			private GrammarNode getNode()
			{
				return this.node;
			}

			
			[LineNumberTable(new byte[]
			{
				162,
				70,
				109
			})]
			
			private void addLeftContext(Collection collection)
			{
				this.leftContexts.addAll(collection);
			}

			
			
			
			private List getEntryPoints(ContextPair contextPair)
			{
				return (List)this.entryPoints.get(contextPair);
			}

			[LineNumberTable(new byte[]
			{
				162,
				10,
				103,
				112,
				116,
				19,
				198
			})]
			
			private void pullRightContexts()
			{
				GrammarArc[] successors = this.getSuccessors();
				GrammarArc[] array = successors;
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					GrammarArc grammarArc = array[i];
					FlatLinguist.GState gstate = this.this_0.getGState(grammarArc.getGrammarNode());
					this.rightContexts.addAll(gstate.getStartingContexts());
				}
			}

			[LineNumberTable(new byte[]
			{
				162,
				33,
				103,
				102,
				104
			})]
			
			internal virtual void pushLeftContexts()
			{
				Collection endingContexts = this.getEndingContexts();
				HashSet hashSet = new HashSet();
				this.pushLeftContexts(hashSet, endingContexts);
			}

			[LineNumberTable(new byte[]
			{
				162,
				228,
				108,
				127,
				22,
				103,
				103,
				43,
				166
			})]
			
			private void expandWord(UnitContext unitContext)
			{
				Word word = this.node.getWord();
				FlatLinguist.access_100(this.this_0, new StringBuilder().append("  Expanding word ").append(word).append(" for lc ").append(unitContext).toString());
				Pronunciation[] pronunciations = word.getPronunciations();
				for (int i = 0; i < pronunciations.Length; i++)
				{
					this.expandPronunciation(unitContext, pronunciations[i], i);
				}
			}

			[LineNumberTable(new byte[]
			{
				163,
				128,
				114,
				104,
				102,
				147,
				125
			})]
			
			private void addExitPoint(ContextPair contextPair, SentenceHMMState sentenceHMMState)
			{
				object obj = (List)this.exitPoints.get(contextPair);
				if ((List)obj == null)
				{
					obj = new ArrayList();
					this.exitPoints.put(contextPair, (ArrayList)obj);
				}
				object obj2 = obj;
				List list;
				if (obj2 != null)
				{
					if ((list = (obj2 as List)) == null)
					{
						throw new IncompatibleClassChangeError();
					}
				}
				else
				{
					list = null;
				}
				list.add(sentenceHMMState);
			}

			[LineNumberTable(new byte[]
			{
				162,
				180,
				102,
				127,
				9,
				108,
				108,
				114,
				111,
				105,
				103,
				144,
				159,
				11,
				101,
				108
			})]
			
			private void addEmptyEntryPoints()
			{
				HashMap hashMap = new HashMap();
				Iterator iterator = this.entryPoints.entrySet().iterator();
				while (iterator.hasNext())
				{
					Map.Entry entry = (Map.Entry)iterator.next();
					ContextPair contextPair = (ContextPair)entry.getKey();
					if (this.needsEmptyVersion(contextPair))
					{
						ContextPair contextPair2 = ContextPair.get(contextPair.getLeftContext(), UnitContext.EMPTY);
						object obj = (List)hashMap.get(contextPair2);
						if ((List)obj == null)
						{
							obj = new ArrayList();
							hashMap.put(contextPair2, (ArrayList)obj);
						}
						object obj2 = obj;
						Collection collection = (Collection)entry.getValue();
						List list;
						if (obj2 != null)
						{
							if ((list = (obj2 as List)) == null)
							{
								throw new IncompatibleClassChangeError();
							}
						}
						else
						{
							list = null;
						}
						list.addAll(collection);
					}
				}
				this.entryPoints.putAll(hashMap);
			}

			[LineNumberTable(new byte[]
			{
				162,
				205,
				103,
				103
			})]
			
			private bool needsEmptyVersion(ContextPair contextPair)
			{
				UnitContext leftContext = contextPair.getLeftContext();
				Unit[] units = leftContext.getUnits();
				return units.Length > 0 && this.getRightContextSize(units[0]) < this.getRightContextSize();
			}

			
			
			private int getRightContextSize(Unit unit)
			{
				return (!unit.isFiller()) ? this.getRightContextSize() : 0;
			}

			[LineNumberTable(new byte[]
			{
				163,
				29,
				168,
				127,
				32,
				117,
				105,
				127,
				27,
				104,
				115,
				100,
				159,
				6,
				137,
				104,
				108,
				101,
				131,
				99,
				109,
				53,
				168,
				100,
				127,
				5,
				100,
				111,
				50,
				168,
				98
			})]
			
			private void expandPronunciation(UnitContext unitContext, Pronunciation pronunciation, int which)
			{
				UnitContext startingContext = this.getStartingContext(pronunciation);
				string name = new StringBuilder().append("P(").append(pronunciation.getWord()).append('[').append(unitContext).append(',').append(startingContext).append("])-G").append(this.getNode().getID()).toString();
				PronunciationState pronunciationState = new PronunciationState(name, pronunciation, which);
				FlatLinguist.access_100(this.this_0, new StringBuilder().append("     Expanding ").append(pronunciationState.getPronunciation()).append(" for lc ").append(unitContext).toString());
				ContextPair contextPair = ContextPair.get(unitContext, startingContext);
				List list = (List)this.entryPoints.get(contextPair);
				if (list == null)
				{
					string text = new StringBuilder().append("No EP list for context pair ").append(contextPair).toString();
					
					throw new Error(text);
				}
				list.add(pronunciationState);
				Unit[] units = pronunciation.getUnits();
				int num = units.Length - this.getRightContextSize();
				if (num < 0)
				{
					num = 0;
				}
				SentenceHMMState sentenceHMMState = pronunciationState;
				int num2 = 0;
				while (sentenceHMMState != null && num2 < num)
				{
					sentenceHMMState = this.attachUnit(pronunciationState, sentenceHMMState, units, num2, unitContext, UnitContext.EMPTY);
					num2++;
				}
				SentenceHMMState sentenceHMMState2 = sentenceHMMState;
				Iterator iterator = this.rightContexts.iterator();
				while (iterator.hasNext())
				{
					UnitContext unitContext2 = (UnitContext)iterator.next();
					sentenceHMMState = sentenceHMMState2;
					int num3 = num;
					while (sentenceHMMState != null && num3 < units.Length)
					{
						sentenceHMMState = this.attachUnit(pronunciationState, sentenceHMMState, units, num3, unitContext, unitContext2);
						num3++;
					}
				}
			}

			[LineNumberTable(new byte[]
			{
				163,
				77,
				108,
				108,
				103,
				104,
				127,
				6,
				140,
				110,
				111,
				110,
				111,
				105,
				143,
				237,
				69,
				106,
				100,
				144,
				130,
				112,
				136,
				234,
				69,
				105,
				110,
				138,
				137
			})]
			
			private SentenceHMMState attachUnit(PronunciationState parent, SentenceHMMState sentenceHMMState, Unit[] array, int num, UnitContext unitContext, UnitContext unitContext2)
			{
				Unit[] lc = this.getLC(unitContext, array, num);
				Unit[] rc = this.getRC(array, num, unitContext2);
				UnitContext unitContext3 = UnitContext.get(rc);
				LeftRightContext context = LeftRightContext.get(lc, rc);
				Unit unit = FlatLinguist.access_200(this.this_0).getUnit(array[num].getName(), array[num].isFiller(), context);
				ExtendedUnitState extendedUnitState = new ExtendedUnitState(parent, num, unit);
				float logInsertionProbablity;
				if (extendedUnitState.getUnit().isSilence())
				{
					logInsertionProbablity = FlatLinguist.access_300(this.this_0);
				}
				else if (extendedUnitState.getUnit().isFiller())
				{
					logInsertionProbablity = FlatLinguist.access_400(this.this_0);
				}
				else if (extendedUnitState.getWhich() == 0)
				{
					logInsertionProbablity = FlatLinguist.access_500(this.this_0);
				}
				else
				{
					logInsertionProbablity = FlatLinguist.access_600(this.this_0);
				}
				SentenceHMMState existingState = this.getExistingState(extendedUnitState);
				if (existingState != null)
				{
					this.attachState(sentenceHMMState, existingState, 0f, logInsertionProbablity);
					return null;
				}
				this.attachState(sentenceHMMState, extendedUnitState, 0f, logInsertionProbablity);
				this.addStateToCache(extendedUnitState);
				sentenceHMMState = this.expandUnit(extendedUnitState);
				if (extendedUnitState.isLast())
				{
					UnitContext unitContext4 = this.generateNextLeftContext(unitContext, array[num]);
					ContextPair contextPair = ContextPair.get(unitContext4, unitContext3);
					this.addExitPoint(contextPair, sentenceHMMState);
				}
				return sentenceHMMState;
			}

			[LineNumberTable(new byte[]
			{
				163,
				146,
				103,
				101,
				112,
				132,
				104,
				106,
				102,
				101,
				142,
				233,
				59,
				232,
				72
			})]
			
			private Unit[] getLC(UnitContext unitContext, Unit[] array, int num)
			{
				Unit[] units = unitContext.getUnits();
				int num2 = units.Length + num;
				int num3 = java.lang.Math.min(num2, this.getLeftContextSize(array[num]));
				int num4 = num - num3;
				Unit[] array2 = new Unit[num3];
				for (int i = 0; i < array2.Length; i++)
				{
					int num5 = num4 + i;
					if (num5 < 0)
					{
						array2[i] = units[units.Length + num5];
					}
					else
					{
						array2[i] = array[num5];
					}
				}
				return array2;
			}

			[LineNumberTable(new byte[]
			{
				163,
				173,
				103,
				100,
				104,
				144,
				104,
				106,
				102,
				102,
				139,
				236,
				59,
				232,
				72
			})]
			
			private Unit[] getRC(Unit[] array, int num, UnitContext unitContext)
			{
				Unit[] units = unitContext.getUnits();
				int num2 = num + 1;
				int num3 = array.Length - num2 + units.Length;
				int num4 = java.lang.Math.min(num3, this.getRightContextSize(array[num]));
				Unit[] array2 = new Unit[num4];
				for (int i = 0; i < array2.Length; i++)
				{
					int num5 = num2 + i;
					if (num5 < array.Length)
					{
						array2[i] = array[num5];
					}
					else
					{
						array2[i] = units[num5 - array.Length];
					}
				}
				return array2;
			}

			
			
			private SentenceHMMState getExistingState(SentenceHMMState sentenceHMMState)
			{
				return (SentenceHMMState)this.existingStates.get(sentenceHMMState.getSignature());
			}

			[LineNumberTable(new byte[]
			{
				164,
				134,
				151,
				127,
				10,
				143
			})]
			
			protected internal virtual void attachState(SentenceHMMState prevState, SentenceHMMState nextState, float logLanguageProbablity, float logInsertionProbablity)
			{
				prevState.connect(this.this_0.getArc(nextState, logLanguageProbablity, logInsertionProbablity));
				if (FlatLinguist.access_900(this.this_0))
				{
					bool flag = FlatLinguist.access_1008(this.this_0) != 0;
					int num = 1000;
					if (num == -1 || (flag ? 1 : 0) % num == 0)
					{
						java.lang.System.@out.print(".");
					}
				}
			}

			[LineNumberTable(new byte[]
			{
				164,
				176,
				115
			})]
			
			private void addStateToCache(SentenceHMMState sentenceHMMState)
			{
				this.existingStates.put(sentenceHMMState.getSignature(), sentenceHMMState);
			}

			[LineNumberTable(new byte[]
			{
				164,
				1,
				168,
				141,
				152
			})]
			
			protected internal virtual SentenceHMMState expandUnit(UnitState unit)
			{
				HMMStateState hmmstates = this.getHMMStates(unit);
				if (unit.getUnit().isSilence())
				{
					this.attachState(hmmstates, unit, 0f, FlatLinguist.access_300(this.this_0));
				}
				return hmmstates;
			}

			[LineNumberTable(new byte[]
			{
				163,
				239,
				103,
				110,
				99,
				102,
				112,
				102
			})]
			
			internal virtual UnitContext generateNextLeftContext(UnitContext unitContext, Unit unit)
			{
				Unit[] units = unitContext.getUnits();
				int num = java.lang.Math.min(units.Length, this.getLeftContextSize());
				if (num == 0)
				{
					return UnitContext.EMPTY;
				}
				Unit[] array = (Unit[])Arrays.copyOfRange(units, 1, num + 1);
				array[num - 1] = unit;
				return UnitContext.get(array);
			}

			
			
			private int getLeftContextSize(Unit unit)
			{
				return (!unit.isFiller()) ? this.getLeftContextSize() : 0;
			}

			[LineNumberTable(new byte[]
			{
				164,
				21,
				103,
				103,
				116,
				103,
				105,
				115,
				104,
				107
			})]
			
			private HMMStateState getHMMStates(UnitState unitState)
			{
				Unit unit = unitState.getUnit();
				HMMPosition position = unitState.getPosition();
				HMM hmm = FlatLinguist.access_700(this.this_0).lookupNearestHMM(unit, position, false);
				HMMState initialState = hmm.getInitialState();
				HMMStateState hmmstateState = new HMMStateState(unitState, initialState);
				this.attachState(unitState, hmmstateState, 0f, 0f);
				this.addStateToCache(hmmstateState);
				return this.expandHMMTree(unitState, hmmstateState);
			}

			[LineNumberTable(new byte[]
			{
				164,
				41,
				98,
				157,
				110,
				150,
				148,
				106,
				105,
				100,
				146,
				112,
				104,
				234,
				50,
				233,
				81
			})]
			
			private HMMStateState expandHMMTree(UnitState unitState, HMMStateState hmmstateState)
			{
				HMMStateState result = hmmstateState;
				HMMStateArc[] successors = hmmstateState.getHMMState().getSuccessors();
				int num = successors.Length;
				for (int i = 0; i < num; i++)
				{
					HMMStateArc hmmstateArc = successors[i];
					HMMStateState hmmstateState2;
					if (hmmstateArc.getHMMState().isEmitting())
					{
						HMMStateState.__<clinit>();
						hmmstateState2 = new HMMStateState(unitState, hmmstateArc.getHMMState());
					}
					else
					{
						NonEmittingHMMState.__<clinit>();
						hmmstateState2 = new NonEmittingHMMState(unitState, hmmstateArc.getHMMState());
					}
					SentenceHMMState existingState = this.getExistingState(hmmstateState2);
					float logProbability = hmmstateArc.getLogProbability();
					if (existingState != null)
					{
						this.attachState(hmmstateState, existingState, 0f, logProbability);
					}
					else
					{
						this.attachState(hmmstateState, hmmstateState2, 0f, logProbability);
						this.addStateToCache(hmmstateState2);
						result = this.expandHMMTree(unitState, hmmstateState2);
					}
				}
				return result;
			}

			
			[LineNumberTable(new byte[]
			{
				164,
				111,
				126,
				103,
				124,
				105,
				122,
				110,
				98,
				101
			})]
			
			private void connect(List list, List list2, float num)
			{
				Iterator iterator = list.iterator();
				while (iterator.hasNext())
				{
					SearchState searchState = (SearchState)iterator.next();
					SentenceHMMState sentenceHMMState = (SentenceHMMState)searchState;
					Iterator iterator2 = list2.iterator();
					while (iterator2.hasNext())
					{
						SearchState searchState2 = (SearchState)iterator2.next();
						SentenceHMMState nextState = (SentenceHMMState)searchState2;
						sentenceHMMState.connect(this.this_0.getArc(nextState, num, 0f));
						this.exitConnections++;
					}
				}
			}

			
			[LineNumberTable(new byte[]
			{
				164,
				151,
				113,
				127,
				6,
				104,
				98
			})]
			
			public virtual Collection getStates()
			{
				ArrayList arrayList = new ArrayList(this.existingStates.values());
				Iterator iterator = this.entryPoints.values().iterator();
				while (iterator.hasNext())
				{
					List list = (List)iterator.next();
					arrayList.addAll(list);
				}
				return arrayList;
			}

			[LineNumberTable(new byte[]
			{
				164,
				206,
				118,
				118,
				118,
				118,
				102,
				113,
				113,
				113,
				113,
				118
			})]
			
			internal virtual void dumpDetails()
			{
				this.dumpCollection(" entryPoints", this.entryPoints.keySet());
				this.dumpCollection(" entryPoints states", this.entryPoints.values());
				this.dumpCollection(" exitPoints", this.exitPoints.keySet());
				this.dumpCollection(" exitPoints states", this.exitPoints.values());
				this.dumpNextNodes();
				this.dumpExitPoints(this.exitPoints.values());
				this.dumpCollection(" startingContexts", this.getStartingContexts());
				this.dumpCollection(" branchingInFrom", this.leftContexts);
				this.dumpCollection(" branchingOutTo", this.rightContexts);
				this.dumpCollection(" existingStates", this.existingStates.keySet());
			}

			
			[LineNumberTable(new byte[]
			{
				164,
				254,
				127,
				5,
				118,
				127,
				5,
				98
			})]
			
			private void dumpCollection(string text, Collection collection)
			{
				java.lang.System.@out.println(new StringBuilder().append("     ").append(text).toString());
				Iterator iterator = collection.iterator();
				while (iterator.hasNext())
				{
					object obj = iterator.next();
					java.lang.System.@out.println(new StringBuilder().append("         ").append(obj).toString());
				}
			}

			[LineNumberTable(new byte[]
			{
				164,
				223,
				111,
				121,
				63,
				10,
				166
			})]
			
			private void dumpNextNodes()
			{
				java.lang.System.@out.println("     Next Grammar Nodes: ");
				GrammarArc[] successors = this.node.getSuccessors();
				int num = successors.Length;
				for (int i = 0; i < num; i++)
				{
					GrammarArc grammarArc = successors[i];
					java.lang.System.@out.println(new StringBuilder().append("          ").append(grammarArc.getGrammarNode()).toString());
				}
			}

			
			[LineNumberTable(new byte[]
			{
				164,
				236,
				126,
				126,
				127,
				5,
				125,
				63,
				11,
				168,
				101,
				101
			})]
			
			private void dumpExitPoints(Collection collection)
			{
				Iterator iterator = collection.iterator();
				while (iterator.hasNext())
				{
					List list = (List)iterator.next();
					Iterator iterator2 = list.iterator();
					while (iterator2.hasNext())
					{
						SearchState searchState = (SearchState)iterator2.next();
						java.lang.System.@out.println(new StringBuilder().append("      Arcs from: ").append(searchState).toString());
						SearchStateArc[] successors = searchState.getSuccessors();
						int num = successors.Length;
						for (int i = 0; i < num; i++)
						{
							SearchStateArc searchStateArc = successors[i];
							java.lang.System.@out.println(new StringBuilder().append("          ").append(searchStateArc.getState()).toString());
						}
					}
				}
			}

			[LineNumberTable(new byte[]
			{
				165,
				11,
				109,
				159,
				11
			})]
			
			public override string toString()
			{
				if (this.node.isEmpty())
				{
					return new StringBuilder().append("GState ").append(this.node).append("(empty)").toString();
				}
				return new StringBuilder().append("GState ").append(this.node).append(" word ").append(this.node.getWord()).toString();
			}

			
			
			private Map entryPoints;

			
			
			private Map exitPoints;

			
			
			private Map existingStates;

			
			private GrammarNode node;

			
			
			private Set rightContexts;

			
			
			private Set leftContexts;

			
			private Set startingContexts;

			private int exitConnections;

			
			internal FlatLinguist this_0 = this_0;
		}
	}
}

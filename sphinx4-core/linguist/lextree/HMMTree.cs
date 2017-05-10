using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.ngram;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.lextree
{
	
	internal sealed class HMMTree : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		
		
		internal static Set access$000(HMMTree hmmtree)
		{
			return hmmtree.exitPoints;
		}

		
		
		internal static HMMPool access$100(HMMTree hmmtree)
		{
			return hmmtree.hmmPool;
		}

		
		
		internal static Set access$200(HMMTree hmmtree)
		{
			return hmmtree.entryPoints;
		}

		
		
		internal static Dictionary access$300(HMMTree hmmtree)
		{
			return hmmtree.dictionary;
		}

		
		
		internal static InitialWordNode access$402(HMMTree hmmtree, InitialWordNode result)
		{
			hmmtree.initialNode = result;
			return result;
		}

		
		
		
		internal static float access$500(HMMTree hmmtree, Word word)
		{
			return hmmtree.getWordUnigramProbability(word);
		}

		
		
		internal static Map access$600(HMMTree hmmtree)
		{
			return hmmtree.wordNodeMap;
		}

		
		
		internal static WordNode access$702(HMMTree hmmtree, WordNode result)
		{
			hmmtree.sentenceEndWordNode = result;
			return result;
		}

		[LineNumberTable(new byte[]
		{
			163,
			50,
			102,
			104,
			103,
			100,
			146,
			138
		})]
		
		private float getWordUnigramProbability(Word word)
		{
			float num = 0f;
			if (!word.isFiller())
			{
				Word[] words = new Word[]
				{
					word
				};
				num = this.lm.getProbability(new WordSequence(words));
				num *= this.languageWeight;
			}
			return num;
		}

		[LineNumberTable(new byte[]
		{
			162,
			155,
			102,
			114,
			102,
			107,
			102
		})]
		
		private void compile()
		{
			this.collectEntryAndExitUnits();
			this.entryPointTable = new HMMTree.EntryPointTable(this, this.entryPoints);
			this.addWords();
			this.entryPointTable.createEntryPointMaps();
			this.freeze();
		}

		[LineNumberTable(new byte[]
		{
			162,
			194,
			103,
			126,
			108,
			106,
			107,
			116,
			110,
			238,
			59,
			230,
			71,
			133,
			104,
			127,
			15,
			159,
			15
		})]
		
		private void collectEntryAndExitUnits()
		{
			Set set = this.getAllWords();
			Iterator iterator = set.iterator();
			while (iterator.hasNext())
			{
				Word word = (Word)iterator.next();
				for (int i = 0; i < word.getPronunciations().Length; i++)
				{
					Pronunciation pronunciation = word.getPronunciations()[i];
					Unit unit = pronunciation.getUnits()[0];
					Unit unit2 = pronunciation.getUnits()[pronunciation.getUnits().Length - 1];
					this.entryPoints.add(unit);
					this.exitPoints.add(unit2);
				}
			}
			if (this.debug)
			{
				java.lang.System.@out.println(new StringBuilder().append("Entry Points: ").append(this.entryPoints.size()).toString());
				java.lang.System.@out.println(new StringBuilder().append("Exit Points: ").append(this.exitPoints.size()).toString());
			}
		}

		[LineNumberTable(new byte[]
		{
			162,
			229,
			103,
			123,
			103,
			98
		})]
		
		private void addWords()
		{
			Set set = this.getAllWords();
			Iterator iterator = set.iterator();
			while (iterator.hasNext())
			{
				Word word = (Word)iterator.next();
				this.addWord(word);
			}
		}

		[LineNumberTable(new byte[]
		{
			162,
			217,
			107,
			103,
			103,
			103,
			103,
			107,
			107
		})]
		
		private void freeze()
		{
			this.entryPointTable.freeze();
			this.dictionary = null;
			this.lm = null;
			this.exitPoints = null;
			this.allWords = null;
			this.wordNodeMap.clear();
			this.endNodeMap.clear();
		}

		internal InitialWordNode getInitialNode()
		{
			return this.initialNode;
		}

		
		[LineNumberTable(new byte[]
		{
			162,
			180,
			108,
			105,
			127,
			6,
			104,
			116,
			43,
			230,
			69
		})]
		
		private void dumpTree(int num, Node node, Map map)
		{
			if (map.get(node) == null)
			{
				map.put(node, node);
				java.lang.System.@out.println(new StringBuilder().append(Utilities.pad(num)).append(node).toString());
				if (!(node is WordNode))
				{
					Node[] successors = node.getSuccessors();
					int num2 = successors.Length;
					for (int i = 0; i < num2; i++)
					{
						Node node2 = successors[i];
						this.dumpTree(num + 1, node2, map);
					}
				}
			}
		}

		
		[LineNumberTable(new byte[]
		{
			163,
			68,
			107,
			107,
			127,
			6,
			109,
			99,
			141,
			130,
			104,
			158,
			183
		})]
		
		private Set getAllWords()
		{
			if (this.allWords == null)
			{
				this.allWords = new HashSet();
				Iterator iterator = this.lm.getVocabulary().iterator();
				while (iterator.hasNext())
				{
					string str = (string)iterator.next();
					Word word = this.dictionary.getWord(str);
					if (word != null)
					{
						this.allWords.add(word);
					}
				}
				if (this.addFillerWords)
				{
					this.allWords.addAll(Arrays.asList(this.dictionary.getFillerWords()));
				}
				else
				{
					this.allWords.add(this.dictionary.getSilenceWord());
				}
			}
			return this.allWords;
		}

		[LineNumberTable(new byte[]
		{
			162,
			242,
			104,
			103,
			115,
			41,
			168
		})]
		
		private void addWord(Word word)
		{
			float wordUnigramProbability = this.getWordUnigramProbability(word);
			Pronunciation[] pronunciations = word.getPronunciations();
			Pronunciation[] array = pronunciations;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Pronunciation pronunciation = array[i];
				this.addPronunciation(pronunciation, wordUnigramProbability);
			}
		}

		[LineNumberTable(new byte[]
		{
			163,
			8,
			103,
			100,
			141,
			136,
			104,
			103,
			99,
			110,
			101,
			104,
			119,
			100,
			159,
			57,
			139,
			227,
			55,
			235,
			77,
			103,
			108,
			108,
			114,
			110,
			136,
			98,
			135
		})]
		
		private void addPronunciation(Pronunciation pronunciation, float num)
		{
			Unit[] units = pronunciation.getUnits();
			Unit unit = units[0];
			HMMTree.EntryPoint entryPoint = this.entryPointTable.getEntryPoint(unit);
			entryPoint.addProbability(num);
			if (units.Length > 1)
			{
				Node node = entryPoint.getNode();
				Unit unit2 = unit;
				for (int i = 1; i < units.Length - 1; i++)
				{
					unit = units[i];
					Unit unit3 = units[i + 1];
					HMM hmm = this.hmmPool.getHMM(unit, unit2, unit3, HMMPosition.__INTERNAL);
					if (hmm == null)
					{
						this.logger.severe(new StringBuilder().append("Missing HMM for unit ").append(unit.getName()).append(" with lc=").append(unit2.getName()).append(" rc=").append(unit3.getName()).toString());
					}
					else
					{
						node = node.addSuccessor(hmm, num);
					}
					unit2 = unit;
				}
				unit = units[units.Length - 1];
				EndNode endNode = new EndNode(unit, unit2, num);
				EndNode endNode2 = node.addSuccessor(endNode, num);
				WordNode wordNode = endNode2.addSuccessor(pronunciation, num, this.wordNodeMap);
				if (wordNode.getWord().isSentenceEndWord())
				{
					this.sentenceEndWordNode = wordNode;
				}
			}
			else
			{
				entryPoint.addSingleUnitWord(pronunciation);
			}
		}

		[LineNumberTable(new byte[]
		{
			158,
			224,
			67,
			232,
			39,
			103,
			107,
			235,
			88,
			103,
			103,
			103,
			107,
			107,
			103,
			137,
			117,
			102
		})]
		
		internal HMMTree(HMMPool hmmpool, Dictionary dictionary, LanguageModel languageModel, bool flag, float num)
		{
			this.addSilenceWord = true;
			this.entryPoints = new HashSet();
			this.exitPoints = new HashSet();
			this.hmmPool = hmmpool;
			this.dictionary = dictionary;
			this.lm = languageModel;
			this.endNodeMap = new HashMap();
			this.wordNodeMap = new HashMap();
			this.addFillerWords = flag;
			this.languageWeight = num;
			this.logger = Logger.getLogger(ClassLiteral<HMMTree>.Value.getSimpleName());
			this.compile();
		}

		[LineNumberTable(new byte[]
		{
			162,
			92,
			109
		})]
		
		public Node[] getEntryPoint(Unit unit, Unit unit2)
		{
			HMMTree.EntryPoint entryPoint = this.entryPointTable.getEntryPoint(unit2);
			return entryPoint.getEntryPointsFromLeftContext(unit).getSuccessors();
		}

		[LineNumberTable(new byte[]
		{
			162,
			104,
			119,
			166,
			102,
			103,
			103,
			127,
			8,
			118,
			111,
			100,
			110,
			139,
			105,
			125,
			105,
			9,
			200,
			165,
			124,
			211
		})]
		
		public HMMNode[] getHMMNodes(EndNode endNode)
		{
			HMMNode[] array = (HMMNode[])this.endNodeMap.get(endNode.getKey());
			if (array == null)
			{
				HashMap hashMap = new HashMap();
				Unit baseUnit = endNode.getBaseUnit();
				Unit leftContext = endNode.getLeftContext();
				Iterator iterator = this.entryPoints.iterator();
				while (iterator.hasNext())
				{
					Unit unit = (Unit)iterator.next();
					HMM hmm = this.hmmPool.getHMM(baseUnit, leftContext, unit, HMMPosition.__END);
					HMMNode hmmnode = (HMMNode)hashMap.get(hmm);
					if (hmmnode == null)
					{
						hmmnode = new HMMNode(hmm, 0f);
						hashMap.put(hmm, hmmnode);
					}
					hmmnode.addRC(unit);
					Node[] successors = endNode.getSuccessors();
					int num = successors.Length;
					for (int i = 0; i < num; i++)
					{
						Node node = successors[i];
						WordNode wordNode = (WordNode)node;
						hmmnode.addSuccessor(wordNode);
					}
				}
				array = (HMMNode[])hashMap.values().toArray(new HMMNode[hashMap.size()]);
				this.endNodeMap.put(endNode.getKey(), array);
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			162,
			141,
			122
		})]
		
		public WordNode getSentenceEndWordNode()
		{
			if (!HMMTree.assertionsDisabled && this.sentenceEndWordNode == null)
			{
				
				throw new AssertionError();
			}
			return this.sentenceEndWordNode;
		}

		[LineNumberTable(new byte[]
		{
			162,
			165,
			111,
			102,
			110,
			111
		})]
		
		internal void dumpTree()
		{
			java.lang.System.@out.println("Dumping Tree ...");
			HashMap hashMap = new HashMap();
			this.dumpTree(0, this.getInitialNode(), hashMap);
			java.lang.System.@out.println("... done Dumping Tree");
		}

		
		static HMMTree()
		{
		}

		
		private HMMPool hmmPool;

		private InitialWordNode initialNode;

		private Dictionary dictionary;

		private LanguageModel lm;

		
		private bool addFillerWords;

		
		private bool addSilenceWord;

		
		
		private Set entryPoints;

		
		private Set exitPoints;

		
		private Set allWords;

		private HMMTree.EntryPointTable entryPointTable;

		private bool debug;

		
		private float languageWeight;

		
		
		private Map endNodeMap;

		
		
		private Map wordNodeMap;

		private WordNode sentenceEndWordNode;

		private Logger logger;

		
		internal static bool assertionsDisabled = !ClassLiteral<HMMTree>.Value.desiredAssertionStatus();

		
		[SourceFile("HMMTree.java")]
		
		internal sealed class EntryPoint : java.lang.Object
		{
			
			[LineNumberTable(new byte[]
			{
				163,
				247,
				104,
				107,
				127,
				11,
				103,
				114,
				130
			})]
			
			private Collection getEntryPointRC()
			{
				if (this.rcSet == null)
				{
					this.rcSet = new HashSet();
					Iterator iterator = this.baseNode.getSuccessorMap().values().iterator();
					while (iterator.hasNext())
					{
						Node node = (Node)iterator.next();
						UnitNode unitNode = (UnitNode)node;
						this.rcSet.add(unitNode.getBaseUnit());
					}
				}
				return this.rcSet;
			}

			internal float getProbability()
			{
				return this.totalProbability;
			}

			[LineNumberTable(new byte[]
			{
				164,
				82,
				121,
				104,
				106,
				233,
				61,
				230,
				70
			})]
			
			private void connectEntryPointNode(Node node, Unit unit)
			{
				Node[] successors = this.baseNode.getSuccessors();
				int num = successors.Length;
				for (int i = 0; i < num; i++)
				{
					Node node2 = successors[i];
					UnitNode unitNode = (UnitNode)node2;
					if (unitNode.getBaseUnit() == unit)
					{
						node.addSuccessor(unitNode);
					}
				}
			}

			
			[LineNumberTable(new byte[]
			{
				164,
				40,
				144,
				127,
				9,
				190,
				112,
				99,
				112,
				139,
				168,
				103,
				142,
				127,
				8,
				121,
				150,
				116,
				119,
				109,
				108,
				174,
				110,
				101,
				133
			})]
			
			private void connectSingleUnitWords(Unit lc, Node node, HashMap hashMap)
			{
				if (!this.singleUnitWords.isEmpty())
				{
					Iterator iterator = HMMTree.access$200(this.this$0).iterator();
					while (iterator.hasNext())
					{
						Unit unit = (Unit)iterator.next();
						HMM hmm = HMMTree.access$100(this.this$0).getHMM(this.baseUnit, lc, unit, HMMPosition.__SINGLE);
						HMMNode hmmnode;
						if ((hmmnode = (HMMNode)hashMap.get(hmm)) == null)
						{
							hmmnode = (HMMNode)node.addSuccessor(hmm, this.getProbability());
							hashMap.put(hmm, hmmnode);
						}
						else
						{
							node.putSuccessor(hmm, hmmnode);
						}
						hmmnode.addRC(unit);
						this.nodeCount++;
						Iterator iterator2 = this.singleUnitWords.iterator();
						while (iterator2.hasNext())
						{
							Pronunciation pronunciation = (Pronunciation)iterator2.next();
							if (pronunciation.getWord() == HMMTree.access$300(this.this$0).getSentenceStartWord())
							{
								HMMTree.access$402(this.this$0, new InitialWordNode(pronunciation, hmmnode));
							}
							else
							{
								float num = HMMTree.access$500(this.this$0, pronunciation.getWord());
								WordNode wordNode = hmmnode.addSuccessor(pronunciation, num, HMMTree.access$600(this.this$0));
								if (pronunciation.getWord() == HMMTree.access$300(this.this$0).getSentenceEndWord())
								{
									HMMTree.access$702(this.this$0, wordNode);
								}
							}
							this.nodeCount++;
						}
					}
				}
			}

			[LineNumberTable(new byte[]
			{
				163,
				169,
				111,
				103,
				112,
				107,
				107,
				107
			})]
			
			internal EntryPoint(HMMTree hmmtree, Unit unit)
			{
				this.baseUnit = unit;
				this.baseNode = new Node(float.MinValue);
				this.unitToEntryPointMap = new HashMap();
				this.singleUnitWords = new ArrayList();
				this.totalProbability = float.MinValue;
			}

			
			
			internal Node getEntryPointsFromLeftContext(Unit unit)
			{
				return (Node)this.unitToEntryPointMap.get(unit);
			}

			internal void addProbability(float num)
			{
				if (num > this.totalProbability)
				{
					this.totalProbability = num;
				}
			}

			[LineNumberTable(new byte[]
			{
				163,
				213,
				127,
				6,
				102,
				98,
				103,
				103
			})]
			
			internal void freeze()
			{
				Iterator iterator = this.unitToEntryPointMap.values().iterator();
				while (iterator.hasNext())
				{
					Node node = (Node)iterator.next();
					node.freeze();
				}
				this.singleUnitWords = null;
				this.rcSet = null;
			}

			internal Node getNode()
			{
				return this.baseNode;
			}

			[LineNumberTable(new byte[]
			{
				163,
				237,
				109
			})]
			
			internal void addSingleUnitWord(Pronunciation pronunciation)
			{
				this.singleUnitWords.add(pronunciation);
			}

			[LineNumberTable(new byte[]
			{
				164,
				6,
				102,
				134,
				127,
				9,
				108,
				127,
				8,
				191,
				1,
				114,
				113,
				141,
				171,
				110,
				106,
				101,
				106,
				111,
				101
			})]
			
			internal void createEntryPointMap()
			{
				HashMap hashMap = new HashMap();
				HashMap hashMap2 = new HashMap();
				Iterator iterator = HMMTree.access$000(this.this$0).iterator();
				while (iterator.hasNext())
				{
					Unit unit = (Unit)iterator.next();
					Node node = new Node(float.MinValue);
					Iterator iterator2 = this.getEntryPointRC().iterator();
					while (iterator2.hasNext())
					{
						Unit unit2 = (Unit)iterator2.next();
						HMM hmm = HMMTree.access$100(this.this$0).getHMM(this.baseUnit, unit, unit2, HMMPosition.__BEGIN);
						Node node2;
						if ((node2 = (Node)hashMap.get(hmm)) == null)
						{
							node2 = node.addSuccessor(hmm, this.getProbability());
							hashMap.put(hmm, node2);
						}
						else
						{
							node.putSuccessor(hmm, node2);
						}
						this.nodeCount++;
						this.connectEntryPointNode(node2, unit2);
					}
					this.connectSingleUnitWords(unit, node, hashMap2);
					this.unitToEntryPointMap.put(unit, node);
				}
			}

			[LineNumberTable(new byte[]
			{
				164,
				93,
				127,
				11,
				52,
				133,
				98,
				103,
				111,
				123,
				118,
				105,
				98,
				106,
				143,
				98,
				106
			})]
			
			internal void dump()
			{
				java.lang.System.@out.println(new StringBuilder().append("EntryPoint ").append(this.baseUnit).append(" RC Followers: ").append(this.getEntryPointRC().size()).toString());
				int num = 0;
				Collection entryPointRC = this.getEntryPointRC();
				java.lang.System.@out.print("    ");
				Iterator iterator = entryPointRC.iterator();
				while (iterator.hasNext())
				{
					Unit unit = (Unit)iterator.next();
					java.lang.System.@out.print(Utilities.pad(unit.getName(), 4));
					int num2 = num;
					num++;
					if (num2 >= 12)
					{
						num = 0;
						java.lang.System.@out.println();
						java.lang.System.@out.print("    ");
					}
				}
				java.lang.System.@out.println();
			}

			
			internal Unit baseUnit;

			
			internal Node baseNode;

			
			
			internal Map unitToEntryPointMap;

			
			internal List singleUnitWords;

			internal int nodeCount;

			
			internal Set rcSet;

			internal float totalProbability;

			
			internal HMMTree this$0 = hmmtree;
		}

		
		[SourceFile("HMMTree.java")]
		
		internal sealed class EntryPointTable : java.lang.Object
		{
			
			
			internal HMMTree.EntryPoint getEntryPoint(Unit unit)
			{
				return (HMMTree.EntryPoint)this.entryPoints.get(unit);
			}

			
			[LineNumberTable(new byte[]
			{
				163,
				108,
				111,
				107,
				123,
				116,
				98
			})]
			
			internal EntryPointTable(HMMTree hmmtree, Collection collection)
			{
				this.entryPoints = new HashMap();
				Iterator iterator = collection.iterator();
				while (iterator.hasNext())
				{
					Unit unit = (Unit)iterator.next();
					this.entryPoints.put(unit, new HMMTree.EntryPoint(hmmtree, unit));
				}
			}

			[LineNumberTable(new byte[]
			{
				163,
				129,
				127,
				6,
				102,
				98
			})]
			
			internal void createEntryPointMaps()
			{
				Iterator iterator = this.entryPoints.values().iterator();
				while (iterator.hasNext())
				{
					HMMTree.EntryPoint entryPoint = (HMMTree.EntryPoint)iterator.next();
					entryPoint.createEntryPointMap();
				}
			}

			[LineNumberTable(new byte[]
			{
				163,
				137,
				127,
				6,
				102,
				98
			})]
			
			internal void freeze()
			{
				Iterator iterator = this.entryPoints.values().iterator();
				while (iterator.hasNext())
				{
					HMMTree.EntryPoint entryPoint = (HMMTree.EntryPoint)iterator.next();
					entryPoint.freeze();
				}
			}

			[LineNumberTable(new byte[]
			{
				163,
				145,
				127,
				6,
				102,
				98
			})]
			
			internal void dump()
			{
				Iterator iterator = this.entryPoints.values().iterator();
				while (iterator.hasNext())
				{
					HMMTree.EntryPoint entryPoint = (HMMTree.EntryPoint)iterator.next();
					entryPoint.dump();
				}
			}

			
			
			private Map entryPoints;

			
			internal HMMTree this$0 = hmmtree;
		}
	}
}

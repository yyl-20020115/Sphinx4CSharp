using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.ngram;
using edu.cmu.sphinx.util;
using ikvm.@internal;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.lextree
{
	internal sealed class HMMTree : Object
	{
		internal static Set access_000(HMMTree hmmtree)
		{
			return hmmtree.exitPoints;
		}
		
		internal static HMMPool access_100(HMMTree hmmtree)
		{
			return hmmtree.hmmPool;
		}
		
		internal static Set access_200(HMMTree hmmtree)
		{
			return hmmtree.entryPoints;
		}
		
		internal static dictionary.Dictionary access_300(HMMTree hmmtree)
		{
			return hmmtree.dictionary;
		}
		
		internal static InitialWordNode access_402(HMMTree hmmtree, InitialWordNode result)
		{
			hmmtree.initialNode = result;
			return result;
		}		
		
		internal static float access_500(HMMTree hmmtree, Word word)
		{
			return hmmtree.getWordUnigramProbability(word);
		}
		
		internal static Map access_600(HMMTree hmmtree)
		{
			return hmmtree.wordNodeMap;
		}
		
		internal static WordNode access_702(HMMTree hmmtree, WordNode result)
		{
			hmmtree.sentenceEndWordNode = result;
			return result;
		}
		
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
		
		private void compile()
		{
			this.collectEntryAndExitUnits();
			this.entryPointTable = new HMMTree.EntryPointTable(this, this.entryPoints);
			this.addWords();
			this.entryPointTable.createEntryPointMaps();
			this.freeze();
		}
		
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
		
		internal HMMTree(HMMPool hmmpool, dictionary.Dictionary dictionary, LanguageModel languageModel, bool flag, float num)
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

		public Node[] getEntryPoint(Unit unit, Unit unit2)
		{
			HMMTree.EntryPoint entryPoint = this.entryPointTable.getEntryPoint(unit2);
			return entryPoint.getEntryPointsFromLeftContext(unit).getSuccessors();
		}
		
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
		
		public WordNode getSentenceEndWordNode()
		{
			if (!HMMTree.assertionsDisabled && this.sentenceEndWordNode == null)
			{
				
				throw new AssertionError();
			}
			return this.sentenceEndWordNode;
		}
		
		internal void dumpTree()
		{
			java.lang.System.@out.println("Dumping Tree ...");
			HashMap hashMap = new HashMap();
			this.dumpTree(0, this.getInitialNode(), hashMap);
			java.lang.System.@out.println("... done Dumping Tree");
		}
		
		private HMMPool hmmPool;

		private InitialWordNode initialNode;

		private dictionary.Dictionary dictionary;

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
		
		internal sealed class EntryPoint : Object
		{
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
			
			private void connectSingleUnitWords(Unit lc, Node node, HashMap hashMap)
			{
				if (!this.singleUnitWords.isEmpty())
				{
					Iterator iterator = HMMTree.access_200(this.this_0).iterator();
					while (iterator.hasNext())
					{
						Unit unit = (Unit)iterator.next();
						HMM hmm = HMMTree.access_100(this.this_0).getHMM(this.baseUnit, lc, unit, HMMPosition.__SINGLE);
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
							if (pronunciation.getWord() == HMMTree.access_300(this.this_0).getSentenceStartWord())
							{
								HMMTree.access_402(this.this_0, new InitialWordNode(pronunciation, hmmnode));
							}
							else
							{
								float num = HMMTree.access_500(this.this_0, pronunciation.getWord());
								WordNode wordNode = hmmnode.addSuccessor(pronunciation, num, HMMTree.access_600(this.this_0));
								if (pronunciation.getWord() == HMMTree.access_300(this.this_0).getSentenceEndWord())
								{
									HMMTree.access_702(this.this_0, wordNode);
								}
							}
							this.nodeCount++;
						}
					}
				}
			}
			
			internal EntryPoint(HMMTree hmmtree, Unit unit)
			{
				this.this_0 = hmmtree;
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
			
			internal void addSingleUnitWord(Pronunciation pronunciation)
			{
				this.singleUnitWords.add(pronunciation);
			}
			
			internal void createEntryPointMap()
			{
				HashMap hashMap = new HashMap();
				HashMap hashMap2 = new HashMap();
				Iterator iterator = HMMTree.access_000(this.this_0).iterator();
				while (iterator.hasNext())
				{
					Unit unit = (Unit)iterator.next();
					Node node = new Node(float.MinValue);
					Iterator iterator2 = this.getEntryPointRC().iterator();
					while (iterator2.hasNext())
					{
						Unit unit2 = (Unit)iterator2.next();
						HMM hmm = HMMTree.access_100(this.this_0).getHMM(this.baseUnit, unit, unit2, HMMPosition.__BEGIN);
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
			
			internal HMMTree this_0;
		}
		
		internal sealed class EntryPointTable : Object
		{
			internal HMMTree.EntryPoint getEntryPoint(Unit unit)
			{
				return (HMMTree.EntryPoint)this.entryPoints.get(unit);
			}
			
			internal EntryPointTable(HMMTree hmmtree, Collection collection)
			{
				this.this_0 = hmmtree;
				this.entryPoints = new HashMap();
				Iterator iterator = collection.iterator();
				while (iterator.hasNext())
				{
					Unit unit = (Unit)iterator.next();
					this.entryPoints.put(unit, new HMMTree.EntryPoint(hmmtree, unit));
				}
			}
			
			internal void createEntryPointMaps()
			{
				Iterator iterator = this.entryPoints.values().iterator();
				while (iterator.hasNext())
				{
					HMMTree.EntryPoint entryPoint = (HMMTree.EntryPoint)iterator.next();
					entryPoint.createEntryPointMap();
				}
			}
			
			internal void freeze()
			{
				Iterator iterator = this.entryPoints.values().iterator();
				while (iterator.hasNext())
				{
					HMMTree.EntryPoint entryPoint = (HMMTree.EntryPoint)iterator.next();
					entryPoint.freeze();
				}
			}
			
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
			
			internal HMMTree this_0;
		}
	}
}

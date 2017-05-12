using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public abstract class Grammar : Object, Configurable, GrammarInterface
	{	
		protected internal virtual void newGrammar()
		{
			this.maxIdentity = 0;
			this.grammarNodes = new HashSet();
			this.initialNode = null;
		}

		protected internal abstract GrammarNode createGrammar();
		
		private void addFillerWords()
		{
			HashSet hashSet = new HashSet(this.getGrammarNodes());
			Word[] interWordFillers = this.getInterWordFillers();
			if (interWordFillers.Length == 0)
			{
				return;
			}
			Iterator iterator = hashSet.iterator();
			while (iterator.hasNext())
			{
				GrammarNode grammarNode = (GrammarNode)iterator.next();
				if (!grammarNode.isEmpty() && !grammarNode.getWord().isFiller())
				{
					GrammarNode grammarNode2 = grammarNode.splitNode(this.maxIdentity + 1);
					this.add(grammarNode2);
					GrammarNode grammarNode3 = this.createGrammarNode(false);
					GrammarNode grammarNode4 = this.createGrammarNode(false);
					grammarNode4.add(grammarNode3, 0f);
					grammarNode4.add(grammarNode2, 0f);
					grammarNode.add(grammarNode3, 0f);
					Word[] array = interWordFillers;
					int num = array.Length;
					for (int i = 0; i < num; i++)
					{
						Word word = array[i];
						GrammarNode grammarNode5 = this.createGrammarNode(this.maxIdentity + 1, word.getSpelling());
						grammarNode3.add(grammarNode5, 0f);
						grammarNode5.add(grammarNode4, 0f);
					}
				}
			}
		}
		
		private void addSilenceWords()
		{
			HashSet hashSet = new HashSet(this.getGrammarNodes());
			Iterator iterator = hashSet.iterator();
			while (iterator.hasNext())
			{
				GrammarNode grammarNode = (GrammarNode)iterator.next();
				if (!grammarNode.isEmpty() && !grammarNode.getWord().isFiller())
				{
					GrammarNode grammarNode2 = this.createGrammarNode(this.maxIdentity + 1, this.dictionary.getSilenceWord().getSpelling());
					GrammarNode grammarNode3 = grammarNode.splitNode(this.maxIdentity + 1);
					this.add(grammarNode3);
					grammarNode.add(grammarNode2, 0f);
					grammarNode2.add(grammarNode3, 0f);
					grammarNode2.add(grammarNode2, 0f);
				}
			}
		}
		
		private void optimizeGrammar()
		{
			Set set = this.getGrammarNodes();
			Iterator iterator = set.iterator();
			while (iterator.hasNext())
			{
				GrammarNode grammarNode = (GrammarNode)iterator.next();
				grammarNode.optimize();
			}
		}
		
		public virtual void dumpStatistics()
		{
			if (this.logger.isLoggable(Level.INFO))
			{
				int num = 0;
				this.logger.info(new StringBuilder().append("Num nodes : ").append(this.getNumNodes()).toString());
				Iterator iterator = this.grammarNodes.iterator();
				while (iterator.hasNext())
				{
					GrammarNode grammarNode = (GrammarNode)iterator.next();
					num += grammarNode.getSuccessors().Length;
				}
				this.logger.info(new StringBuilder().append("Num arcs  : ").append(num).toString());
				this.logger.info(new StringBuilder().append("Avg arcs  : ").append((float)num / (float)this.getNumNodes()).toString());
			}
		}

		public virtual int getNumNodes()
		{
			return this.grammarNodes.size();
		}
		
		public virtual string getRandomSentence()
		{
			StringBuilder stringBuilder = new StringBuilder();
			GrammarNode grammarNode = this.getInitialNode();
			while (!grammarNode.isFinalNode())
			{
				if (!grammarNode.isEmpty())
				{
					Word word = grammarNode.getWord();
					if (!word.isFiller())
					{
						stringBuilder.append(word.getSpelling()).append(' ');
					}
				}
				grammarNode = this.selectRandomSuccessor(grammarNode);
			}
			return String.instancehelper_trim(stringBuilder.toString());
		}

		public virtual GrammarNode getInitialNode()
		{
			return this.initialNode;
		}
		
		private GrammarNode selectRandomSuccessor(GrammarNode grammarNode)
		{
			GrammarArc[] successors = grammarNode.getSuccessors();
			if (successors.Length > 1)
			{
				double[] array = new double[successors.Length];
				double num = (double)0f;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = ((double)successors[0].getProbability() + 1E-10) / ((double)successors[i].getProbability() + 1E-10);
					num += array[i];
				}
				for (int i = 0; i < array.Length; i++)
				{
					double[] array2 = array;
					int num2 = i;
					double[] array3 = array2;
					array3[num2] /= num;
				}
				double num3 = this.randomizer.nextDouble();
				int num4 = 0;
				int num5 = 0;
				while (num3 > 1E-10)
				{
					num4 = num5;
					num3 -= array[num5];
					num5++;
				}
				return successors[num4].getGrammarNode();
			}
			return successors[0].getGrammarNode();
		}

		public virtual dictionary.Dictionary getDictionary()
		{
			return this.dictionary;
		}
		
		private void add(GrammarNode grammarNode)
		{
			if (grammarNode.getID() > this.maxIdentity)
			{
				this.maxIdentity = grammarNode.getID();
			}
			if (this.idCheck)
			{
				Iterator iterator = this.grammarNodes.iterator();
				while (iterator.hasNext())
				{
					GrammarNode grammarNode2 = (GrammarNode)iterator.next();
					if (grammarNode2.getID() == grammarNode.getID())
					{
						string text = new StringBuilder().append("DUP ID ").append(grammarNode2).append(" and ").append(grammarNode).toString();
						
						throw new Error(text);
					}
				}
			}
			this.grammarNodes.add(grammarNode);
		}
		
		protected internal virtual GrammarNode createGrammarNode(int identity, string word)
		{
			Word[][] empty_ALTERNATIVE = Grammar.EMPTY_ALTERNATIVE;
			Word word2 = this.getDictionary().getWord(word);
			GrammarNode grammarNode;
			if (word2 != null)
			{
				Word[][] array = new Word[][]
				{
					new Word[1]
				};
				array[0][0] = word2;
				grammarNode = new GrammarNode(identity, array);
				this.add(grammarNode);
			}
			else
			{
				grammarNode = this.createGrammarNode(identity, false);
				this.logger.warning(new StringBuilder().append("Can't find pronunciation for ").append(word).toString());
			}
			return grammarNode;
		}
		
		protected internal virtual GrammarNode createGrammarNode(int identity, bool isFinal)
		{
			GrammarNode grammarNode = new GrammarNode(identity, isFinal);
			this.add(grammarNode);
			return grammarNode;
		}
		
		public virtual Set getGrammarNodes()
		{
			return this.grammarNodes;
		}
		
		private Word[] getInterWordFillers()
		{
			ArrayList arrayList = new ArrayList();
			Word[] fillerWords = this.dictionary.getFillerWords();
			Word[] array = fillerWords;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Word word = array[i];
				if (word != this.dictionary.getSentenceStartWord() && word != this.dictionary.getSentenceEndWord())
				{
					arrayList.add(word);
				}
			}
			return (Word[])arrayList.toArray(new Word[arrayList.size()]);
		}
		
		protected internal virtual GrammarNode createGrammarNode(bool isFinal)
		{
			return this.createGrammarNode(this.maxIdentity + 1, isFinal);
		}
		
		public Grammar(bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, dictionary.Dictionary dictionary)
		{
			this.randomizer = new java.util.Random((long)((ulong)56));
			this.logger = Logger.getLogger(Object.instancehelper_getClass(this).getName());
			this._optimizeGrammar = optimizeGrammar;
			this._addSilenceWords = addSilenceWords;
			this._addFillerWords = addFillerWords;
			this.dictionary = dictionary;
		}
		
		public Grammar()
		{
			this._optimizeGrammar = true;
			this.randomizer = new java.util.Random((long)((ulong)56));
		}
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this._optimizeGrammar = ps.getBoolean("optimizeGrammar").booleanValue();
			this._addSilenceWords = ps.getBoolean("addSilenceWords").booleanValue();
			this._addFillerWords = ps.getBoolean("addFillerWords").booleanValue();
			this.dictionary = (dictionary.Dictionary)ps.getComponent("dictionary");
		}
		
		public virtual void allocate()
		{
			this.dictionary.allocate();
			this.newGrammar();
			sphinx.util.Timer timer = TimerPool.getTimer(this, "grammarLoad");
			timer.start();
			this.initialNode = this.createGrammar();
			timer.stop();
		}
		
		public virtual void deallocate()
		{
			this.initialNode = null;
			this.grammarNodes = null;
			this.dictionary.deallocate();
		}
		
		protected internal virtual void postProcessGrammar()
		{
			if (this._addFillerWords)
			{
				this.addFillerWords();
			}
			else if (this._addSilenceWords)
			{
				this.addSilenceWords();
			}
			if (this._optimizeGrammar)
			{
				this.optimizeGrammar();
			}
			this.dumpStatistics();
		}
		
		public virtual void dumpRandomSentences(string path, int count)
		{
			try
			{
				HashSet hashSet = new HashSet();
				PrintWriter printWriter = new PrintWriter(new FileOutputStream(path));
				for (int i = 0; i < count; i++)
				{
					string randomSentence = this.getRandomSentence();
					if (!hashSet.contains(randomSentence))
					{
						hashSet.add(randomSentence);
						printWriter.println(randomSentence);
					}
				}
				printWriter.close();
			}
			catch (IOException ex)
			{
				this.logger.severe(new StringBuilder().append("Can't write random sentences to ").append(path).append(' ').append(ex).toString());
			}
		}
		
		public virtual void dumpRandomSentences(int count)
		{
			HashSet hashSet = new HashSet();
			for (int i = 0; i < count; i++)
			{
				string randomSentence = this.getRandomSentence();
				if (!hashSet.contains(randomSentence))
				{
					hashSet.add(randomSentence);
				}
			}
			ArrayList arrayList = new ArrayList(hashSet);
			Collections.sort(arrayList);
			Iterator iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				java.lang.System.@out.println(text);
			}
		}
		
		public virtual void dumpGrammar(string name)
		{
			this.getInitialNode().dumpDot(name);
		}
		
		protected internal virtual GrammarNode createGrammar(string bogusText)
		{
			string text = "Does not create grammar with reference text";
			
			throw new NoSuchMethodException(text);
		}
		
		protected internal virtual GrammarNode createGrammarNode(int identity, string[][] alts)
		{
			Word[][] array = new Word[alts.Length][];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Word[alts[i].Length];
				for (int j = 0; j < alts[i].Length; j++)
				{
					Word word = this.getDictionary().getWord(alts[i][j]);
					if (word == null)
					{
						array = Grammar.EMPTY_ALTERNATIVE;
						break;
					}
					array[i][j] = word;
				}
			}
			GrammarNode grammarNode = new GrammarNode(identity, array);
			this.add(grammarNode);
			return grammarNode;
		}
		
		protected internal virtual GrammarNode createGrammarNode(string word)
		{
			return this.createGrammarNode(this.maxIdentity + 1, word);
		}
		
		static Grammar()
		{
			int num = 0;
			int num2 = 0;
			int[] array = new int[2];
			int num3 = num2;
			array[1] = num3;
			num3 = num;
			array[0] = num3;
			Grammar.EMPTY_ALTERNATIVE = (Word[][])ByteCodeHelper.multianewarray(typeof(Word[][]).TypeHandle, array);
		}

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_GRAMMAR = "showGrammar";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_OPTIMIZE_GRAMMAR = "optimizeGrammar";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_ADD_SIL_WORDS = "addSilenceWords";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_ADD_FILLER_WORDS = "addFillerWords";

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

		protected internal Logger logger;

		private bool _optimizeGrammar;

		private bool _addSilenceWords;

		private bool _addFillerWords;

		protected internal dictionary.Dictionary dictionary;

		protected internal GrammarNode initialNode;
		
		private Set grammarNodes;
		
		private static Word[][] EMPTY_ALTERNATIVE;

		private java.util.Random randomizer;

		private int maxIdentity;

		private bool idCheck;
	}
}

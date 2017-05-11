using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	public class SimpleNGramModel : java.lang.Object, LanguageModel, Configurable
	{		
		public SimpleNGramModel(URL urlLocation, dictionary.Dictionary dictionary, float unigramWeight, int desiredMaxDepth)
		{
			this.urlLocation = urlLocation;
			this.unigramWeight = unigramWeight;
			this.logMath = LogMath.getLogMath();
			this.desiredMaxDepth = desiredMaxDepth;
			this.dictionary = dictionary;
			this.map = new HashMap();
			this.vocabulary = new HashSet();
			this.tokens = new LinkedList();
		}
	
		private void load(URL url, float num, dictionary.Dictionary dictionary)
		{
			float num2 = this.logMath.linearToLog((double)num);
			float num3 = this.logMath.linearToLog((double)1f - (double)num);
			this.open(url);
			this.readUntil("\\data\\");
			ArrayList arrayList = new ArrayList();
			string text;
			while ((text = this.readLine()) != null)
			{
				if (java.lang.String.instancehelper_startsWith(text, "ngram"))
				{
					StringTokenizer stringTokenizer = new StringTokenizer(text, " \t\n\r\f=");
					if (stringTokenizer.countTokens() != 3)
					{
						this.corrupt(new StringBuilder().append("corrupt ngram field ").append(text).append(' ').append(stringTokenizer.countTokens()).toString());
					}
					stringTokenizer.nextToken();
					int num4 = Integer.parseInt(stringTokenizer.nextToken());
					int i = Integer.parseInt(stringTokenizer.nextToken());
					arrayList.add(num4 - 1, Integer.valueOf(i));
					this.maxNGram = java.lang.Math.max(num4, this.maxNGram);
				}
				else if (java.lang.String.instancehelper_equals(text, "\\1-grams:"))
				{
					break;
				}
			}
			int num5 = ((Integer)arrayList.get(0)).intValue() - 1;
			float num6 = -this.logMath.linearToLog((double)num5);
			for (int i = 0; i < arrayList.size(); i++)
			{
				int num7 = i + 1;
				int num8 = ((Integer)arrayList.get(i)).intValue();
				for (int j = 0; j < num8; j++)
				{
					StringTokenizer stringTokenizer2 = new StringTokenizer(this.readLine());
					int num9 = stringTokenizer2.countTokens();
					if (num9 != num7 + 1 && num9 != num7 + 2)
					{
						this.corrupt("Bad format");
					}
					float logSource = Float.parseFloat(stringTokenizer2.nextToken());
					float logSource2 = 0f;
					ArrayList arrayList2 = new ArrayList(this.maxNGram);
					for (int k = 0; k < num7; k++)
					{
						string text2 = stringTokenizer2.nextToken();
						this.vocabulary.add(text2);
						Word word = dictionary.getWord(text2);
						if (word == null)
						{
							word = Word.__UNKNOWN;
						}
						arrayList2.add(word);
					}
					WordSequence wordSequence = new WordSequence(arrayList2);
					if (stringTokenizer2.hasMoreTokens())
					{
						logSource2 = Float.parseFloat(stringTokenizer2.nextToken());
					}
					float num10 = this.logMath.log10ToLog(logSource);
					float num11 = this.logMath.log10ToLog(logSource2);
					if (num7 == 1)
					{
						float logVal = num10 + num2;
						float logVal2 = num6 + num3;
						num10 = this.logMath.addAsLinear(logVal, logVal2);
					}
					this.put(wordSequence, num10, num11);
				}
				if (i < arrayList.size() - 1)
				{
					string text3 = new StringBuilder().append("\\").append(num7 + 1).append("-grams:").toString();
					this.readUntil(text3);
				}
			}
			this.readUntil("\\end\\");
			this.close();
		}
		
		private Probability getProb(WordSequence wordSequence)
		{
			return (Probability)this.map.get(wordSequence);
		}
		
		public virtual float getBackoff(WordSequence wordSequence)
		{
			float result = 0f;
			Probability prob = this.getProb(wordSequence);
			if (prob != null)
			{
				result = prob.logBackoff;
			}
			return result;
		}
		
		public virtual float getProbability(WordSequence wordSequence)
		{
			Probability prob = this.getProb(wordSequence);
			float result;
			if (prob == null)
			{
				if (wordSequence.size() > 1)
				{
					result = this.getBackoff(wordSequence.getOldest()) + this.getProbability(wordSequence.getNewest());
				}
				else
				{
					result = float.MinValue;
				}
			}
			else
			{
				result = prob.logProbability;
			}
			return result;
		}
		
		private void open(URL url)
		{
			this.lineNumber = 0;
			this.fileName = url.toString();
			this.reader = new BufferedReader(new InputStreamReader(url.openStream()));
		}
		
		private void readUntil(string text)
		{
			try
			{
				while (!java.lang.String.instancehelper_equals(this.readLine(), text))
				{
				}
			}
			catch (IOException ex)
			{
				goto IL_16;
			}
			return;
			IL_16:
			this.corrupt(new StringBuilder().append("Premature EOF while waiting for ").append(text).toString());
		}
		
		private string readLine()
		{
			this.lineNumber++;
			string text = this.reader.readLine();
			if (text == null)
			{
				this.corrupt("Premature EOF");
			}
			return java.lang.String.instancehelper_trim(text);
		}
		
		private void corrupt(string text)
		{
			string text2 = new StringBuilder().append("Corrupt Language Model ").append(this.fileName).append(" at line ").append(this.lineNumber).append(':').append(text).toString();
			
			throw new IOException(text2);
		}
		
		private void put(WordSequence wordSequence, float num, float num2)
		{
			this.map.put(wordSequence, new Probability(num, num2));
			this.tokens.add(wordSequence);
		}
		
		private void close()
		{
			this.reader.close();
			this.reader = null;
		}
		
		public SimpleNGramModel(string location, dictionary.Dictionary dictionary, float unigramWeight, int desiredMaxDepth) : this(ConfigurationManagerUtils.resourceToURL(location), dictionary, unigramWeight, desiredMaxDepth)
		{
		}
		
		public SimpleNGramModel()
		{
		}
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logMath = LogMath.getLogMath();
			if (this.allocated)
			{
				string text = "Can't change properties after allocation";
				
				throw new RuntimeException(text);
			}
			this.urlLocation = ConfigurationManagerUtils.getResource("location", ps);
			this.unigramWeight = ps.getFloat("unigramWeight");
			this.desiredMaxDepth = ps.getInt("maxDepth");
			this.dictionary = (dictionary.Dictionary)ps.getComponent("dictionary");
			this.map = new HashMap();
			this.vocabulary = new HashSet();
			this.tokens = new LinkedList();
		}
		
		public virtual void allocate()
		{
			this.allocated = true;
			this.load(this.urlLocation, this.unigramWeight, this.dictionary);
			if (this.desiredMaxDepth > 0 && this.desiredMaxDepth < this.maxNGram)
			{
				this.maxNGram = this.desiredMaxDepth;
			}
		}

		public virtual void deallocate()
		{
			this.allocated = false;
		}

		public virtual string getName()
		{
			return this.name;
		}

		public virtual float getSmear(WordSequence wordSequence)
		{
			return 0f;
		}

		public virtual int getMaxDepth()
		{
			return this.maxNGram;
		}
		
		public virtual Set getVocabulary()
		{
			return Collections.unmodifiableSet(this.vocabulary);
		}

		public virtual void onUtteranceEnd()
		{
		}
		
		private string listToString(List list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Iterator iterator = list.iterator();
			while (iterator.hasNext())
			{
				Word word = (Word)iterator.next();
				stringBuilder.append(word).append(' ');
			}
			return stringBuilder.toString();
		}
		
		public virtual void dump()
		{
			Iterator iterator = this.map.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				java.lang.System.@out.println(new StringBuilder().append(entry.getKey()).append(" ").append(entry.getValue()).toString());
			}
		}
		
		private string getRepresentation(List list)
		{
			if (list.isEmpty())
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			Iterator iterator = list.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				stringBuilder.append(text).append('+');
			}
			stringBuilder.setLength(stringBuilder.length() - 1);
			return stringBuilder.toString();
		}

		public virtual LinkedList getNGrams()
		{
			return this.tokens;
		}

		private string name;

		private LogMath logMath;

		private URL urlLocation;

		private float unigramWeight;

		private dictionary.Dictionary dictionary;

		private int desiredMaxDepth;

		private int maxNGram;
		
		private Map map;
		
		private Set vocabulary;

		protected internal int lineNumber;

		protected internal BufferedReader reader;

		protected internal string fileName;

		private bool allocated;

		private LinkedList tokens;
	}
}

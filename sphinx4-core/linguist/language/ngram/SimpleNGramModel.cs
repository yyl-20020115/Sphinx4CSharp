using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.language.ngram.LanguageModel"
	})]
	public class SimpleNGramModel : java.lang.Object, LanguageModel, Configurable
	{
		[LineNumberTable(new byte[]
		{
			21,
			104,
			103,
			104,
			107,
			104,
			103,
			107,
			107,
			107
		})]
		
		public SimpleNGramModel(URL urlLocation, Dictionary dictionary, float unigramWeight, int desiredMaxDepth)
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			159,
			111,
			111,
			134,
			135,
			139,
			102,
			109,
			112,
			109,
			106,
			127,
			0,
			47,
			165,
			104,
			110,
			110,
			113,
			115,
			117,
			162,
			149,
			113,
			112,
			102,
			116,
			108,
			109,
			105,
			112,
			139,
			110,
			135,
			109,
			105,
			105,
			110,
			106,
			100,
			135,
			234,
			57,
			232,
			73,
			105,
			105,
			142,
			111,
			143,
			101,
			103,
			103,
			241,
			72,
			236,
			27,
			235,
			103,
			108,
			127,
			10,
			232,
			20,
			235,
			111,
			107,
			102
		})]
		
		private void load(URL url, float num, Dictionary dictionary)
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

		[LineNumberTable(new byte[]
		{
			160,
			69,
			102,
			104,
			99,
			135
		})]
		
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

		[LineNumberTable(new byte[]
		{
			97,
			102,
			104,
			99,
			105,
			110,
			175,
			168,
			231,
			69
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			30,
			103,
			108,
			98,
			116
		})]
		
		private void open(URL url)
		{
			this.lineNumber = 0;
			this.fileName = url.toString();
			this.reader = new BufferedReader(new InputStreamReader(url.openStream()));
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			45,
			217,
			2,
			97,
			159,
			1
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			14,
			110,
			108,
			99,
			139
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		
		private void corrupt(string text)
		{
			string text2 = new StringBuilder().append("Corrupt Language Model ").append(this.fileName).append(" at line ").append(this.lineNumber).append(':').append(text).toString();
			
			throw new IOException(text2);
		}

		[LineNumberTable(new byte[]
		{
			160,
			248,
			118,
			109
		})]
		
		private void put(WordSequence wordSequence, float num, float num2)
		{
			this.map.put(wordSequence, new Probability(num, num2));
			this.tokens.add(wordSequence);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			58,
			107,
			103
		})]
		
		private void close()
		{
			this.reader.close();
			this.reader = null;
		}

		[Throws(new string[]
		{
			"java.net.MalformedURLException",
			"java.lang.ClassNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			15,
			145
		})]
		
		public SimpleNGramModel(string location, Dictionary dictionary, float unigramWeight, int desiredMaxDepth) : this(ConfigurationManagerUtils.resourceToURL(location), dictionary, unigramWeight, desiredMaxDepth)
		{
		}

		[LineNumberTable(new byte[]
		{
			32,
			134
		})]
		
		public SimpleNGramModel()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			43,
			139,
			104,
			176,
			113,
			113,
			113,
			118,
			107,
			107,
			107
		})]
		
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
			this.dictionary = (Dictionary)ps.getComponent("dictionary");
			this.map = new HashMap();
			this.vocabulary = new HashSet();
			this.tokens = new LinkedList();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			63,
			103,
			120,
			105,
			110,
			172
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			160,
			119,
			102,
			123,
			113
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			127,
			127,
			6,
			127,
			23
		})]
		
		public virtual void dump()
		{
			Iterator iterator = this.map.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				java.lang.System.@out.println(new StringBuilder().append(entry.getKey()).append(" ").append(entry.getValue()).toString());
			}
		}

		
		[LineNumberTable(new byte[]
		{
			160,
			140,
			104,
			102,
			102,
			123,
			113,
			110
		})]
		
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

		private Dictionary dictionary;

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

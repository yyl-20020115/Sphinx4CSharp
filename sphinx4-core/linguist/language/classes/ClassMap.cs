using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.language.classes
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public class ClassMap : java.lang.Object, Configurable
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			22,
			104,
			103,
			134
		})]
		
		public virtual void allocate()
		{
			if (!this.allocated)
			{
				this.allocated = true;
				this.loadClassDefs();
			}
		}

		public virtual void deallocate()
		{
			this.allocated = false;
			this.wordToClassProbabilities = null;
			this.classVocabulary = null;
		}

		
		
		public virtual ClassProbability getClassProbability(string word)
		{
			return (ClassProbability)this.wordToClassProbabilities.get(word);
		}

		
		
		public virtual Word getClassAsWord(string text)
		{
			return (Word)this.classVocabulary.get(text);
		}

		
		
		
		public virtual Set getWordsInClass(string className)
		{
			return (Set)this.classToWord.get(className);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			65,
			102,
			144,
			109,
			108,
			105,
			127,
			1,
			154,
			103,
			109,
			104,
			114,
			159,
			30,
			114,
			42,
			134,
			117,
			105,
			101,
			102,
			102,
			127,
			26
		})]
		
		private void loadClassDefs()
		{
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(this.classDefsLocation.openStream()));
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				StringTokenizer stringTokenizer = new StringTokenizer(text, " \t\n\r\f=");
				if (stringTokenizer.countTokens() != 3)
				{
					string text2 = new StringBuilder().append("corrupt word to class def: ").append(text).append("; ").append(stringTokenizer.countTokens()).toString();
					
					throw new IOException(text2);
				}
				string text3 = stringTokenizer.nextToken();
				float num = Float.parseFloat(stringTokenizer.nextToken());
				string text4 = stringTokenizer.nextToken();
				if (this.logger.isLoggable(Level.FINE))
				{
					this.logger.fine(new StringBuilder().append(text4).append(" --> ").append(text3).append(" ").append(num).toString());
				}
				this.wordToClassProbabilities.put(text4, new ClassProbability(text3, this.logMath.linearToLog((double)num)));
				this.classVocabulary.put(text3, new Word(text3, null, false));
				this.addWordInClass(text3, text4);
			}
			bufferedReader.close();
			this.checkClasses();
			this.logger.info(new StringBuilder().append("Loaded word to class mappings for ").append(this.wordToClassProbabilities.size()).append(" words").toString());
		}

		[LineNumberTable(new byte[]
		{
			116,
			114,
			104,
			102,
			147,
			125
		})]
		
		private void addWordInClass(string text, string text2)
		{
			object obj = (Set)this.classToWord.get(text);
			if ((Set)obj == null)
			{
				obj = new HashSet();
				this.classToWord.put(text, (HashSet)obj);
			}
			object obj2 = obj;
			Set set;
			if (obj2 != null)
			{
				if ((set = (obj2 as Set)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				set = null;
			}
			set.add(text2);
		}

		[LineNumberTable(new byte[]
		{
			94,
			102,
			127,
			9,
			114,
			99,
			153,
			159,
			13,
			133,
			127,
			5,
			127,
			9,
			159,
			39,
			101
		})]
		
		private void checkClasses()
		{
			HashMap hashMap = new HashMap();
			Iterator iterator = this.wordToClassProbabilities.values().iterator();
			while (iterator.hasNext())
			{
				ClassProbability classProbability = (ClassProbability)iterator.next();
				Float @float = (Float)hashMap.get(classProbability.getClassName());
				if (@float == null)
				{
					hashMap.put(classProbability.getClassName(), Float.valueOf(0f));
				}
				else
				{
					hashMap.put(classProbability.getClassName(), Float.valueOf((float)this.logMath.logToLinear(classProbability.getLogProbability()) + @float.floatValue()));
				}
			}
			iterator = hashMap.entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				if (java.lang.Math.abs((double)1f - (double)((Float)entry.getValue()).floatValue()) > 0.001)
				{
					this.logger.warning(new StringBuilder().append("Word probabilities for class ").append((string)entry.getKey()).append(" sum to ").append(entry.getValue()).toString());
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			186,
			232,
			52,
			235,
			69,
			235,
			69,
			171,
			118,
			103,
			107
		})]
		
		public ClassMap(URL classDefsLocation)
		{
			this.classVocabulary = new HashMap();
			this.wordToClassProbabilities = new HashMap();
			this.classToWord = new HashMap();
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
			this.classDefsLocation = classDefsLocation;
			this.logMath = LogMath.getLogMath();
		}

		[LineNumberTable(new byte[]
		{
			0,
			232,
			46,
			235,
			69,
			235,
			69,
			235,
			74
		})]
		
		public ClassMap()
		{
			this.classVocabulary = new HashMap();
			this.wordToClassProbabilities = new HashMap();
			this.classToWord = new HashMap();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			8,
			140,
			104,
			144,
			113
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			if (this.allocated)
			{
				string text = "Can't change properties after allocation";
				
				throw new RuntimeException(text);
			}
			this.classDefsLocation = ConfigurationManagerUtils.getResource("classDefsLocation", ps);
		}

		
		[NameSig("getClassProbability", "(Ljava.lang.java.lang.String;)Ledu.cmu.sphinx.linguist.language.classes.ClassProbability;")]
		public object getClassProbability(string word)
		{
			return this.getClassProbability(word);
		}

		
		[NameSig("getClassProbability", "(Ljava.lang.java.lang.String;)Ledu.cmu.sphinx.linguist.language.classes.ClassProbability;")]
		protected internal object _0(string word)
		{
			return this.getClassProbability(word);
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;"
		})]
		public const string PROP_CLASS_DEFS_LOCATION = "classDefsLocation";

		private Logger logger;

		private bool allocated;

		private URL classDefsLocation;

		private LogMath logMath;

		
		private Map classVocabulary;

		
		private Map wordToClassProbabilities;

		
		
		private HashMap classToWord;
	}
}

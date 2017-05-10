using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.ngram;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.classes
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.language.ngram.LanguageModel"
	})]
	public class ClassBasedLanguageModel : java.lang.Object, LanguageModel, Configurable
	{
		[LineNumberTable(new byte[]
		{
			114,
			107,
			127,
			6,
			109,
			131,
			143,
			141,
			98
		})]
		
		private void makeVocabulary()
		{
			this.vocabulary = new HashSet();
			Iterator iterator = this.classLM.getVocabulary().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				Set wordsInClass = this.classMap.getWordsInClass(text);
				if (wordsInClass == null)
				{
					this.vocabulary.add(text);
				}
				else
				{
					this.vocabulary.addAll(wordsInClass);
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			191,
			232,
			60,
			231,
			69,
			103,
			103
		})]
		
		public ClassBasedLanguageModel(ClassMap classMap, LanguageModel classLM)
		{
			this.allocated = false;
			this.classMap = classMap;
			this.classLM = classLM;
		}

		[LineNumberTable(new byte[]
		{
			4,
			232,
			55,
			231,
			75
		})]
		
		public ClassBasedLanguageModel()
		{
			this.allocated = false;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			15,
			104,
			101,
			214,
			118,
			118
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			if (this.allocated)
			{
				string name = ClassLiteral<ClassBasedLanguageModel>.Value.getName();
				string propertyName = null;
				string msg = "Can't change properties after allocation";
				
				throw new PropertyException(name, propertyName, msg);
			}
			this.classMap = (ClassMap)ps.getComponent("classMap");
			this.classLM = (LanguageModel)ps.getComponent("classLanguageModel");
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			30,
			104,
			103,
			107,
			107,
			134
		})]
		
		public virtual void allocate()
		{
			if (!this.allocated)
			{
				this.allocated = true;
				this.classMap.allocate();
				this.classLM.allocate();
				this.makeVocabulary();
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			43,
			103,
			107,
			107,
			103
		})]
		
		public virtual void deallocate()
		{
			this.allocated = false;
			this.classLM.deallocate();
			this.classMap.deallocate();
			this.vocabulary = null;
		}

		[LineNumberTable(new byte[]
		{
			56,
			108,
			102,
			106,
			104,
			103,
			108,
			145,
			107,
			103,
			132,
			232,
			54,
			233,
			78,
			108,
			103
		})]
		
		public virtual float getProbability(WordSequence wordSequence)
		{
			Word[] array = new Word[wordSequence.size()];
			float num = 0f;
			for (int i = 0; i < array.Length; i++)
			{
				Word word = wordSequence.getWord(i);
				ClassProbability classProbability = this.classMap.getClassProbability(word.getSpelling());
				array[i] = ((classProbability != null) ? this.classMap.getClassAsWord(classProbability.getClassName()) : word);
				if (i == array.Length - 1 && classProbability != null)
				{
					num = classProbability.getLogProbability();
				}
			}
			float probability = this.classLM.getProbability(new WordSequence(array));
			return probability + num;
		}

		public virtual float getSmear(WordSequence wordSequence)
		{
			return 0f;
		}

		
		public virtual Set getVocabulary()
		{
			return this.vocabulary;
		}

		
		
		public virtual int getMaxDepth()
		{
			return this.classLM.getMaxDepth();
		}

		public virtual void onUtteranceEnd()
		{
		}

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
		public const string PROP_CLASS_LANGUAGE_MODEL = "classLanguageModel";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/language/classes/ClassMap, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_CLASS_MAP = "classMap";

		private LanguageModel classLM;

		
		private Set vocabulary;

		private bool allocated;

		private ClassMap classMap;
	}
}

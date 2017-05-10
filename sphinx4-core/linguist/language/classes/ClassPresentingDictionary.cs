using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.classes
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.dictionary.Dictionary"
	})]
	public class ClassPresentingDictionary : java.lang.Object, Dictionary, Configurable
	{
		[LineNumberTable(new byte[]
		{
			159,
			175,
			232,
			60,
			231,
			69,
			103,
			103
		})]
		
		public ClassPresentingDictionary(ClassMap classMap, Dictionary wordDictionary)
		{
			this.allocated = false;
			this.classMap = classMap;
			this.wordDictionary = wordDictionary;
		}

		[LineNumberTable(new byte[]
		{
			159,
			180,
			232,
			55,
			231,
			75
		})]
		
		public ClassPresentingDictionary()
		{
			this.allocated = false;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			185,
			118,
			150
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.classMap = (ClassMap)ps.getComponent("classMap");
			this.wordDictionary = (Dictionary)ps.getComponent("wordDictionary");
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			191,
			104,
			103,
			107,
			139
		})]
		
		public virtual void allocate()
		{
			if (!this.allocated)
			{
				this.allocated = true;
				this.wordDictionary.allocate();
				this.classMap.allocate();
			}
		}

		public virtual void deallocate()
		{
			this.allocated = false;
			this.wordDictionary = null;
		}

		[LineNumberTable(new byte[]
		{
			16,
			109
		})]
		
		public virtual Word getWord(string text)
		{
			Word classAsWord = this.classMap.getClassAsWord(text);
			return (classAsWord == null) ? this.wordDictionary.getWord(text) : classAsWord;
		}

		
		
		public virtual Word getSentenceStartWord()
		{
			return this.wordDictionary.getSentenceStartWord();
		}

		
		
		public virtual Word getSentenceEndWord()
		{
			return this.wordDictionary.getSentenceEndWord();
		}

		
		
		public virtual Word getSilenceWord()
		{
			return this.wordDictionary.getSilenceWord();
		}

		
		
		public virtual Word[] getFillerWords()
		{
			return this.wordDictionary.getFillerWords();
		}

		[LineNumberTable(new byte[]
		{
			50,
			127,
			16,
			47
		})]
		
		public override string toString()
		{
			return new StringBuilder().append("Word dictionary:\n").append(java.lang.Object.instancehelper_toString(this.wordDictionary)).append("Classes:\n").append(java.lang.Object.instancehelper_toString(this.classMap)).toString();
		}

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
		public const string PROP_WORD_DICTIONARY = "wordDictionary";

		private bool allocated;

		private Dictionary wordDictionary;

		private ClassMap classMap;
	}
}

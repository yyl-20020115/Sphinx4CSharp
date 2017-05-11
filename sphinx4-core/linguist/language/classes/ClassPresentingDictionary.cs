﻿using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.classes
{
	public class ClassPresentingDictionary : java.lang.Object, Dictionary, Configurable
	{		
		public ClassPresentingDictionary(ClassMap classMap, Dictionary wordDictionary)
		{
			this.allocated = false;
			this.classMap = classMap;
			this.wordDictionary = wordDictionary;
		}
		
		public ClassPresentingDictionary()
		{
			this.allocated = false;
		}
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.classMap = (ClassMap)ps.getComponent("classMap");
			this.wordDictionary = (Dictionary)ps.getComponent("wordDictionary");
		}
		
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

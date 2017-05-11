using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.linguist.language.ngram;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.util;

namespace edu.cmu.sphinx.linguist.language.classes
{
	public class ClassBasedLanguageModel : java.lang.Object, LanguageModel, Configurable
	{		
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
		
		public ClassBasedLanguageModel(ClassMap classMap, LanguageModel classLM)
		{
			this.allocated = false;
			this.classMap = classMap;
			this.classLM = classLM;
		}
		
		public ClassBasedLanguageModel()
		{
			this.allocated = false;
		}
		
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
		
		public virtual void deallocate()
		{
			this.allocated = false;
			this.classLM.deallocate();
			this.classMap.deallocate();
			this.vocabulary = null;
		}
		
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

using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	public interface LanguageModel : Configurable
	{
		void allocate();

		void deallocate();

		float getProbability(WordSequence ws);

		float getSmear(WordSequence ws);
		
		Set getVocabulary();

		int getMaxDepth();

		void onUtteranceEnd();
	}

	public abstract class LanguageModelBase : java.lang.Object, LanguageModel
	{
		[S4String(new object[]
{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"."
})]
		public const string PROP_LOCATION = "location";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_UNIGRAM_WEIGHT = "unigramWeight";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			-1
		})]
		public const string PROP_MAX_DEPTH = "maxDepth";

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

		public abstract void allocate();
		public abstract void deallocate();
		public abstract int getMaxDepth();
		public abstract float getProbability(WordSequence ws);
		public abstract float getSmear(WordSequence ws);
		public abstract Set getVocabulary();
		public abstract void newProperties(PropertySheet ps);
		public abstract void onUtteranceEnd();

		public static class __Fields
		{
			public const string PROP_LOCATION = "location";

			public const string PROP_UNIGRAM_WEIGHT = "unigramWeight";

			public const string PROP_MAX_DEPTH = "maxDepth";

			public const string PROP_DICTIONARY = "dictionary";
		}
	}
}

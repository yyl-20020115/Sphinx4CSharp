using System;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public interface LanguageModel : Configurable
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		void allocate();

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		void deallocate();

		float getProbability(WordSequence ws);

		float getSmear(WordSequence ws);

		
		Set getVocabulary();

		int getMaxDepth();

		void onUtteranceEnd();

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

		
		public static class __Fields
		{
			public const string PROP_LOCATION = "location";

			public const string PROP_UNIGRAM_WEIGHT = "unigramWeight";

			public const string PROP_MAX_DEPTH = "maxDepth";

			public const string PROP_DICTIONARY = "dictionary";
		}
	}
}

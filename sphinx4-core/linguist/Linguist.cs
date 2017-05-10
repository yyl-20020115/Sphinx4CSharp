using System;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist
{
	public interface Linguist : Configurable
	{
		void startRecognition();

		SearchGraph getSearchGraph();

		void stopRecognition();
		void allocate();

		void deallocate();

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_WORD_INSERTION_PROBABILITY = "wordInsertionProbability";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_UNIT_INSERTION_PROBABILITY = "unitInsertionProbability";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_SILENCE_INSERTION_PROBABILITY = "silenceInsertionProbability";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_FILLER_INSERTION_PROBABILITY = "fillerInsertionProbability";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string PROP_LANGUAGE_WEIGHT = "languageWeight";

		
		public static class __Fields
		{
			public const string PROP_WORD_INSERTION_PROBABILITY = "wordInsertionProbability";

			public const string PROP_UNIT_INSERTION_PROBABILITY = "unitInsertionProbability";

			public const string PROP_SILENCE_INSERTION_PROBABILITY = "silenceInsertionProbability";

			public const string PROP_FILLER_INSERTION_PROBABILITY = "fillerInsertionProbability";

			public const string PROP_LANGUAGE_WEIGHT = "languageWeight";
		}
	}
}

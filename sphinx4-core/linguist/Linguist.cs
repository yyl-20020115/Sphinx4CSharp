﻿using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.linguist
{
	public interface Linguist : Configurable
	{
		void startRecognition();

		SearchGraph getSearchGraph();

		void stopRecognition();
		void allocate();

		void deallocate();
	}

	public abstract class LinguistBase : Object, Linguist
	{
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

		public abstract void allocate();
		public abstract void deallocate();
		public abstract SearchGraph getSearchGraph();
		public abstract void newProperties(PropertySheet ps);
		public abstract void startRecognition();
		public abstract void stopRecognition();

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

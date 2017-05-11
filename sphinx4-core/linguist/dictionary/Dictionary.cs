using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.linguist.dictionary
{
	public interface Dictionary : Configurable
	{
		void allocate();

		void deallocate();

		Word getWord(string str);

		Word getSilenceWord();

		Word[] getFillerWords();

		Word getSentenceStartWord();

		Word getSentenceEndWord();

		public const string SENTENCE_START_SPELLING = "<s>";

		public const string SENTENCE_END_SPELLING = "</s>";

		public const string SILENCE_SPELLING = "<sil>";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;"
		})]
		public const string PROP_DICTIONARY = "dictionaryPath";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			""
		})]
		public const string PROP_G2P_MODEL_PATH = "g2pModelPath";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			1
		})]
		public const string PROP_G2P_MAX_PRONUNCIATIONS = "g2pMaxPron";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;"
		})]
		public const string PROP_FILLER_DICTIONARY = "fillerPath";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"mandatory",
			false
		})]
		public const string PROP_WORD_REPLACEMENT = "wordReplacement";

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/UnitManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			},
			"defaultClass",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/acoustic/UnitManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_UNIT_MANAGER = "unitManager";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"mandatory",
			false
		})]
		public const string PROP_ADDENDA = "addenda";

		
		public static class __Fields
		{
			public const string SENTENCE_START_SPELLING = "<s>";

			public const string SENTENCE_END_SPELLING = "</s>";

			public const string SILENCE_SPELLING = "<sil>";

			public const string PROP_DICTIONARY = "dictionaryPath";

			public const string PROP_G2P_MODEL_PATH = "g2pModelPath";

			public const string PROP_G2P_MAX_PRONUNCIATIONS = "g2pMaxPron";

			public const string PROP_FILLER_DICTIONARY = "fillerPath";

			public const string PROP_WORD_REPLACEMENT = "wordReplacement";

			public const string PROP_UNIT_MANAGER = "unitManager";

			public const string PROP_ADDENDA = "addenda";
		}
	}
}

using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.trainer
{
	public interface ControlFile : Configurable
	{
		void startUtteranceIterator();

		bool hasMoreUtterances();

		Utterance nextUtterance();

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"an4_train.fileids"
		})]
		public const string PROP_AUDIO_FILE = "audioFile";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"an4_train.transcription"
		})]
		public const string PROP_TRANSCRIPT_FILE = "transcriptFile";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			1
		})]
		public const string PROP_WHICH_BATCH = "whichBatch";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			1
		})]
		public const string PROP_TOTAL_BATCHES = "totalBatches";

		
		public static class __Fields
		{
			public const string PROP_AUDIO_FILE = "audioFile";

			public const string PROP_TRANSCRIPT_FILE = "transcriptFile";

			public const string PROP_WHICH_BATCH = "whichBatch";

			public const string PROP_TOTAL_BATCHES = "totalBatches";
		}
	}
}

using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.util
{
	public class GapInsertionDetector : Object
	{
		public GapInsertionDetector(string referenceFile, string hypothesisFile, bool showGapInsertions)
		{
			this.referenceFile = new ReferenceFile(referenceFile);
			this.hypothesisFile = new HypothesisFile(hypothesisFile);
		}
		
		public virtual int detect()
		{
			int num = 0;
			int num2 = 0;
			ReferenceUtterance referenceUtterance = this.referenceFile.nextUtterance();
			StringBuilder stringBuilder = new StringBuilder();
			while (num2 == 0)
			{
				HypothesisWord hypothesisWord = this.hypothesisFile.nextWord();
				if (hypothesisWord != null)
				{
					int num3 = 0;
					while (referenceUtterance != null && referenceUtterance.getEndTime() < hypothesisWord.getStartTime())
					{
						referenceUtterance = this.referenceFile.nextUtterance();
					}
					if (referenceUtterance != null)
					{
						if (referenceUtterance.isSilenceGap())
						{
							num3 = 1;
						}
						else
						{
							while (referenceUtterance.getEndTime() < hypothesisWord.getEndTime())
							{
								referenceUtterance = this.referenceFile.nextUtterance();
								if (referenceUtterance == null || referenceUtterance.isSilenceGap())
								{
									num3 = 1;
									break;
								}
							}
						}
					}
					else
					{
						num3 = 1;
					}
					if (num3 != 0)
					{
						num++;
						if (this.showGapInsertions)
						{
							stringBuilder.append("GapInsError: Utterance: ").append(this.hypothesisFile.getUtteranceCount()).append(" Word: ").append(hypothesisWord.getText()).append(" (").append(hypothesisWord.getStartTime()).append(',').append(hypothesisWord.getEndTime()).append("). ");
							if (referenceUtterance != null)
							{
								if (!GapInsertionDetector.assertionsDisabled && !referenceUtterance.isSilenceGap())
								{
									
									throw new AssertionError();
								}
								stringBuilder.append("Reference: <sil> (").append(referenceUtterance.getStartTime()).append(',').append(referenceUtterance.getEndTime()).append(')');
							}
							stringBuilder.append('\n');
						}
					}
				}
				else
				{
					num2 = 1;
				}
			}
			if (this.showGapInsertions)
			{
				java.lang.System.@out.println(stringBuilder);
			}
			return num;
		}
		
		public static void main(string[] argv)
		{
			if (argv.Length < 2)
			{
				java.lang.System.@out.println("Usage: java GapInsertionDetector <referenceFile> <hypothesisFile>");
			}
			try
			{
				string text = argv[0];
				string text2 = argv[1];
				GapInsertionDetector gapInsertionDetector = new GapInsertionDetector(text, text2, true);
				java.lang.System.@out.println(new StringBuilder().append("# of gap insertions: ").append(gapInsertionDetector.detect()).toString());
			}
			catch (System.Exception ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
		}

		private ReferenceFile referenceFile;

		private HypothesisFile hypothesisFile;

		private bool showGapInsertions;
		
		internal static bool assertionsDisabled = !ClassLiteral<GapInsertionDetector>.Value.desiredAssertionStatus();
	}
}

using System;

using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.util
{
	public class GapInsertionDetector : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			190,
			104,
			108,
			108
		})]
		
		public GapInsertionDetector(string referenceFile, string hypothesisFile, bool showGapInsertions)
		{
			this.referenceFile = new ReferenceFile(referenceFile);
			this.hypothesisFile = new HypothesisFile(hypothesisFile);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			11,
			98,
			98,
			108,
			102,
			102,
			109,
			103,
			163,
			100,
			110,
			206,
			99,
			104,
			133,
			111,
			108,
			100,
			103,
			99,
			226,
			70,
			163,
			103,
			100,
			107,
			127,
			1,
			125,
			127,
			9,
			99,
			122,
			120,
			152,
			169,
			98,
			130,
			101,
			104,
			139
		})]
		
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

		[LineNumberTable(new byte[]
		{
			77,
			101,
			207,
			100,
			132,
			137,
			191,
			33,
			2,
			98,
			135
		})]
		
		public static void main(string[] argv)
		{
			if (argv.Length < 2)
			{
				java.lang.System.@out.println("Usage: java GapInsertionDetector <referenceFile> <hypothesisFile>");
			}
			Exception ex3;
			try
			{
				string text = argv[0];
				string text2 = argv[1];
				GapInsertionDetector gapInsertionDetector = new GapInsertionDetector(text, text2, true);
				java.lang.System.@out.println(new StringBuilder().append("# of gap insertions: ").append(gapInsertionDetector.detect()).toString());
			}
			catch (Exception ex)
			{
				Exception ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
				if (ex2 == null)
				{
					throw;
				}
				ex3 = ex2;
				goto IL_62;
			}
			return;
			IL_62:
			Exception ex4 = ex3;
			Throwable.instancehelper_printStackTrace(ex4);
		}

		
		static GapInsertionDetector()
		{
		}

		private ReferenceFile referenceFile;

		private HypothesisFile hypothesisFile;

		private bool showGapInsertions;

		
		internal static bool assertionsDisabled = !ClassLiteral<GapInsertionDetector>.Value.desiredAssertionStatus();
	}
}
